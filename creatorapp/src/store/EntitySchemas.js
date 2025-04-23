
export const entitySchemas = {
    Character: {
        primaryDisplay: (e) => `${e.name}${e.alias ? ` (${e.alias})` : ''}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: false },
            { key: 'alias', label: 'Alias', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'role', label: 'Role', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'magic', label: 'Magic', type: 'text', section: 'header', showInCompact: true, editable: true },

            { key: 'factionId', label: 'Faction', type: 'fk', fkType: 'Faction', section: 'relation', showInCompact: false, editable: true },
            { key: 'locationId', label: 'Location', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true },
            { key: 'languageId', label: 'Language', type: 'fk', fkType: 'Language', section: 'relation', showInCompact: false, editable: true },
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },
            { key: 'birthDateId', label: 'Birth Date', type: 'fk', fkType: 'Date', section: 'details', showInCompact: false, editable: false },

            { key: 'personality', label: 'Personality', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'socialStatus', label: 'Social Status', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'occupation', label: 'Occupation', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'desire', label: 'Desire', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'fear', label: 'Fear', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'weakness', label: 'Weakness', type: 'textarea', section: 'summary', showInCompact: false, editable: false },
            { key: 'motivation', label: 'Motivation', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'flaw', label: 'Flaw', type: 'textarea', section: 'summary', showInCompact: false, editable: false },
            { key: 'misbelief', label: 'Misbelief', type: 'textarea', section: 'summary', showInCompact: false, editable: false },
            { key: 'definingFeatures', label: 'Defining Features', type: 'text', section: 'summary', showInCompact: false, editable: true },

            { key: 'gender', label: 'Gender', type: 'text', section: 'details', showInCompact: false, editable: false },
            { key: 'heightCm', label: 'Height (cm)', type: 'number', section: 'details', showInCompact: false, editable: true },
            { key: 'build', label: 'Build', type: 'text', section: 'details', showInCompact: false, editable: true },
            { key: 'hair', label: 'Hair', type: 'text', section: 'details', showInCompact: false, editable: true },
            { key: 'eyes', label: 'Eyes', type: 'text', section: 'details', showInCompact: false, editable: false },
        ],
    },

    Faction: {
        primaryDisplay: (e) => `${e.name}${e.alias ? ` (${e.alias})` : ''}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'alias', label: 'Alias', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'magic', label: 'Magic', type: 'text', section: 'header', showInCompact: false, editable: true },
            { key: 'foundingYear', label: 'Founding Year', type: 'number', section: 'header', showInCompact: false, editable: true },

            { key: 'founderId', label: 'Founder', type: 'fk', fkType: 'Character', section: 'relation', showInCompact: false, editable: true },
            { key: 'leaderId', label: 'Leader', type: 'fk', fkType: 'Character', section: 'relation', showInCompact: false, editable: true },
            { key: 'hqLocationId', label: 'HQ Location', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true },
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },
        ],
    },

    Item: {
        primaryDisplay: (e) => `${e.name}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'origin', label: 'Origin', type: 'text', section: 'header', showInCompact: false, editable: true },
            { key: 'effects', label: 'Effects', type: 'textarea', section: 'header', showInCompact: false, editable: true },

            { key: 'ownerId', label: 'Owner', type: 'fk', fkType: 'Character', section: 'relation', showInCompact: true, editable: true },
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },
        ],
    },

    Language: {
        primaryDisplay: (e) => `${e.name || 'Unnamed Language'}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true }, //header
            { key: 'type', label: 'Type', type: 'text', section: 'header', showInCompact: true, editable: true }, //header section
            { key: 'locationIds', label: 'Spoken In', type: 'multiFk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true }, //relation section
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true }, //also into the relation section, defines into which row the card will go on a page - but doesn't need to be displayed
            { key: 'Text', label: 'Text', type: 'textarea', section: 'summary', showInCompact: false, editable: true }, //summary section}
        ],
    },

    Location: {
        primaryDisplay: (e) => `${e.name} (${e.type})`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'type', label: 'Type', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'biome', label: 'Biome', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'cultures', label: 'Cultures', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'politics', label: 'Politics', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'totalPopulation', label: 'Population', type: 'number', section: 'summary', showInCompact: false, editable: true },
            { key: 'divineMagicians', label: 'Divine Mages', type: 'number', section: 'summary', showInCompact: false, editable: true },
            { key: 'wildMagicians', label: 'Wild Mages', type: 'number', section: 'summary', showInCompact: false, editable: true },

            { key: 'parentLocationId', label: 'Parent Location', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true },
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },

            { key: 'Languages', label: 'Languages Spoken', type: 'multiFk', fkType: 'Language', section: 'relation', showInCompact: false, editable: true },
            { key: 'Events', label: 'Events', type: 'multiFk', fkType: 'Event', section: 'relation', showInCompact: false, editable: true },
        ],
    },

    Event: {
        primaryDisplay: (e) => `${e.name} (${e.purpose})`, // How it will be displayed
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'description', label: 'Description', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'purpose', label: 'Purpose', type: 'text', section: 'summary', showInCompact: true, editable: true },

            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },
            { key: 'locationId', label: 'Location', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true },

            { key: 'startDateId', label: 'Start Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'endDateId', label: 'End Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
        ],
    },

    PlotPoint: {
        primaryDisplay: (e) => `${e.title}`,
        fields: [
            { key: 'title', label: 'Title', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'description', label: 'Description', type: 'textarea', section: 'summary', showInCompact: false, editable: true },

            { key: 'startDateId', label: 'Start Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'endDateId', label: 'End Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'ChapterPlotPoints', label: 'Chapters', type: 'multiFk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },

            { key: 'events', label: 'Events', type: 'multiFk', fkType: 'Event', section: 'relation', showInCompact: false, editable: false },

            { key: 'plotPointRivers', label: 'Rivers', type: 'multiFk', fkType: 'River', section: 'relation', showInCompact: false, editable: true },
            { key: 'plotPointRoutes', label: 'Routes', type: 'multiFk', fkType: 'Route', section: 'relation', showInCompact: false, editable: true }, 
        ],
    },


    Chapter: {
        primaryDisplay: (e) => `Chapter ${e.chapterNumber}: ${e.chapterTitle}`, // Display Title
        fields: [
            { key: 'chapterNumber', label: 'Chapter Number', type: 'number', section: 'header', showInCompact: true, editable: true },
            { key: 'chapterTitle', label: 'Title', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'wordCount', label: 'Word Count', type: 'number', section: 'header', showInCompact: false, editable: true },

            { key: 'actId', label: 'Act', type: 'fk', fkType: 'Act', section: 'relation', showInCompact: false, editable: true },
            { key: 'povCharacterId', label: 'POV Character', type: 'fk', fkType: 'Character', section: 'relation', showInCompact: false, editable: true },
            { key: 'chapterPlotPoints', label: 'PlotPoints', type: 'multiFk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },
            { key: 'startDateId', label: 'Start Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'endDateId', label: 'End Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'scenes', label: 'Scenes', type: 'multiFk', fkType: 'Scene', section: 'relation', showInCompact: false, editable: true },

            { key: 'chapterText', label: 'Text', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'chapterSummary', label: 'Summary', type: 'textarea', section: 'summary', showInCompact: false, editable: true },
            { key: 'toDo', label: 'To Do', type: 'textarea', section: 'summary', showInCompact: false, editable: true },

        ],
    },

    Act: {
        primaryDisplay: (e) => `Act ${e.actNumber}: ${e.actTitle}`,
        fields: [
            { key: 'actNumber', label: 'Act Number', type: 'number', section: 'header', showInCompact: true, editable: true }, // Header Section
            { key: 'actTitle', label: 'Title', type: 'text', section: 'header', showInCompact: true, editable: true }, // Header Section
            { key: 'actWordCount', label: 'Word Count', type: 'number', section: 'header', showInCompact: false, editable: true }, // Header Section

            { key: 'bookId', label: 'Book', type: 'fk', fkType: 'Book', section: 'relation', showInCompact: false, editable: true }, // Foreign Key to Book
            { key: 'chapters', label: 'Chapters', type: 'multiFk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true }, // Foreign Key to Chapter, Multiple Chapters

            { key: 'actSummary', label: 'Summary', type: 'textarea', section: 'summary', showInCompact: false, editable: true }, // Summary Section
            { key: 'actToDo', label: 'To-Do', type: 'textarea', section: 'summary', showInCompact: false, editable: true }, // To-do Section
        ],
    },

    Book: {
        primaryDisplay: (e) => `Book ${e.bookNumber} - ${e.bookTitle}: ${e.seriesTitle}`,
        fields: [
            { key: 'bookNumber', label: 'Book Number', type: 'number', section: 'header', showInCompact: true, editable: true }, // Header Section
            { key: 'seriesTitle', label: 'Series Title', type: 'text', section: 'header', showInCompact: true, editable: true }, // Header Section
            { key: 'bookTitle', label: 'Book Title', type: 'text', section: 'header', showInCompact: true, editable: true }, // Header Section
            { key: 'bookWordCount', label: 'Word Count', type: 'number', section: 'header', showInCompact: false, editable: true }, // Header Section

            { key: 'acts', label: 'Acts', type: 'multiFk', fkType: 'Act', section: 'relation', showInCompact: false, editable: true }, // Foreign Key to Act, Multiple Acts

            { key: 'bookSummary', label: 'Summary', type: 'textarea', section: 'summary', showInCompact: false, editable: true }, // Summary Section
            { key: 'bookToDo', label: 'To-Do', type: 'textarea', section: 'summary', showInCompact: false, editable: true }, // To-do Section
        ],
    },

    Date: {
        primaryDisplay: (e) => `${e.weekday}, ${e.month} ${e.day}, ${e.year}`, // This is how the date entry is displayed
        fields: [
            { key: 'day', label: 'Day', type: 'number', section: 'header', showInCompact: true, editable: true }, // The day of the date entry
            { key: 'weekday', label: 'Weekday', type: 'text', section: 'header', showInCompact: true, editable: true }, // The weekday name
            { key: 'month', label: 'Month', type: 'text', section: 'header', showInCompact: true, editable: true }, // The month name
            { key: 'year', label: 'Year', type: 'number', section: 'header', showInCompact: true, editable: true }, // The year
        ],
    },

    CharacterRelationship: {
        primaryDisplay: (e) =>
            `${e.Character1?.name || '???'} > ${e.relationshipType || '?'} < ${e.Character2?.name || '???'}`,
        fields: [
            { key: 'character1Id', label: 'Character 1', type: 'fk', fkType: 'Character', section: 'header', showInCompact: true, editable: true },
            { key: 'character2Id', label: 'Character 2', type: 'fk', fkType: 'Character', section: 'header', showInCompact: true, editable: true },
            { key: 'relationshipType', label: 'Type', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'relationshipDynamic', label: 'Dynamic', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },
        ],
    },

    ConversationTurn: {
        primaryDisplay: (e) => `${e.prompt?.slice(0, 20)}...`,
        fields: [
            { key: 'prompt', label: 'Prompt', type: 'textarea', section: 'details', showInCompact: false, editable: false }, // details section
            { key: 'response', label: 'Response', type: 'textarea', section: 'details', showInCompact: false, editable: false }, // details section
            { key: 'role', label: 'Role', type: 'text', section: 'summary', showInCompact: false, editable: false }, // summary section
            { key: 'danMode', label: 'Dan Mode', type: 'boolean', section: 'summary', showInCompact: false, editable: false }, // summary section
            { key: 'isSummary', label: 'Is Summary', type: 'boolean', section: 'summary', showInCompact: false, editable: false }, // summary section
            { key: 'tokenCount', label: 'Token Count', type: 'number', section: 'summary', showInCompact: false, editable: false }, // summary section
            { key: 'summaryLevel', label: 'Summary Level', type: 'number', section: 'summary', showInCompact: false, editable: false }, // summary section
            { key: 'timestamp', label: 'Timestamp', type: 'text', section: 'summary', showInCompact: false, editable: false }, // summary section
            { key: 'plotPointId', label: 'PlotPoint', type: 'fk', fkType: 'PlotPoint', section: 'relation', showInCompact: false, editable: false }, // relation section
            { key: 'parentId', label: 'Parent Turn', type: 'fk', fkType: 'ConversationTurn', section: 'relation', showInCompact: false, editable: false } // relation section
        ],
    },


    Currency: {
        primaryDisplay: (e) => `${e.name ?? 'Unnamed Currency'}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: false, editable: true }, // header section
            { key: 'crown', label: 'Crown', type: 'number', section: 'header', showInCompact: false, editable: true }, // header section
            { key: 'shilling', label: 'Shilling', type: 'number', section: 'header', showInCompact: false, editable: true }, // header section
            { key: 'penny', label: 'Penny', type: 'number', section: 'header', showInCompact: false, editable: true } // header section
        ],
    },


    Era: {
        primaryDisplay: (e) => `${e.name ?? 'Unnamed Era'}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'magicSystem', label: 'Magic System', type: 'text', section: 'header', showInCompact: false, editable: true },
            { key: 'magicStatus', label: 'Magic Status', type: 'text', section: 'header', showInCompact: false, editable: true },

            { key: 'startDateId', label: 'Start Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'endDateId', label: 'End Date', type: 'fk', fkType: 'Date', section: 'relation', showInCompact: true, editable: true },
            { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true },

            { key: 'description', label: 'Description', type: 'textarea', section: 'summary', showInCompact: false, editable: true }
        ],
    },


    PriceExample: {
        primaryDisplay: (e) => `${e.name ?? 'Unnamed Item'}${e.category ? ` (${e.category})` : ''}`,
        fields: [
            { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'category', label: 'Category', type: 'text', section: 'header', showInCompact: true, editable: true },
            { key: 'exclusivity', label: 'Exclusivity', type: 'text', section: 'summary', showInCompact: false, editable: true },
            { key: 'price', label: 'Price', type: 'number', section: 'summary', showInCompact: true, editable: true }
        ],
    },

        River: {
            primaryDisplay: (e) => `${e.name ?? 'Unnamed River'}`,
            fields: [
                { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
                { key: 'depthMeters', label: 'Depth (m)', type: 'number', section: 'header', showInCompact: false, editable: true },
                { key: 'widthMeters', label: 'Width (m)', type: 'number', section: 'header', showInCompact: false, editable: true },
                { key: 'flowDirection', label: 'Flow Direction', type: 'text', section: 'summary', showInCompact: false, editable: true },

                { key: 'sourceLocationId', label: 'Source Location', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true },
                { key: 'destinationLocationId', label: 'Destination Location', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true }
            ],
        },


        Route: {
            primaryDisplay: (e) => `${e.name ?? 'Unnamed Route'}`,
            fields: [
                { key: 'name', label: 'Name', type: 'text', section: 'header', showInCompact: true, editable: true },
                { key: 'type', label: 'Type', type: 'text', section: 'header', showInCompact: false, editable: true },
                { key: 'length', label: 'Length (km)', type: 'number', section: 'header', showInCompact: false, editable: true },
                { key: 'travelTime', label: 'Travel Time', type: 'text', section: 'summary', showInCompact: false, editable: true },
                { key: 'notes', label: 'Notes', type: 'textarea', section: 'summary', showInCompact: false, editable: true },

                { key: 'fromId', label: 'From', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true },
                { key: 'toId', label: 'To', type: 'fk', fkType: 'Location', section: 'relation', showInCompact: false, editable: true }
            ],
        },


        Scene: {
            primaryDisplay: (e) => `Scene ${e.sceneNumber ?? ''} - ${e.sceneTitle ?? 'Untitled'}`,
            fields: [
                { key: 'sceneNumber', label: 'Scene Number', type: 'number', section: 'header', showInCompact: true, editable: true },
                { key: 'sceneTitle', label: 'Title', type: 'text', section: 'header', showInCompact: true, editable: true },
                { key: 'sceneWordCount', label: 'Word Count', type: 'number', section: 'header', showInCompact: false, editable: false },

                { key: 'sceneText', label: 'Text', type: 'textarea', section: 'details', showInCompact: false, editable: true },
                { key: 'sceneSummary', label: 'Summary', type: 'textarea', section: 'details', showInCompact: false, editable: true },
                { key: 'sceneToDo', label: 'To Do', type: 'textarea', section: 'summary', showInCompact: false, editable: true },

                { key: 'chapterId', label: 'Chapter', type: 'fk', fkType: 'Chapter', section: 'relation', showInCompact: false, editable: true }
            ],
        }
    }
    ;

// Add this at the bottom of your entitySchemas.js file
export const entitySchemaFieldMap = {};

for (const [entityType, schema] of Object.entries(entitySchemas)) {
    entitySchemaFieldMap[entityType] = Object.fromEntries(
        schema.fields.map((field) => [field.key, field])
    );
}

export function getFieldsForDisplay(entityType, displayMode) {
    const fields = getFields(entityType);

    if (displayMode === 'compact') {
        return fields.filter(f => f.showInCompact);
    }

    if (displayMode === 'date') {
        return fields.filter(f => f.key === 'startDateId' || f.key === 'endDateId');
    }

    return fields; // default to all
}
