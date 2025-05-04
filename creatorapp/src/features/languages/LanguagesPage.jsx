import '../../features/entities/ModularEntityPage.css';
import ModularEntityPage from '../../features/entities/ModularEntityPage';


//add header assign Language to Character
export default function LanguagesPage() {
    return <ModularEntityPage cardEntity="Character" sectionEntity="Language" updateFK="languageId" />;
}

//add header assign language to location
//add a toggle to switch between assigning languages to a character, or languages to a Location
