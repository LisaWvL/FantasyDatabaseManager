import '../entities/ModularEntityPage.css';
import ModularEntityPage from '../entities/ModularEntityPage';

export default function ItemsPage() {
    return <ModularEntityPage cardEntity="Item" sectionEntity="Character" updateFK="ownerId" />;
}
