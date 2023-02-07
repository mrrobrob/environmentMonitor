import { DataDisplay } from "./data/DataDisplay";
import { LatestData } from "./data/LatestData";

const AppRoutes = [
    {
        index: true,
        element: <DataDisplay />
    },
    {
        path: '/latest',
        element: <LatestData />
    }
];

export default AppRoutes;
