//TimelineStoryView.jsx in src/pages
//TODO this page shows my story on a timeline axis, with the main story being driven by Plotpoints and the writing progress by chapters that link to versions of entities
//the TimelineStoryView is two horizontal time scales, one for in story timeine (plotpoints) and synchronized below the book timeline
//there is a way to switch between showing chapter or plotpoints or both
//use TimelineStoryView component and TimelineStoryView.css
//add a link in the sidebare that brings the user to this page

//TimelineStoryView.jsx
import React, { useState } from 'react';
import TimeLineStoryView from './TimelineStoryView.jsx';
import TimelineStoryViewModal from './TimelineViewModal.jsx';
import './TimelineStoryView.css';

export default function TimelineStoryView() {
  const [_viewMode, setViewMode] = useState('both'); // Options: 'both', 'plotpoints', 'chapters'

  const handleViewToggle = (mode) => {
    setViewMode(mode);
  };

  return (
    <div className="timeline-story-view">
      <h2>📖 Story Timeline</h2>
      <div className="timeline-controls">
        <label>Show:</label>
        <button onClick={() => handleViewToggle('both')}>PlotPoints + Chapters</button>
        <button onClick={() => handleViewToggle('plotpoints')}>PlotPoints Only</button>
        <button onClick={() => handleViewToggle('chapters')}>Chapters Only</button>
      </div>
      {/*<TimelineStoryView viewMode={viewMode} />*/}
    </div>
  );
}


//import React, { useEffect, useState } from 'react';
//import { fetchPlotPoints } from '../plotpoints/PlotPointApi';
//import { fetchChapters } from '../chapters/ChapterApi';
//import './TimelineStoryView.css';


//export default function TimelineStoryView({ viewMode }) {
//    const [plotpoints, setPlotpoints] = useState([]);
//    const [chapters, setChapters] = useState([]);
//    const [selectedPlotPoint, setSelectedPlotPoint] = useState(null);
//    const [selectedChapter, setSelectedChapter] = useState(null);
//    const [isEditing, setIsEditing] = useState(false);
//    const [isAdding, setIsAdding] = useState(false);
//    const [isDeleting, setIsDeleting] = useState(false);
//    const [isConfirming, setIsConfirming] = useState(false);
//    const [isCancelling, setIsCancelling] = useState(false);
//    const [isSaving, setIsSaving] = useState(false);


//    useEffect(() => {
//        loadData();
//    }, []);

//    const loadData = async () => {
//        try {
//            const [pp, ss] = await Promise.all([fetchPlotPoints(), fetchChapters()]);
//            setPlotpoints(pp);
//            setChapters(ss);
//        } catch (err) {
//            console.error('❌ Failed to fetch timeline data', err);
//        }
//    };

//    const handleClick = (plotpoint, chapter) => {
//        setSelectedPlotPoint(plotpoint);
//        setSelectedChapter(chapter);
//    };

//    const handleEdit = (plotpoint, chapter) => {
//        setSelectedPlotPoint(plotpoint);
//        setSelectedChapter(chapter);
//        setIsEditing(true);
//        alert('📝 Edit mode triggered. [Not yet implemented]');
//    };

//    const handleAdd = () => {
//        setIsAdding(true);
//        alert('➕ Add mode triggered. [Not yet implemented]');
//    };

//    const handleDelete = (plotpoint, chapter) => {
//        setSelectedPlotPoint(plotpoint);
//        setSelectedChapter(chapter);
//        setIsDeleting(true);
//        alert('❌ Delete triggered. [Not yet implemented]');
//    };

//    const handleConfirm = () => {
//        setIsConfirming(true);
//        alert('✅ Confirm action. [Not yet implemented]');
//    };

//    const handleCancel = () => {
//        setIsCancelling(true);
//        alert('🚫 Cancel action. [Not yet implemented]');
//    };

//    const handleSave = () => {
//        setIsSaving(true);
//        alert('💾 Save action. [Not yet implemented]');
//    };

//    return (
//        <div className="timeline-view">
//            <div className="timeline-header">
//                <h3>Timeline View Mode: {viewMode}</h3>
//                <h2>📅 Story Timeline Viewer</h2>
//                <button onClick={handleAdd}>+ Add PlotPoint</button>
//            </div>
//            <div className="timeline-content">
//                <div className="plotpoints-column">
//                    <h3>PlotPoints</h3>
//                    {plotpoints.map((pp) => (
//                        <div
//                            key={pp.id}
//                            className="plotpoint"
//                            onClick={() => handleClick(pp, selectedChapter)}
//                            onDoubleClick={() => handleEdit(pp, selectedChapter)}
//                            onContextMenu={(e) => {
//                                e.preventDefault();
//                                handleDelete(pp, selectedChapter);
//                            }}
//                        >
//                            {pp.title} ({pp.startDay} {pp.startMonth} {pp.startYear})
//                        </div>
//                    ))}
//                </div>
//                <div className="chapters-column">
//                    <h3>Chapters</h3>
//                    {chapters.map((snap) => (
//                        <div
//                            key={snap.id}
//                            className="chapter"
//                            onClick={() => handleClick(selectedPlotPoint, snap)}
//                            onDoubleClick={() => handleEdit(selectedPlotPoint, snap)}
//                            onContextMenu={(e) => {
//                                e.preventDefault();
//                                handleDelete(selectedPlotPoint, snap);
//                            }}
//                        >
//                            📖 {snap.chapterName} — Book {snap.book}, Act {snap.act}, Chapter {snap.chapter}
//                        </div>
//                    ))}
//                </div>
//            </div>
//        </div>
//    );
//}
