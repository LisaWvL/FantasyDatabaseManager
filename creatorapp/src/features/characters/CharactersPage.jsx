// CharacterPage.jsx

import '../entities/ModularEntityPage.css';
import ModularEntityPage from '../entities/ModularEntityPage';

export default function CharactersPage() {
    return <ModularEntityPage cardEntity="Character" sectionEntity="Chapter" updateFK="ChapterId" />;
}
