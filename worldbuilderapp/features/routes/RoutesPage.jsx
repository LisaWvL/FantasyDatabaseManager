import '../../features/entities/EntityPage.css';
import ModularEntityPage from '../../features/entities/ModularEntityPage';


export default function RoutesPage() {
    return <ModularEntityPage cardEntity="Route" sectionEntity="Location" updateFK="fromId" />;
}

//add tabs where i can switch between assigning FromLocation and ToLocation, or split the layout into two columns, one represents From the other To
//another tab or create a new route