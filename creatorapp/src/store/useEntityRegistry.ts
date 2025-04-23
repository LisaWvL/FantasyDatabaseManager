//import { create } from 'zustand'
//import { EntityUpdater, EntityDeleter, EntityFetcher } from '../store/EntityManager' //EntityFetcher
//import { EntitySchemas} from '../store/EntitySchemas'
//import { EntityMap, EntityType } from '../types/entities';

//import type {
//    Act,
//    Book,
//    Scene,
//    River,
//    Route,
//    Character,
//    Currency,
//    Chapter,
//    Era,
//    Event,
//    Item,
//    Location,
//    Language,
//    Faction,
//    CharacterRelationship,
//    Date,
//    PriceExample,
//    ConversationTurn,
//    PlotPoint,
//} from '../types/entities';


//export type CacheState = {
//    [K in keyof EntityMap]?: Record<number, EntityMap[K]>
//}

//export type RegistryStore = {
//    cache: CacheState

//    get: <T extends keyof EntityMap>(type: T, id: number) => EntityMap[T] | null
//    getAll: <T extends keyof EntityMap>(type: T) => EntityMap[T][]
//    getAllById: <T extends keyof EntityMap>(type: T, ids: number[]) => EntityMap[T][]

//    update: <T extends keyof EntityMap>(type: T, id: number, data: Partial<EntityMap[T]>) => void
//    updateAllOfType: <T extends keyof EntityMap>(type: T, data: EntityMap[T][]) => void
//    //updateAll: (allData: Partial<CacheState>) => void
//    setFieldToNull: <T extends keyof EntityMap>(type: T, id: number, field: keyof EntityMap[T]) => void

//    loadAllIntoCache: () => Promise<void>
//    persistEntity: <T extends keyof EntityMap>(type: T, id: number) => Promise<void>
//    persistField: <T extends keyof EntityMap>(type: T, id: number, field: keyof EntityMap[T]) => Promise<void>
//    //persistAllOfType: <T extends keyof EntityMap>(type: T) => Promise<void>
//    persistAll: () => Promise<void>
//    deleteEntity: <T extends keyof EntityMap>(type: T, id: number) => Promise<void>

//    //resolveForeignKeys: <T extends keyof EntityMap>(type: T, entity: EntityMap[T]) => Promise<void>
//    getForeignKeys: (type: keyof typeof EntitySchemas) => { key: string, fkType?: EntityType }[]
//    getReverseLinks: (targetType: keyof typeof EntitySchemas) => [string, string][]
//    //getEntitiesLinkedTo: <T extends keyof EntityMap>(type: T, id: number) => Record<string, EntityMap[keyof EntityMap][]>
//    //getLinkedEntities: <T extends keyof EntityMap>(type: T, id: number, depth?: number, visited?: Set<string>) => Record<string, EntityMap[keyof EntityMap][]>
//    getTypeById: <T extends keyof EntityMap>(type: T, id: number) => EntityMap[T] | null
//    getAllOfType: <T extends keyof EntityMap>(type: T) => EntityMap[T][]
//}

//function mergeIntoCache<T extends { id: number }>(
//    existing: Record<number, T>,
//    incoming: T[]
//): Record<number, T> {
//    const merged = { ...existing };
//    for (const item of incoming) {
//        merged[item.id] = { ...merged[item.id], ...item };
//    }
//    return merged;
//}


//const useEntityRegistry = create<RegistryStore>((set, get) => ({
//    cache: {},

//    get(type, id) {
//        return get().cache[type]?.[id] ?? null
//    },

//    getAll(type) {
//        return Object.values(get().cache[type] || {})
//    },

//    getAllById<T extends keyof EntityMap>(type: T, ids: number[]) {
//        const cache = get().cache[type] as Record<number, EntityMap[T]> || {}
//        return ids.map(id => cache[id]).filter(Boolean) as EntityMap[T][]
//    },


