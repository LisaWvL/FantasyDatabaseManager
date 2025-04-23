import React from 'react';
import './SmallEntityCard.css';
import { safeUpper, safeLower } from '../utils/UpperLowerCase';

const typeColors = {
    character: '#a368ff',
    chapter: '#ff8a65', //here we need variations of colors, i don't want them all to have the same color that'll get confusing in the Date, this is more like an overlay over the date
    era: '#ffb74d', //they can be one color, but i want them to be different from the other colors, this is more like an overlay over the date
    scene: '#4fc3f7',
    faction: '#ffb74d',
    item: '#81c784',
    event: '#f06292', //they can be one color, but i want them to be different from the other colors
    location: '#64b5f6',
    language: '#7986cb',
    plotpoint: '#9575cd', //here we need variations of colors, i don't want them all to have the same color that'll get confusing in the Date
    relationship: '#ffd54f',
    default: '#888'
};

export default function SmallEntityCard({
    entity,
    entityType,
    onContextMenu,
    onDragStart,
    onDragEnd,
    draggable = true
}) {
    const color = typeColors[safeLower(entityType, `SmallEntityCard: color for ${entity?.name}`)] || typeColors.default;

    const handleContextMenu = (e) => {
        e.preventDefault();
        onContextMenu?.(e, entity, entityType);
    };

    const handleDragStart = (e) => {
        // Always embed this, even if onDragStart exists
        if (entity?.id && entityType) {
            e.dataTransfer.setData('entityId', entity.id.toString());
            e.dataTransfer.setData('entityType', entityType);
            e.dataTransfer.effectAllowed = 'move';
        }

        // Also call external handler if provided
        onDragStart?.(e);
    };

    return (
        <div
            className="small-entity-card"
            style={{ borderLeft: `4px solid ${color}` }}
            onContextMenu={handleContextMenu}
            draggable={draggable}
            onDragStart={handleDragStart}
            onDragEnd={(e) => onDragEnd?.(e)}
        >
            <div className="entity-type" style={{ color }}>
                {safeUpper(entityType, `SmallEntityCard: label for ${entity?.name}`)}
            </div>
            <div className="entity-name">
                {entity.name || entity.title || entity.alias || `#${entity.id}`}
            </div>
        </div>
    );
}
