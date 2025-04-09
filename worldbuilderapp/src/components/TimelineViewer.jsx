/* eslint-disable no-unused-vars */
//TODO this component displays plotpoints and chapters in a shared timeline, while plotpoints visualize the timeline of the story in the book, chapters reference which act or whapter of the book this happens in
//TODO the component should be named TimelineViewer
//TODO the component should be a functional component
//TODO the component should be created in the components folder
//TODO the component should be exported as the default export
//TODO the component should import React
//TODO the component should import the useState hook from React
//TODO the component should import the useEffect hook from React
//TODO the component should import the Timeline model
//TODO the component should import the PlotPoint model
//TODO the component should import the Chapter model
//TODO the component should have a state variable called timeline, setTimeline
//TODO the component should have a state variable called plotpoints, setPlotpoints
//TODO the component should have a state variable called chapters, setChapters
//TODO the component should have a state variable called selectedPlotPoint, setSelectedPlotPoint
//TODO the component should have a state variable called selectedChapter, setSelectedChapter
//TODO the component should have a state variable called isEditing, setIsEditing
//TODO the component should have a state variable called isAdding, setIsAdding
//TODO the component should have a state variable called isDeleting, setIsDeleting
//TODO the component should have a state variable called isConfirming, setIsConfirming
//TODO the component should have a state variable called isCancelling, setIsCancelling
//TODO the component should have a state variable called isSaving, setIsSaving
//TODO the component should have a useEffect hook that will run once on mount
//TODO the useEffect hook will fetch all Timeline entities from the database
//TODO the useEffect hook will fetch all PlotPoint entities from the database
//TODO the useEffect hook will fetch all Chapter entities from the database
//TODO the useEffect hook will set the timeline, plotpoints, and chapters state variables
//TODO the component should have a handleClick function that takes a plotpoint and chapter as arguments
//TODO the handleClick function will set the selectedPlotPoint and selectedChapter state variables
//TODO the component should have a handleEdit function that takes a plotpoint and chapter as arguments
//TODO the handleEdit function will set the selectedPlotPoint and selectedChapter state variables
//TODO the handleEdit function will set the isEditing state variable to true
//TODO the component should have a handleAdd function that takes no arguments
//TODO the handleAdd function will set the isAdding state variable to true
//TODO the component should have a handleDelete function that takes a plotpoint and chapter as arguments
//TODO the handleDelete function will set the selectedPlotPoint and selectedChapter state variables
//TODO the handleDelete function will set the isDeleting state variable to true
//TODO the component should have a handleConfirm function that takes no arguments
//TODO the handleConfirm function will set the isConfirming state variable to true
//TODO the component should have a handleCancel function that takes no arguments
//TODO the handleCancel function will set the isCancelling state variable to true
//TODO the component should have a handleSave function that takes no arguments
//TODO the handleSave function will set the isSaving state variable to true
//TODO the component should return the JSX code
//TODO the component should render a div element with the class "timeline-viewer"
//TODO the div element should contain a div element with the class "timeline"
//TODO the div element with the class "timeline" should contain a div element with the class "plotpoints"
//TODO the div element with the class "timeline" should contain a div element with the class "chapters"
//TODO the div element with the class "plotpoints" should contain a div element with the class "plotpoint"
//TODO the div element with the class "chapters" should contain a div element with the class "chapter"
//TODO the div element with the class "plotpoint" should display the plotpoint title
//TODO the div element with the class "chapter" should display the chapter version number
//TODO the div element with the class "plotpoint" should have an onClick event that calls the handleClick function
//TODO the div element with the class "plotpoint" should have an onDoubleClick event that calls the handleEdit function
//TODO the div element with the class "plotpoint" should have a right-click context menu event that calls the handleDelete function
//TODO the div element with the class "chapter" should have an onClick event that calls the handleClick function
//TODO the div element with the class "chapter" should have an onDoubleClick event that calls the handleEdit function
//TODO the div element with the class "chapter" should have a right-click context menu event that calls the handleDelete function
//TODO the component should be styled with CSS
//TODO the component should display the plotpoints and chapters in a shared timeline view
//TODO the component should allow users to interact with the plotpoints and chapters
//TODO the component should be used to visualize the timeline of the story in the worldbuilder application
//TODO the component should be used to navigate and explore the plotpoints and chapters of the story
//TODO the component should be used to track changes and updates to the story over time
//TODO the component should be used to compare different versions of the story in the worldbuilder application
//TODO the component should be used to identify and resolve conflicts between different versions of the story
//TODO the component should be imported in the App.js file
//TODO the component should be rendered in the main content area of the application
//
// src/components/TimelineViewer.jsx
// src/components/TimelineViewer.jsx
// src/components/TimelineViewer.jsx
import React, { useEffect, useState } from "react";
import { fetchPlotPoints } from '../api/PlotPointApi';
import { fetchChapters } from '../api/ChapterApi';
import "../styles/TimelineViewer.css";

