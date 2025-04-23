// 📁 src/stores/EntityRegistry.ts
import { EntityFetcher } from '@/store/EntityManager'
import { EntitySchemas } from '@/store/EntitySchemas'
import { ForeignKeyDefinition } from '@/store/EntitySchemas'
import useEntityRegistry from '@/store/useEntityRegistry' // Assuming this is your zustand hook
import { EntityMap, CacheState } from '@/types/entities'




export type RegistryStore = {
  cache: CacheState

  // Cache Getters
  get: <T extends keyof EntityMap>(type: T, id: number) => EntityMap[T] | null
  getAll: <T extends keyof EntityMap>(type: T) => EntityMap[T][]
  getAllById: <T extends keyof EntityMap>(type: T, ids: number[]) => EntityMap[T][]
  getAllOfType: <T extends keyof EntityMap>(type: T) => EntityMap[T][]
  getTypeById: <T extends keyof EntityMap>(type: T, id: number) => EntityMap[T] | null

  // Cache Setters
  update: <T extends keyof EntityMap>(type: T, id: number, data: Partial<EntityMap[T]>) => void
  updateAllOfType: <T extends keyof EntityMap>(type: T, data: EntityMap[T][]) => void
  updateAll: (allData: Partial<CacheState>) => void
  setFieldToNull: <T extends keyof EntityMap>(type: T, id: number, field: keyof EntityMap[T]) => void
  setField: <T extends keyof EntityMap>(type: T, id: number, field: keyof EntityMap[T], value: any) => void


  // Database Sync
  loadAllIntoCache: () => Promise<void>
  persistEntity: <T extends keyof EntityMap>(type: T, id: number) => Promise<void>
  persistField: <T extends keyof EntityMap>(type: T, id: number, field: keyof EntityMap[T]) => Promise<void>
  persistAllOfType: <T extends keyof EntityMap>(type: T) => Promise<void>
  persistAll: () => Promise<void>
  deleteEntity: <T extends keyof EntityMap>(type: T, id: number) => Promise<void>
  deleteAllOfType: <T extends keyof EntityMap>(type: T) => Promise<void>
  deleteAll: () => Promise<void>

  //Foreign Keys and Relatoins and reverse links
    resolveForeignKeys: <T extends keyof EntityMap>(entityType: T, entity: EntityMap[T]) => Promise<void>
    getForeignKeys: (type: keyof typeof EntitySchemas) => ForeignKeyDefinition[]
    getReverseLinks: (targetType: keyof typeof EntitySchemas) => [string, string][]
    getEntitiesLinkedTo: (type: keyof EntityMap, id: number) => Record<string, EntityMap[keyof EntityMap][]>
    getLinkedEntities: <T extends keyof EntityMap>(type: T, id: number, depth?: number) => Record<string, EntityMap[keyof EntityMap][]>
}

