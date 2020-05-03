import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'telegramServiceStatus'
})
export class TelegramServiceStatusPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    switch(value as number){
      case 1: return "Stopped";
      case 2: return "Running";
      case 3: return "Ok";
      case 4: return "Error";
      case 5: return "Error (Internal)";
      case 0: 
      default:
        return "Unknown";
    }
  }

}
