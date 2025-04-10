import React, { useEffect, useRef, useState } from 'react';
import ReactDOM from 'react-dom';
import axiosInstance from '../../src/api/axiosInstance';
import './TooltipLink.css';

export default function TooltipLink({ entityType, entityId, displayValue }) {
  const [tooltipData, setTooltipData] = useState(null);
  const [showTooltip, setShowTooltip] = useState(false);
  const [position, setPosition] = useState({ top: 0, left: 0 });
  const linkRef = useRef(null);
  const tooltipRef = useRef(null);

  useEffect(() => {
    if (!entityId || !entityType) return;

    axiosInstance
      .get(`/${entityType.toLowerCase()}/${entityId}`)
      .then((res) => setTooltipData(res.data))
      .catch((err) => console.error(`❌ Failed to fetch ${entityType}`, err));
  }, [entityId, entityType]);

  useEffect(() => {
    const updatePosition = () => {
      if (!linkRef.current || !tooltipRef.current) return;

      const linkRect = linkRef.current.getBoundingClientRect();
      const tooltipWidth = tooltipRef.current.offsetWidth;
      const gap = 10;

      let left = linkRect.left + window.scrollX - tooltipWidth - gap;
      let top = linkRect.top + window.scrollY;

      // Flip to right if left is offscreen
      if (left < 0) {
        left = linkRect.right + window.scrollX + gap;
      }

      setPosition({ top, left });
    };

    if (showTooltip) {
      updatePosition();
      window.addEventListener('resize', updatePosition);
      window.addEventListener('scroll', updatePosition, true);
      return () => {
        window.removeEventListener('resize', updatePosition);
        window.removeEventListener('scroll', updatePosition, true);
      };
    }
  }, [showTooltip]);

  useEffect(() => {
    const handleClickOutside = (e) => {
      if (
        tooltipRef.current &&
        !tooltipRef.current.contains(e.target) &&
        !linkRef.current.contains(e.target)
      ) {
        setShowTooltip(false);
      }
    };

    if (showTooltip) {
      document.addEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [showTooltip]);

  const getDisplayName = () => {
    if (displayValue) return displayValue;
    if (!tooltipData) return `(${entityType})`;

    // Try resolving a smart fallback
    return tooltipData.name || tooltipData.title || tooltipData.alias || `(${entityType})`;
  };

  const tooltip =
    showTooltip && tooltipData ? (
      <div
        ref={tooltipRef}
        className="tooltip-content tooltip-floating"
        style={{ top: `${position.top}px`, left: `${position.left}px` }}
      >
        <div className="tooltip-header">{entityType.toUpperCase()}</div>
        <hr />
        {Object.entries(tooltipData)
          .filter(
            ([k, v]) => k.toLowerCase() !== 'id' && (typeof v === 'string' || typeof v === 'number')
          )
          .map(([k, v], idx) => (
            <div key={idx} className="tooltip-line">
              <span className="tooltip-label">
                {k
                  .replace(/([a-z])([A-Z])/g, '$1 $2')
                  .replace(/Id$/, '')
                  .replace(/^./, (str) => str.toUpperCase())}
                :
              </span>
              <span className="tooltip-value">{v}</span>
            </div>
          ))}
      </div>
    ) : null;

  return (
    <>
      <span ref={linkRef} className="tooltip-link" onClick={() => setShowTooltip((prev) => !prev)}>
        {getDisplayName()}
      </span>
      {ReactDOM.createPortal(tooltip, document.body)}
    </>
  );
}
