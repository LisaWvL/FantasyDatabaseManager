//TODO - Add the following code to the ContextMenu.jsx file:
// ✅ This is the ContextMenu component that displays the right-click options

// src/components/ContextMenu.jsx
import { useEffect, useRef } from 'react';
import './ContextMenu.css';

export default function ContextMenu({ options, position, onClose }) {
  const ref = useRef();

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (ref.current && !ref.current.contains(e.target)) {
        onClose();
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [onClose]);

  return (
    <ul
      className="context-menu"
      ref={ref}
      style={{ top: position.y, left: position.x, position: 'absolute' }}
    >
      {options.map((opt, i) => (
        <li key={i} onClick={opt.onClick}>
          {opt.label}
        </li>
      ))}
    </ul>
  );
}
