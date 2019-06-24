import { Component, OnInit, ViewChild } from '@angular/core';
import { TelegramStatusService } from 'src/app/services/telegram-status/telegram-status.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-telegram-status',
  templateUrl: './telegram-status.component.html',
  styleUrls: ['./telegram-status.component.scss']
})
export class TelegramStatusComponent implements OnInit {

  disableControlButtons: boolean = false;

  @ViewChild('registerClientBtn') registerClientBtn;
  @ViewChild('unregisterClientBtn') unregisterClientBtn;
  @ViewChild('registerWebhookBtn') registerWebhookBtn;
  @ViewChild('unregisterWebhookBtn') unregisterWebhookBtn;
  @ViewChild('checkWebhookBtn') checkWebhookBtn;
  @ViewChild('updateWebhookBtn') updateWebhookBtn;

  constructor(private telegramStatusSvc: TelegramStatusService, private snackBar: MatSnackBar) { }

  registerClient() {
    this.disableControls(this.registerClientBtn);
    this.telegramStatusSvc.registerClient().subscribe(_ => {
      this.enableControls(this.registerClientBtn);
    });
  }

  unregisterClient() {
    this.disableControls(this.unregisterClientBtn);
    this.telegramStatusSvc.unregisterClient().subscribe(_ => {
      this.enableControls(this.unregisterClientBtn);
    });
  }

  registerWebhook() {
    this.disableControls(this.registerWebhookBtn);
    this.telegramStatusSvc.registerWebhook().subscribe(_ => {
      this.enableControls(this.registerWebhookBtn);
    });
  }

  unregisterWebhook() {
    this.disableControls(this.unregisterWebhookBtn);
    this.telegramStatusSvc.unregisterWebhook().subscribe(_ => {
      this.enableControls(this.unregisterWebhookBtn);
    });
  }

  checkWebhook() {
    this.disableControls(this.checkWebhookBtn);
    this.telegramStatusSvc.checkWebhook().subscribe(_ => {
      this.enableControls(this.checkWebhookBtn);
    });
  }

  updateWebhook() {
    this.disableControls(this.updateWebhookBtn);
    this.telegramStatusSvc.updateWebhook().subscribe(_ => {
      this.enableControls(this.updateWebhookBtn);
    });
  }

  ngOnInit() {
  }

  private disableControls(btn: any) {
    btn.options.active = true;
    this.disableControlButtons = true;
  }

  private enableControls(btn: any) {
    btn.options.active = false;
    this.disableControlButtons = false;
    this.snackBar.open(`'${btn.options.text}' procedure completed. Check log for details.`, 'Ok', { duration: 5000 });
  }

}
