// src/components/EntityCard.jsx
import React from 'react';
import '../styles/EntityCard.css';

export default function EntityCard({ type, entity, onClose }) {
    const renderAttributes = () => {
        return Object.entries(entity)
            .filter(([, val]) => val && typeof val !== 'object')
            .map(([key, val]) => (
                <div className="attribute-box" key={key}>
                    <span className="label">{key}:</span>
                    <span className="value">{val.toString()}</span>
                </div>
            ));
    };

    return (
        <div className="entity-overlay">
            <div className="entity-card">
                <div className="card-header">
                    <h3>{entity.name || entity.title || entity.alias || `[Unnamed ${type}]`}</h3>
                    <button className="close-btn" onClick={onClose}>❌</button>
                </div>
                <h4 className="entity-type">{type}</h4>
                <div className="card-details">
                    {renderAttributes()}
                </div>
            </div>
        </div>
    );
}