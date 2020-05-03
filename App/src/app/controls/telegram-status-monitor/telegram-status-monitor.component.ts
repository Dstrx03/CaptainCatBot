import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ViewSystemLogDialogComponent } from '../dialogs/view-system-log-dialog/view-system-log-dialog.component';
import { TelegramStatusViewModel } from 'src/app/models/telegramStatusViewModel';
import { TelegramStatusService } from 'src/app/services/telegram-status/telegram-status.service';
import { SignalrHub } from 'src/app/services/signalr/signalr-hub.service';
import { SignalrConnectionService } from 'src/app/services/signalr/signalr-connection.service';
import { Subscription } from 'rxjs';
import { tap, mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-telegram-status-monitor',
  templateUrl: './telegram-status-monitor.component.html',
  styleUrls: ['./telegram-status-monitor.component.scss']
})
export class TelegramStatusMonitorComponent implements OnInit, OnDestroy {

  status: TelegramStatusViewModel;
  loading: boolean = false;

  private hub: SignalrHub;

  private statusSubscription: Subscription;
  private hubSubscription: Subscription;

  constructor(
      public dialog: MatDialog, 
      private telegramStatusSvc: TelegramStatusService,
      private signalrConnectionSvc: SignalrConnectionService) {
    this.status = {} as TelegramStatusViewModel;
    this.hub = this.signalrConnectionSvc.createHub("TelegramBotHub");
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
    switch(status) {
      case 0: 
      case 1:
      case 4: 
      case 5: return "warn-color";
      case 2:
      case 3: return "accent-color";
      default:
        return "";
    }
  }

  private updateStatusInfo(){
    this.loading = true;
    this.statusSubscription = this.telegramStatusSvc.getTelegramStatusInfo()
      .subscribe(telegramStatusInfo => {
        this.status = telegramStatusInfo;
        this.loading = false;
    });
  }

  ngOnInit() {
    this.updateStatusInfo();
    this.hub.listen('statusInfoUpdated').then(o => {
      this.hubSubscription = o.pipe(
        mergeMap((res: boolean) => this.telegramStatusSvc.getTelegramStatusInfo()),
        tap(telegramStatusInfo => this.status = telegramStatusInfo)
      ).subscribe();
    });
  }

  ngOnDestroy() {
    if (this.statusSubscription !== undefined) this.statusSubscription.unsubscribe();
    if (this.hubSubscription !== undefined) this.hubSubscription.unsubscribe();
    this.hub.disconnect();
  }

}
