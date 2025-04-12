import React, { useState, useEffect, useRef } from 'react';
import TooltipLink from './TooltipLink';
import { EntityFetcher } from './entityManager';
import './EntityCard.css';

export default function EntityCard({
  entity,
  entityType,
  schema,
  onUpdate,
  onDelete,
  onCreateNewVersion,
  draggable = true,
  onDragStart,
  onDragEnd, // ✅ Pass from parent (EntityPage)
}) {
  const [isEditMode, setIsEditMode] = useState(entity.isNew || entity.isEditMode || false);
  const [localEntity, setLocalEntity] = useState({ ...entity });
  const [dropdownData, setDropdownData] = useState({});
  const [showDetails, setShowDetails] = useState(false);
  const [showSummary, setShowSummary] = useState(false);
  const isFirstRender = useRef(true);

  useEffect(() => {
    async function loadDropdowns() {
      const fkFields = schema.fields.filter((f) => f.type === 'fk' || f.type === 'multiFk');
      const result = {};
      for (const field of fkFields) {
        try {
          const options = await EntityFetcher.fetchAll(field.fkType);
          result[field.key] = options;
        } catch (err) {
          console.error(`❌ Failed to load options for ${field.fkType}`, err);
        }
      }
      setDropdownData(result);
    }
    loadDropdowns();
  }, [schema]);

  useEffect(() => {
    const handleKeyDown = (e) => {
      if (e.key === 'Escape') {
        setLocalEntity({ ...entity });
        setIsEditMode(false);
      }
    };
    if (isEditMode) {
      window.addEventListener('keydown', handleKeyDown);
    }
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isEditMode, entity]);

  const handleBlur = () => {
    if (isEditMode && !isFirstRender.current) {
      onUpdate(localEntity);
      setIsEditMode(false);
    }
    isFirstRender.current = false;
  };

  const handleChange = (field, value) => {
    setLocalEntity((prev) => ({ ...prev, [field.key]: value }));
  };

  const renderField = (field) => {
    const value = localEntity[field.key];
    const skipInHeader = ['name', 'alias'];

    if (!isEditMode && field.section === 'header' && skipInHeader.includes(field.key)) return null;

    if (isEditMode) {
      if (field.type === 'fk') {
        const options = dropdownData[field.key] || [];
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <select
              value={value || ''}
              onChange={(e) => handleChange(field, parseInt(e.target.value))}
              onBlur={handleBlur}
            >
              <option value="">Select...</option>
              {options.map((opt) => (
                <option key={opt.id} value={opt.id}>
                  {opt.name || opt.title || opt.alias || `#${opt.id}`}
                </option>
              ))}
            </select>
          </div>
        );
      } else if (field.type === 'multiFk') {
        const options = dropdownData[field.key] || [];
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <select
              multiple
              value={value || []}
              onChange={(e) =>
                handleChange(
                  field,
                  Array.from(e.target.selectedOptions, (o) => parseInt(o.value))
                )
              }
              onBlur={handleBlur}
            >
              {options.map((opt) => (
                <option key={opt.id} value={opt.id}>
                  {opt.name || opt.title || opt.alias || `#${opt.id}`}
                </option>
              ))}
            </select>
          </div>
        );
      } else {
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <textarea
              className="edit-field"
              value={value || ''}
              onChange={(e) => handleChange(field, e.target.value)}
              onBlur={handleBlur}
            />
          </div>
        );
      }
    } else {
      if (field.type === 'fk') {
        return (
          <div className="attribute-box" key={field.key}>
            <TooltipLink
              entityType={field.fkType}
              entityId={value}
              displayValue={entity[`${field.key.replace(/Id$/, '')}Name`] || `(${field.label})`}
            />
          </div>
        );
      } else if (field.type === 'multiFk') {
        const items = value || [];
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <div className="multi-fk-list">
              {items.map((id) => (
                <div key={id}>
                  <TooltipLink entityType={field.fkType} entityId={id} displayValue={id} />
                </div>
              ))}
            </div>
          </div>
        );
      } else {
        return (
          <div className="attribute-box" key={field.key}>
            <span className="label">{field.label}</span>
            <span className="value">{value}</span>
          </div>
        );
      }
    }
  };

  const renderSection = (sectionName) => {
    const fields = schema.fields.filter((f) => f.section === sectionName);
    if (fields.length === 0) return null;

    return (
      <div className={`card-section ${sectionName}`}>
        {sectionName !== 'header' && <h5 className="section-title">{sectionName.toUpperCase()}</h5>}
        {fields.map((field) => renderField(field))}
      </div>
    );
  };

  return (
    <div
      className={`entity-card ${entityType.toLowerCase()}-card`}
      draggable={draggable}
      onDragStart={(e) => onDragStart?.(e)}
      onDragEnd={(e) => onDragEnd?.(e)} // ✅ This now works
    >
      <div className="card-header fancy-header">
        <div className="card-actions">
          <button onClick={() => onCreateNewVersion?.(entity)}>➕</button>
          <button onClick={() => setIsEditMode(true)}>✏️</button>
          <button onClick={() => onDelete?.(entity)}>🗑️</button>
        </div>
        <h3>{schema.primaryDisplay(localEntity)}</h3>
        {schema.subDisplay && <h4>{schema.subDisplay(localEntity)}</h4>}
      </div>

      {renderSection('header')}
      {renderSection('relation')}

      {showSummary && renderSection('summary')}
      {schema.fields.some((f) => f.section === 'summary') && (
        <button className="details-toggle" onClick={() => setShowSummary((prev) => !prev)}>
          {showSummary ? '▲ Summary' : '▼ Summary'}
        </button>
      )}

      {showDetails && renderSection('details')}
      {schema.fields.some((f) => f.section === 'details') && (
        <button className="details-toggle" onClick={() => setShowDetails((prev) => !prev)}>
          {showDetails ? '▲ Details' : '▼ Details'}
        </button>
      )}
    </div>
  );
}
