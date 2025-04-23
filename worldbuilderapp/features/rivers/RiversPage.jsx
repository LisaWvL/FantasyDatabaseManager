import '../../features/entities/EntityPage.css';
import ModularEntityPage from '../../features/entities/ModularEntityPage';


export default function RiversPage() {
    return <ModularEntityPage cardEntity="River" sectionEntity="Location" updateFK="sourceId" />;
}

//add tabs where i can switch between assigning SourceLocation and DestinationLocation or create a new river