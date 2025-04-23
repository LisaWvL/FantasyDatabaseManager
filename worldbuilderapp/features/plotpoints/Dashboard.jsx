import { useEffect, useState } from 'react';
import CalendarDayCell from './CalendarDayCell';
import CalendarGrid from './CalendarGrid';
import './CalendarGrid.css';
import PlotPointModal from './PlotPointModal';
import MainLayout from '../../src/layout/MainLayout';
import useEntityRegistry from '../../utils/EntityRegistry';
import { entitySchemas } from '../../utils/entitySchemas';
import {
    useDragAndDrop,
    useDragScroll
} from '../../utils/dragDropHandlers';
import Card from '../../src/components/Card';
import { useEntityContextMenu } from '../../hooks/useEntityContextMenu';
import './Dashboard.css';

export default function Dashboard() {
    const registry = useEntityRegistry();
    const [calendar, setCalendar] = useState([]);
    //const [collapsedMonths, setCollapsedMonths] = useState({});
    const [showModal, setShowModal] = useState(false);
    const [editingPlotPointId, setEditingPlotPointId] = useState(null);
    const [setNewPlotPointId] = useState(null); //newPlotPointId, 
    const [isOverSidebar] = useState(false);

    const schema = entitySchemas.PlotPoint;
    const allPlotPoints = Object.values(registry.cache['PlotPoint'] || {});

    const [hasLoaded, setHasLoaded] = useState(false);

    useEffect(() => {
        if (hasLoaded) return;

        const loadData = async () => {
            await registry.load('Calendar');
            await registry.loadDashboard();

            const calendarData = registry.cache.Calendar || {};
            setCalendar(Object.values(calendarData));
            setHasLoaded(true);
        };

        loadData();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [hasLoaded]);



    useDragScroll(isOverSidebar);

    const { handleDragStart } = useDragAndDrop({
        handleUpdateEntity: ({ entity, payload }) => {
            registry.update(entity.entityType, entity.id, payload);
        },
    });

    //const calendarById = Object.fromEntries(calendar.map(day => [day.id, day]));

    //const groupedByMonth = calendar.reduce((acc, day) => {
    //    if (!acc[day.month]) acc[day.month] = [];
    //    acc[day.month].push(day);
    //    return acc;
    //}, {});

    //const plotpointsByDay = allPlotPoints.reduce((acc, pp) => {
    //    if (!pp.startDateId) return acc;
    //    const isReversed = pp.endDateId && pp.endDateId < pp.startDateId;
    //    const start = isReversed ? pp.endDateId : pp.startDateId;
    //    const end = isReversed ? pp.startDateId : (pp.endDateId || pp.startDateId);
    //    const colorIndex = pp.id % 10;

    //    for (let dayId = start; dayId <= end; dayId++) {
    //        if (!calendarById[dayId]) continue;
    //        if (!acc[dayId]) acc[dayId] = [];
    //        acc[dayId].push({
    //            ...pp,
    //            isGhost: dayId !== (isReversed ? pp.endDateId : pp.startDateId),
    //            colorIndex,
    //            isReversed
    //        });
    //    }

    //    return acc;
    //}, {});

    const handleResize = (id, direction, newDayId) => {
        const pp = registry.get('PlotPoint', id);
        if (!pp) return;
        const updated = {
            startDateId: direction === 'start' ? newDayId : pp.startDateId,
            endDateId: direction === 'end' ? newDayId : pp.endDateId ?? pp.startDateId
        };
        registry.update('PlotPoint', id, updated);
    };

    const handleDropPlotPoint = (id, dayId) => {
        const pp = registry.get('PlotPoint', id);
        if (!pp) return;

        const originalStart = pp.startDateId;
        const originalEnd = pp.endDateId ?? pp.startDateId;
        const isReversed = originalEnd < originalStart;
        const duration = Math.abs(originalEnd - originalStart);

        const newStart = isReversed ? dayId + duration : dayId;
        const newEnd = isReversed ? dayId : dayId + duration;

        registry.update('PlotPoint', id, { startDateId: newStart, endDateId: newEnd });
        setNewPlotPointId(null);
    };

    const handleNewPlotPoint = (saved) => {
        registry.update('PlotPoint', saved.id, saved);
        setNewPlotPointId(saved.id);
        setShowModal(false);
    };

    const handleDeleteEntity = (entity) => {
        registry.delete('PlotPoint', entity.id);
    };

    const { showContextMenu, contextMenuPortal } = useEntityContextMenu({
        onCreate: () => setShowModal(true),
        onEdit: (entity) => {
            setEditingPlotPointId(entity.id);
            setShowModal(true);
        },
        onDelete: handleDeleteEntity,
    });

    const [isSidebarOpen, setIsSidebarOpen] = useState(true);
    return (
        <MainLayout
            headerContent={
                <button className="add-button" onClick={() => setShowModal(true)}>
                    ＋ Add PlotPoint
                </button>
            }
            unassignedSidebar={{
                entityType: 'PlotPoint',
                items: allPlotPoints,
                isUnassigned: (pp) => !pp.startDateId,
                onDropToUnassigned: async (id) => {
                    const pp = registry.get('PlotPoint', id);
                    if (!pp) return;
                    registry.update('PlotPoint', id, { startDateId: null, endDateId: null });
                },
                onContextMenu: (e, item) => showContextMenu(e, item, 'PlotPoint'),
                renderItem: (item) => (
                    <Card
                        key={item.id}
                        entity={item}
                        entityType="PlotPoint"
                        displayMode="compact"
                        onFieldUpdate={() => { }}
                        onDelete={() => handleDeleteEntity(item)}
                        onCreateNewVersion={() => { }}
                        draggable
                        onDragStart={(e) => handleDragStart(e, item, 'unassigned-sidebar')}
                        onResizeEnd={(direction, newDayId) => handleResize(item.id, direction, newDayId)}
                        onDragEnd={() => { }}
                        onContextMenu={(e) => showContextMenu(e, item, 'PlotPoint')}
                    />
                ),
                isSidebarOpen,
                setIsSidebarOpen, // ✅ actually toggleable now!
            }}
        >
            <CalendarGrid
                calendarDays={calendar}
                plotPoints={allPlotPoints}
                onDropPlotPoint={(e, id) => {
                    const targetDayId = parseInt(e.currentTarget.dataset.dayid, 10);
                    if (!isNaN(targetDayId)) {
                        handleDropPlotPoint(id, targetDayId);
                    }
                }}
                onContextMenu={(e, pp) => showContextMenu(e, pp, 'PlotPoint')}
                onResizeEnd={(id, direction, newDayId) => handleResize(id, direction, newDayId)}
            />

            {contextMenuPortal}

            {showModal && (
                <PlotPointModal
                    onClose={() => {
                        setShowModal(false);
                        setEditingPlotPointId(null);
                    }}
                    onSave={handleNewPlotPoint}
                    plotPointId={editingPlotPointId}
                    entityType="PlotPoint"
                    schema={schema}
                    registry={registry}
                />
            )}
        </MainLayout>
    );
}


            // {Object.entries(groupedByMonth).map(([month, days]) => (
            //    <div key={month} className="month-block">
            //        <div className="month-header sticky">
            //            <button
            //                className="collapse-btn"
            //                onClick={() =>
            //                    setCollapsedMonths(prev => ({ ...prev, [month]: !prev[month] }))
            //                }
            //            >
            //                {collapsedMonths[month] ? '▶' : '▼'} {month}
            //            </button>
            //        </div>
            //        {!collapsedMonths[month] && (
            //            <div className="calendar-grid">
            //                {days.map((day) => (
            //                    <CalendarDayCell
            //                        key={day.id}
            //                        day={day}
            //                        weekday={calendarById[day.id]}
            //                        month={month}
            //                        year={day.year}
            //                        onDropPlotPoint={handleDropPlotPoint}
            //                    >
            //                        {(plotpointsByDay[day.id] || []).map(pp => (
            //                            <Card
            //                                key={`${pp.id}-${day.id}`}
            //                                entity={pp}
            //                                entityType="PlotPoint"
            //                                displayMode="calendar"
            //                                draggable
            //                                onDragStart={(e) => handleDragStart(e, pp, 'calendar', { sourceDayId: day.id })}
            //                                onDragEnd={() => { }}
            //                                onFieldUpdate={(field, value) =>
            //                                    registry.update('PlotPoint', pp.id, { [field]: value })
            //                                }
            //                                onDelete={() => handleDeleteEntity(pp)}
            //                                onCreateNewVersion={() => setShowModal(true)}
            //                                isGhost={pp.isGhost}
            //                                onResizeEnd={(direction, newDayId) => handleResize(pp.id, direction, newDayId)}
            //                                isReversed={pp.isReversed}
            //                                onContextMenu={(e) => showContextMenu(e, pp, 'plotpoint')}
            //                            />
            //                        ))}
            //                        {newPlotPointId &&
            //                            newPlotPointId ===
            //                            (plotpointsByDay[day.id] || []).find(p => p.id === newPlotPointId)?.id && (
            //                                <div className="new-plotpoint-hint">📌 Drop here</div>
            //                            )}
            //                    </CalendarDayCell>
            //                ))}
            //            </div>
            //        )}
            //    </div>
            //))}