// src/utils/EntityManager.js

import * as ActApi from '../chapters/ActApi';
import * as BookApi from '../chapters/BookApi';
import * as CalendarApi from '../calendar/CalendarApi';
import * as ChapterApi from '../chapters/ChapterApi';
import * as CharacterApi from '../characters/CharacterApi';
import * as CharacterRelationshipApi from '../characters/CharacterRelationshipApi';
import * as ConversationTurnApi from '../ai/ConversationTurnApi';
import * as CurrencyApi from '../items/CurrencyApi';
//import * as DropdownApi from '../api/DropdownApi';
import * as EraApi from '../eras/EraApi';
import * as EventApi from '../events/EventApi';
import * as FactionApi from '../factions/FactionApi';
import * as ItemApi from '../items/ItemApi';
import * as LanguageApi from '../languages/LanguageApi';
import * as LocationApi from '../locations/LocationApi';
import * as PlotPointApi from '../plotpoints/PlotPointApi';
import * as PriceExampleApi from '../items/PriceExampleApi';
import * as RiverApi from '../rivers/RiverApi';
import * as RouteApi from '../routes/RouteApi';
import * as SceneApi from '../chapters/SceneApi';

const entityApiMap = {
  Act: ActApi,
  Book: BookApi,
  Calendar: CalendarApi,
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

export const EntityUpdater = {
  update: async (entityName, id, payload) => {
    const api = entityApiMap[entityName];
    if (!api || !api[`update${entityName}`]) {
      throw new Error(`❌ No update method found for entity: ${entityName}`);
    }
    return await api[`update${entityName}`](id, payload);
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
