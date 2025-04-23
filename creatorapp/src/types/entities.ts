// 📁 src/types/entities.ts

// ────────────────────────────────
// 👑 Characters
// ────────────────────────────────
export interface Character{
  id: number;
  name: string;
  alias?: string;
  role?: string;
  magic?: string;
  
  factionId?: number | null; // FK to Faction
  Faction?: Faction; // Navigation Property to Faction
    locationId?: number | null; // FK to Location
    Location?: Location; // Navigation Property to Location
    languageId?: number | null; // FK to Language
    Language?: Language; // Navigation Property to Language
    chapterId?: number | null; // FK to Chapter
    Chapter?: Chapter; // Navigation Property to Chapter
    birthDateId?: number | null; // FK to Date
    birthDate?: Date; // Navigation Property to Date

  personality?: string;
  socialStatus?: string;
  occupation?: string;
  desire?: string;
  fear?: string;
  weakness?: string;
  motivation?: string;
  flaw?: string;
  misbelief?: string;
  definingfeatures?: string;

  gender?: string;
  heightCm?: number;
  build?: string;
  hair?: string;
  eyes?: string;
}

export interface Faction{
  id: number;
  name: string;
  alias?: string;
  foundingYear?: number;
  magic: string;

  founderId?: number  | null; // FK to Character
  Founder?: Character; // Navigation Property to Character (Founder)
  leaderId?: number  | null; // FK to Character
    Leader?: Character; // Navigation Property to Character (Leader)
  hqLocationId?: number  | null; // FK to Location
    HQLocation?: Location; // Navigation Property to Location (HQ)
  chapterId?: number  | null; // FK to Chapter
    Chapter?: Chapter; // Navigation Property to Chapter
}

export interface Language{
  id: number
  name: string
  type?: string
  locationIds?: number[]
    Locations?: Location[] // Many-to-Many relation with Location
  text?: string

}
export interface CharacterRelationship{
  id: number
  character1Id: number
    Character1?: Character // Navigation Property to Character
  character2Id: number
    Character2?: Character // Navigation Property to Character

  relationshipType: string
  relationshipDynamic?: string

  chapterId?: number
    Chapter?: Chapter // Navigation Property to Chapter
}

// ────────────────────────────────
// 🗓️ Timeline
// ────────────────────────────────

export interface PlotPoint {
  id: number;
  title: string;
  description?: string;

  startDateId?: number  | null;
  startDate?: Date;

  endDateId?: number  | null;
  endDate?: Date;

  events: Event[];
    Chapters?: Chapter[];

  plotPointRivers: PlotPointRiver[];
  plotPointRoutes: PlotPointRoute[];
}

export interface Event{
  id: number;
  name: string;
  description?: string;
  purpose?: string;

  chapterId?: number  | null; // FK to Chapter
    Chapter?: Chapter; // Navigation Property to Chapter

  locationId?: number  | null; // FK to Location
    Location?: Location; // Navigation Property to Location

  dateId?: number  | null; // FK to Date
    Date?: Date; // Navigation Property to Date
}

export interface Era {
  id: number;
  name?: string;
  description?: string;
  magicSystem?: string;
  magicStatus?: string;

  chapterId?: number|null;
    Chapter?: Chapter; // Navigation Property to Chapter
  startDateId?:  number  | null;
    StartDate?: Date; // Navigation Property to Date (Start Date)
  endDateId?:  number  | null;
    EndDate?: Date; // Navigation Property to Date (End Date)
}

export interface Date {
  id: number;
  day?: number; // The day of the date entry
  weekday?: string; // The weekday (e.g., "Monday", "Tuesday")
  month?: string; // The month (e.g., "January", "February")
    year?: number; // The year (e.g., 2021)

}


// ────────────────────────────────
// 🧰 Items & Economy
// ────────────────────────────────
export interface Item {
  id: number;
  name: string;
  origin?: string;
  effects?: string;

  ownerId?:  number  | null;  // FK to Character
    Owner?: Character;  // Navigation Property to Character (Owner)
  chapterId?:  number  | null;  // FK to Chapter
    Chapter?: Chapter;  // Navigation Property to Chapter
}

export interface Currency {
  id: number;
  name?: string;
  crown?: number;
  shilling?: number;
  penny?: number;
}

export interface PriceExample {
  id: number;
  category?: string;
  name?: string;
  exclusivity?: string;
  price?: number;
}

// ────────────────────────────────
// 🌍 Geography
// ────────────────────────────────
export interface Location {
  id: number;
  name: string;
  type?: string;
  biome?: string;
  cultures?: string;
  politics?: string;
  totalPopulation?: number;
  divineMagicians?: number;
  wildMagicians?: number;

  parentLocationId?:  number  | null; // FK to another Location (for nested locations)
    ParentLocation?: Location; // Navigation Property to another Location
  chapterId?:  number  | null; // FK to Chapter
    Chapter?: Chapter; // Navigation Property to Chapter

  LanguageLocations?: LanguageLocation[]; // Many-to-Many relation with Language
  Events?: Event[]; // One-to-Many relation with Event
}

export interface River {
  id: number;
  name?: string;
  depthMeters?: number;
  widthMeters?: number;
  flowDirection?: string;