//    update(type, id, data) {
//        set(state => {
//            const current = state.cache[type]?.[id]
//            if (!current) return state
//            return {
//                cache: {
//                    ...state.cache,
//                    [type]: {
//                        ...state.cache[type],
//                        [id]: { ...current, ...data },
//                    },
//                },
//            }
//        })
//    },

//    async loadAllIntoCache() {
//        const cache = get().cache;
//        const result: Partial<CacheState> = {};

//        try {
//            const plotPoints = await EntityFetcher.fetchAll('PlotPoint');
//            const existingPlotPoints = (cache.PlotPoint || {}) as Record<number, PlotPoint>;
//            result.PlotPoint = mergeIntoCache(existingPlotPoints, plotPoints) as Record<number, PlotPoint>;
//        } catch (err) {
//            console.error('❌ Failed to load PlotPoints', err);
//        }

//        try {
//            const acts = await EntityFetcher.fetchAll('Act');
//            const existing = (cache.Act || {}) as Record<number, Act>;
//            result.Act = mergeIntoCache(existing, acts) as Record<number, Act>;
//        } catch (err) {
//            console.error('❌ Failed to load Acts', err);
//        }

//        try {
//            const books = await EntityFetcher.fetchAll('Book');
//            const existing = (cache.Book || {}) as Record<number, Book>;
//            result.Book = mergeIntoCache(existing, books) as Record<number, Book>;
//        } catch (err) {
//            console.error('❌ Failed to load Books', err);
//        }

//        try {
//            const scenes = await EntityFetcher.fetchAll('Scene');
//            const existing = (cache.Scene || {}) as Record<number, Scene>;
//            result.Scene = mergeIntoCache(existing, scenes) as Record<number, Scene>;
//        } catch (err) {
//            console.error('❌ Failed to load Scenes', err);
//        }

//        try {
//            const rivers = await EntityFetcher.fetchAll('River');
//            const existing = (cache.River || {}) as Record<number, River>;
//            result.River = mergeIntoCache(existing, rivers) as Record<number, River>;
//        } catch (err) {
//            console.error('❌ Failed to load Rivers', err);
//        }

//        try {
//            const routes = await EntityFetcher.fetchAll('Route');
//            const existing = (cache.Route || {}) as Record<number, Route>;
//            result.Route = mergeIntoCache(existing, routes) as Record<number, Route>;
//        } catch (err) {
//            console.error('❌ Failed to load Routes', err);
//        }

//        try {
//            const characters = await EntityFetcher.fetchAll('Character');
//            const existing = (cache.Character || {}) as Record<number, Character>;
//            result.Character = mergeIntoCache(existing, characters) as Record<number, Character>;
//        } catch (err) {
//            console.error('❌ Failed to load Characters', err);
//        }

//        try {
//            const currencies = await EntityFetcher.fetchAll('Currency');
//            const existing = (cache.Currency || {}) as Record<number, Currency>;
//            result.Currency = mergeIntoCache(existing, currencies) as Record<number, Currency>;
//        } catch (err) {
//            console.error('❌ Failed to load Currencies', err);
//        }

//        try {
//            const chapters = await EntityFetcher.fetchAll('Chapter');
//            const existing = (cache.Chapter || {}) as Record<number, Chapter>;
//            result.Chapter = mergeIntoCache(existing, chapters) as Record<number, Chapter>;
//        } catch (err) {
//            console.error('❌ Failed to load Chapters', err);
//        }

//        try {
//            const eras = await EntityFetcher.fetchAll('Era');
//            const existing = (cache.Era || {}) as Record<number, Era>;
//            result.Era = mergeIntoCache(existing, eras) as Record<number, Era>;
//        } catch (err) {
//            console.error('❌ Failed to load Eras', err);
//        }

//        try {
//            const events = await EntityFetcher.fetchAll('Event');
//            const existing = (cache.Event || {}) as Record<number, Event>;
//            result.Event = mergeIntoCache(existing, events) as Record<number, Event>;
//        } catch (err) {
//            console.error('❌ Failed to load Events', err);
//        }

