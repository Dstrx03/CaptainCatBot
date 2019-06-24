import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'yesNo'
})
export class YesNoPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (typeof value === 'boolean') {
      return value ? 'Yes' : 'No';
    }
    return '';
  }

}
