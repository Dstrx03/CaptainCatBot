import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'telegramServiceStatus'
})
export class TelegramServiceStatusPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    switch(value as number){
      case 1: return "Stopped";
      case 2: return "Running";
      case 0: 
      default:
        return "Unknown";
    }
  }

}
