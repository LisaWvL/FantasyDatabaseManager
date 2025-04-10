// Updated AssistantPage.jsx with summary toggle, faded styling, and memory-aware message tracking

import { useEffect, useRef, useState } from 'react';
import { fetchPlotPoints, fetchPlotPointById, fetchPlotPointEntities } from '../plotpoints/PlotPointApi';
import { fetchChapterById, fetchChapterEntities } from './ChapterApi';
import {
  fetchConversationHistory,
  sendChatTurn,
  requestSummaryTurn,
} from '../ai/ConversationTurnApi';

import './AssistantPage.css';
import EntityCard from '../entities/EntityCard';

export default function AssistantPage() {
  const [plotpoints, setPlotpoints] = useState([]);
  const [selectedId, setSelectedId] = useState('');
  const [entitySummary, setEntitySummary] = useState({});
  const [prompt, setPrompt] = useState('');
  const [danMode, setDanMode] = useState(false);
  const [loading, setLoading] = useState(false);
  const [conversationHistory, setConversationHistory] = useState([]);
  const [_expandedEntity, setExpandedEntity] = useState(null);
  const responseRef = useRef(null);

  const extractDisplayName = (entity, type) => {
    if (!entity || typeof entity !== 'object') return '[Unnamed]';
    switch (type.toLowerCase()) {
      case 'characters':
        return entity.name || '[Unnamed Character]';
      case 'items':
        return `${entity.name || '[Unnamed Item]'} (${entity.owner?.name || 'none'})`;
      case 'eras':
        return `${entity.name || '[Unnamed Era]'} (${entity.magicStatus || 'Magic Unknown'})`;
      case 'events':
        return entity.name || '[Unnamed Event]';
      case 'locations':
        return `${entity.name || '[Unnamed Location]'} (${entity.type || 'unknown'})`;
      case 'factions':
        return entity.name || '[Unnamed Faction]';
      case 'relationships':
        return `${entity.character1?.name || '[?]'} > ${entity.relationshipType || 'Unknown'} < ${entity.character2?.name || '[?]'}`;
      case 'rivers':
      case 'routes':
        return entity.name || '[Unnamed]';
      default:
        return entity.name || entity.title || entity.alias || '[Unnamed]';
    }
  };

  useEffect(() => {
    fetchPlotPoints().then(setPlotpoints);
  }, []);

  useEffect(() => {
    if (!selectedId) return;

    const loadPlotPointDetails = async () => {
      setLoading(true);

      try {
        const plotPointId = parseInt(selectedId);
        const plotPoint = await fetchPlotPointById(plotPointId);

        const [_chapter, chapterEntities, plotpointEntities, history] = await Promise.all([
          fetchChapterById(plotPoint.chapterId),
          fetchChapterEntities(plotPoint.chapterId),
          fetchPlotPointEntities(plotPointId),
          fetchConversationHistory(plotPointId),
        ]);

        const combined = { ...chapterEntities };

        if (plotpointEntities.routes?.length)
          combined.routes = [...(combined.routes || []), ...plotpointEntities.routes];

        if (plotpointEntities.rivers?.length)
          combined.rivers = [...(combined.rivers || []), ...plotpointEntities.rivers];

        combined.startDate = plotpointEntities.startDate;
        combined.endDate = plotpointEntities.endDate;

        setEntitySummary(combined);
        setConversationHistory(history);
      } catch (err) {
        console.error('❌ Failed to load plot point context:', err);
      }

      setLoading(false);
    };

    loadPlotPointDetails();
  }, [selectedId]);

  const handleSubmit = async () => {
    if (!selectedId || !prompt) return;
    setLoading(true);

    try {
      const response = await sendChatTurn({
        plotpointId: parseInt(selectedId),
        history: [...conversationHistory, { role: 'user', content: prompt }],
        danMode,
      });

      setConversationHistory((prev) => [
        ...prev,
        { role: 'user', content: prompt },
        { role: 'assistant', content: response.reply },
      ]);

      setPrompt('');
    } catch (err) {
      console.error('❌ Failed to generate response:', err);
    }

    setLoading(false);

    if (responseRef.current) {
      setTimeout(() => {
          if (responseRef.current) {
              responseRef.current.scrollIntoView({ behavior: 'smooth' });
          }
      }, 100);
    }
  };

  const handleSummarize = async () => {
    try {
      const latestTurns = conversationHistory.slice(0, -4); // summarize all but last few
      const textToSummarize = latestTurns.map((t) => `${t.role}: ${t.content}`).join('\n');
      const summary = await requestSummaryTurn({
        prompt: textToSummarize,
        response: '[ChapterSummary to follow]',
        plotPointId: parseInt(selectedId),
        isSummary: true,
      });

      setConversationHistory((prev) => [summary, ...prev.slice(latestTurns.length)]);
    } catch (err) {
      console.error('❌ Failed to summarize:', err);
    }
  };

  return (
    <div className="assistant-page">
      <div className="entity-sidebar">
        <h3>Select PlotPoint</h3>
        <select value={selectedId} onChange={(e) => setSelectedId(e.target.value)}>
          <option value="">-- Select --</option>
          {plotpoints.map((p) => (
            <option key={p.id} value={p.id}>
              {p.title} ({p.startDateName})
            </option>
          ))}
        </select>

        <div className="toggle-dan">
          <label>
            <input type="checkbox" checked={danMode} onChange={() => setDanMode(!danMode)} /> Enable
            DAN Mode
          </label>
        </div>

        <button className="summarize-btn" onClick={handleSummarize}>
          Summarize earlier turns
        </button>

        {Object.entries(entitySummary).map(
          ([type, list]) =>
            Array.isArray(list) &&
            list.length > 0 && (
              <div key={type}>
                <h4>{type}</h4>
                {list.map((e) => (
                  <div key={e.id} className="entity-label" onClick={() => setExpandedEntity(e)}>
                    {extractDisplayName(e, type)}
                  </div>
                ))}
              </div>
            )
        )}
      </div>

      <div className="chat-section">
        <div className="chat-history">
          {conversationHistory.map((msg, i) => (
            <div key={i} className={`chat-bubble ${msg.role}${msg.isSummary ? ' summary' : ''}`}>
              {msg.content}
            </div>
          ))}
          <div ref={responseRef}></div>
        </div>

        <div className="chat-input">
          <textarea
            value={prompt}
            onChange={(e) => setPrompt(e.target.value)}
            rows={3}
            placeholder="Ask something..."
          />
          <button disabled={loading} onClick={handleSubmit}>
            Send
          </button>
        </div>
      </div>

      {/*{expandedEntity && (*/}
      {/*  <EntityCard entity={expandedEntity} onBlur={() => setExpandedEntity(null)} />*/}
      {/*)}*/}
      </div>

  );
}
