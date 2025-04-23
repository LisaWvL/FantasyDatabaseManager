// src/stores/EntityRegistry.js
import { create } from 'zustand';
import { EntityFetcher, EntityUpdater } from './EntityManager';

// All entity types used in the app
const ENTITY_TYPES = [
    'Act', 'Book', 'Calendar', 'Chapter', 'Character', 'CharacterRelationship',
    'ConversationTurn', 'Currency', 'Era', 'Event', 'Faction', 'Item',
    'Language', 'Location', 'PlotPoint', 'PriceExample', 'River', 'Route', 'Scene'
];

const useEntityRegistry = create((set, get) => ({
    cache: {},

    async load(type) {
        const items = await EntityFetcher.fetchAll(type);
        set(state => ({
            cache: {
                ...state.cache,
                [type]: Object.fromEntries(items.map(i => [i.id, i]))
            }
        }));
    },

    get(type, id) {
        return get().cache?.[type]?.[id] ?? null;
    },

    update(type, id, data) {
        EntityUpdater.update(type, id, data);
        set(state => {
            const updated = { ...state.cache[type][id], ...data };
            return {
                cache: {
                    ...state.cache,
                    [type]: {
                        ...state.cache[type],
                        [id]: updated
                    }
                }
            };
        });
    },

    setFieldToNull(type, id, field) {
        EntityUpdater.setNull(type, id, field);
        set(state => {
            const entity = state.cache[type]?.[id];
            if (!entity) return {};
            return {
                cache: {
                    ...state.cache,
                    [type]: {
                        ...state.cache[type],
                        [id]: {
                            ...entity,
                            [field]: null
                        }
                    }
                }
            };
        });
    },



    // ✅ Load everything (e.g. full app prefetch)
    async loadAll() {
        const fullCache = {};
        for (const type of ENTITY_TYPES) {
            try {
                const items = await EntityFetcher.fetchAll(type);
                fullCache[type] = Object.fromEntries(items.map(i => [i.id, i]));
            } catch (err) {
                console.error(`❌ Failed to load ${type}:`, err);
            }
        }
        set({ cache: fullCache });
    },

    // ✅ Targeted load: Dashboard
    async loadDashboard() {
        const types = ['PlotPoint', 'Event', 'Chapter', 'Era'];
        for (const type of types) {
            try {
                const items = await EntityFetcher.fetchAll(type);
                set(state => ({
                    cache: {
                        ...state.cache,
                        [type]: Object.fromEntries(items.map(i => [i.id, i]))
                    }
                }));
            } catch (err) {
                console.error(`❌ Failed to load ${type} for dashboard`, err);
            }
        }
    },

    // ✅ Targeted load: Writing Assistant (everything)
    async loadWritingAssistant() {
        await get().loadAll();
    },

    // ✅ Targeted load: Timeline Story View
    async loadTimeline() {
        const types = ['PlotPoint', 'Chapter', 'Era'];
        for (const type of types) {
            try {
                const items = await EntityFetcher.fetchAll(type);
                set(state => ({
                    cache: {
                        ...state.cache,
                        [type]: Object.fromEntries(items.map(i => [i.id, i]))
                    }
                }));
            } catch (err) {
                console.error(`❌ Failed to load ${type} for timeline`, err);
            }
        }
    },

    // 🔮 FUTURE: Delete from cache manually after server deletion
    // delete(type, id) {
    //     set(state => {
    //         const { [id]: _, ...rest } = state.cache[type] || {};
    //         return {
    //             cache: {
    //                 ...state.cache,
    //                 [type]: rest
    //             }
    //         };
    //     });
    // },

    // 🔮 FUTURE: Replace all items of a type (used for filters or search results)
    // replace(type, items) {
    //     set(state => ({
    //         cache: {
    //             ...state.cache,
    //             [type]: Object.fromEntries(items.map(i => [i.id, i]))
    //         }
    //     }));
    // },
}));

export default useEntityRegistry;
