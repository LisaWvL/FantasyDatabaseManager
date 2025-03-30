import { useEffect, useRef, useState } from 'react';
import {
    fetchPlotPoints,
    fetchPlotPointById
} from '../api/PlotPointApi';
import { fetchSnapshotById, fetchSnapshotEntities } from '../api/SnapshotApi';
import "../styles/AssistantPage.css";
import EntityCard from '../components/EntityCard'; // we'll define this


export default function AssistantPage() {
    const [plotpoints, setPlotpoints] = useState([]);
    const [selectedId, setSelectedId] = useState('');
    const [selectedPlotPoint, setSelectedPlotPoint] = useState(null);
    const [snapshotInfo, setSnapshotInfo] = useState(null);
    const [entitySummary, setEntitySummary] = useState('');
    const [prompt, setPrompt] = useState('');
    const [danMode, _setDanMode] = useState(false);
    const [response, setResponse] = useState('');
    const [loading, setLoading] = useState(false);
    const responseRef = useRef(null);
    const [highlightedEntities, setHighlightedEntities] = useState([]);
    const [expandedEntity, setExpandedEntity] = useState(null); // holds the entity and its type


    useEffect(() => {
        fetchPlotPoints().then(setPlotpoints);
    }, []);

    useEffect(() => {
        if (!selectedId) return;

        fetchPlotPointById(selectedId).then(pp => {
            setSelectedPlotPoint(pp);

            if (pp.snapshotId) {
                fetchSnapshotById(pp.snapshotId).then(setSnapshotInfo);

                fetchSnapshotEntities(pp.snapshotId).then(entityData => {
                    const entityMap = {};
                    const entityNames = new Set();

                    for (const [type, list] of Object.entries(entityData)) {
                        if (Array.isArray(list)) {
                            entityMap[type] = list;
                            list.forEach(ent => {
                                const name = ent.Name || ent.Title || ent.Alias;
                                if (name) entityNames.add(name);
                            });
                        }
                    }

                    setEntitySummary(entityMap);
                    setHighlightedEntities([...entityNames]);
                });
            }
        });
    }, [selectedId]);

    useEffect(() => {
        if (responseRef.current) {
            responseRef.current.scrollIntoView({ behavior: 'smooth' });
        }
    }, [response]);

    const sendPrompt = async (customPrompt) => {
        setLoading(true);
        setResponse('');
        try {
            const res = await fetch('http://localhost:8000/generate-scene', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    plotpointId: parseInt(selectedId),
                    prompt: customPrompt,
                    danMode,
                }),
            });
            const data = await res.json();
            setResponse(data.response);
        } catch (err) {
            setResponse('❌ Error generating scene.');
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const handleSubmit = () => sendPrompt(prompt);
    const handleRegenerate = () => sendPrompt(prompt);
    const handleContinue = () => sendPrompt(`${prompt}\n\nContinue the scene from where you left off.`);

    const highlightText = (text) => {
        if (!highlightedEntities.length) return text;
        const escaped = highlightedEntities.map(e => e.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'));
        const regex = new RegExp(`\\b(${escaped.join('|')})\\b`, 'gi');
        return text.split(regex).map((part, i) =>
            regex.test(part)
                ? <mark key={i} className="bg-yellow-200 px-1 rounded">{part}</mark>
                : part
        );
    };

    return (
        <div className="assistant-layout">
            <aside className="assistant-sidebar">
                <h2>Select PlotPoint</h2>
                <select
                    className="dropdown"
                    value={selectedId}
                    onChange={(e) => setSelectedId(e.target.value)}
                >
                    <option value="">-- Choose PlotPoint --</option>
                    {plotpoints.map(pp => (
                        <option key={pp.id} value={pp.id}>
                            {pp.title}
                        </option>
                    ))}
                </select>

                {selectedPlotPoint && (
                    <div className="plotpoint-info">
                        <div><strong>Snapshot:</strong> {snapshotInfo?.snapshotName}</div>
                        <div><strong>Start:</strong> {selectedPlotPoint.startDateName}</div>
                        <div><strong>End:</strong> {selectedPlotPoint.endDateName}</div>
                        <div className="description">{selectedPlotPoint.description}</div>
                    </div>
                )}

                {entitySummary && (
                    <div className="entity-summary">
                        <h3>🔗 Connected Entities</h3>
                        {Object.entries(entitySummary).map(([type, entities]) => (
                            <div key={type}>
                                <h4>{type}</h4>
                                <ul>
                                    {entities.map((ent, idx) => (
                                        <li key={idx}>
                                            <button className="entity-link" onClick={() => setExpandedEntity({ type, entity: ent })}>
                                                {ent.name || ent.title || ent.alias || '[Unnamed]'}
                                            </button>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        ))}
                    </div>
                )}
            </aside>

            <main className="assistant-main">
                <div className="assistant-response">
                    <h3>📜 Assistant Output</h3>
                    <div ref={responseRef}>
                        {response ? highlightText(response) : <span className="placeholder">Waiting for your prompt...</span>}
                    </div>
                </div>

                <div className="prompt-section">
                    <label>Your Prompt</label>
                    <textarea
                        value={prompt}
                        onChange={(e) => setPrompt(e.target.value)}
                        placeholder="Describe what you want to write..."
                    />
                    <div className="response-buttons">
                        <button onClick={handleSubmit} disabled={!prompt || loading}>
                            {loading ? 'Generating...' : 'Send'}
                        </button>
                        <button onClick={handleRegenerate} disabled={!prompt || loading}>🔄 Regenerate</button>
                        <button onClick={handleContinue} disabled={!response || loading}>➕ Continue</button>
                    </div>
                </div>
            </main>
            {expandedEntity && (
                <EntityCard
                    type={expandedEntity.type}
                    entity={expandedEntity.entity}
                    onClose={() => setExpandedEntity(null)}
                />
            )}

        </div>
    );
}