//        try {
//            const items = await EntityFetcher.fetchAll('Item');
//            const existing = (cache.Item || {}) as Record<number, Item>;
//            result.Item = mergeIntoCache(existing, items) as Record<number, Item>;
//        } catch (err) {
//            console.error('❌ Failed to load Items', err);
//        }

//        try {
//            const locations = await EntityFetcher.fetchAll('Location');
//            const existing = (cache.Location || {}) as Record<number, Location>;
//            result.Location = mergeIntoCache(existing, locations) as Record<number, Location>;
//        } catch (err) {
//            console.error('❌ Failed to load Locations', err);
//        }

//        try {
//            const languages = await EntityFetcher.fetchAll('Language');
//            const existing = (cache.Language || {}) as Record<number, Language>;
//            result.Language = mergeIntoCache(existing, languages) as Record<number, Language>;
//        } catch (err) {
//            console.error('❌ Failed to load Languages', err);
//        }

//        try {
//            const factions = await EntityFetcher.fetchAll('Faction');
//            const existing = (cache.Faction || {}) as Record<number, Faction>;
//            result.Faction = mergeIntoCache(existing, factions) as Record<number, Faction>;
//        } catch (err) {
//            console.error('❌ Failed to load Factions', err);
//        }

//        try {
//            const relationships = await EntityFetcher.fetchAll('CharacterRelationship');
//            const existing = (cache.CharacterRelationship || {}) as Record<number, CharacterRelationship>;
//            result.CharacterRelationship = mergeIntoCache(existing, relationships) as Record<number, CharacterRelationship>;
//        } catch (err) {
//            console.error('❌ Failed to load Character Relationships', err);
//        }

//        try {
//            const dates = await EntityFetcher.fetchAll('Date');
//            const existing = (cache.Date || {}) as Record<number, Date>;
//            result.Date = mergeIntoCache(existing, dates) as Record<number, Date>;
//        } catch (err) {
//            console.error('❌ Failed to load Dates', err);
//        }

//        try {
//            const prices = await EntityFetcher.fetchAll('PriceExample');
//            const existing = (cache.PriceExample || {}) as Record<number, PriceExample>;
//            result.PriceExample = mergeIntoCache(existing, prices) as Record<number, PriceExample>;
//        } catch (err) {
//            console.error('❌ Failed to load Price Examples', err);
//        }

//        try {
//            const turns = await EntityFetcher.fetchAll('ConversationTurn');
//            const existing = (cache.ConversationTurn || {}) as Record<number, ConversationTurn>;
//            result.ConversationTurn = mergeIntoCache(existing, turns) as Record<number, ConversationTurn>;
//        } catch (err) {
//            console.error('❌ Failed to load Conversation Turns', err);
//        }

//        // 💾 Commit all to Zustand
//        set({ cache: result });
//    },

//    async deleteAllOfType<T extends keyof EntityMap>(type: T) {
//        const ids = Object.keys(get().cache[type] || {}).map(Number)
//        for (const id of ids) {
//            await EntityDeleter.delete(type, id)
//        }
//        set(state => {
//            const newCache: CacheState = { ...state.cache }
//            delete newCache[type]
//            return { cache: newCache }
//        })
//    },


//    async deleteAll() {
//        const cache = get().cache
//        for (const type in cache) {
//            const typed = type as keyof EntityMap
//            const ids = Object.keys(cache[typed] || {}).map(Number)
//            for (const id of ids) {
//                await EntityDeleter.delete(typed, id)
//            }
//        }
//        set({ cache: {} })
//    },


//    updateAllOfType(type, data) {
//        set(state => ({
//            cache: {
//                ...state.cache,
//                [type]: mergeIntoCache(state.cache[type] || {}, data),
//            },
//        }))
//    },
 



//    getAllOfType(type) {
//        return get().getAll(type)
//    },

//    getTypeById(type, id) {
//        return get().get(type, id)
//    },

