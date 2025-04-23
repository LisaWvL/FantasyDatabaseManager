import React from 'react';
import './ConfirmDialog.css';

export default function ConfirmDialog({ title, message, onCancel, onConfirm }) {
    return (
        <div className="confirm-dialog-overlay">
            <div className="confirm-dialog">
                <h3>{title}</h3>
                <p>{message}</p>
                <div className="confirm-dialog-actions">
                    <button className="btn-secondary" onClick={onCancel}>Cancel</button>
                    <button className="btn-danger" onClick={onConfirm}>Delete</button>
                </div>
            </div>
        </div>
    );
}
