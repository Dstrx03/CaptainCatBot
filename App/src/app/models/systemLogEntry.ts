export interface SystemLogEntry {
    Id: string;
    EntryDescriptor: string;
    EntryDate: Date;
    Entry: string;
}

export interface SystemLogEntriesPackage {
    Entries: SystemLogEntry[];
    IsLast: boolean;
}