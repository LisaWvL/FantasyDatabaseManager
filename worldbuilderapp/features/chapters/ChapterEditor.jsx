import { useEffect, useState, useCallback } from 'react';
import TiptapEditorWithToolbar from './TiptapEditorWithToolbar';
import { updateChapter } from './ChapterApi';
import './ChapterEditor.css';
import SmallEntityCard from '../../utils/SmallEntityCard';

export default function ChapterEditor({ full, onSave, onDropToChapter }) {
    const { chapter, entities = [] } = full;
    const [chapterText, setChapterText] = useState('');
    const [chaptersummary, setChapterSummary] = useState('');
    const [toDo, setToDo] = useState('');
    const [chapterTitle, setChapterTitle] = useState('');
    const [povCharacterName, setPovCharacterName] = useState('');
    const [wordCount, setWordCount] = useState(0);
    const [foldChapter, setFoldChapter] = useState(false);
    const [foldChapterSummary, setFoldChapterSummary] = useState(false);
    const [foldTodo, setFoldTodo] = useState(false);
    const [isPOVHovering, setIsPOVHovering] = useState(false);
    const [isContextHovering, setIsContextHovering] = useState(false);

    useEffect(() => {
        setChapterText(chapter.chapterText || '');
        setChapterSummary(chapter.summary || '');
        setToDo(chapter.toDo || '');
        setChapterTitle(chapter.chapterTitle || '');
        setPovCharacterName(chapter.povCharacterName || '');
        setWordCount(chapter.wordCount || 0);
    }, [chapter]);

    const handleSave = useCallback(async () => {
        const updated = {
            ...chapter,
            chapterText,
            summary: chaptersummary,
            toDo,
            chapterTitle,
            povCharacterName,
            wordCount: chapterText.replace(/<[^>]+>/g, '').split(/\s+/).filter(Boolean).length,
        };
        try {
            await updateChapter(chapter.id, updated);
            onSave(updated);
        } catch (err) {
            console.error('❌ Save failed:', err);
        }
    }, [chapter, chapterText, chaptersummary, toDo, chapterTitle, povCharacterName, onSave]);

    useEffect(() => {
        const saveOnBlur = () => handleSave();
        window.addEventListener('beforeunload', saveOnBlur);
        return () => window.removeEventListener('beforeunload', saveOnBlur);
    }, [handleSave]);

    const gridColumns = [!foldChapterSummary, !foldTodo].filter(Boolean).length === 0
        ? '1fr'
        : [!foldChapterSummary, !foldTodo].filter(Boolean).length === 1
            ? '2fr 1fr'
            : '2fr 1fr 1fr';

    const handleContextMenu = () => { };
    const handleDragStart = () => { };
    const handleDragEnd = () => { };

    return (
        <div
            className="chapter-editor"
            onDragOver={(e) => e.preventDefault()}
            onDrop={(e) => {
                const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);
                const entityType = e.dataTransfer.getData('entityType');
                if (entityId && entityType && onDropToChapter) {
                    onDropToChapter(entityId, entityType, chapter.id);
                }
            }}
        >
            <div className="editor-header">
                <button className="fold-in-button" onClick={() => setFoldChapter(!foldChapter)}>
                    {foldChapter ? '▼' : '▲'}
                </button>

                <input
                    className="chapter-title-input"
                    placeholder="Chapter Title"
                    value={chapterTitle}
                    onChange={(e) => setChapterTitle(e.target.value)}
                    onBlur={handleSave}
                />

                <div
                    className={`pov-dropzone-panel ${isPOVHovering ? 'drag-hover' : ''}`}
                    onDragOver={(e) => {
                        e.preventDefault();
                        setIsPOVHovering(true);
                    }}
                    onDragLeave={() => setIsPOVHovering(false)}
                    onDrop={(e) => {
                        e.preventDefault();
                        setIsPOVHovering(false);
                        const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);
                        const entityType = e.dataTransfer.getData('entityType');
                        if (entityType === 'Character') {
                            onDropToChapter(entityId, entityType, chapter.id, true); // isPOV = true
                        }
                    }}
                >
                    <div className="dropzone-title">POV Character</div>
                    {povCharacterName ? (
                        <SmallEntityCard
                            entity={{ name: povCharacterName }}
                            entityType="Character"
                            onContextMenu={handleContextMenu}
                            onDragStart={handleDragStart}
                            onDragEnd={handleDragEnd}
                            draggable
                        />
                    ) : (
                        <em className="dropzone-placeholder">Drop a Character here</em>
                    )}
                </div>

                {foldChapterSummary && <button onClick={() => setFoldChapterSummary(false)}>Summary</button>}
                {foldTodo && <button onClick={() => setFoldTodo(false)}>ToDo</button>}
                <button className="add-scene-button" onClick={() => alert('Scene creation coming soon')}>+ Scene</button>
            </div>

            <div
                className={`context-dropzone ${isContextHovering ? 'drag-hover' : ''}`}
                onDragOver={(e) => {
                    e.preventDefault();
                    setIsContextHovering(true);
                }}
                onDragLeave={() => setIsContextHovering(false)}
                onDrop={(e) => {
                    e.preventDefault();
                    setIsContextHovering(false);
                    const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);
                    const entityType = e.dataTransfer.getData('entityType');
                    if (entityId && entityType) {
                        onDropToChapter(entityId, entityType, chapter.id);
                    }
                }}
            >
                <div className="dropzone-title">Connected Entities</div>
                {entities.length > 0 ? (
                    entities.map((entity) => (
                        <SmallEntityCard
                            key={`${entity.entityType?.toLowerCase?.()}-${entity.id}`}
                            entity={entity}
                            entityType={entity.entityType}
                            onContextMenu={handleContextMenu}
                            onDragStart={handleDragStart}
                            onDragEnd={handleDragEnd}
                            draggable
                        />
                    ))
                ) : (
                    <em className="dropzone-placeholder">Drop any entity here</em>
                )}
            </div>

            {!foldChapter && (
                <div className="editor-body" style={{ gridTemplateColumns: gridColumns }}>
                    <div className="rich-text expand full-height">
                        <TiptapEditorWithToolbar
                            content={chapterText}
                            placeholder="Write your chapter here..."
                            onUpdate={(html) => setChapterText(html)}
                        />
                        <button className="save-button" onClick={handleSave}>💾 Save</button>
                    </div>

                    {!foldChapterSummary && (
                        <div className="rich-text scrollable full-height">
                            <div className="summary-header">
                                <span>Chapter Summary</span>
                                <button onClick={() => setFoldChapterSummary(true)}>⬆</button>
                            </div>
                            <TiptapEditorWithToolbar
                                content={chaptersummary}
                                placeholder="Chapter summary..."
                                onUpdate={(html) => setChapterSummary(html)}
                            />
                            <button className="save-button" onClick={handleSave}>💾 Save</button>
                        </div>
                    )}

                    {!foldTodo && (
                        <div className="rich-text scrollable full-height">
                            <div className="todo-header">
                                <span>To-Do List</span>
                                <button onClick={() => setFoldTodo(true)}>⬆</button>
                            </div>
                            <div>Word Count: {wordCount}</div>
                            <TiptapEditorWithToolbar
                                content={toDo}
                                placeholder="Things to fix or improve..."
                                onUpdate={(html) => setToDo(html)}
                            />
                            <button className="save-button" onClick={handleSave}>💾 Save</button>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
}