// CharactersPage.jsx
import ModularEntityPage from '../entities/ModularEntityPage';

export default function CharactersPage() {
    return (
        unassignedSidebar = {
            entityType: 'Character',
            items: [], // You should fetch and pass unassigned cards here
            isUnassigned: (card) => !card.ChapterId,
            // same renderItem, onDropToUnassigned etc. logic
        })
} < ModularEntityPage cardEntity="Character" sectionEntity="Chapter" updateFK="ChapterId" />;

