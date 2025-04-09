import React, { useEffect } from 'react';
import { useEditor, EditorContent } from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';
import Underline from '@tiptap/extension-underline';
import TextStyle from '@tiptap/extension-text-style';
import Color from '@tiptap/extension-color';
import FontFamily from '@tiptap/extension-font-family';
import TextAlign from '@tiptap/extension-text-align';
import Heading from '@tiptap/extension-heading';
import Placeholder from '@tiptap/extension-placeholder';

import '../styles/TiptapEditorWithToolbar.css';

export default function TiptapEditorWithToolbar({ content, onUpdate, placeholder }) {
    const editor = useEditor({
        extensions: [
            StarterKit,
            Underline,
            TextStyle,
            Color,
            FontFamily,
            TextAlign.configure({
                types: ['heading', 'paragraph'],
            }),
            Heading.configure({
                levels: [1, 2, 3],
            }),
            Placeholder.configure({
                placeholder,
            }),
        ],
        content,
        onUpdate: ({ editor }) => {
            onUpdate(editor.getHTML());
        },
    });

    useEffect(() => {
        if (editor && content !== editor.getHTML()) {
            editor.commands.setContent(content);
        }
    }, [content, editor]);


    if (!editor) return null;

    return (
        <div className="editor-wrapper">
            <div className="toolbar">
                <select
                    onChange={(e) => editor.chain().focus().setFontFamily(e.target.value).run()}
                    defaultValue="Default"
                >
                    <option disabled>Font</option>
                    <option value="serif">Serif</option>
                    <option value="sans-serif">Sans</option>
                    <option value="Georgia">Georgia</option>
                    <option value="Times New Roman">Times</option>
                    <option value="Courier New">Courier</option>
                </select>

                <button onClick={() => editor.chain().focus().toggleBold().run()}
                    className={editor.isActive('bold') ? 'active' : ''}><b>B</b></button>

                <button onClick={() => editor.chain().focus().toggleItalic().run()}
                    className={editor.isActive('italic') ? 'active' : ''}><i>I</i></button>

                <button onClick={() => editor.chain().focus().toggleUnderline().run()}
                    className={editor.isActive('underline') ? 'active' : ''}><u>U</u></button>

                <button onClick={() => editor.chain().focus().toggleBulletList().run()}
                    className={editor.isActive('bulletList') ? 'active' : ''}>• List</button>

                <button onClick={() => editor.chain().focus().toggleOrderedList().run()}
                    className={editor.isActive('orderedList') ? 'active' : ''}>1. List</button>

                <button onClick={() => editor.chain().focus().setTextAlign('left').run()}
                    className={editor.isActive({ textAlign: 'left' }) ? 'active' : ''}>⇤</button>

                <button onClick={() => editor.chain().focus().setTextAlign('center').run()}
                    className={editor.isActive({ textAlign: 'center' }) ? 'active' : ''}>⎯</button>

                <button onClick={() => editor.chain().focus().setTextAlign('right').run()}
                    className={editor.isActive({ textAlign: 'right' }) ? 'active' : ''}>⇥</button>
            </div>

            <div className="editor-box">
                <EditorContent editor={editor} />
            </div>
        </div>
    );
}