  sourceLocationId?:  number  | null;
    SourceLocation?: Location;
  destinationLocationId?:  number  | null;
    DestinationLocation?: Location;
}

export interface Route {
  id: number;
  name?: string;
  type?: string;
  length?: number;
  notes?: string;
  travelTime?: string;

  fromId?:  number  | null;
    From?: Location;
  toId?:  number  | null;
    To?: Location;
}

// ────────────────────────────────
// ✍️ Writing Assistant
// ────────────────────────────────

export interface Chapter {
  id: number;
  chapterNumber?: number;
  chapterTitle?: string;
  chapterText?: string;
  wordCount?: number;
  chapterSummary?: string;
  toDo?: string;

  startDateId?:  number  | null; // FK to Date (Start Date)
    StartDate?: Date; // Navigation Property to Date (Start Date)

  endDateId?:  number  | null; // FK to Date (End Date)
    EndDate?: Date; // Navigation Property to Date (End Date)

  actId?:  number  | null; // FK to Act
    Act?: Act; // Navigation Property to Act

  povCharacterId?:  number  | null; // FK to Character (POV Character)
    POVCharacter?: Character; // Navigation Property to Character (POV Character)

  plotPointId?:  number  | null; // FK to PlotPoint
    PlotPoint?: PlotPoint; // Navigation Property to PlotPoint

  Scenes?: Scene[]; // Related Scenes
}

export interface PlotPointRiver {
    id: number;
    plotPointId: number; // FK to PlotPoint
    PlotPoint?: PlotPoint; // Navigation Property to PlotPoint
    riverId: number; // FK to River
    River?: River; // Navigation Property to River
}
export interface PlotPointRoute {
    id: number;
    plotPointId: number; // FK to PlotPoint
    PlotPoint?: PlotPoint; // Navigation Property to PlotPoint
    routeId: number; // FK to Route
    Route?: Route; // Navigation Property to Route
}
export interface LanguageLocation {
    id: number;
    languageId: number; // FK to Language
    Language?: Language; // Navigation Property to Language
    locationId: number; // FK to Location
    Location?: Location; // Navigation Property to Location
}

export interface ConversationTurn {
  id: number;

  prompt?: string;
  response?: string;
  danMode?: boolean;
  timestamp: string; // 🕒 ISO string (since you use DateTime in .NET)

  isSummary?: boolean;
  role?: 'system' | 'user' | 'assistant' | 'summary';
  tokenCount?: number;
  summaryLevel?: number;
  parentId?:  number  | null;

  plotPointId?:  number  | null; // FK to PlotPoint
    PlotPoint?: PlotPoint; // Navigation Property to PlotPoint
}

export interface Act{
  id: number;
  actTitle?: string; // Act Title
  actNumber?: number; // Act Number
  actSummary?: string; // Summary of the Act
  actToDo?: string; // To-do for the Act
  actWordCount?: number; // Word count for the Act

  bookId?:  number  | null; // Foreign key to Book
    Book?: Book; // Navigation Property to Book

    Chapters?: Chapter[]; // Collection of Chapters related to this Act
}

export interface Book  {
  id: number;
  bookNumber?: number; // The book number in the series
  seriesTitle?: string; // Title of the series this book is part of
  bookTitle?: string; // Title of the book
  bookWordCount?: number; // Word count of the book
  bookSummary?: string; // Summary of the book's content
  bookToDo?: string; // To-do list or notes for the book

    Acts?: Act[]; // Collection of Acts associated with this book
}

export interface Scene {
  id: number;
  sceneNumber?: number;
  sceneTitle?: string;
  sceneText?: string;
  sceneWordCount?: number;
  sceneSummary?: string;
  sceneToDo?: string;

  chapterId?:  number  | null;
    Chapter?: Chapter;
}


// ────────────────────────────────
// 🧠 Shared
// ────────────────────────────────
export type EntityType =
  | 'PlotPoint'
  | 'Act'
  | 'Book'
  | 'Scene'
  | 'Date'
  | 'CharacterRelationship'
  | 'PlotPoint'
  | 'Character'
  | 'Currency'
  | 'Faction'
  | 'Item'
  | 'Event'
  | 'Era'
  | 'Chapter'
  | 'Language'
  | 'Location'
  | 'River'
  | 'Route'
  | 'PriceExample'
  | 'ConversationTurn'

export type EntityMap = {
    PlotPoint: PlotPoint
    Act: Act
    Book: Book
    Scene: Scene
    River: River
    Route: Route
    Character: Character
    Currency: Currency
    Chapter: Chapter
    Era: Era
    Event: Event
    Item: Item
    Location: Location
    Language: Language
    Faction: Faction
    CharacterRelationship: CharacterRelationship
    Date: Date
    PriceExample: PriceExample
    ConversationTurn: ConversationTurn
}


export type ForeignKeyMap = {
    [K in keyof EntityMap]?: (keyof EntityMap)[]
}

export interface BaseEntity {
  id: number
  [key: string]: unknown
}

export type CacheState = {
    [K in keyof EntityMap]?: Record<number, EntityMap[K]>
}