//    setField<T extends keyof EntityMap>(
//        type: T,
//        id: number,
//        field: keyof EntityMap[T],
//        value: EntityMap[T][typeof field]
//    ) {
//        set(state => {
//            const entity = state.cache[type]?.[id]
//            if (!entity) return state
//            return {
//                cache: {
//                    ...state.cache,
//                    [type]: {
//                        ...state.cache[type],
//                        [id]: {
//                            ...entity,
//                            [field]: value
//                        }
//                    }
//                }
//            }
//        })
//    },


//    setFieldToNull(type, id, field) {
//        set(state => {
//            const entity = state.cache[type]?.[id]
//            if (!entity) return state
//            return {
//                cache: {
//                    ...state.cache,
//                    [type]: {
//                        ...state.cache[type],
//                        [id]: {
//                            ...entity,
//                            [field]: null,
//                        },
//                    },
//                },
//            }
//        })
//    },






//    async persistEntity(type, id) {
//        const entity = get().cache[type]?.[id]
//        if (entity) {
//            await EntityUpdater.update(type, id, entity)
//        }
//    },

//    async persistField(type, id, field) {
//        const entity = get().cache[type]?.[id]
//        if (entity && field in entity) {
//            await EntityUpdater.update(type, id, {
//                [field]: entity[field],
//            } as Partial<EntityMap[typeof type]>)
//        }
//    },

 


//    async persistAll() {
//        const { cache } = get()
//        for (const type in cache) {
//            const typed = type as keyof EntityMap
//            const items = cache[typed] || {}
//            for (const id in items) {
//                const entity = items[+id]
//                await EntityUpdater.update(typed, +id, entity)
//            }
//        }
//    },

//    async deleteEntity(type, id) {
//        await EntityDeleter.delete(type, id)
//        set(state => {
//            // eslint-disable-next-line @typescript-eslint/no-unused-vars
//            const { [id]: _, ...rest } = state.cache[type] || {}
//            return {
//                cache: {
//                    ...state.cache,
//                    [type]: rest,
//                },
//            }
//        })
//    },


//    getForeignKeys(type: keyof typeof EntitySchemas) {
//        return EntitySchemas[type].fields.filter(
//            (f): f is Extract<typeof f, { type: 'fk' | 'multiFk'; fkType: EntityType }> =>
//                (f.type === 'fk' || f.type === 'multiFk') && 'fkType' in f && !!f.fkType
//        )
//            .map(f => ({
//                key: f.key,
//                fkType: f.fkType,
//            }))
//            .filter(f => f.fkType !== undefined) as { key: string; fkType: EntityType }[]
//    },


//    getReverseLinks(targetType: keyof typeof EntitySchemas): [string, string][] {
//        const result: [string, string][] = []

//        for (const sourceType in EntitySchemas) {
//            const schema = EntitySchemas[sourceType as keyof typeof EntitySchemas]
//            for (const field of schema.fields) {
//                if (
//                    (field.type === 'fk' || field.type === 'multiFk') &&
//                    'fkType' in field &&
//                    field.fkType === targetType
//                ) {
//                    result.push([sourceType, field.key])
//                }
//            }
//        }

//        return result

//    },



//}))

//export default useEntityRegistry



//       //async persistAllOfType<T extends keyof EntityMap>(type: T) {
//    //    const items = get().cache[type] || {};
//    //    for (const id in items) {
//    //        const entity = items[+id] as EntityMap[T];
//    //        await EntityUpdater.update(type, +id, entity);
//    //    }
////},



////getEntitiesLinkedTo(type: keyof EntityMap, id: number) {
////    const registry = get()
////    const results: Record<string, EntityMap[keyof EntityMap][]> = {}

////    for (const sourceType in EntitySchemas) {
////        const schema = EntitySchemas[sourceType as keyof typeof EntitySchemas]
////        for (const field of schema.fields) {
////            if (
////                (field.type === 'fk' || field.type === 'multiFk') &&
////                'fkType' in field &&
////                field.fkType === type
////            ) {
////                const matches = registry.getAll(sourceType as keyof EntityMap).filter(e => e[field.key] === id)
////                if (matches.length) {
////                    results[sourceType] ??= []
////                    results[sourceType].push(...matches)
////                }
////            }
////        }
////    }

