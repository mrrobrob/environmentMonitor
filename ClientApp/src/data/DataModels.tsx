
export interface DataRecord {
    id: number;
    when: string;
    value: number;
    dataSourceId: number;
}

export interface DataSource {
    id: number;
    machineId: string;
    key: string;
}