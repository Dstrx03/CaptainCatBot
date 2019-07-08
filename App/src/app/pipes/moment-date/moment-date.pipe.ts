import { Pipe, PipeTransform, Inject, LOCALE_ID } from '@angular/core';
import { DatePipe } from '@angular/common';
import { GlobalService } from 'src/app/infrastructure/global.service';
import * as moment from 'moment-timezone';

/**
 * A moment timezone pipe to support parsing based on time zone abbreviations
 * covers all cases of offset variation due to daylight saving.
 *
 * Same API as DatePipe with additional timezone abbreviation support
 * Official date pipe dropped support for abbreviations names from Angular V5
 */

@Pipe({
  name: 'momentDate'
})
export class MomentDatePipe extends DatePipe implements PipeTransform {

  constructor(@Inject(LOCALE_ID) locale: string, private globalSvc: GlobalService) {
    super(locale);
  }

  transform(value: string | Date, format: string = 'mediumDate', timezone: string = null): string {
    if (value === null || value === undefined ) return null;

    if (timezone === null) timezone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    if (timezone === 'host') timezone = this.globalSvc.serverTimezoneIana;

    const timezoneOffset = moment(value).tz(timezone).format('Z');

    if (value instanceof Date) return super.transform(value, format, timezoneOffset);

    const regexp = /^(\d){4}-(\d){2}-(\d){2}T(\d){2}:(\d){2}:(\d){2}\.(\d){1,7}([\+-])(\d){2}:(\d){2}$/;
    if (regexp.test(value)) value = value.substring(0, value.length - 6);

    const clientOffset = moment(value).tz(timezone).utcOffset();
    const serverOffset = moment(value).tz(this.globalSvc.serverTimezoneIana).utcOffset();

    const valueTransformed = moment.utc(value)
      .utcOffset(clientOffset - serverOffset)
      .format(`YYYY-MM-DDTHH:mm:ss${timezoneOffset}`);

    return super.transform(valueTransformed, format, timezoneOffset);
  }

}
