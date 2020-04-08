import { Component, OnInit, ViewChild, QueryList, ViewChildren } from '@angular/core';
import { FormControlHelper } from 'src/app/infrastructure/helpers/form-control-helper';
import { FormControl, Validators} from '@angular/forms';
import { SystemLoggingSettings } from 'src/app/models/systemLoggingSettings';
import { MatSnackBar, MatDialog } from '@angular/material';
import { SystemLoggingService } from 'src/app/services/system-logging/system-logging.service';
import { AppTimeSpanConvertor, AppTimeSpan } from 'src/app/infrastructure/convertors/app-time-span-convertor';
import { AppMatSpinnerButtonComponent } from 'src/app/controls/app-mat-spinner-button/app-mat-spinner-button.component';
import { ViewSystemLogDialogComponent } from 'src/app/controls/dialogs/view-system-log-dialog/view-system-log-dialog.component';

@Component({
  selector: 'app-system-logging',
  templateUrl: './system-logging.component.html',
  styleUrls: ['./system-logging.component.scss']
})
export class SystemLoggingComponent implements OnInit {

  @ViewChild('saveSettingsBtn') saveSettingsBtn: AppMatSpinnerButtonComponent;
  @ViewChildren('resetCleanThresholdBtn') resetCleanThresholdBtns: QueryList<AppMatSpinnerButtonComponent>;

  loadingLoggingSettings: boolean = false;
  systemLoggingSettingsForms: SystemLoggingSettingsForm[];

  constructor(
    private loggingSvc: SystemLoggingService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar) { 
    this.loadingLoggingSettings = true;
    this.systemLoggingSettingsForms = [];
  }

  ngOnInit() {
    this.loggingSvc.getSettings().subscribe((result: SystemLoggingSettings[]) => {
      this.initSettingsForms(result);
      this.loadingLoggingSettings = false;
    })
  }

  ngAfterViewInit(): void {
    this.resetCleanThresholdBtns.changes.subscribe(() => {
      this.resetCleanThresholdBtns.forEach(b => {
        let form = this.systemLoggingSettingsForms.find(f => f.formId  + '_btn' === b.id);
        if (form) form.resetCleanThresholdBtn = b;
      });
    });
  }

  saveSettings() {
    this.saveSettingsBtn.options.active = true;
    
    let systemLoggingSettings: SystemLoggingSettings[] = []; 
    this.systemLoggingSettingsForms.forEach(settings => {
      settings.controlHelper.disable();
      systemLoggingSettings.push(settings.getResult());
    });

    this.loggingSvc.updateSettings(systemLoggingSettings).subscribe((result: SystemLoggingSettings[]) => {
      this.initSettingsForms(result);
      this.saveSettingsBtn.options.active = false;
      this.snackBar.open('System Logging settings updated.', 'Ok', { duration: 5000 });
    });
  }

  private initSettingsForms(settings: SystemLoggingSettings[]) {
    this.systemLoggingSettingsForms = [];
    settings.forEach(settings => {
      this.systemLoggingSettingsForms.push(new SystemLoggingSettingsForm(settings, this.loggingSvc, this.dialog))
    });
  }

  isSettingsFormsErrorOrLoading() {
    return this.systemLoggingSettingsForms.some(x => x.controlHelper.isError() || x.loadingLoggingSettings);
  }

}

export class SystemLoggingSettingsForm {

  constructor(private settings: SystemLoggingSettings, private loggingSvc: SystemLoggingService, private dialog: MatDialog) {
    this.initForm(settings);
  }

  formId: string;
  loadingLoggingSettings: boolean = false;
  systemLoggingSettings: SystemLoggingSettings;
  appTimeSpanConvertor: AppTimeSpanConvertor;
  cleanThreshold: AppTimeSpan;

  valueFormControl = new FormControl('', [
    Validators.required,
    Validators.max(1000),
    Validators.min(1)
  ]);
  kindFormControl = new FormControl('');
  controlHelper = new FormControlHelper([this.valueFormControl, this.kindFormControl]);

  resetCleanThresholdBtn: AppMatSpinnerButtonComponent;

  resetCleanThreshold() {
    this.resetCleanThresholdBtn.options.active = true;
    this.loadingLoggingSettings = true;
    this.controlHelper.disable();
    this.loggingSvc.resetCleanThreshold(this.systemLoggingSettings.Descriptor).subscribe((result: SystemLoggingSettings) => {
      this.initForm(result);
      this.resetCleanThresholdBtn.options.active = false;
      this.loadingLoggingSettings = false;
      this.controlHelper.enable();
    });
  }

  initForm(settings: SystemLoggingSettings) {
    this.formId = `${this.settings.Descriptor}_settingsForm`
    this.systemLoggingSettings = settings;
    this.appTimeSpanConvertor = new AppTimeSpanConvertor();
    this.cleanThreshold = this.appTimeSpanConvertor.convertFromString(settings.CleanThresholdString);
  }

  getResult(): SystemLoggingSettings {
    let cleanThresholdString = this.appTimeSpanConvertor.convertFromTimeSpan(this.cleanThreshold);
    let isDefaultCleanThreshold = this.systemLoggingSettings.IsDefaultCleanThreshold && this.systemLoggingSettings.CleanThresholdString === cleanThresholdString;

    return {
      Name: this.systemLoggingSettings.Name, 
      Descriptor: this.systemLoggingSettings.Descriptor, 
      CleanThresholdString: cleanThresholdString,
      IsDefaultCleanThreshold: isDefaultCleanThreshold
    };
  }

  loggingServiceViewLog() {
    this.dialog.open(ViewSystemLogDialogComponent, {
      autoFocus: false,
      minWidth: '63vw',
      data: {
        descriptor: this.systemLoggingSettings.Descriptor,
        name: `Descriptor:${this.systemLoggingSettings.Descriptor}`
      }
    });
  }
}
