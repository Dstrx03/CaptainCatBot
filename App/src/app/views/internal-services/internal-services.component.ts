import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControlHelper } from 'src/app/infrastructure/helpers/form-control-helper';
import { FormControl, Validators} from '@angular/forms';
import { InternalServicesService } from '../../services/internal-services/internal-services.service';
import { RefresherSettings } from 'src/app/models/RefresherSettings';
import { ViewSystemLogDialogComponent } from 'src/app/controls/dialogs/view-system-log-dialog/view-system-log-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material';
import { ATriggerSettings } from 'src/app/models/atriggerSettings';
import { AppMatSpinnerButtonComponent } from 'src/app/controls/app-mat-spinner-button/app-mat-spinner-button.component';

@Component({
  selector: 'app-internal-services',
  templateUrl: './internal-services.component.html',
  styleUrls: ['./internal-services.component.scss']
})
export class InternalServicesComponent implements OnInit {

  loadingRefresherData: boolean = false;

  @ViewChild('saveRefresherBtn') saveRefresherBtn: AppMatSpinnerButtonComponent;

  refresherSettings: RefresherSettings;

  refresherEnabledFormControl = new FormControl();
  refresherIntervalFormControl = new FormControl('', [
    Validators.required,
    Validators.max(240),
    Validators.min(1)
  ]);
  refresherControlHelper = new FormControlHelper([this.refresherEnabledFormControl, this.refresherIntervalFormControl]);

  
  loadingATriggerData: boolean = false;

  @ViewChild('saveATriggerBtn') saveATriggerBtn: AppMatSpinnerButtonComponent;

  atriggerSettings: ATriggerSettings;

  atriggerEnabledFormControl = new FormControl();
  atriggerTimeSliceFormControl = new FormControl('', [
    Validators.required,
    Validators.max(240),
    Validators.min(1)
  ]);
  atriggerControlHelper = new FormControlHelper([this.atriggerEnabledFormControl, this.atriggerTimeSliceFormControl]);

  constructor(
      private internalSvcsSvc: InternalServicesService,
      public dialog: MatDialog,
      private snackBar: MatSnackBar) {

    this.loadingRefresherData = true;
    this.refresherControlHelper.disable();
    this.refresherSettings = {IsEnabled: false, IntervalMinutes: undefined};

    this.loadingATriggerData = true;
    this.atriggerControlHelper.disable();
    this.atriggerSettings = {IsEnabled: false, TimeSliceMinutes: undefined};
  }

  refresherSaveChanges() {
    this.saveRefresherBtn.options.active = true;
    this.internalSvcsSvc.refresherSaveSettings(this.refresherSettings).subscribe((result: RefresherSettings) => {
      this.refresherSettings = result;
      this.saveRefresherBtn.options.active = false;
      this.snackBar.open('Refresher Service settings updated.', 'Ok', { duration: 5000 });
    });
  }

  refresherViewLog() {
    this.dialog.open(ViewSystemLogDialogComponent, {
      autoFocus: false,
      minWidth: '63vw',
      data: {
        descriptor: 'RefresherService',
        name: 'Refresher Service'
      }
    });
  }

  atriggerSaveChanges() {
    this.saveATriggerBtn.options.active = true;
    this.internalSvcsSvc.atriggerSaveSettings(this.atriggerSettings).subscribe((result: ATriggerSettings) => {
      this.atriggerSettings = result;
      this.saveATriggerBtn.options.active = false;
      this.snackBar.open('ATrigger Service settings updated.', 'Ok', { duration: 5000 });
    });
  }

  atriggerViewLog() {
    this.dialog.open(ViewSystemLogDialogComponent, {
      autoFocus: false,
      minWidth: '63vw',
      data: {
        descriptor: 'ATriggerService',
        name: 'ATrigger Service'
      }
    });
  }

  ngOnInit() {
    this.internalSvcsSvc.refresherGetSettings().subscribe((result: RefresherSettings) => {
      this.refresherSettings = result;
      this.loadingRefresherData = false;
      this.refresherControlHelper.enable();
    });
    this.internalSvcsSvc.atriggerGetSettings().subscribe((result: ATriggerSettings) => {
      this.atriggerSettings = result;
      this.loadingATriggerData = false;
      this.atriggerControlHelper.enable();
    });
  }

}
