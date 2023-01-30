import './App.css';
import { DataDisplay } from './DataDisplay';

export const App = () =>
    <div className="App">
        <header className="App-header">
            <p>House Monitoring</p>
        </header>
        <div>
            <DataDisplay />
        </div>
    </div>
