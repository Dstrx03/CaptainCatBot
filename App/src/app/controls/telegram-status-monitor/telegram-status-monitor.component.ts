import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ViewSystemLogDialogComponent } from '../dialogs/view-system-log-dialog/view-system-log-dialog.component';
import { TelegramStatusViewModel } from 'src/app/models/telegramStatusViewModel';
import { TelegramStatusService } from 'src/app/services/telegram-status/telegram-status.service';

@Component({
  selector: 'app-telegram-status-monitor',
  templateUrl: './telegram-status-monitor.component.html',
  styleUrls: ['./telegram-status-monitor.component.scss']
})
export class TelegramStatusMonitorComponent implements OnInit {

  status: TelegramStatusViewModel;
  loading: boolean = false;

  constructor(public dialog: MatDialog, private telegramStatusSvc: TelegramStatusService) {
    this.status = {} as TelegramStatusViewModel;
  }

  viewLog() {
    this.dialog.open(ViewSystemLogDialogComponent, {
      autoFocus: false,
      minWidth: '63vw',
      data: {
        descriptor: 'TelegramBot',
        name: 'Telegram'
      }
    });
  }

  getTelSvcStatusClass(status: number) {
    switch(status){
      case 0: 
      case 1: return "warn-color";
      case 2: return "accent-color";
      default:
        return "";
    }
  }

  ngOnInit() {
    this.loading = true;
    this.telegramStatusSvc.getTelegramStatusInfo()
      .subscribe(telegramStatusInfo => {
        this.status = telegramStatusInfo;
        this.loading = false;
      });
  }

}
