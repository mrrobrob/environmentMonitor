import { Route, Routes } from 'react-router-dom';
import './App.css';
import AppRoutes from './AppRoutes';
import { Header } from './Header';

export const App = () =>
    <div className="App">
        <Header />
        <Routes>
            {AppRoutes.map((route, index) => {
                const { element, ...rest } = route;
                return <Route key={index} {...rest} element={element} />;
            })}
        </Routes>
    </div>
