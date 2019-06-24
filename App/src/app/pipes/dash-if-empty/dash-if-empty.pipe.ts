import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dashIfEmpty'
})
export class DashIfEmptyPipe implements PipeTransform {

  dash: string = '-';

  transform(value: any, args?: any): any {
    return (value !== undefined && value !== null) ? 
      (typeof value === 'string') ? 
        (value.length > 0) ? 
          value : 
          this.dash : 
        value : 
      this.dash;
  }

}
