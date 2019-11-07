import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SystemLoggingService } from 'src/app/services/system-logging/system-logging.service';
import { SystemLogEntry, SystemLogEntriesPackage } from 'src/app/models/systemLogEntry';
import { Subscription } from 'rxjs';
import { MatSnackBar } from '@angular/material';
import { SignalrConnectionService } from 'src/app/services/signalr/signalr-connection.service';
import { SignalrHub } from 'src/app/services/signalr/signalr-hub.service';

@Component({
  selector: 'app-view-system-log-dialog',
  templateUrl: './view-system-log-dialog.component.html',
  styleUrls: ['./view-system-log-dialog.component.scss']
})
export class ViewSystemLogDialogComponent implements OnInit {

  private getSubscription: Subscription;
  private cleanSubscription: Subscription;
  private entryAddedSubscription: Subscription;
  private entriesRemovedSubscription: Subscription;

  private hub: SignalrHub;

  logEntries: SystemLogEntry[];
  loadingEntries = false;
  allEntriesLoaded = false;
  entriesLoadCount = 20;

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
    private signalrConnectionSvc: SignalrConnectionService
  ) {
    this.logEntries = [];
    this.loadingEntries = false;
    this.hub = this.signalrConnectionSvc.createHub(`${data.descriptor}SystemLoggingServiceHub`);
  }

  cleanEntries() {
    this.cleanLogBtn.options.active = true;
    const submittedThreshold = this.cleanThreshold;
    this.cleanSubscription = this.loggingSvc.clean(this.data.descriptor, submittedThreshold.v, this.logEntries.length).subscribe((result: SystemLogEntriesPackage) => {
      this.cleanLogBtn.options.active = false;
      this.addEntries(result.Entries);
      this.allEntriesLoaded = result.IsLast;
      const snackMsg = submittedThreshold.v === -1 ? 'Removed all entries.' : `Removed entries older than ${submittedThreshold.c}.`;
      this.snackBar.open(snackMsg, 'Ok', { duration: 5000 });
    });
  }

  loadNextEntries() {
    this.loadEntries(this.logEntries[this.logEntries.length - 1].Id, this.entriesLoadCount);
  }

  private loadEntries(lastEntryId: string, count: number) {
    this.loadingEntries = true;
    this.getSubscription = this.loggingSvc.getNextEntries(this.data.descriptor, lastEntryId, count).subscribe((result: SystemLogEntriesPackage) => {
      this.addEntries(result.Entries);
      this.allEntriesLoaded = result.IsLast;
      this.loadingEntries = false;
    });
  }

  private sortEntries() {
    this.logEntries.sort((a: SystemLogEntry, b: SystemLogEntry) => {
      const at: number = a.EntryDate !== undefined && a.EntryDate !== null ? new Date(a.EntryDate).getTime() : 0;
      const bt: number = b.EntryDate !== undefined && b.EntryDate !== null ? new Date(b.EntryDate).getTime() : 0;
      return bt - at;
    });
  }

  private addEntries(entries: SystemLogEntry[]) {
    var lastLoadedEntries = [];
    entries.forEach(x => { 
      if (this.logEntries.find(z => z.Id == x.Id) === undefined) 
        lastLoadedEntries.push(x) 
    });
    this.logEntries = this.logEntries.concat(lastLoadedEntries);
    this.sortEntries();
  }

  private removeEntries(ids: string[]) {
    ids.forEach(id => {
      const index = this.logEntries.findIndex(x => x.Id === id);
      if (index > -1) this.logEntries.splice(index, 1);
    });
  }

  ngOnInit() {
    this.loadEntries(null, this.entriesLoadCount);
    this.hub.listen('entryAdded').then(o => {
      this.entryAddedSubscription = o.subscribe(res => {
        this.addEntries([res[0] as SystemLogEntry])
      })
    });
    this.hub.listen('entriesRemoved').then(o => {
      this.entriesRemovedSubscription = o.subscribe(res => {
        this.removeEntries(res[0]);
      })
    });
  }

  ngOnDestroy() {
    if (this.getSubscription !== undefined) this.getSubscription.unsubscribe();
    if (this.cleanSubscription !== undefined) this.cleanSubscription.unsubscribe();
    if (this.entryAddedSubscription !== undefined) this.entryAddedSubscription.unsubscribe();
    if (this.entriesRemovedSubscription !== undefined) this.entriesRemovedSubscription.unsubscribe();
    this.hub.disconnect();
  }

}

export class ViewSystemLogDialogData {
  descriptor: string;
  name: string;
}
