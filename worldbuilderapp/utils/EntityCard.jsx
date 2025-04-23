import { useState, useEffect, useRef } from 'react';
import TooltipLink from './TooltipLink';
import { EntityFetcher } from './EntityManager'
import './EntityCard.css';

export default function EntityCard({
  entity,
  entityType,
  schema,
  onUpdate,
  onDelete,
  onCreateNewVersion,
  draggable = true,
  onDragStart,
  onDragEnd, // ✅ Pass from parent (EntityPage)
}) {
  const [isEditMode, setIsEditMode] = useState(entity.isNew || entity.isEditMode || false);
  const [localEntity, setLocalEntity] = useState({ ...entity });
  const [dropdownData, setDropdownData] = useState({});
  const [showDetails, setShowDetails] = useState(false);
  const [showSummary, setShowSummary] = useState(false);
  const isFirstRender = useRef(true);

  useEffect(() => {
    async function loadDropdowns() {
      const fkFields = schema.fields.filter((f) => f.type === 'fk' || f.type === 'multiFk');
      const result = {};
      for (const field of fkFields) {
        try {
          const options = await EntityFetcher.fetchAll(field.fkType);
          result[field.key] = options;
        } catch (err) {
          console.error(`❌ Failed to load options for ${field.fkType}`, err);
        }
      }
      setDropdownData(result);
    }
    loadDropdowns();
  }, [schema]);

  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === 'Escape') {
        setLocalEntity({ ...entity });
        setIsEditMode(false);
      }
    };
    if (isEditMode) {
      window.addEventListener('keydown', handleKeyDown);
    }
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isEditMode, entity]);

  const handleBlur = () => {
    if (isEditMode && !isFirstRender.current) {
      onUpdate(localEntity);
      setIsEditMode(false);
    }
    isFirstRender.current = false;
  };

  const handleChange = (field, value) => {
    setLocalEntity((prev) => ({ ...prev, [field.key]: value }));
  };

  const renderField = (field) => {
    const value = localEntity[field.key];
    const skipInHeader = ['name', 'alias'];

    if (!isEditMode && field.section === 'header' && skipInHeader.includes(field.key)) return null;

    if (isEditMode) {
      if (field.type === 'fk') {
        const options = dropdownData[field.key] || [];
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <select
              value={value || ''}
              onChange={(e) => handleChange(field, parseInt(e.target.value))}
              onBlur={handleBlur}
            >
              <option value="">Select...</option>
              {options.map((opt) => (
                <option key={opt.id} value={opt.id}>
                  {opt.name || opt.title || opt.alias || `#${opt.id}`}
                </option>
              ))}
            </select>
          </div>
        );
      } else if (field.type === 'multiFk') {
        const options = dropdownData[field.key] || [];
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <select
              multiple
              value={value || []}
              onChange={(e) =>
                handleChange(
                  field,
                  Array.from(e.target.selectedOptions, (o) => parseInt(o.value))
                )
              }
              onBlur={handleBlur}
            >
              {options.map((opt) => (
                <option key={opt.id} value={opt.id}>
                  {opt.name || opt.title || opt.alias || `#${opt.id}`}
                </option>
              ))}
            </select>
          </div>
        );
      } else {
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <textarea
              className="edit-field"
              value={value || ''}
              onChange={(e) => handleChange(field, e.target.value)}
              onBlur={handleBlur}
            />
          </div>
        );
      }
    } else {
      if (field.type === 'fk') {
        return (
          <div className="attribute-box" key={field.key}>
            <TooltipLink
              entityType={field.fkType}
              entityId={value}
              displayValue={entity[`${field.key.replace(/Id$/, '')}Name`] || `(${field.label})`}
            />
          </div>
        );
      } else if (field.type === 'multiFk') {
        const items = value || [];
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <div className="multi-fk-list">
              {items.map((id) => (
                <div key={id}>
                  <TooltipLink entityType={field.fkType} entityId={id} displayValue={id} />
                </div>
              ))}
            </div>
          </div>
        );
      } else {
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <span className="value">{value}</span>
          </div>
        );
      }
    }
  };

  const renderSection = (sectionName) => {
    const fields = schema.fields.filter((f) => f.section === sectionName);
    if (fields.length === 0) return null;

    return (
      <div className={`card-section ${sectionName}`}>
        {sectionName !== 'header' && <h5 className="section-title">{sectionName.toUpperCase()}</h5>}
        {fields.map((field) => renderField(field))}
      </div>
    );
  };

  return (
    <div
      className={`entity-card ${entityType.toLowerCase()}-card`}
      draggable={draggable}
      onDragStart={(e) => onDragStart?.(e)}
      onDragEnd={(e) => onDragEnd?.(e)} // ✅ This now works
    >
      <div className="card-header fancy-header">
        <div className="card-actions">
          <button onClick={() => onCreateNewVersion?.(entity)}>➕</button>
          <button onClick={() => setIsEditMode(true)}>✏️</button>
          <button onClick={() => onDelete?.(entity)}>🗑️</button>
        </div>
        <h3>{schema.primaryDisplay(localEntity)}</h3>
        {schema.subDisplay && <h4>{schema.subDisplay(localEntity)}</h4>}
      </div>

      {renderSection('header')}
      {renderSection('relation')}

      {showSummary && renderSection('summary')}
      {schema.fields.some((f) => f.section === 'summary') && (
        <button className="details-toggle" onClick={() => setShowSummary((prev) => !prev)}>
          {showSummary ? '▲ Summary' : '▼ Summary'}
        </button>
      )}

      {showDetails && renderSection('details')}
      {schema.fields.some((f) => f.section === 'details') && (
        <button className="details-toggle" onClick={() => setShowDetails((prev) => !prev)}>
          {showDetails ? '▲ Details' : '▼ Details'}
        </button>
      )}
    </div>
  );
}



