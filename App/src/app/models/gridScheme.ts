export interface GridScheme {
    Name: string;
    Columns: GridSchemeColumn[];
}

export interface GridSchemeColumn {
    ColumnId: string;
    Caption: string;
    Sortable: boolean;
    Visible: boolean;
    Searchable: boolean;
    Type: number;
    Actions: GridSchemeAction[];
}

export interface GridSchemeAction {
    Name: string;
    ButtonIconSet: string;
    ButtonIcon: string;
    ButtonColor: string;
    TooltipText: string;
}
