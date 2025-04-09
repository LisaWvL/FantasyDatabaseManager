import React, { useEffect, useRef, useState } from 'react';
import ChapterEditor from '../components/ChapterEditor';
import { fetchPlotPointsForChapter } from '../api/PlotPointApi';
import { fetchChapters } from '../api/ChapterApi';
import '../styles/WritingAssistantPage.css';

export default function WritingAssistantPage() {
    const [chapterHeaders, setChapterHeaders] = useState([]);
    const [activeChapters, setActiveChapters] = useState([]);
    const scrollRef = useRef(null);

    useEffect(() => {
        fetchChapters().then(async (all) => {
            setChapterHeaders(all);
            const filtered = all.filter(s =>
                s.chapterTitle || s.chapterText || s.summary || s.povCharacterName || s.toDo
            );

            const loaded = await Promise.all(
                filtered.map(async (s) => {
                    const [entities, plotPoints] = await Promise.all([
                        fetch(`/api/chapter/${s.id}/entities`).then(r => r.json()),
                        fetchPlotPointsForChapter(s.id),
                    ]);
                    return { id: s.id, entities, plotPoints };
                })
            );

            setActiveChapters(loaded);
        });
    }, []);

    const handleSave = (updatedChapter) => {
        setChapterHeaders(prev =>
            prev.map(s => s.id === updatedChapter.id ? updatedChapter : s)
        );
    };

    const handleNewChapter = async () => {
        const usedIds = activeChapters.map(s => s.id);
        const next = chapterHeaders.find(s => !usedIds.includes(s.id));
        if (!next) return;

        const [entities, plotPoints] = await Promise.all([
            fetch(`/api/chapter/${next.id}/entities`).then(r => r.json()),
            fetchPlotPointsForChapter(next.id),
        ]);

        setActiveChapters(prev => [...prev, { id: next.id, entities, plotPoints }]);
        setTimeout(() => scrollRef.current?.scrollIntoView({ behavior: 'smooth' }), 100);
    };

    const totalWordCount = chapterHeaders.reduce((sum, s) => sum + (s.wordCount || 0), 0);

    return (
        <div className="chapter-page">
            <div className="chapter-header-bar">
                <button onClick={() => scrollRef.current?.scrollIntoView({ behavior: 'smooth' })}>
                    ⬆ Go to Top of Current Chapter
                </button>
                <h2>Word Count: {totalWordCount}</h2>
                <button className="new-chapter-button" onClick={handleNewChapter}>
                    ➕ New Chapter
                </button>
            </div>

            <div className="chapter-editor-list">
                {activeChapters.map((s, index) => (
                    <div key={s.id} ref={index === activeChapters.length - 1 ? scrollRef : null}>
                        <ChapterEditor
                            chapterId={s.id}
                            plotPoints={s.plotPoints}
                            entities={s.entities}
                            chapterOptions={chapterHeaders}
                            onSave={handleSave}
                        />
                    </div>
                ))}
            </div>
        </div>
    );
}
