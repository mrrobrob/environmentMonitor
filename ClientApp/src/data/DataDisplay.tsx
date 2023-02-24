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
    const [dataRecords, setDataRecords] = useState<DataRecord[]>();
    const [dataSources, setDataSources] = useState<DataSource[]>();

    const [machineFilter, setMachineFilter] = React.useState("");
    const [keyFilter, setKeyFilter] = React.useState("");
    const now = new Date();
    const [dateTo, setDateTo] = React.useState(new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59, 59));
    const [dateFrom, setDateFrom] = React.useState(new Date(now.getFullYear(), now.getMonth(), now.getDate()));

    const refreshData = React.useCallback(() => {

        const queryString = new URLSearchParams({
            Machine: machineFilter,
            Key: keyFilter,
            From: dateFrom.toISOString(),
            To: dateTo.toISOString(),
        });

        fetch("api/dataRecord/getAll?" + queryString)
            .then(response => response.json())
            .then(data => setDataRecords(data))
    }, [keyFilter, machineFilter, dateTo, dateFrom, setDataRecords]);

    useEffect(() => {
        refreshData();
    }, [refreshData])

    useEffect(() => {
        fetch("api/dataSource/getAll")
            .then(response => response.json())
            .then(dataSources => setDataSources(dataSources))
    }, []);

    if (!dataRecords || !dataSources) {
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

    const groupedData = dataRecords
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

    const handleMachineFilterChange = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const { value } = ev.currentTarget;

        setMachineFilter(value);
    }

    const handleKeyFilterChange = (ev: React.ChangeEvent<HTMLInputElement>) => {
        const { value } = ev.currentTarget;

        setKeyFilter(value);
    }

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
            <label>Machine: <input type="text" onChange={handleMachineFilterChange} value={machineFilter} /></label>
            <label>Key: <input type="text" onChange={handleKeyFilterChange} value={keyFilter} /></label>
            <label>From: <input type="date" onChange={handleDateFromChange} value={dateFromStr} /> </label>
            <label>To: <input type="date" onChange={handleDateToChange} value={dateToStr} /> </label>
            <button onClick={refreshData}>Refresh</button>
        </div>
        <div style={{ minHeight: 400 }}>
            <Line options={options} data={chartData} />
        </div>
    </>
}