// src/hooks/useRowEditKeyboard.js
import { useEffect } from 'react';

export function useRowEditKeyboard(ref, onSave, onCancel) {
    useEffect(() => {
        const handleKey = (e) => {
            if (!ref.current || !ref.current.contains(document.activeElement)) return;

            if (e.key === 'Enter') {
                e.preventDefault();
                onSave();
            } else if (e.key === 'Escape') {
                e.preventDefault();
                onCancel();
            }
        };
        document.addEventListener('keydown', handleKey);
        return () => document.removeEventListener('keydown', handleKey);
    }, [ref, onSave, onCancel]);
}
