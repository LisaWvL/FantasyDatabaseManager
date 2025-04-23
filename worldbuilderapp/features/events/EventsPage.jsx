import '../entities/ModularEntityPage.css';
import ModularEntityPage from '../entities/ModularEntityPage';

export default function EventsPage() {
    return <ModularEntityPage cardEntity="Event" sectionEntity="Chapter" updateFK="chapterId" />;
}