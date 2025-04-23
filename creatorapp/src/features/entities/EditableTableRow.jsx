// src/features/entities/EditableTableRow.jsx
import { useState, useEffect, useRef, useCallback } from 'react';

export default function EditableTableRow({
    rowData,
    isNew,
    characters = [],
    chapters = [],
    onSave,
    onCancel,
    onDelete,
    onCopy,
    onChapter, // ✅ optional
}) {
    const [formData, setFormData] = useState({ ...rowData });
    const rowRef = useRef(null);

    const handleSave = useCallback(() => {
        onSave(formData);
    }, [formData, onSave]);

    useEffect(() => {
        if (isNew) {
            setFormData({
                character1Id: '',
                character2Id: '',
                relationshipType: '',
                relationshipDynamic: '',
                chapterId: '',
            });
        }
    }, [isNew]);

    useEffect(() => {
        const handleClickOutside = (event) => {
            if (rowRef.current && !rowRef.current.contains(event.target)) {
                handleSave();
            }
        };
        document.addEventListener('mousedown', handleClickOutside);
        return () => {
            document.removeEventListener('mousedown', handleClickOutside);
        };
    }, [handleSave]);

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    return (
        <tr ref={rowRef}>
            <td>
                <select name="character1Id" value={formData.character1Id} onChange={handleChange}>
                    <option value="">-- Select --</option>
                    {characters.map((c) => (
                        <option key={c.id} value={c.id}>
                            {c.name}
                        </option>
                    ))}
                </select>
            </td>
            <td>
                <select name="character2Id" value={formData.character2Id} onChange={handleChange}>
                    <option value="">-- Select --</option>
                    {characters.map((c) => (
                        <option key={c.id} value={c.id}>
                            {c.name}
                        </option>
                    ))}
                </select>
            </td>
            <td>
                <input name="relationshipType" value={formData.relationshipType} onChange={handleChange} />
            </td>
            <td>
                <input
                    name="relationshipDynamic"
                    value={formData.relationshipDynamic}
                    onChange={handleChange}
                />
            </td>
            <td>
                <select name="chapterId" value={formData.chapterId} onChange={handleChange}>
                    <option value="">-- Select --</option>
                    {chapters.map((s) => (
                        <option key={s.id} value={s.id}>
                            {s.chapterName}
                        </option>
                    ))}
                </select>
            </td>
            <td>
                <button onClick={handleSave}>💾</button>
                <button onClick={onCancel}>✖️</button>
                <button onClick={onCopy}>📄</button>
                <button
                    onClick={() => {
                        if (confirm('❌ Are you sure you want to delete this relationship?')) {
                            onDelete(formData.id);
                        }
                    }}
                >
                    🗑️
                </button>
                {onChapter && (
                    <button onClick={() => onChapter(formData.id)}>📘</button>
                )}
            </td>
        </tr>
    );
}
