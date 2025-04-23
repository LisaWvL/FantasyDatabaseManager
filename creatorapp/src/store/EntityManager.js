// src/utils/EntityManager.js
import * as  ActApi  from '../features/chapters/ActApi';
import * as  BookApi from '../features/chapters/BookApi';
import * as  DateApi from '../features/plotpoints/DateApi';
import * as  ChapterApi from '../features/chapters/ChapterApi';
import * as CharacterApi from '../features/characters/CharacterApi';
import * as  CharacterRelationshipApi from '../features/characters/CharacterRelationshipApi';
import * as  ConversationTurnApi from '../features/ai/ConversationTurnApi';
import * as  CurrencyApi from '../features/items/CurrencyApi';
//import * as  DropdownApi from '../features/api/DropdownApi';
import * as  EraApi from '../features/eras/EraApi';
import * as  EventApi from '../features/events/EventApi';
import * as  FactionApi from '../features/factions/FactionApi';
import * as  ItemApi from '../features/items/ItemApi';
import * as  LanguageApi from '../features/languages/LanguageApi';
import * as LocationApi from '../features/locations/LocationApi';
import * as PlotPointApi from '../features/plotpoints/PlotPointApi';
import * as  PriceExampleApi from '../features/items/PriceExampleApi';
import * as  RiverApi from '../features/rivers/RiverApi';
import * as  RouteApi from '../features/routes/RouteApi';
import * as SceneApi from '../features/chapters/SceneApi';

const entityApiMap = {
    Act: ActApi,
    Book: BookApi,
    Date: DateApi,
    Chapter: ChapterApi,
    Character: CharacterApi,
    CharacterRelationship: CharacterRelationshipApi,
    ConversationTurn: ConversationTurnApi,
    Currency: CurrencyApi,
    Era: EraApi,
    Event: EventApi,
    Faction: FactionApi,
    Item: ItemApi,
    Language: LanguageApi,
    Location: LocationApi,
    PlotPoint: PlotPointApi,
    PriceExample: PriceExampleApi,
    River: RiverApi,
    Route: RouteApi,
    Scene: SceneApi,
};

export const EntityCreator = {
    create: async (entityName, payload) => {
        const api = entityApiMap[entityName];
        if (!api || !api[`create${entityName}`]) {
            throw new Error(`❌ No create method found for entity: ${entityName}`);
        }
        return await api[`create${entityName}`](payload);
    },
};


// src/store/EntityManager.js

export const EntityUpdater = {
    update: async (entityName, id, payload) => {
        const api = entityApiMap[entityName];
        if (!api || !api[`update${entityName}`]) {
            throw new Error(`❌ No update method found for entity: ${entityName}`);
        }
        return await api[`update${entityName}`](id, payload);
    },

    setNull: async (entityName, id, field) => {
        const endpoint = `/api/${entityName.toLowerCase()}/${id}/setnull?fieldName=${field}`;
        const res = await fetch(endpoint, { method: 'PATCH' });
        if (!res.ok) throw new Error(`❌ Failed to set '${field}' to null for ${entityName} ${id}`);
        return await res.json();
    },
};



export const EntityDeleter = {
    delete: async (entityName, id) => {
        const api = entityApiMap[entityName];
        if (!api || !api[`delete${entityName}`]) {
            throw new Error(`❌ No delete method found for entity: ${entityName}`);
        }
        return await api[`delete${entityName}`](id);
    },
};

export const EntityFetcher = {
    fetchAll: async (entityName) => {
        const api = entityApiMap[entityName];

        // ✅ Special case for Date
        if (entityName === 'Date') {
            return await api.fetchDateGrid();
        }

        if (!api || !api[`fetch${entityName}s`]) {
            throw new Error(`❌ No fetchAll method found for entity: ${entityName}`);
        }
        return await api[`fetch${entityName}s`]();
    },

    fetchById: async (entityName, id) => {
        const api = entityApiMap[entityName];
        if (!api || !api[`fetch${entityName}ById`]) {
            throw new Error(`❌ No fetchById method found for entity: ${entityName}`);
        }
        return await api[`fetch${entityName}ById`](id);
    },




};