export default function TimelineViewer() {
    const [plotpoints, setPlotpoints] = useState([]);
    const [chapters, setChapters] = useState([]);
    const [selectedPlotPoint, setSelectedPlotPoint] = useState(null);
    const [selectedChapter, setSelectedChapter] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [isAdding, setIsAdding] = useState(false);
    const [isDeleting, setIsDeleting] = useState(false);
    const [isConfirming, setIsConfirming] = useState(false);
    const [isCancelling, setIsCancelling] = useState(false);
    const [isSaving, setIsSaving] = useState(false);

    useEffect(() => {
        loadData();
    }, []);

    const loadData = async () => {
        try {
            const [pp, ss] = await Promise.all([
                fetchPlotPoints(),
                fetchChapters()
            ]);
            setPlotpoints(pp);
            setChapters(ss);
        } catch (err) {
            console.error("❌ Failed to fetch timeline data", err);
        }
    };

    const handleClick = (plotpoint, chapter) => {
        setSelectedPlotPoint(plotpoint);
        setSelectedChapter(chapter);
    };

    const handleEdit = (plotpoint, chapter) => {
        setSelectedPlotPoint(plotpoint);
        setSelectedChapter(chapter);
        setIsEditing(true);
        alert("📝 Edit mode triggered. [Not yet implemented]");
    };

    const handleAdd = () => {
        setIsAdding(true);
        alert("➕ Add mode triggered. [Not yet implemented]");
    };

    const handleDelete = (plotpoint, chapter) => {
        setSelectedPlotPoint(plotpoint);
        setSelectedChapter(chapter);
        setIsDeleting(true);
        alert("❌ Delete triggered. [Not yet implemented]");
    };

    const handleConfirm = () => {
        setIsConfirming(true);
        alert("✅ Confirm action. [Not yet implemented]");
    };

    const handleCancel = () => {
        setIsCancelling(true);
        alert("🚫 Cancel action. [Not yet implemented]");
    };

    const handleSave = () => {
        setIsSaving(true);
        alert("💾 Save action. [Not yet implemented]");
    };

    return (
        <div className="timeline-viewer">
            <div className="timeline-header">
                <h2>📅 Story Timeline Viewer</h2>
                <button onClick={handleAdd}>+ Add PlotPoint</button>
            </div>
            <div className="timeline-content">
                <div className="plotpoints-column">
                    <h3>PlotPoints</h3>
                    {plotpoints.map(pp => (
                        <div
                            key={pp.id}
                            className="plotpoint"
                            onClick={() => handleClick(pp, selectedChapter)}
                            onDoubleClick={() => handleEdit(pp, selectedChapter)}
                            onContextMenu={(e) => {
                                e.preventDefault();
                                handleDelete(pp, selectedChapter);
                            }}
                        >
                            {pp.title} ({pp.startDay} {pp.startMonth} {pp.startYear})
                        </div>
                    ))}
                </div>
                <div className="chapters-column">
                    <h3>Chapters</h3>
                    {chapters.map(snap => (
                        <div
                            key={snap.id}
                            className="chapter"
                            onClick={() => handleClick(selectedPlotPoint, snap)}
                            onDoubleClick={() => handleEdit(selectedPlotPoint, snap)}
                            onContextMenu={(e) => {
                                e.preventDefault();
                                handleDelete(selectedPlotPoint, snap);
                            }}
                        >
                            📖 {snap.chapterName} — Book {snap.book}, Act {snap.act}, Chapter {snap.chapter}
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}
