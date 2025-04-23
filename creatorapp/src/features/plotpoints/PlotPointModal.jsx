import { useEffect, useState } from 'react';
import { createPlotPoint, getRelatedActs, getRelatedChapters } from './PlotPointApi';
import { fetchBooks, fetchRivers, fetchRoutes } from '../../api/DropdownApi';
import { toggleLinkedEntity } from '../../utils/plotPointFormUtils';
import './PlotPointModal.css';

export default function PlotPointModal({ onClose, onSave, schema, registry }) {
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');

    const [books, setBooks] = useState([]);
    const [acts, setActs] = useState([]);
    const [chapters, setChapters] = useState([]);
    const [rivers, setRivers] = useState([]);
    const [routes, setRoutes] = useState([]);

    const [selectedBookId, setSelectedBookId] = useState('');
    const [selectedActId, setSelectedActId] = useState('');
    const [selectedChapterId, setSelectedChapterId] = useState('');

    const [linkedEntities, setLinkedEntities] = useState({
        riverIds: [],
        routeIds: [],
        eraIds: [],
    });

    useEffect(() => {
        async function loadInitialData() {
            try {
                const [bookList, riverList, routeList] = await Promise.all([
                    fetchBooks(),
                    fetchRivers(),
                    fetchRoutes(),
                ]);
                setBooks(bookList);
                setRivers(riverList);
                setRoutes(routeList);
            } catch (err) {
                console.error('❌ Failed to load dropdowns:', err);
            }
        }

        loadInitialData();
    }, []);

    const handleBookChange = async (bookId) => {
        setSelectedBookId(bookId);
        setSelectedActId('');
        setSelectedChapterId('');
        setActs([]);
        setChapters([]);
        try {
            if (bookId) {
                const relatedActs = await getRelatedActs(bookId);
                setActs(relatedActs);
            }
        } catch (err) {
            console.error(`❌ Could not load acts for book ${bookId}`, err);
        }
    };

    const handleActChange = async (actId) => {
        setSelectedActId(actId);
        setSelectedChapterId('');
        setChapters([]);
        try {
            if (actId) {
                const relatedChapters = await getRelatedChapters(actId);
                setChapters(relatedChapters);
            }
        } catch (err) {
            console.error(`❌ Could not load chapters for act ${actId}`, err);
        }
    };

    const handleCheckboxChange = (field, id) => {
        setLinkedEntities(prev => toggleLinkedEntity(field, id, prev));
    };

    const handleSubmit = async () => {
        try {
            const payload = {
    title,
    description,
    ...linkedEntities,
};

            const result = await createPlotPoint(payload);
            onSave(result);
        } catch (err) {
            console.error('❌ Failed to create plot point:', err);
        }
    };

    return (
        <div className="plotpoint-modal-overlay">
            <div className="plotpoint-modal">
                <div className="modal-header">
                    <h2>Create PlotPoint</h2>
                    <button className="modal-close" onClick={onClose}>✕</button>
                </div>

                <div className="modal-body">
                    <label>Title</label>
                    <input value={title} onChange={(e) => setTitle(e.target.value)} />

                    <label>Description</label>
                    <textarea value={description} onChange={(e) => setDescription(e.target.value)} />

                    <div className="form-group-row">
                        <div className="form-group">
                            <label>Book</label>
                            <select value={selectedBookId} onChange={(e) => handleBookChange(e.target.value)}>
                                <option value="">Select Book</option>
                                {books.map(book => (
                                    <option key={book.id} value={book.id}>{book.name}</option>
                                ))}
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Act</label>
                            <select
                                value={selectedActId}
                                onChange={(e) => handleActChange(e.target.value)}
                                disabled={!selectedBookId}
                            >
                                <option value="">Select Act</option>
                                {acts.map(act => (
                                    <option key={act.id} value={act.id}>{act.actTitle}</option>
                                ))}
                            </select>
                        </div>

                        <div className="form-group">
                            <label>Chapter</label>
                            <select
                                value={selectedChapterId}
                                onChange={(e) => setSelectedChapterId(e.target.value)}
                                disabled={!selectedActId}
                            >
                                <option value="">Select Chapter</option>
                                {chapters.map(ch => (
                                    <option key={ch.id} value={ch.id}>{ch.chapterTitle}</option>
                                ))}
                            </select>
                        </div>
                    </div>

                    <fieldset>
                        <legend>Rivers</legend>
                        {rivers.map((r) => (
                            <label key={r.id} className="checkbox-label">
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
                        {routes.map((r) => (
                            <label key={r.id} className="checkbox-label">
                                <input
                                    type="checkbox"
                                    checked={linkedEntities.routeIds.includes(r.id)}
                                    onChange={() => handleCheckboxChange('routeIds', r.id)}
                                />
                                {r.name}
                            </label>
                        ))}
                    </fieldset>
                </div>

                <div className="modal-footer">
                    <button className="btn-primary" onClick={handleSubmit}>Create</button>
                    <button className="btn-secondary" onClick={onClose}>Cancel</button>
                </div>
            </div>
        </div>
    );
}
