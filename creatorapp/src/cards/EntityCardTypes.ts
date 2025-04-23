// 📁 src/cards/EntityCardTypes.ts

export type DisplayMode = 'compact' | 'basic' | 'full';

export type CardContext =
    | 'dashboard'
    | 'unassignedsidebar'
    | 'chapterEditor'
    | 'writingassistant'
    | 'list'
    | 'timeline'
    | 'section'
    | 'edit'
    | 'clone'
    | 'create'
    | "pov-dropzone"
    | "context-dropzone"

export type EntityCardProps<T> = {
    entityId: number;
    displayMode: DisplayMode;
    context?: CardContext;
    entity?: T;
};

