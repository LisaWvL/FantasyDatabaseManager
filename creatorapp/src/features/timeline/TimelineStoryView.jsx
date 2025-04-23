import { useEffect, useState } from 'react';
import MainLayout from '../../styles/MainLayout'; // ✅ Enhanced layout
import '../../styles/MainLayout.css';
import './TimelineStoryView.css';

import { EntityFetcher } from '../../store/EntityManager';
import PlotPointTimeline from './PlotPointTimeline';
import ChapterTimeline from './ChapterTimeline';
import EraTimeline from './EraTimeline';

export default function TimelineStoryView() {
    const [plotPoints, setPlotPoints] = useState([]);
    const [chapters, setChapters] = useState([]);
    const [eras, setEras] = useState([]);
    const [viewMode, setViewMode] = useState('all'); // all | pp_eras | chapters_eras | pp_chapters
    const [timelineRange, _setTimelineRange] = useState({ startYear: -2000, endYear: 2050 });

    useEffect(() => {
        async function loadData() {
            try {
                const [ppData, chData, eraData] = await Promise.all([
                    EntityFetcher.fetchAll('PlotPoint'),
                    EntityFetcher.fetchAll('Chapter'),
                    EntityFetcher.fetchAll('Era')
                ]);

                setPlotPoints(ppData);
                setChapters(chData);
                setEras(eraData);
            } catch (err) {
                console.error('❌ Failed to load timeline entities:', err);
            }
        }

        loadData();
    }, []);

    const handleViewToggle = (mode) => setViewMode(mode);

    const headerContent = (
        <div className="timeline-controls">
            <label>Show:</label>
            <button onClick={() => handleViewToggle('all')}>PlotPoints + Chapters + Eras</button>
            <button onClick={() => handleViewToggle('pp_eras')}>PlotPoints + Eras</button>
            <button onClick={() => handleViewToggle('chapters_eras')}>Chapters + Eras</button>
            <button onClick={() => handleViewToggle('pp_chapters')}>PlotPoints + Chapters</button>
        </div>
    );

    return (
        <MainLayout
            headerContent={headerContent}
            unassignedSidebar={{
                entityType: 'timeline',
                items: [],
                isUnassigned: () => false,
                onDropToUnassigned: () => { },
                onContextMenu: () => { },
                renderItem: () => null,
                isSidebarOpen: true,
                setIsSidebarOpen: () => { },
                isOverSidebar: false,
                onSidebarDragOver: () => { },
                onSidebarDragLeave: () => { }
            }}
        >
            <div className="timeline-header">
                <h2>Timeline Story View</h2>
                <div className="timeline-controls">
                    <label>Show:</label>
                    <button onClick={() => handleViewToggle('all')}>PlotPoints + Chapters + Eras</button>
                    <button onClick={() => handleViewToggle('pp_eras')}>PlotPoints + Eras</button>
                    <button onClick={() => handleViewToggle('chapters_eras')}>Chapters + Eras</button>
                    <button onClick={() => handleViewToggle('pp_chapters')}>PlotPoints + Chapters</button>
                </div>
            </div>
            <div className="timeline-visual-wrapper">
                {(viewMode === 'all' || viewMode === 'pp_eras' || viewMode === 'pp_chapters') && (
                    <PlotPointTimeline plotPoints={plotPoints} range={timelineRange} />
                )}
                {(viewMode === 'all' || viewMode === 'chapters_eras' || viewMode === 'pp_chapters') && (
                    <ChapterTimeline chapters={chapters} range={timelineRange} />
                )}
                {(viewMode === 'all' || viewMode === 'chapters_eras' || viewMode === 'pp_eras') && (
                    <EraTimeline eras={eras} range={timelineRange} />
                )}
            </div>
            <div className="timeline-footer">
                <p>Timeline Footer Content</p>
            </div>
        </MainLayout >
    );
}