////    return results
////},


////getLinkedEntities<T extends keyof EntityMap>(
////    type: T,
////    id: number,
////    depth = 2,
////    visited = new Set<string>()
////): Record<string, EntityMap[keyof EntityMap][]> {
////    const registry = get()
////    const entity = registry.get(type, id)
////    if (!entity || depth <= 0) return {}

////    const results: Record<string, EntityMap[keyof EntityMap][]> = {}
////    const entityKey = `${type}:${id}`
////    if (visited.has(entityKey)) return results
////    visited.add(entityKey)

////    const schema = EntitySchemas[type as keyof typeof EntitySchemas]
////    for (const field of schema.fields) {
////        if (
////            (field.type === 'fk' || field.type === 'multiFk') &&
////            'fkType' in field &&
////            field.fkType
////        ) {
////            const allTypes = Object.keys(EntitySchemas) as (keyof EntityMap)[];
////            const targetId = entity[field.key] as number
////            const target = registry.get(field.fkType, targetId)
////            if (target) {
////                results[field.key] = [target]
////                Object.assign(
////                    results,
////                    registry.getLinkedEntities(field.fkType, targetId, depth - 1, visited)
////                )
////            }
////        }
////    }

////    for (const otherType in EntitySchemas) {
////        const otherSchema = EntitySchemas[otherType as keyof typeof EntitySchemas]
////        for (const field of otherSchema.fields) {
////            if (
////                (field.type === 'fk' || field.type === 'multiFk') &&
////                'fkType' in field &&
////                field.fkType === type
////            ) {
////                const related = registry.getAll(otherType as keyof EntityMap).filter(e => e[field.key] === id)
////                if (related.length > 0) {
////                    results[`linkedFrom:${otherType}`] = related
////                    for (const r of related) {
////                        Object.assign(
////                            results,
////                            registry.getLinkedEntities(otherType as keyof EntityMap, r.id, depth - 1, visited)
////                        )
////                    }
////                }
////            }
////        }
////    }

////    return results
////}



////async resolveForeignKeys<T extends keyof EntityMap>(type: T, entity: EntityMap[T]): Promise<void> {
////    const schema = EntitySchemas[type];
////    const registry = get();

////    for (const field of schema.fields) {
////        if (field.type === 'fk' && 'fkType' in field && field.fkType) {
////            const fkType = field.fkType as keyof EntityMap;
////            const id = entity[field.key] as number;
////            if (typeof id === 'number' && !registry.get(fkType, id)) {
////                const [fetched] = await EntityFetcher.fetchById(fkType, id);
////                if (fetched) registry.update(fkType, fetched.id, fetched);
////            }
////        } else if (field.type === 'multiFk' && 'fkType' in field && field.fkType) {
////            const fkType = field.fkType as keyof EntityMap;
////            const raw = entity[field.key];
////            const ids = Array.isArray(raw) ? raw.filter((x): x is number => typeof x === 'number') : [];
////            const missing = ids.filter(id => !registry.get(fkType, id));

////            if (missing.length > 0) {
////                const fetched = await EntityFetcher.fetchById(fkType, missing);
////                for (const fkEntity of fetched) {
////                    registry.update(fkType, fkEntity.id, fkEntity);
////                }
////            }
////        }
////    }
////},



////updateAll(allData: Partial<CacheState>) {
////    set(state => {
////        const newCache: Partial<CacheState> = { ...state.cache };

////        for (const type in allData) {
////            const typed = type as keyof EntityMap;
////            const current = (state.cache[typed] || {}) as Record<number, EntityMap[typeof typed]>;
////            const incoming = (allData[typed] || {}) as Record<number, EntityMap[typeof typed]>;

////            newCache[typed] = {
////                ...current,
////                ...incoming
////            } as Record<number, EntityMap[typeof typed]>;
////        }

////        return { cache: newCache as CacheState };
////    });
////},
