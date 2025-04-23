//import { useEffect, useState } from 'react';
//import EntityCard from './EntityCard';
//import { EntityFetcher, EntityUpdater, EntityDeleter } from './entityManager';
//import { entitySchemas } from './entitySchemas';
//import useDragScroll from '../../src/hooks/useDragScroll';
//import './EntityPage.css';


//export default function EntityPage({ entityType }) {
//  const schema = entitySchemas[entityType];
//  const [entities, setEntities] = useState([]);
//  const [chapters, setChapters] = useState([]);
//  const [acts, setActs] = useState([]);
//  const [books, setBooks] = useState([]);
//  const [filteredChapterId, setFilteredChapterId] = useState(null);
//  const [dragOverChapterId, setDragOverChapterId] = useState(null);
//  const [isDragging, setIsDragging] = useState(false);

//  useDragScroll(isDragging);

//  useEffect(() => {
//    const loadData = async () => {
//      try {
//        const [loadedBooks, loadedActs, loadedChapters, loadedEntities] = await Promise.all([
//          EntityFetcher.fetchAll('Book'),
//          EntityFetcher.fetchAll('Act'),
//          EntityFetcher.fetchAll('Chapter'),
//          EntityFetcher.fetchAll(entityType),
//        ]);
//        setBooks(loadedBooks);
//        setActs(loadedActs);
//        setChapters(loadedChapters);
//        setEntities(loadedEntities);
//      } catch (err) {
//        console.error(`❌ Failed to load ${entityType} data`, err);
//      }
//    };
//    loadData();
//  }, [entityType]);

//  const handleDragStart = () => setIsDragging(true);
//  const handleDragEnd = () => setIsDragging(false);

//  const handleUpdateEntity = async (updatedEntity) => {
//    try {
//      const saved = await EntityUpdater.update(entityType, updatedEntity.id, updatedEntity);
//      setEntities((prev) => prev.map((e) => (e.id === saved.id ? saved : e)));
//    } catch (err) {
//      console.error(`❌ Failed to update ${entityType}`, err);
//    }
//  };

//  const handleDeleteEntity = async (entity) => {
//    try {
//      await EntityDeleter.delete(entityType, entity.id);
//      setEntities((prev) => prev.filter((e) => e.id !== entity.id));
//    } catch (err) {
//      console.error(`❌ Failed to delete ${entityType}`, err);
//    }
//  };

//  const handleCreateNewVersion = (entity) => {
//    const editableKeys = schema.fields.filter((f) => f.key !== 'chapterId').map((f) => f.key);
//    const newEntity = {
//      ...editableKeys.reduce((obj, key) => {
//        obj[key] = entity[key];
//        return obj;
//      }, {}),
//      name: entity.name,
//      chapterId: entity.chapterId,
//      isNew: true,
//    };
//    setEntities((prev) => {
//      const index = prev.findIndex((e) => e.id === entity.id);
//      const updated = [...prev];
//      updated.splice(index + 1, 0, newEntity);
//      return updated;
//    });
//  };

//  const handleCreateEmptyEntity = () => {
//    const newEntity = {
//      name: `New ${entityType}`,
//      chapterId: null,
//      isNew: true,
//    };
//    setEntities((prev) => [newEntity, ...prev]);
//  };

//  const getChapterLabel = (chapter) => {
//    const act = acts.find((a) => a.id === chapter.actId);
//    const book = books.find((b) => b.id === act?.bookId);
//    return book && act
//      ? `Book ${book.bookNumber} – Act ${act.actNumber} – Chapter ${chapter.chapterNumber}: ${chapter.chapterTitle}`
//      : 'Unlinked Chapter';
//  };

//  const baseEntities = entities.filter(
//    (e) => !e.chapterId || e.chapterId === 0 || e.chapterId === ''
//  );

//  const handleDropToBase = (e) => {
//    e.preventDefault();
//    const entityId = parseInt(e.dataTransfer.getData(`${entityType.toLowerCase()}Id`));
//    const draggedEntity = entities.find((e) => e.id === entityId);
//    if (draggedEntity) {
//      const updatedEntity = {
//        ...draggedEntity,
//        chapterId: null,
//      };
//      handleUpdateEntity(updatedEntity);
//    }
//    setDragOverChapterId(null);
//  };

