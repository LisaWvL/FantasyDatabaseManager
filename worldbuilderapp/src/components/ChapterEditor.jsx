// ChapterEditor.jsx
import React, { useEffect, useState, useCallback } from 'react';
import TiptapEditorWithToolbar from './TiptapEditorWithToolbar';
import { fetchChapterById, updateChapter } from '../api/ChapterApi';
import { fetchCharacters } from '../api/CharacterApi';
import '../styles/ChapterEditor.css';

export default function ChapterEditor({ chapterId, foldAll, onSave }) {
    const [chapter, setChapter] = useState(null);
    const [chapterText, setChapterText] = useState('');
    const [chaptersummary, setChapterSummary] = useState('');
    const [toDo, setToDo] = useState('');
    const [chapterTitle, setChapterTitle] = useState('');
    const [povCharacterName, setPovCharacterName] = useState('');
    const [characters, setCharacters] = useState([]);
    const [wordCount, setWordCount] = useState(0);
    const [foldChapter, setFoldChapter] = useState(false);
    const [foldChapterSummary, setFoldChapterSummary] = useState(false);
    const [foldTodo, setFoldTodo] = useState(false);
    const [showContext, setShowContext] = useState(false);

    useEffect(() => {
        fetchCharacters().then(setCharacters);
    }, []);

    useEffect(() => {
        fetchChapterById(chapterId).then(s => {
            setChapter(s);
            setChapterText(s.chapterText || '');
            setChapterSummary(s.summary || '');
            setToDo(s.toDo || '');
            setChapterTitle(s.chapterTitle || '');
            setPovCharacterName(s.povCharacterName || '');
            setWordCount(s.wordCount || 0);
        });
    }, [chapterId]);

    useEffect(() => {
        setFoldChapter(foldAll);
    }, [foldAll]);

    const handleSave = useCallback(async () => {
        if (!chapter) return;
        const updated = {
            ...chapter,
            chapterText,
            chaptersummary,
            toDo,
            chapterTitle,
            povCharacterName,
            wordCount: chapterText.replace(/<[^>]+>/g, '').split(/\s+/).filter(Boolean).length,
        };
        try {
            await updateChapter(chapterId, updated);
            onSave(updated);
        } catch (err) {
            console.error('❌ Save failed:', err);
        }
    }, [chapterText, chaptersummary, toDo, chapterTitle, povCharacterName, chapter, chapterId, onSave]);

    useEffect(() => {
        const saveOnBlur = () => handleSave();
        window.addEventListener('beforeunload', saveOnBlur);
        return () => window.removeEventListener('beforeunload', saveOnBlur);
    }, [handleSave]);

    if (!chapter) return null;

    return (
        <div className="chapter-editor">
            <div className="editor-header">
                <button onClick={() => setFoldChapter(!foldChapter)}>{foldChapter ? '▼' : '▲'}</button>

                <select value={chapter.id} onChange={() => { }}>
                    <option>{chapter.chapterName}</option>
                </select>

                <input
                    className="chapter-title-input"
                    placeholder="Chapter Title"
                    value={chapterTitle}
                    onChange={e => setChapterTitle(e.target.value)}
                    onBlur={handleSave}
                />

                <select
                    className="chapter-title-input"
                    value={povCharacterName}
                    onChange={(e) => {
                        setPovCharacterName(e.target.value);
                        handleSave();
                    }}
                >
                    <option value="">Select POV</option>
                    {characters.map(char => (
                        <option key={char.id} value={char.name}>{char.name}</option>
                    ))}
                </select>

                <button onClick={() => setShowContext(prev => !prev)}>📘 Context</button>
                <button onClick={() => alert('Scene creation coming soon')}>➕ Add Scene</button>
            </div>

            {showContext && (
                <div className="context-panel">
                    <div><strong>PlotPoint:</strong> (Coming soon)</div>
                    {/* Add connected entity rendering here later */}
                </div>
            )}

            {!foldChapter && (
                <div className="editor-body" style={{ gridTemplateColumns: '2fr 1fr 1fr' }}>
                    <div className="rich-text expand full-height">
                        <TiptapEditorWithToolbar
                            content={chapterText}
                            placeholder="Write your chapter here..."
                            onUpdate={(html) => setChapterText(html)}
                        />
                        <button className="save-button" onClick={handleSave}>💾 Save</button>
                    </div>

                    <div className="rich-text scrollable full-height">
                        <div className="summary-header">
                            <span>ChapterSummary</span>
                            <button onClick={() => setFoldChapterSummary(!foldChapterSummary)}>{foldChapterSummary ? '⬆' : '⬇'}</button>
                        </div>
                        <TiptapEditorWithToolbar
                            content={chaptersummary}
                            placeholder="ChapterSummary..."
                            onUpdate={(html) => setChapterSummary(html)}
                        />
                        <button className="save-button" onClick={handleSave}>💾 Save</button>
                    </div>

                    <div className="rich-text scrollable full-height">
                        <div className="todo-header">
                            <span>ToDo List</span>
                            <button onClick={() => setFoldTodo(!foldTodo)}>{foldTodo ? '⬆' : '⬇'}</button>
                        </div>
                        <div>Word Count: {wordCount}</div>
                        <TiptapEditorWithToolbar
                            content={toDo}
                            placeholder="Things to fix or improve..."
                            onUpdate={(html) => setToDo(html)}
                        />
                        <button className="save-button" onClick={handleSave}>💾 Save</button>
                    </div>
                </div>
            )}
        </div>
    );
}
