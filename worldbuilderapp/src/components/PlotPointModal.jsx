import React, { useEffect, useState } from 'react';
import {
    fetchRivers,
    fetchRoutes,
    fetchChapters
} from '../api/DropdownApi';
import {
    fetchCalendarDayByMonthAndDay,
    fetchMonths
} from '../api/CalendarApi';
import { createPlotPoint } from '../api/PlotPointApi';
import '../styles/PlotPointModal.css';

export default function PlotPointModal({ onClose, onSave }) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [chapterId, setChapterId] = useState(null);
    const [bookOverride, setBookOverride] = useState('');
    const [chapterOverride, setChapterOverride] = useState('');

    const [startDay, setStartDay] = useState('');
    const [startMonth, setStartMonth] = useState('');
    const [endDay, setEndDay] = useState('');
    const [endMonth, setEndMonth] = useState('');
    const [setStartDateId] = useState(null);
    const [setEndDateId] = useState(null);

    const [months, setMonths] = useState([]);
    const [chapters, setChapters] = useState([]);
    const [rivers, setRivers] = useState([]);
    const [routes, setRoutes] = useState([]);

    const [linkedEntities, setLinkedEntities] = useState({
        riverIds: [],
        routeIds: [],
        eraIds: []
    });

    useEffect(() => {
        const loadData = async () => {
            setMonths(await fetchMonths());
            setChapters(await fetchChapters());
            setRivers(await fetchRivers());
            setRoutes(await fetchRoutes());
        };
        loadData();
    }, []);

    const handleCheckboxChange = (field, id) => {
        setLinkedEntities(prev => {
            const exists = prev[field].includes(id);
            const updated = exists ? prev[field].filter(x => x !== id) : [...prev[field], id];
            return { ...prev, [field]: updated };
        });
    };

    const resolveCalendarIds = async () => {
        try {
            const start = await fetchCalendarDayByMonthAndDay(startMonth, parseInt(startDay));
            const end = await fetchCalendarDayByMonthAndDay(endMonth, parseInt(endDay));
            setStartDateId(start.id);
            setEndDateId(end.id);
            return { startId: start.id, endId: end.id };
        } catch (err) {
            console.error("❌ Failed to resolve calendar days:", err);
            throw err;
        }
    };

    const handleSubmit = async () => {
        try {
            const { startId, endId } = await resolveCalendarIds();
            const payload = {
                title,
                description,
                startDateId: startId,
                endDateId: endId,
                chapterId,
                bookOverride,
                chapterOverride,
                ...linkedEntities
            };
            const result = await createPlotPoint(payload);
            onSave(result);
        } catch (err) {
            console.error("❌ Failed to create plot point:", err);
        }
    };

    return (
        <div className="plotpoint-modal">
            <div className="modal-content">
                <h2>Create PlotPoint</h2>

                <label>Title</label>
                <input value={title} onChange={e => setTitle(e.target.value)} />

                <label>Description</label>
                <textarea value={description} onChange={e => setDescription(e.target.value)} />

                <div className="date-row">
                    <label>Start Date</label>
                    <input type="number" placeholder="Day" value={startDay} onChange={e => setStartDay(e.target.value)} />
                    <select value={startMonth} onChange={e => setStartMonth(e.target.value)}>
                        <option value="">Month</option>
                        {months.map(m => (
                            <option key={m.name} value={m.name}>{m.name}</option>
                        ))}
                    </select>
                </div>

                <div className="date-row">
                    <label>End Date</label>
                    <input type="number" placeholder="Day" value={endDay} onChange={e => setEndDay(e.target.value)} />
                    <select value={endMonth} onChange={e => setEndMonth(e.target.value)}>
                        <option value="">Month</option>
                        {months.map(m => (
                            <option key={m.name} value={m.name}>{m.name}</option>
                        ))}
                    </select>
                </div>

                <label>Chapter</label>
                <select onChange={e => setChapterId(Number(e.target.value))}>
                    <option value="">(Optional)</option>
                    {chapters.map(s => (
                        <option key={s.id} value={s.id}>{s.chapterName}</option>
                    ))}
                </select>

                <label>Book Override</label>
                <input value={bookOverride} onChange={e => setBookOverride(e.target.value)} />

                <label>Chapter Override</label>
                <input value={chapterOverride} onChange={e => setChapterOverride(e.target.value)} />

                <fieldset>
                    <legend>Rivers</legend>
                    {rivers.map(r => (
                        <label key={r.id}>
                            <input
                                type="checkbox"
                                checked={linkedEntities.riverIds.includes(r.id)}
                                onChange={() => handleCheckboxChange('riverIds', r.id)}
                            />
                            {r.name}
                        </label>
                    ))}
                </fieldset>

                <fieldset>
                    <legend>Routes</legend>
                    {routes.map(r => (
                        <label key={r.id}>
                            <input
                                type="checkbox"
                                checked={linkedEntities.routeIds.includes(r.id)}
                                onChange={() => handleCheckboxChange('routeIds', r.id)}
                            />
                            {r.name}
                        </label>
                    ))}
                </fieldset>

                <div className="modal-buttons">
                    <button onClick={handleSubmit}>Save</button>
                    <button onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
}
