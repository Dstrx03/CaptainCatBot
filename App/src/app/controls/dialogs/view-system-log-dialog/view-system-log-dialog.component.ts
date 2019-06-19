import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SystemLoggingService } from 'src/app/services/system-logging/system-logging.service';
import { SystemLogEntry } from 'src/app/models/systemLogEntry';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-view-system-log-dialog',
  templateUrl: './view-system-log-dialog.component.html',
  styleUrls: ['./view-system-log-dialog.component.scss']
})
export class ViewSystemLogDialogComponent implements OnInit {

  private getSubscription: Subscription;
  private cleanSubscription: Subscription;

  logEntries: SystemLogEntry[];
  loadingEntries = false;

  @ViewChild('cleanLogBtn') cleanLogBtn;
  cleanThresholdOptions = [
    { c: 'Delete All', v: -1 },
    { c: '5 minutes', v: 60*5 },
    { c: '15 minutes', v: 60*15 },
    { c: '30 minutes', v: 60*30 },
    { c: '1 hour', v: 60*60 },
    { c: '2 hours', v: 60*60*2 },
    { c: '4 hours', v: 60*60*4 },
    { c: '8 hours', v: 60*60*8 },
    { c: '12 hours', v: 60*60*12 },
    { c: '24 hours', v: 60*60*24 },
    { c: '48 hours', v: 60*60*48 },
    { c: '3 days', v: 60*60*24*3 },
    { c: '5 days', v: 60*60*24*5 },
    { c: '7 days', v: 60*60*24*7 },
    { c: '2 weeks', v: 60*60*24*7*2 },
    { c: '4 weeks', v: 60*60*24*7*4 }
  ]
  cleanThreshold = this.cleanThresholdOptions[14];

  constructor(
    public dialogRef: MatDialogRef<ViewSystemLogDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ViewSystemLogDialogData,
    private loggingSvc: SystemLoggingService,
    private snackBar: MatSnackBar,
  ) {
    this.logEntries = [];
    this.loadingEntries = false;
  }

  clean() {
    this.cleanLogBtn.options.active = true;
    const submittedThreshold = this.cleanThreshold;
    this.getSubscription = this.loggingSvc.clean(this.data.descriptor, submittedThreshold.v).subscribe((result: SystemLogEntry[]) => {
      this.cleanLogBtn.options.active = false;
      this.logEntries = result;
      const snackMsg = submittedThreshold.v === -1 ? 'Removed all entries.' : `Removed entries older than ${submittedThreshold.c}.`;
      this.snackBar.open(snackMsg, 'Ok', { duration: 5000 });
    })
  }

  ngOnInit() {
    this.loadingEntries = true;
    this.cleanSubscription = this.loggingSvc.getEntries(this.data.descriptor).subscribe((result: SystemLogEntry[]) => {
      this.logEntries = result;
      this.loadingEntries = false;
    });
  }

  ngOnDestroy() {
    if (this.getSubscription !== undefined) this.getSubscription.unsubscribe();
    if (this.cleanSubscription !== undefined) this.cleanSubscription.unsubscribe();
  }

}

export class ViewSystemLogDialogData {
  descriptor: string;
  name: string;
}
