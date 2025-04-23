import React from 'react'
import PlotPointCard from '@/features/plotpoints/PlotPointCard'
import CharacterCard from '@/features/characters/CharacterCard'
// ... more imports

import type { EntityType, BaseEntity } from '@/types/entities'

interface Props {
  entity: BaseEntity
  entityType: EntityType
  displayMode?: 'compact' | 'full' | 'calendar'
  isGhost?: boolean
  isReversed?: boolean
  onContextMenu?: (e: React.MouseEvent, entity: BaseEntity) => void
}

export default function CardRenderer({
  entity,
  entityType,
  displayMode = 'compact',
  isGhost,
  isReversed,
  onContextMenu
}: Props) {
  switch (entityType.toLowerCase()) {
    case 'plotpoint':
      return (
        <PlotPointCard
          entity={entity}
          displayMode={displayMode}
          isGhost={isGhost}
          isReversed={isReversed}
          onContextMenu={onContextMenu}
        />
      )
    case 'character':
      return (
        <CharacterCard
          entity={entity}
          displayMode={displayMode}
          isGhost={isGhost}
          isReversed={isReversed}
          onContextMenu={onContextMenu}
        />
      )
    // Add more entity types here

    default:
      return <div>Unsupported entity type: {entityType}</div>
  }
}
