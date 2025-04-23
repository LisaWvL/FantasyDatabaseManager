// 📁 src/utils/unassignedFieldRules.ts
import { EntityType } from '../types/entities';
import type { CardContext } from '../cards/EntityCardTypes';

export const unassignedFieldByContext: Partial<Record<EntityType, Partial<Record<CardContext, string>>>> = {

    //Which Field values get updated with what, when dropped to specified component, from specified compoenent or context or compoanent+context
    //PlotPoint: {
    //    dashboard: 'startDateId' and/ or  'endDateId' with that cells DateId
    //    timeline: 'startDateId' and/ or  'endDateId',with that cells DateId
    //    unassignedsidebar while on dashboard or timeline: 'startDateId' or 'endDateId', set to null
    //    'context-dropzone': 'ChapterId', remove that entry from junctiontable where that plotpointId and ChapterId are present


    //},
    //Character: {
    //    chapterEditor: 'ChapterId',
    // item-section: OwnerId of the item updated with the CharacterId
    //Location-section: LocationId
    //language-sextion: LanguageId
    //        unassignedsidebar while on chapterEditor coming from context - dropzone: 'ChapterId',
    //        unassignedsidebar while on chapterEditor coming from POV-dropzone: 'POVCharacterId' of that Chapter to null,
    //    from unassigned or POV-dropzone to 'context-dropzone': 'ChapterId',
    //    from context-dropzone or'pov-dropzone': 'That Chapters POVCharacterId becomes this CharacterId'
    //    renders as card on all individual pages of entities that have a CharacterId FK
    //},


    //Event: {
    //    timeline: 'startDateId' and / or 'endDateId',with that cells DateId
    //    dashboard: 'startDateId' and / or  'endDateId',with that cells DateId
    //    from dashbaord to unassignedsidebar: 'startDateId' or 'endDateId',
    //    'context-dropzone': 'ChapterId',
    //    'location-section': 'LocationId',
    //    renders as card on all individual pages of entities that have a EventId FK
    //},


    //Chapter: {
    //    dashboard: 'startDateId' and/ or  'endDateId' with that cells DateId
    //    timeline: 'startDateId' and/ or  'endDateId',with that cells DateId
    //    unassignedsidebar while on dashboard or timeline: 'startDateId' or 'endDateId', set to null
    //    doesn't render as card on WritingAssistant page / ChapterEditor
    //    renders as card on all individual pages of entities that have a ChapterId FK
    //},


    //Era: {
    //    dashboard: 'startDateId' and/ or  'endDateId' with that cells DateId
    //    timeline: 'startDateId' and/ or  'endDateId',with that cells DateId
    //    unassignedsidebar while on dashboard or timeline: 'startDateId' or 'endDateId', set to null
    //    unassignedsidebar while on chapterEditor coming from context - dropzone: 'ChapterId',
    //    renders as card on all individual pages of entities that have a EraId FK
    //},

    //Faction: {
    //    Character-section
    //    unassignedsidebar while on chapterEditor coming from context - dropzone: 'ChapterId',
    //    renders as card on all individual pages of entities that have a FactionId FK
    //    location-section: HQLocationId
    //    from unassignedsidebar to 'context-dropzone': 'ChapterId',
    //    When dropped on a character section, the user is asked whether to set that Character as LeaderId or FounderId




    //Location: {
    //    unassignedsidebar while on chapterEditor coming from context - dropzone: 'ChapterId',
    //    when a character gets dropped on it, the characters LocationId updates
    //    when a faction gets dropped on it, the Factions HQLocationID updates
    //    When a river gets dropped on it, the user is asked wheter to set this as sourceLocation destination Location
    //    when a route gets dropped on it, the user is asked wheter to set this as From Location or as To Location
    //    renders as card on all individual pages of entities that have a LocationId FK


    //Item: {
    //    unassignedsidebar while on chapterEditor coming from context - dropzone: 'ChapterId',
    //    When dropped to context-dropzone from unassignedsidebar: set 'ChapterId',
    //    When dropped on a character section, set that Character as OwnerId
    //    renders as card on all individual pages of entities that have a ItemId FK
    //}


};
