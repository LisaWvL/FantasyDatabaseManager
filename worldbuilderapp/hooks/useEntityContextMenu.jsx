import { useState } from 'react';
import ReactDOM from 'react-dom';
import ContextMenu from '../utils/ContextMenu';
import ConfirmDialog from '../utils/ConfirmDialog';

export function useEntityContextMenu({ onCreate, onEdit, onDelete }) {
    const [menuState, setMenuState] = useState(null);
    const [confirmingDelete, setConfirmingDelete] = useState(false);

    const showContextMenu = (e, entity, entityType) => {
        e.preventDefault();
        setMenuState({
            x: e.clientX,
            y: e.clientY,
            entity,
            entityType,
        });
    };

    const handleCloseMenu = () => setMenuState(null);

    const handleDelete = async () => {
        setConfirmingDelete(false);

        if (typeof onDelete === 'function' && menuState?.entity) {
            console.log('🟢 Deleting:', menuState.entity);
            await onDelete(menuState.entity);
        } else {
            console.warn('⚠️ onDelete is not a function or menuState is missing.');
        }

        handleCloseMenu();
    };

    return {
        showContextMenu,
        contextMenuPortal: (
            <>
                {menuState &&
                    ReactDOM.createPortal(
                        <ContextMenu
                            x={menuState.x}
                            y={menuState.y}
                            onClose={handleCloseMenu}
                            onClone={() => {
                                handleCloseMenu();
                                onCreate?.(menuState.entityType);
                            }}
                            onEdit={() => {
                                handleCloseMenu();
                                onEdit?.(menuState.entity);
                            }}
                            onDelete={() => {
                                setConfirmingDelete(true); // ✅ Only sets flag
                            }}
                        />,
                        document.body
                    )}

                {confirmingDelete &&
                    ReactDOM.createPortal(
                        <ConfirmDialog
                            title="Delete this item?"
                            message={`Are you sure you want to delete this ${menuState?.entityType}? This cannot be undone.`}
                            onCancel={() => setConfirmingDelete(false)}
                            onConfirm={handleDelete}
                        />,
                        document.body
                    )}
            </>
        )
    };
}
