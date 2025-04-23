// 📁 src/cards/EntityCardFactory.tsx
import type { EntityType } from '../types/entities';
import type { CardContext, DisplayMode } from './EntityCardTypes';
import { entityColorHexMap } from '../utils/colorMapping';

// Specialized cards
import { PlotPointCard } from './PlotPointCard';
import  ChapterCard  from './ChapterCard';
import { EventCard } from './EventCard';
import { EraCard } from './EraCard';
import CharacterCard from './CharacterCard';
import { LocationCard } from './LocationCard';

export type EntityCardProps<T extends EntityType = EntityType> = {
    entityType: T;
    entityId: number;
    context: CardContext;
    DroppedOn?: string;
    Payload?: string;
    isGhost?: boolean;
    isReversed?: boolean;
    onResizeEnd?: (direction: 'start' | 'end', newDayId: number) => void;
};

/** 🔧 Determines display mode per entity and context */
function getDisplayMode(context: CardContext): DisplayMode {
    switch (context) {
        case 'timeline':
        case 'dashboard':
        case 'pov-dropzone':
        case 'context-dropzone':
        case 'writingassistant':
        case 'list':
            return 'compact';
        case 'section':
        case 'unassignedsidebar':
            return 'basic';
        case 'edit':
        case 'clone':
        case 'create':
            return 'full';
        default:
            return 'compact';
    }
}

/** 🧠 Dispatcher to render correct card by entity type */
export function EntityCard<T extends EntityType>({
    entityType,
    entityId,
    context,
    DroppedOn,
    Payload,
    isGhost,
    isReversed,
    onResizeEnd,
}: EntityCardProps<T>) {
    const displayMode = getDisplayMode(context);
    const colorIndex = entityColorHexMap[entityType] || entityColorHexMap.default;

    if (entityId === undefined) {
        return (
            <div className="p-2 border rounded bg-red-100 text-sm text-red-700">
                Missing entityId for {entityType}
            </div>
        );
    }


    switch (entityType) {
        case 'PlotPoint':
            return (
                <PlotPointCard
                    entityType="PlotPoint"
                    entityId={entityId}
                    context={context}
                    displayMode={displayMode}
                    DroppedOn={DroppedOn}
                    Payload={Payload}
                    colorIndex={colorIndex}
                    isGhost={isGhost}
                    isReversed={isReversed}
                    onResizeEnd={onResizeEnd}
                />
            );

        case 'Chapter':
            return (
                <ChapterCard
                    entityType='Chapter'
                    entityId={entityId}
                    context={context}
                    displayMode={displayMode}
                    DroppedOn={DroppedOn}
                    Payload={Payload}
                    colorIndex={colorIndex}
                    isGhost={isGhost}
                    isReversed={isReversed}
                    onResizeEnd={onResizeEnd}                />
            );

        case 'Event':
            return (
                <EventCard
                    entityType="Event"
                    entityId={entityId}
                    context={context}
                    displayMode={displayMode}
                    colorIndex={colorIndex}
                />
            );

        case 'Era':
            return (
                <EraCard
                    entityType="Era"
                    entityId={entityId}
                    context={context}
                    displayMode={displayMode}
                    colorIndex={colorIndex}
                />
            );

        case 'Character':
            return (
                <CharacterCard
                    entityType="Character"
                    entityId={entityId}
                    context={context}
                    displayMode={displayMode}
                    colorIndex={colorIndex}
                />
            );

        case 'Location':
            return (
                <LocationCard
                    entityType="Location"
                    entityId={entityId}
                    context={context}
                    displayMode={displayMode}
                    colorIndex={colorIndex}
                />
            );

        default:
            return (
                <div className="p-2 border rounded bg-gray-100 text-sm text-gray-500">
                    Unknown entity type: {entityType} #{entityId}
                </div>
            );
    }
}
