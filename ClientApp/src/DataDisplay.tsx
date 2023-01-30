import { useEffect, useState } from "react";

interface DataRecord {
    id: number;
    when: Date;
    machineId: string;
    key: string;
    value: number;
}

export const DataDisplay = () => {
    const [data, setData] = useState<DataRecord[]>();

    useEffect(() => {
        fetch("api/getAll")
            .then(response => response.json())
            .then(data => setData(data))
    }, [])

    if (!data) {
        return <p>Loading</p>
    }

    return <table>
        <thead>
            <tr>
                <th>When</th>                
                <th>Machine Id</th>
                <th>Key</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            {data.map(record =>
                <tr key={record.id}>
                    <td>{new Date(record.when).toISOString()}</td>
                    <td>{record.machineId}</td>
                    <td>{record.key}</td>
                    <td>{record.value}</td>
                </tr>
            )}
        </tbody>
    </table>
}