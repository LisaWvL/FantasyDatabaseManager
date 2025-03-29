/* eslint-disable no-unused-vars */
//TODO this component displays plotpoints and snapshots in a shared timeline, while plotpoints visualize the timeline of the story in the book, snapshots reference which act or whapter of the book this happens in
//TODO the component should be named TimelineViewer
//TODO the component should be a functional component
//TODO the component should be created in the components folder
//TODO the component should be exported as the default export
//TODO the component should import React
//TODO the component should import the useState hook from React
//TODO the component should import the useEffect hook from React
//TODO the component should import the Timeline model
//TODO the component should import the PlotPoint model
//TODO the component should import the Snapshot model
//TODO the component should have a state variable called timeline, setTimeline
//TODO the component should have a state variable called plotpoints, setPlotpoints
//TODO the component should have a state variable called snapshots, setSnapshots
//TODO the component should have a state variable called selectedPlotPoint, setSelectedPlotPoint
//TODO the component should have a state variable called selectedSnapshot, setSelectedSnapshot
//TODO the component should have a state variable called isEditing, setIsEditing
//TODO the component should have a state variable called isAdding, setIsAdding
//TODO the component should have a state variable called isDeleting, setIsDeleting
//TODO the component should have a state variable called isConfirming, setIsConfirming
//TODO the component should have a state variable called isCancelling, setIsCancelling
//TODO the component should have a state variable called isSaving, setIsSaving
//TODO the component should have a useEffect hook that will run once on mount
//TODO the useEffect hook will fetch all Timeline entities from the database
//TODO the useEffect hook will fetch all PlotPoint entities from the database
//TODO the useEffect hook will fetch all Snapshot entities from the database
//TODO the useEffect hook will set the timeline, plotpoints, and snapshots state variables
//TODO the component should have a handleClick function that takes a plotpoint and snapshot as arguments
//TODO the handleClick function will set the selectedPlotPoint and selectedSnapshot state variables
//TODO the component should have a handleEdit function that takes a plotpoint and snapshot as arguments
//TODO the handleEdit function will set the selectedPlotPoint and selectedSnapshot state variables
//TODO the handleEdit function will set the isEditing state variable to true
//TODO the component should have a handleAdd function that takes no arguments
//TODO the handleAdd function will set the isAdding state variable to true
//TODO the component should have a handleDelete function that takes a plotpoint and snapshot as arguments
//TODO the handleDelete function will set the selectedPlotPoint and selectedSnapshot state variables
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
//TODO the div element with the class "timeline" should contain a div element with the class "snapshots"
//TODO the div element with the class "plotpoints" should contain a div element with the class "plotpoint"
//TODO the div element with the class "snapshots" should contain a div element with the class "snapshot"
//TODO the div element with the class "plotpoint" should display the plotpoint title
//TODO the div element with the class "snapshot" should display the snapshot version number
//TODO the div element with the class "plotpoint" should have an onClick event that calls the handleClick function
//TODO the div element with the class "plotpoint" should have an onDoubleClick event that calls the handleEdit function
//TODO the div element with the class "plotpoint" should have a right-click context menu event that calls the handleDelete function
//TODO the div element with the class "snapshot" should have an onClick event that calls the handleClick function
//TODO the div element with the class "snapshot" should have an onDoubleClick event that calls the handleEdit function
//TODO the div element with the class "snapshot" should have a right-click context menu event that calls the handleDelete function
//TODO the component should be styled with CSS
//TODO the component should display the plotpoints and snapshots in a shared timeline view
//TODO the component should allow users to interact with the plotpoints and snapshots
//TODO the component should be used to visualize the timeline of the story in the worldbuilder application
//TODO the component should be used to navigate and explore the plotpoints and snapshots of the story
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
import { fetchSnapshots } from '../api/SnapshotApi';
import "../styles/TimelineViewer.css";

export default function TimelineViewer() {
    const [plotpoints, setPlotpoints] = useState([]);
    const [snapshots, setSnapshots] = useState([]);
    const [selectedPlotPoint, setSelectedPlotPoint] = useState(null);
    const [selectedSnapshot, setSelectedSnapshot] = useState(null);
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
                fetchSnapshots()
            ]);
            setPlotpoints(pp);
            setSnapshots(ss);
        } catch (err) {
            console.error("❌ Failed to fetch timeline data", err);
        }
    };

    const handleClick = (plotpoint, snapshot) => {
        setSelectedPlotPoint(plotpoint);
        setSelectedSnapshot(snapshot);
    };

    const handleEdit = (plotpoint, snapshot) => {
        setSelectedPlotPoint(plotpoint);
        setSelectedSnapshot(snapshot);
        setIsEditing(true);
        alert("📝 Edit mode triggered. [Not yet implemented]");
    };

    const handleAdd = () => {
        setIsAdding(true);
        alert("➕ Add mode triggered. [Not yet implemented]");
    };

    const handleDelete = (plotpoint, snapshot) => {
        setSelectedPlotPoint(plotpoint);
        setSelectedSnapshot(snapshot);
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
                            onClick={() => handleClick(pp, selectedSnapshot)}
                            onDoubleClick={() => handleEdit(pp, selectedSnapshot)}
                            onContextMenu={(e) => {
                                e.preventDefault();
                                handleDelete(pp, selectedSnapshot);
                            }}
                        >
                            {pp.title} ({pp.startDay} {pp.startMonth} {pp.startYear})
                        </div>
                    ))}
                </div>
                <div className="snapshots-column">
                    <h3>Snapshots</h3>
                    {snapshots.map(snap => (
                        <div
                            key={snap.id}
                            className="snapshot"
                            onClick={() => handleClick(selectedPlotPoint, snap)}
                            onDoubleClick={() => handleEdit(selectedPlotPoint, snap)}
                            onContextMenu={(e) => {
                                e.preventDefault();
                                handleDelete(selectedPlotPoint, snap);
                            }}
                        >
                            📖 {snap.snapshotName} — Book {snap.book}, Act {snap.act}, Chapter {snap.chapter}
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}
