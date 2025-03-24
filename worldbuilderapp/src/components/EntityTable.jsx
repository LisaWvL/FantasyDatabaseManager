import React from "react";

export default function EntityTable({ data, onEdit, onDelete, onSnapshot }) {
    if (!data || data.length === 0) return <p>No data available.</p>;

    const columns = Object.keys(data[0]).filter(key => !key.endsWith("Id") && key !== "id");

    return (
        <table className="table table-bordered table-hover">
            <thead className="table-dark">
                <tr>
                    {columns.map(col => <th key={col}>{col}</th>)}
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {data.map(entity => (
                    <tr key={entity.id}>
                        {columns.map(col => <td key={col}>{entity[col]}</td>)}
                        <td>
                            <button className="btn btn-sm btn-warning me-1" onClick={() => onEdit(entity.id)}>Edit</button>
                            <button className="btn btn-sm btn-info me-1" onClick={() => onSnapshot(entity.id)}>New Snapshot</button>
                            <button className="btn btn-sm btn-danger" onClick={() => onDelete(entity.id)}>Delete</button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}
