// 📁 src/utils/colorMapping.ts
import { EntityType } from '../types/entities';

export const entityColorHexMap: Record<EntityType | 'default', string> = {
    Character: '#a368ff',
    Chapter: '#ff8a65',
    Era: '#ffb74d',
    Scene: '#4fc3f7',
    Faction: '#ffb74d',
    Item: '#81c784',
    Event: '#f06292',
    Location: '#64b5f6',
    Language: '#7986cb',
    PlotPoint: '#9575cd',
    CharacterRelationship: '#ffd54f',
    Act: '#4db6ac',
    Book: '#4db6ac',
    Date: '#4db6ac',
    Currency: '#4db6ac',
    Route: '#4db6ac',
    River: '#4db6ac',
    PriceExample: '#4db6ac',
    ConversationTurn: '#4db6ac',
    default: '#888'
};
