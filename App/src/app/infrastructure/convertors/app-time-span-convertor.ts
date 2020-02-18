export class AppTimeSpanConvertor {

    constructor() {}

    readonly appTimeSpanKindOptions: AppTimeSpanKind[] = [
        { caption: 'minute(s)', value: 'm' },
        { caption: 'hour(s)', value: 'h' },
        { caption: 'day(s)', value: 'd' },
        { caption: 'week(s)', value: 'w' },
    ];

    convertFromString(timeSpanString: string): AppTimeSpan {
        let regexp = new RegExp(/^\d+[mhdw]$/);
        if (!regexp.test(timeSpanString)) return new AppTimeSpan({value: null, kind: null});

        let valueParsed = +timeSpanString.slice(0, timeSpanString.length - 1);
        let kindParsed = timeSpanString.slice(timeSpanString.length - 1);
        let kind = this.appTimeSpanKindOptions.find(x => x.value == kindParsed);
        
        return new AppTimeSpan({ value: valueParsed, kind: kind });
    }

    convertFromTimeSpan(appTimeSpan: AppTimeSpan): string {
        if (!appTimeSpan || !appTimeSpan.value || !appTimeSpan.kind || !appTimeSpan.kind.value) return null;
        return appTimeSpan.value + appTimeSpan.kind.value;
    }

}

export class AppTimeSpan {
    value: number;
    kind: AppTimeSpanKind;

    constructor(init?:Partial<AppTimeSpan>) {
        Object.assign(this, init);
    }
}

export class AppTimeSpanKind {
    caption: string;
    value: string;
}