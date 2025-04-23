////import React from 'react';
//import { EntityType } from '../types/entities';
//import type { CardContext } from './EntityCardTypes';

//// Import all specialized entity cards
//import { PlotPointCard }  from './PlotPointCard';
//import  ChapterCard  from './ChapterCard';
//import { EventCard  } from './EventCard';
//import { EraCard  } from './EraCard';
//import  CharacterCard  from './CharacterCard';
//import { LocationCard } from './LocationCard';
////import { ItemCard } from './ItemCard.';

//// Add more imports as needed

//export type EntityCardProps<T extends EntityType = EntityType> = {
//    entityType: T;
//    entityId?: number;
//    context: CardContext; //Which page are we on when this function is called?
//    DroppedOn?: string; //Where are we dropping the entity?
//};

//export function EntityCard<T extends EntityType>({
//    entityType,
//    entityId,
//    context, 
//    DroppedOn
//}: EntityCardProps<T>) {
//    if (entityId === undefined) {
//        return (
//            <div className="p-2 border rounded bg-red-100 text-sm text-red-700">
//                Missing entityId for {entityType}
//            </div>
//        );
//    }

//    switch (entityType) {

//        case 'PlotPoint':
//            return <PlotPointCard entityTyoe={'PlotPoint'} entityId={entityId} context={context} />;
//        case 'Chapter':
//            return <ChapterCard entityId={entityId} context={context} DroppedOn={DroppedOn} />;
//        case 'Event':
//            return <EventCard entityId={entityId} context={context} DroppedOn={DroppedOn} />;
//        case 'Era':
//            return <EraCard entityId={entityId}  context={context} DroppedOn={DroppedOn} />;
//        case 'Character':
//            return <CharacterCard entityId={entityId}  context={context} DroppedOn={DroppedOn} />;
//        case 'Location':
//            return <LocationCard entityId={entityId} context={context} DroppedOn={DroppedOn} />;
//        case 'Item':
//            return <ItemCard entityId={entityId} context={context} DroppedOn={DroppedOn} />;

//        // Add more entity cases here
//        default:
//            return (
//                <div className="p-2 border rounded bg-gray-100 text-sm text-gray-500">
//                    Unknown entity type: {entityType} #{entityId}
//                </div>
//            );
//    }
//}