//// ✅ ChapterEditor.jsx

//import { useEffect, useState, useCallback } from 'react';
//import TiptapEditorWithToolbar from './TiptapEditorWithToolbar';
//import { updateChapter } from './ChapterApi';
//import './ChapterEditor.css';
//import SmallEntityCard from '../entities/SmallEntityCard';

//export default function ChapterEditor({ full, onSave, onDropToChapter }) {
//    const { chapter, entities = [] } = full;

//    const [chapterText, setChapterText] = useState('');
//    const [chaptersummary, setChapterSummary] = useState('');
//    const [toDo, setToDo] = useState('');
//    const [chapterTitle, setChapterTitle] = useState('');
//    const [povCharacterName, setPovCharacterName] = useState('');
//    const [wordCount, setWordCount] = useState(0);
//    const [foldChapter, setFoldChapter] = useState(false);
//    const [foldChapterSummary, setFoldChapterSummary] = useState(false);
//    const [foldDropZone, setFoldDropZone] = useState(false);
//    const [foldTodo, setFoldTodo] = useState(false);
//    const [isPOVHovering, setIsPOVHovering] = useState(false);
//    const [isContextHovering, setIsContextHovering] = useState(false);

//    useEffect(() => {
//        setChapterText(chapter.chapterText || '');
//        setChapterSummary(chapter.summary || '');
//        setToDo(chapter.toDo || '');
//        setChapterTitle(chapter.chapterTitle || '');
//        setPovCharacterName(chapter.povCharacterName || '');
//        setWordCount(chapter.wordCount || 0);
//    }, [chapter]);

//    const handleSave = useCallback(async () => {
//        const updated = {
//            ...chapter,
//            chapterText,
//            summary: chaptersummary,
//            toDo,
//            chapterTitle,
//            povCharacterName,
//            wordCount: chapterText.replace(/<[^>]+>/g, '').split(/\s+/).filter(Boolean).length,
//        };
//        try {
//            await updateChapter(chapter.id, updated);
//            onSave(updated);
//        } catch (err) {
//            console.error('❌ Save failed:', err);
//        }
//    }, [chapter, chapterText, chaptersummary, toDo, chapterTitle, povCharacterName, onSave]);

//    useEffect(() => {
//        const saveOnBlur = () => handleSave();
//        window.addEventListener('beforeunload', saveOnBlur);
//        return () => window.removeEventListener('beforeunload', saveOnBlur);
//    }, [handleSave]);

//    const gridColumns = [!foldChapterSummary, !foldTodo].filter(Boolean).length === 0
//        ? '1fr'
//        : [!foldChapterSummary, !foldTodo].filter(Boolean).length === 1
//            ? '2fr 1fr'
//            : '2fr 1fr 1fr';

//    const handleContextMenu = () => { };
//    const handleDragStart = () => { };
//    const handleDragEnd = () => { };

//    return (
//        <div
//            className="chapter-editor"
//            onDragOver={(e) => e.preventDefault()}
//        >
//            <div className="editor-header">
//                <button className="fold-in-button" onClick={() => setFoldChapter(!foldChapter)}>
//                    {foldChapter ? '▼' : '▲'}
//                </button>

//                <input
//                    className="chapter-title-input"
//                    placeholder="Chapter Title"
//                    value={chapterTitle}
//                    onChange={(e) => setChapterTitle(e.target.value)}
//                    onBlur={handleSave}
//                />

