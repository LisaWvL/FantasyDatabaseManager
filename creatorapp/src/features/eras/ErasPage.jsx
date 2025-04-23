import '../entities/ModularEntityPage.css';
import ModularEntityPage from '../entities/ModularEntityPage';

export default function ErasPage() {
    return <ModularEntityPage cardEntity="Era" sectionEntity="Chapter" updateFK="chapterId" />;
}


