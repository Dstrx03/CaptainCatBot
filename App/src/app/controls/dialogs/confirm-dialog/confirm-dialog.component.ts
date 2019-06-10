import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {

  @ViewChild('okSpinnerBtn') okSpinnerBtn;

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) { }

  applyAgent(): void {
    this.okSpinnerBtn.options.active = true;
    this.dialogRef.disableClose = true;
    this.data.agent.subscribe(_ => this.dialogRef.close());
  }

  ngOnInit() {
  }

}

export class ConfirmDialogData {
  header: string;
  contentHtml: string;
  useAgent: boolean;
  agent: Observable<any>;
}
