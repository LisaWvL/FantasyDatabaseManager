// src/components/EntityTable.jsx
import React, { useState } from 'react';
import { FaPlus, FaTrash, FaCopy, FaExpand, FaBook } from 'react-icons/fa';
import EditableTableRow from './EditableTableRow';

export default function EntityTable({
    data,
    onCreate,
    onEdit,
    onDelete,
    onCopy,
    onChapter, // ✅ now accepted
    referenceData = {
        characters: [],
        chapters: [],
    },
}) {
    const [selectedIds, _setSelectedIds] = useState([]);
    const [showRelated, setShowRelated] = useState(false);
    const [showAllChapters, setShowAllChapters] = useState(false);

    if (!data || data.length === 0) return <p>No data available.</p>;

    const columns = Object.keys(data[0]).filter(
        (key) => key !== 'id' && !key.toLowerCase().endsWith('id')
    );

    const handleSave = (updatedRow) => {
        onEdit(updatedRow);
    };

    return (
        <div className="entity-table-container">
            <div className="table-controls">
                <button className="control-button" onClick={onCreate} title="Create New">
                    <FaPlus />
                </button>
                <button
                    className="control-button"
                    onClick={() => selectedIds.forEach(onDelete)}
                    title="Delete Selected"
                >
                    <FaTrash />
                </button>
                <button
                    className="control-button"
                    onClick={() => selectedIds.forEach(onCopy)}
                    title="Copy Selected"
                >
                    <FaCopy />
                </button>
                <button
                    className="control-button"
                    onClick={() => setShowRelated((prev) => !prev)}
                    title="Show Related Entities"
                >
                    <FaExpand />
                </button>
                <label>
                    <input
                        type="checkbox"
                        checked={showAllChapters}
                        onChange={() => setShowAllChapters(!showAllChapters)}
                    />
                    Show All Chapters
                </label>
            </div>

            <div className="table-scroll-wrapper">
                <table className="character-table">
                    <thead>
                        <tr>
                            <th></th>
                            {columns.map((col) => (
                                <th key={col}>{col}</th>
                            ))}
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {data.map((row) => (
                            <React.Fragment key={row.id}>
                                <EditableTableRow
                                    rowData={row}
                                    isNew={false}
                                    characters={referenceData?.characters}
                                    chapters={referenceData?.chapters}
                                    onSave={handleSave}
                                    onCancel={() => { }}
                                    onDelete={() => onDelete(row.id)}
                                    onCopy={() => onCopy(row.id)}
                                />
                                {onChapter && (
                                    <tr>
                                        <td colSpan={columns.length + 2}>
                                            <button
                                                className="control-button"
                                                onClick={() => onChapter(row.id)}
                                                title="Assign to Chapter"
                                            >
                                                <FaBook /> Assign to Chapter
                                            </button>
                                        </td>
                                    </tr>
                                )}
                                {showRelated && selectedIds.includes(row.id) && (
                                    <tr className="related-row">
                                        <td colSpan={columns.length + 2}>
                                            <strong>🔍 Related Entities:</strong>
                                            <ul>
                                                {Object.entries(row).map(([key, value]) => {
                                                    if (key.endsWith('Id') && value) {
                                                        const relatedListKey = key.slice(0, -2).toLowerCase() + 's';
                                                        const list = referenceData[relatedListKey];
                                                        const match = list?.find((e) => e.id === value);
                                                        return match ? (
                                                            <li key={key}>
                                                                {key.replace('Id', '')}: {match.name || match.title || '[Unnamed]'}
                                                            </li>
                                                        ) : null;
                                                    }
                                                    return null;
                                                })}
                                            </ul>
                                        </td>
                                    </tr>
                                )}
                            </React.Fragment>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
