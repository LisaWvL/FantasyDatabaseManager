import { useEffect } from 'react';
import './ContextMenu.css';

export default function ContextMenu({ x, y, onClone, onEdit, onDelete, onClose }) {
    useEffect(() => {
        const close = () => onClose();
        setTimeout(() => {
            document.addEventListener('click', close);
        }, 0);
        return () => document.removeEventListener('click', close);
    }, [onClose]);

    return (
        <div className="context-menu" style={{ top: y, left: x }}>
            <div className="context-menu-item" onClick={() => { onClone?.(); }}>➕ Clone</div>
            <div className="context-menu-item" onClick={() => { onEdit?.(); }}>✏️ Edit</div>
            <div
                className="context-menu-item"
                onClick={(e) => {
                    e.stopPropagation(); // keep menu open long enough
                    onDelete?.(); // Triggers confirm dialog setup, not actual delete
                }}
            >
                🗑️ Delete
            </div>
        </div>
    );
}
