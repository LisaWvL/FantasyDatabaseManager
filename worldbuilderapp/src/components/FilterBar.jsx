// src/components/FilterBar.jsx
import React, { useEffect, useState } from 'react';
import { fetchCharacters } from '../api/CharacterApi';
import { fetchSnapshots } from '../api/SnapshotApi';
import { fetchCalendarDayByMonthAndDay, fetchMonths, fetchWeekdays  } from '../api/CalendarApi';
import { fetchFactions } from '../api/FactionApi';
import { fetchLanguages } from '../api/LanguageApi';
import { fetchLocations } from '../api/LocationApi';

export default function FilterBar({ entityType, filters, setFilters }) {
    const [dropdownData, setDropdownData] = useState({
        characters: [],
        snapshots: [],
        months: [],
        factions: [],
        locations: [],
        languages: []
    });

    useEffect(() => {
        async function loadDropdowns() {
            try {
                const [characters, snapshots, calendar, factions, locations, languages] = await Promise.all([
                    fetchCharacters(),
                    fetchSnapshots(),
                    fetchCalendarDayByMonthAndDay(),
                    fetchMonths(),
                    fetchWeekdays(),
                    fetchFactions(),
                    fetchLocations(),
                    fetchLanguages()
                ]);

                setDropdownData({
                    characters,
                    snapshots,
                    months: [...new Set(calendar.map(c => c.month))],
                    factions,
                    locations,
                    languages
                });
            } catch (err) {
                console.error("❌ Failed to load filter dropdown data", err);
            }
        }

        loadDropdowns();
    }, []);

    const handleChange = (key, value) => {
        setFilters(prev => ({ ...prev, [key]: value }));
    };

    const renderDropdown = (label, key, list, labelProp = 'name') => (
        <div className="filter-control" key={key}>
            <label>{label}</label>
            <select value={filters[key] || ''} onChange={e => handleChange(key, e.target.value)}>
                <option value="">-- Any --</option>
                {list.map(opt => (
                    <option key={opt.id} value={opt.id}>{opt[labelProp]}</option>
                ))}
            </select>
        </div>
    );

    const renderTextInput = (label, key, type = 'text') => (
        <div className="filter-control" key={key}>
            <label>{label}</label>
            <input
                type={type}
                value={filters[key] || ''}
                onChange={e => handleChange(key, e.target.value)}
                placeholder={`Enter ${label.toLowerCase()}`}
            />
        </div>
    );

    const entityFilters = {
        character: [
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName'),
            renderDropdown('Faction', 'FactionId', dropdownData.factions),
            renderDropdown('Location', 'LocationId', dropdownData.locations),
            renderDropdown('Language', 'LanguageId', dropdownData.languages)
        ],
        characterrelationship: [
            renderDropdown('Character 1', 'Character1Id', dropdownData.characters),
            renderDropdown('Character 2', 'Character2Id', dropdownData.characters),
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName')
        ],
        item: [
            renderDropdown('Owner', 'OwnerId', dropdownData.characters),
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName')
        ],
        era: [
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName')
        ],
        event: [
            renderDropdown('Location', 'LocationId', dropdownData.locations),
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName'),
            renderDropdown('Month', 'Month', dropdownData.months, ''),
            renderTextInput('Day', 'Day', 'number'),
            renderTextInput('Year', 'Year', 'number')
        ],
        faction: [
            renderDropdown('Founder', 'FounderId', dropdownData.characters),
            renderDropdown('Leader', 'LeaderId', dropdownData.characters),
            renderDropdown('HQ Location', 'HQLocationId', dropdownData.locations),
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName')
        ],
        calendar: [
            renderDropdown('Month', 'Month', dropdownData.months, ''),
            renderTextInput('Day', 'Day', 'number'),
            renderTextInput('Year', 'Year', 'number')
        ],
        plotpoint: [
            renderDropdown('Start Month', 'StartMonth', dropdownData.months, ''),
            renderTextInput('Start Day', 'StartDay', 'number'),
            renderTextInput('Start Year', 'StartYear', 'number'),
            renderDropdown('End Month', 'EndMonth', dropdownData.months, ''),
            renderTextInput('End Day', 'EndDay', 'number'),
            renderTextInput('End Year', 'EndYear', 'number'),
            renderDropdown('Snapshot', 'SnapshotId', dropdownData.snapshots, 'snapshotName')
        ],
        river: [
            renderDropdown('Source Location', 'SourceLocationId', dropdownData.locations),
            renderDropdown('Destination Location', 'DestinationLocationId', dropdownData.locations)
        ],
        route: [
            renderDropdown('From', 'FromId', dropdownData.locations),
            renderDropdown('To', 'ToId', dropdownData.locations)
        ]
    };

    return (
        <div className="filter-bar">
            {entityFilters[entityType.toLowerCase()] || (
                <p>⚠️ No filters defined for <strong>{entityType}</strong>.</p>
            )}
        </div>
    );
}
