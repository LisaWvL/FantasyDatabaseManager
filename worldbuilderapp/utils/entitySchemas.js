// src/utils/entitySchemas.js

export const entitySchemas = {
  Character: {
    primaryDisplay: (e) => `${e.name}${e.alias ? ` (${e.alias})` : ''}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //goes into the header name atop of the card
      { key: 'alias', label: 'Alias', type: 'text', section: 'header' }, //also into the header if there's an alias put it in brackets behind the name
      { key: 'role', label: 'Role', type: 'text', section: 'header' }, //below the name in the header
      { key: 'magic', label: 'Magic', type: 'text', section: 'header' }, //also below the name in the header

      { key: 'factionId', label: 'Faction', type: 'fk', fkType: 'Faction', section: 'relation' }, //below the header is the relation section, this is where related entities go
      { key: 'locationId', label: 'Location', type: 'fk', fkType: 'Location', section: 'relation' }, //also into the relation section
      { key: 'languageId', label: 'Language', type: 'fk', fkType: 'Language', section: 'relation' }, //also into the relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed

      { key: 'personality', label: 'Personality', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'socialStatus', label: 'Social Status', type: 'text', section: 'summary' }, //also into the summary section
      { key: 'occupation', label: 'Occupation', type: 'text', section: 'summary' }, //also into the summary section
      { key: 'desire', label: 'Desire', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'fear', label: 'Fear', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'weakness', label: 'Weakness', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'motivation', label: 'Motivation', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'flaw', label: 'Flaw', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'misbelief', label: 'Misbelief', type: 'textarea', section: 'summary' }, //also into the summary section
      { key: 'definingFeatures', label: 'Features', type: 'text', section: 'summary' }, //also into the summary section

      { key: 'birthDay', label: 'Birth Day', type: 'number', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'birthMonth', label: 'Birth Month', type: 'text', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'birthYear', label: 'Birth Year', type: 'number', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'gender', label: 'Gender', type: 'text', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'heightCm', label: 'Height (cm)', type: 'number', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'build', label: 'Build', type: 'text', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'hair', label: 'Hair', type: 'text', section: 'details' }, //goes into the details section of the card (fold out)
      { key: 'eyes', label: 'Eyes', type: 'text', section: 'details' }, //goes into the details section of the card (fold out)
    ],
  },

  Faction: {
    primaryDisplay: (e) => `${e.name}${e.alias ? ` (${e.alias})` : ''}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'alias', label: 'Alias', type: 'text', section: 'header' }, //header in brackets after name
      { key: 'magic', label: 'Magic', type: 'text', section: 'header' }, //header
      { key: 'foundingYear', label: 'Founding Year', type: 'number', section: 'header' }, //header, can go in brackets after magic

      { key: 'founderId', label: 'Founder', type: 'fk', fkType: 'Character', section: 'relation' }, //relation section
      { key: 'leaderId', label: 'Leader', type: 'fk', fkType: 'Character', section: 'relation' }, //relation section
      {
        key: 'hqLocationId',
        label: 'HQ Location',
        type: 'fk',
        fkType: 'Location',
        section: 'relation',
      }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed
    ],
  },

  Item: {
    primaryDisplay: (e) => `${e.name}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'origin', label: 'Origin', type: 'text', section: 'header' }, //header
      { key: 'effects', label: 'Effects', type: 'textarea', section: 'header' }, //header

      { key: 'ownerId', label: 'Owner', type: 'fk', fkType: 'Character', section: 'relation' }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed
    ],
  },

  Language: {
    primaryDisplay: (e) => `${e.name || 'Unnamed Language'}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'type', label: 'Type', type: 'text', section: 'header' }, //header section

      {
        key: 'locationIds',
        label: 'Spoken In',
        type: 'multiFk',
        fkType: 'Location',
        section: 'relation',
      }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed

      { key: 'Text', label: 'Text', type: 'textarea', section: 'summary' }, //summary section}
    ],
  },

  Location: {
    primaryDisplay: (e) => `${e.name} (${e.type})`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'type', label: 'Type', type: 'text', section: 'header' }, //header

      {
        key: 'languageIds',
        label: 'Languages',
        type: 'multiFk',
        fkType: 'Language',
        section: 'relation',
      }, //relation section
      { key: 'eventIds', label: 'Events', type: 'multiFk', fkType: 'Event', section: 'relation' }, //relation section
      {
        key: 'parentLocationId',
        label: 'Parent Location',
        type: 'fk',
        fkType: 'Location',
        section: 'relation',
      }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed

      { key: 'biome', label: 'Biome', type: 'text', section: 'summary' }, //summary section
      { key: 'cultures', label: 'Cultures', type: 'textarea', section: 'summary' }, //summary section
      { key: 'politics', label: 'Politics', type: 'textarea', section: 'summary' }, //summary section
      { key: 'totalPopulation', label: 'Population', type: 'number', section: 'summary' }, //summary section
      { key: 'divineMagicians', label: 'Divine Mages', type: 'number', section: 'summary' }, //summary section
      { key: 'wildMagicians', label: 'Wild Mages', type: 'number', section: 'summary' }, //summary section
    ],
  },

  Event: {
    primaryDisplay: (e) => `${e.name} (${e.day} ${e.month}, ${e.year})`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'readableDate', label: 'Date', type: 'text', section: 'header' }, //header below the name

      { key: 'locationId', label: 'Location', type: 'fk', fkType: 'Location', section: 'relation' }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed

      { key: 'purpose', label: 'Purpose', type: 'text', section: 'summary' }, //summary section
      { key: 'description', label: 'Description', type: 'textarea', section: 'summary' }, //summary section
    ],
  },

  PlotPoint: {
    primaryDisplay: (e) => `${e.title}`,
    fields: [
      { key: 'title', label: 'Title', type: 'text', section: 'header' }, //header

      {
        key: 'startDateId',
        label: 'Start Date',
        type: 'fk',
        fkType: 'Calendar',
        section: 'header',
      }, //header, below the Title
        { key: 'endDateId', label: 'End Date', type: 'fk', fkType: 'Calendar', section: 'header' }, //header, below the Title
        { key: 'startDateId', label: 'Start Date', type: 'fk', fkType: 'Calendar', section: 'header' }, //header, below the Title

      { key: 'riverIds', label: 'Rivers', type: 'multiFk', fkType: 'River', section: 'relation' }, //relation section
      { key: 'routeIds', label: 'Routes', type: 'multiFk', fkType: 'Route', section: 'relation' }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed

      { key: 'description', label: 'Description', type: 'textarea', section: 'summary' }, //summary section
    ],
  },

  Chapter: {
    primaryDisplay: (e) => `Chapter ${e.chapterNumber}: ${e.chapterTitle}`,
    fields: [
      { key: 'chapterNumber', label: 'Number', type: 'number', section: 'header' }, //header
      { key: 'chapterTitle', label: 'Title', type: 'text', section: 'header' }, //header
      { key: 'wordCount', label: 'Word Count', type: 'number', section: 'header' }, //header

      { key: 'chapterText', label: 'Text', type: 'textarea', section: 'summary' }, //summary
      { key: 'chapterSummary', label: 'Summary', type: 'textarea', section: 'summary' }, //summary
      { key: 'toDo', label: 'To Do', type: 'textarea', section: 'summary' }, //summary

      { key: 'actId', label: 'Act', type: 'fk', fkType: 'Act', section: 'relation' }, //relation section
      { key: 'povCharacterId', label: 'POV', type: 'fk', fkType: 'Character', section: 'relation' }, //relation section
    ],
  },

  Act: {
    primaryDisplay: (e) => `Act ${e.actNumber}: ${e.actTitle}`,
    fields: [
      { key: 'actNumber', label: 'Act Number', type: 'number', section: 'header' }, //header
      { key: 'actTitle', label: 'Act Title', type: 'text', section: 'header' }, //header
      { key: 'wordCount', label: 'Word Count', type: 'number', section: 'header' }, //header
      { key: 'actText', label: 'Text', type: 'textarea', section: 'details' }, //details section
      { key: 'actSummary', label: 'Summary', type: 'textarea', section: 'details' }, //details section
      { key: 'toDo', label: 'To Do', type: 'textarea', section: 'summary' }, //summary
      { key: 'bookId', label: 'Book', type: 'fk', fkType: 'Book', section: 'relation' }, //relation section
    ],
  },

  Book: {
    primaryDisplay: (e) => `Book ${e.bookNumber}: ${e.bookTitle}`,
    fields: [
      { key: 'bookNumber', label: 'Book Number', type: 'number', section: 'header' }, //header
      { key: 'bookTitle', label: 'Book Title', type: 'text', section: 'header' }, //header
      { key: 'wordCount', label: 'Word Count', type: 'number', section: 'header' }, //header
      { key: 'bookText', label: 'Text', type: 'textarea', section: 'details' }, //details section
      { key: 'bookSummary', label: 'Summary', type: 'textarea', section: 'details' }, //details section
      { key: 'toDo', label: 'To Do', type: 'textarea', section: 'summary' }, //summary
      { key: 'seriesId', label: 'Series', type: 'fk', fkType: 'Series', section: 'relation' }, //relation section
    ],
  },

  Calendar: {
    primaryDisplay: (e) => `${e.month} ${e.day}, ${e.year}`,
    fields: [
      { key: 'day', label: 'Day', type: 'number', section: 'header' }, //header
      { key: 'month', label: 'Month', type: 'text', section: 'header' }, //header
      { key: 'year', label: 'Year', type: 'number', section: 'header' }, //header
      { key: 'weekday', label: 'Weekday', type: 'text', section: 'header' }, //header
      { key: 'description', label: 'Description', type: 'textarea', section: 'summary' }, //summary
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //relation section
    ],
  },

  CharacterRelationship: {
    primaryDisplay: (e) => `${e.character1Name} > ${e.relationshipType} < ${e.character2Name}`,
    fields: [
      {
        key: 'character1Id',
        label: 'Character 1',
        type: 'fk',
        fkType: 'Character',
        section: 'header',
      }, //header
      {
        key: 'character2Id',
        label: 'Character 2',
        type: 'fk',
        fkType: 'Character',
        section: 'header',
      }, //header
      { key: 'relationshipType', label: 'Type', type: 'text', section: 'header' }, //header
      { key: 'relationshipDynamic', label: 'Dynamic', type: 'text', section: 'summary' }, //summmary
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //relation section
    ],
  },

  ConversationTurn: {
    primaryDisplay: (e) => `${e.input?.slice(0, 20)}...`,
    fields: [
      {
        key: 'plotPointId',
        label: 'PlotPoint',
        type: 'fk',
        fkType: 'PlotPoint',
        section: 'header',
      }, //header
      { key: 'input', label: 'Input', type: 'textarea', section: 'details' }, //details section
      { key: 'response', label: 'Response', type: 'textarea', section: 'details' }, //details section
      { key: 'timestamp', label: 'Time', type: 'text', section: 'summary' }, //summary
    ],
  },

  Currency: {
    primaryDisplay: (e) => `${e.name}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'abbreviation', label: 'Abbreviation', type: 'text', section: 'header' }, //header
      { key: 'symbol', label: 'Symbol', type: 'text', section: 'header' }, //header
      { key: 'description', label: 'Description', type: 'textarea', section: 'summary' }, //summary
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //relation section
    ],
  },

  Era: {
    primaryDisplay: (e) => `${e.name}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'startYear', label: 'Start Year', type: 'number', section: 'header' }, //header
      { key: 'endYear', label: 'End Year', type: 'number', section: 'header' }, //header
      { key: 'description', label: 'Description', type: 'textarea', section: 'summary' }, //summary
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //relation section
    ],
  },

  PriceExample: {
    primaryDisplay: (e) => `${e.itemName} = ${e.amount} ${e.currencySymbol}`,
    fields: [
      { key: 'itemName', label: 'Item Name', type: 'text', section: 'header' }, //header
      { key: 'amount', label: 'Amount', type: 'number', section: 'header' }, //header
      { key: 'currencySymbol', label: 'Currency Symbol', type: 'text', section: 'header' }, //header
      { key: 'description', label: 'Description', type: 'textarea', section: 'summary' }, //summary
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //relation section
    ],
  },

  River: {
    primaryDisplay: (e) => `${e.name}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'lengthKm', label: 'Length (km)', type: 'number', section: 'header' }, //header
      {
        key: 'sourceLocationId',
        label: 'Source',
        type: 'fk',
        fkType: 'Location',
        section: 'relation',
      }, //relation section
      {
        key: 'mouthLocationId',
        label: 'Mouth',
        type: 'fk',
        fkType: 'Location',
        section: 'relation',
      }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed
    ],
  },

  Route: {
    primaryDisplay: (e) => `${e.name}`,
    fields: [
      { key: 'name', label: 'Name', type: 'text', section: 'header' }, //header
      { key: 'lengthKm', label: 'Length (km)', type: 'number', section: 'header' }, //header
      {
        key: 'startLocationId',
        label: 'Start',
        type: 'fk',
        fkType: 'Location',
        section: 'relation',
      }, //relation section
      { key: 'endLocationId', label: 'End', type: 'fk', fkType: 'Location', section: 'relation' }, //relation section
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed
    ],
  },

  Scene: {
    primaryDisplay: (e) => `Scene ${e.sceneNumber}`,
    fields: [
      { key: 'sceneNumber', label: 'Scene Number', type: 'number', section: 'header' }, //header
      { key: 'sceneTitle', label: 'Scene Title', type: 'text', section: 'header' }, //header
      { key: 'sceneText', label: 'Text', type: 'textarea', section: 'details' }, //details section
      { key: 'sceneSummary', label: 'Summary', type: 'textarea', section: 'details' }, //details section
      { key: 'toDo', label: 'To Do', type: 'textarea', section: 'summary' }, //summary
      { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation' }, //relation section
    ],
  },
};