//                <div
//                    className={`pov-dropzone ${isPOVHovering ? 'drag-hover' : ''}`}
//                    onDragOver={(e) => {
//                        e.preventDefault();
//                        setIsPOVHovering(true);
//                    }}
//                    onDragLeave={() => setIsPOVHovering(false)}
//                    onDrop={(e) => {
//                        e.preventDefault();
//                        setIsPOVHovering(false);
//                        const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);
//                        const entityType = e.dataTransfer.getData('entityType');
//                        if (entityType === 'Character') {
//                            onDropToChapter(entityId, entityType, chapter.id, true); // isPOV = true
//                        }
//                    }}
//                >
//                    <div className="pov-label">POV Character</div>
//                    {povCharacterName ? (
//                        <SmallEntityCard
//                            entity={{ name: povCharacterName }}
//                            entityType="Character"
//                            onContextMenu={handleContextMenu}
//                            onDragStart={handleDragStart}
//                            onDragEnd={handleDragEnd}
//                            draggable
//                        />
//                    ) : (
//                        <em className="dropzone-placeholder">Drop a Character here</em>
//                    )}
//                </div>

//                <div className="folded-panel-buttons">
//                    {foldChapterSummary && <button onClick={() => setFoldChapterSummary(false)}>Summary</button>}
//                    {foldTodo && <button onClick={() => setFoldTodo(false)}>ToDo</button>}
//                    {foldDropZone && <button onClick={() => setFoldDropZone(false)}>Context</button>}
//                </div>
//                <button className="add-scene-button" onClick={() => alert('Scene creation coming soon')}>+ Scene</button>
//            </div>

//            {!foldDropZone && (
//                <div
//                    className={`context-dropzone ${isContextHovering ? 'drag-hover' : ''}`}
//                    onDragOver={(e) => {
//                        e.preventDefault();
//                        setIsContextHovering(true);
//                    }}
//                    onDragLeave={() => setIsContextHovering(false)}
//                    onDrop={(e) => {
//                        e.preventDefault();
//                        setIsContextHovering(false);
//                        const entityId = parseInt(e.dataTransfer.getData('entityId'), 10);
//                        const entityType = e.dataTransfer.getData('entityType');
//                        if (entityId && entityType) {
//                            onDropToChapter(entityId, entityType, chapter.id);
//                        }
//                    }}
//                >
//                    <div className="dropzone-header">
//                        <div className="dropzone-title">Connected Entities</div>
//                        <button className="collapse-button" onClick={() => setFoldDropZone(true)}>▲</button>
//                    </div>

//                    {entities.length > 0 ? (
//                        entities.map((entity) => (
//                            <SmallEntityCard
//                                key={`${entity.entityType?.toLowerCase?.()}-${entity.id}`}
//                                entity={entity}
//                                entityType={entity.entityType}
//                                onContextMenu={handleContextMenu}
//                                onDragStart={handleDragStart}
//                                onDragEnd={handleDragEnd}
//                                draggable
//                            />
//                        ))
//                    ) : (
//                        <em className="dropzone-placeholder">Drop any entity here</em>
//                    )}
//                </div>
//            )}

//            {!foldChapter && (
//                <div className="editor-body" style={{ gridTemplateColumns: gridColumns }}>
//                    <div className="rich-text expand full-height">
//                        <TiptapEditorWithToolbar
//                            content={chapterText}
//                            placeholder="Write your chapter here..."
//                            onUpdate={(html) => setChapterText(html)}
//                        />
//                        <button className="save-button" onClick={handleSave}>💾 Save</button>
//                    </div>

//                    {!foldChapterSummary && (
//                        <div className="rich-text scrollable full-height">
//                            <div className="summary-header">
//                                <span>Chapter Summary</span>
//                                <button onClick={() => setFoldChapterSummary(true)}>▲</button>
//                            </div>
//                            <TiptapEditorWithToolbar
//                                content={chaptersummary}
//                                placeholder="Chapter summary..."
//                                onUpdate={(html) => setChapterSummary(html)}
//                            />
//                            <button className="save-button" onClick={handleSave}>💾 Save</button>
//                        </div>
//                    )}

//                    {!foldTodo && (
//                        <div className="rich-text scrollable full-height">
//                            <div className="todo-header">
//                                <span>To-Do List</span>
//                                <button onClick={() => setFoldTodo(true)}>▲</button>
//                            </div>
//                            <div>Word Count: {wordCount}</div>
//                            <TiptapEditorWithToolbar
//                                content={toDo}
//                                placeholder="Things to fix or improve..."
//                                onUpdate={(html) => setToDo(html)}
//                            />
//                            <button className="save-button" onClick={handleSave}>💾 Save</button>
//                        </div>
//                    )}
//                </div>
//            )}
//        </div>
//    );
//}
