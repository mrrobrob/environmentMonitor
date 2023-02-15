import { useEffect, useState } from "react";
import * as React from "react";
import { Line } from "react-chartjs-2";
import 'chartjs-adapter-date-fns';

import {
    Chart as ChartJS,
    Colors,
    LinearScale,
    TimeScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend,
    Decimation,
} from 'chart.js';
import { DataRecord, DataSource } from "./DataModels";

ChartJS.register(
    Colors,
    Decimation,
    TimeScale,
    LinearScale,
    PointElement,
    LineElement,
    Title,
    Tooltip,
    Legend
);

export const DataDisplay = () => {
    const [data, setData] = useState<DataRecord[]>();
    const [dataSources, setDataSources] = useState<DataSource[]>();

    const [dateTo, setDateTo] = React.useState(new Date());
    const [dateFrom, setDateFrom] = React.useState(new Date(dateTo.getFullYear(), dateTo.getMonth(), dateTo.getDate()));

    useEffect(() => {
        fetch("api/dataRecord/getAll")
            .then(response => response.json())
            .then(data => setData(data))
    }, [])

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

    const options = {        
        responsive: true,
        maintainAspectRatio: false,
        scales: {
            x: {
                type: "time" as const,
                time: {
                    unit: 'minute' as const,
                    displayFormats: {
                        minute: 'yyyy-MM-dd HH:mm'
                    }
                }
            }
        },
        plugins: {
            legend: {
                position: 'right' as const,
            },
            decimation: {
                algorithm: 'lttb' as const,
                enabled: true,
                samples: 150
            }
        },
    };

    const groupedData = data
        .filter(e => {
            const when = new Date(e.when);
            return when > dateFrom && when < dateTo;
        })
        .reduce((result: any, dataRecord) => {
            const key = getDataSource(dataRecord.dataSourceId);
            return {
                ...result,
                [key]: [...(result[key] || []), dataRecord]
            };
        }, {});

    const chartData = {
        datasets: Object.keys(groupedData).map(key => {

            const dataRecords: DataRecord[] = groupedData[key];

            return {
                label: key,
                data: dataRecords.map(dataRecord => ({ x: dataRecord.when, y: dataRecord.value }))
            }
        }),
    };

    const handleDateFromChange = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const date = ev.currentTarget.valueAsDate;

        if (!date) {
            return;
        }

        date.setHours(0, 0, 0, 0);
        setDateFrom(date);
    }

    const handleDateToChange = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const date = ev.currentTarget.valueAsDate;
        if (!date) {
            return;
        }
        date.setHours(23, 59, 59, 999);
        setDateTo(date);
    }

    const dateFromStr = dateFrom.toISOString().substring(0, 10);
    const dateToStr = dateTo.toISOString().substring(0, 10);

    return <>
        <div>
            <label>From: <input type="date" onChange={handleDateFromChange} value={dateFromStr} /> </label>
            <label>To: <input type="date" onChange={handleDateToChange} value={dateToStr} /> </label>
        </div>
        <div style={{minHeight:400}}>
            <Line options={options} data={chartData} />            
        </div>
    </>
}