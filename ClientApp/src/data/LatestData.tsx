import { useEffect, useState } from "react";
import { Table } from "reactstrap";
import { DataRecord } from "./DataModels";


export const LatestData = () => {
    const [data, setData] = useState<DataRecord[]>();

    useEffect(() => {
        fetch("api/latest")
            .then(response => response.json())
            .then(data => setData(data))
    }, [])

    if (!data) {
        return <p>Loading</p>
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
                    <td>{row.machineId} - {row.key}</td>
                    <td>{row.value}</td>
                </tr>
            )}
        </tbody>
    </Table>

}