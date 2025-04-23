// FactionsPage.tsx
import '../../features/entities/EntityPage.css';
import ModularEntityPage from '../../features/entities/ModularEntityPage';

export default function FactionsPage() {
    return <ModularEntityPage cardEntity="Character" sectionEntity="Faction" updateFK="leaderId" />;
}