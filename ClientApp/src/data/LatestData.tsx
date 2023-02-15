import { useEffect, useState } from "react";
import { Table } from "reactstrap";
import { DataRecord, DataSource } from "./DataModels";


export const LatestData = () => {
    const [dataSources, setDataSources] = useState<DataSource[]>();
    const [data, setData] = useState<DataRecord[]>();

    useEffect(() => {
        fetch("api/dataRecord/latest")
            .then(response => response.json())
            .then(dataRecords => setData(dataRecords))
    }, []);

    useEffect(() => {
        fetch("api/dataSource/getAll")
            .then(response => response.json())
            .then(dataSources => setDataSources(dataSources))
    }, []);

    if (!data || !dataSources) {
        return <p>Loading</p>
    }

    const getDataSource = (id: number) => {
        const ds = dataSources.find(e => e.id === id);

        if (!ds) {
            throw new Error(`DataSource not found for id ${id}`);
        }

        return `${ds.machineId} - ${ds.key}`;
    }

    return <Table>
        <thead>
            <tr>
                <th>When</th>
                <th>Identifier</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            {data.map(row =>
                <tr>
                    <td>{row.when}</td>
                    <td>{getDataSource(row.dataSourceId)}</td>
                    <td>{row.value}</td>
                </tr>
            )}
        </tbody>
    </Table>

}