//  return (
//    <div className={`${entityType.toLowerCase()}-page`}>
//      <div className={`${entityType.toLowerCase()}-page-header`}>
//        <h2>{schema.labelPlural} Management</h2>
//        <div className={`${entityType.toLowerCase()}-controls`}>
//          <button onClick={handleCreateEmptyEntity}>➕ Add {schema.label}</button>
//          <select
//            value={filteredChapterId || ''}
//            onChange={(e) => setFilteredChapterId(e.target.value ? parseInt(e.target.value) : null)}
//          >
//            <option value="">Show All Chapters</option>
//            {chapters.map((chapter) => (
//              <option key={chapter.id} value={chapter.id}>
//                {getChapterLabel(chapter)}
//              </option>
//            ))}
//          </select>
//        </div>
//      </div>

//      {baseEntities.length > 0 && (
//        <div
//          className={`chapter-section ${dragOverChapterId === 0 ? 'drag-over' : ''}`}
//          onDragOver={(e) => {
//            e.preventDefault();
//            setDragOverChapterId(0);
//          }}
//          onDragLeave={(e) => {
//              if (e.relatedTarget && e.currentTarget && !e.currentTarget.contains(e.relatedTarget instanceof Node ? e.relatedTarget : null)) {
//              setDragOverChapterId(null);
//            }
//          }}
//          onDrop={handleDropToBase}
//        >
//          <div className="chapter-header">
//            <h3>Base {schema.labelPlural}</h3>
//          </div>
//          <div className={`${entityType.toLowerCase()}-card-row`}>
//            {baseEntities.map((entity) => (
//              <EntityCard
//                key={entity.id || `${entity.name}-new`}
//                entity={entity}
//                entityType={entityType}
//                schema={schema}
//                onCreateNewVersion={handleCreateNewVersion}
//                onUpdate={handleUpdateEntity}
//                onDelete={handleDeleteEntity}
//                draggable
//                onDragStart={(e) => {
//                  e.dataTransfer.setData(`${entityType.toLowerCase()}Id`, entity.id);
//                  handleDragStart();
//                }}
//                onDragEnd={handleDragEnd}
//              />
//            ))}
//          </div>
//        </div>
//      )}

//      {chapters
//        .filter((ch) => !filteredChapterId || ch.id === filteredChapterId)
//        .map((chapter) => {
//          const act = acts.find((a) => a.id === chapter.actId);
//          const book = books.find((b) => b.id === act?.bookId);
//          if (!book || !act) return null;
//          const chapterEntities = entities.filter((e) => e.chapterId === chapter.id);

//          return (
//            <div
//              key={chapter.id}
//              className={`chapter-section ${dragOverChapterId === chapter.id ? 'drag-over' : ''}`}
//              onDragOver={(e) => {
//                e.preventDefault();
//                setDragOverChapterId(chapter.id);
//              }}
//              onDragLeave={(e) => {
//                  if (e.relatedTarget && e.currentTarget && !e.currentTarget.contains(e.relatedTarget instanceof Node ? e.relatedTarget : null)) {
//                  setDragOverChapterId(null);
//                }
//              }}
//              onDrop={(e) => {
//                e.preventDefault();
//                const entityId = parseInt(e.dataTransfer.getData(`${entityType.toLowerCase()}Id`));
//                const draggedEntity = entities.find((e) => e.id === entityId);
//                if (draggedEntity) {
//                  const updatedEntity = {
//                    ...draggedEntity,
//                    chapterId: chapter.id,
//                    actId: act.id,
//                    bookId: book.id,
//                  };
//                  handleUpdateEntity(updatedEntity);
//                }
//                setDragOverChapterId(null);
//              }}
//            >
//              <div className="chapter-header">
//                <h3>
//                  Book: {book.bookNumber} – {book.bookTitle}
//                </h3>
//                <h3>
//                  Act: {act.actNumber} – {act.actTitle}
//                </h3>
//                <h3>
//                  Chapter: {chapter.chapterNumber} – {chapter.chapterTitle}
//                </h3>
//              </div>
//              <div className={`${entityType.toLowerCase()}-card-row`}>
//                {chapterEntities.map((entity) => (
//                  <EntityCard
//                    key={entity.id || `${entity.name}-new`}
//                    entity={entity}
//                    entityType={entityType}
//                    schema={schema}
//                    onCreateNewVersion={handleCreateNewVersion}
//                    onUpdate={handleUpdateEntity}
//                    onDelete={handleDeleteEntity}
//                    draggable
//                    onDragStart={(e) => {
//                      e.dataTransfer.setData(`${entityType.toLowerCase()}Id`, entity.id);
//                      handleDragStart();
//                    }}
//                    onDragEnd={handleDragEnd}
//                  />
//                ))}
//              </div>
//            </div>
//          );
//        })}
//    </div>
//  );
//}
