import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { MatButtonModule, MatCheckboxModule, MatInputModule } from '@angular/material';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatMenuModule } from '@angular/material/menu';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';

import { AppRoutingModule } from './infrastructure/navigation/app-routing.module';
import { AppLoadModule } from './infrastructure/app-load/app-load.module';

import { MatProgressButtonsModule } from 'mat-progress-buttons';

import { AppComponent } from './app.component';
import { DashboardComponent } from './views/dashboard/dashboard.component';
import { LoginComponent } from './views/login/login.component';
import { HomeComponent } from './views/home/home.component';
import { AuthToolbarComponent } from './controls/auth-toolbar/auth-toolbar.component';
import { HeaderComponent } from './controls/header/header.component';
import { SidenavListComponent } from './controls/sidenav-list/sidenav-list.component';
import { SpacerComponent } from './controls/spacer/spacer.component';
import { FooterComponent } from './controls/footer/footer.component';
import { UsersComponent } from './views/users/users.component';
import { AppGridComponent } from './controls/app-grid/app-grid.component';
import { ConfirmDialogComponent } from './controls/dialogs/confirm-dialog/confirm-dialog.component';
import { EditUserDialogComponent } from './controls/dialogs/edit-user-dialog/edit-user-dialog.component';
import { ViewSystemLogDialogComponent } from './controls/dialogs/view-system-log-dialog/view-system-log-dialog.component';
import { SystemComponent } from './views/system/system.component';
import { InternalServicesComponent } from './views/internal-services/internal-services.component';
import { NumbersOnlyDirective } from './directives/numbers-only/numbers-only.directive';
import { TelegramComponent } from './views/telegram/telegram.component';
import { SidenavMenuComponent } from './controls/sidenav-menu/sidenav-menu.component';
import { TelegramStatusComponent } from './views/telegram-status/telegram-status.component';
import { FormValueComponent } from './controls/form-value/form-value.component';
import { TelegramStatusMonitorComponent } from './controls/telegram-status-monitor/telegram-status-monitor.component';
import { TelegramServiceStatusPipe } from './pipes/telegram-service-status/telegram-service-status.pipe';
import { YesNoPipe } from './pipes/yes-no/yes-no.pipe';
import { DashIfEmptyPipe } from './pipes/dash-if-empty/dash-if-empty.pipe';
import { MyCvComponent } from './controls/my-cv/my-cv.component';
import { MomentDatePipe } from './pipes/moment-date/moment-date.pipe';
import { AuthenticationInterceptor } from './infrastructure/http-interceptors/authentication-interceptor/authentication-interceptor.service';
import { SystemLoggingComponent } from './views/system-logging/system-logging.component';
import { AppMatSpinnerButtonComponent } from './controls/app-mat-spinner-button/app-mat-spinner-button.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    LoginComponent,
    HomeComponent,
    AuthToolbarComponent,
    HeaderComponent,
    SidenavListComponent,
    SpacerComponent,
    FooterComponent,
    UsersComponent,
    AppGridComponent,
    ConfirmDialogComponent,
    EditUserDialogComponent,
    ViewSystemLogDialogComponent,
    SystemComponent,
    InternalServicesComponent,
    NumbersOnlyDirective,
    TelegramComponent,
    SidenavMenuComponent,
    TelegramStatusComponent,
    FormValueComponent,
    TelegramStatusMonitorComponent,
    MyCvComponent,
    SystemLoggingComponent,
    AppMatSpinnerButtonComponent,
    TelegramServiceStatusPipe,
    YesNoPipe,
    DashIfEmptyPipe,
    MomentDatePipe
  ],
  entryComponents: [
    ConfirmDialogComponent,
    EditUserDialogComponent,
    ViewSystemLogDialogComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatButtonModule,
    MatCheckboxModule,
    MatInputModule,
    MatToolbarModule,
    MatSidenavModule,
    MatMenuModule,
    MatListModule,
    MatIconModule,
    MatFormFieldModule,
    MatCardModule,
    MatSnackBarModule,
    MatTooltipModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatDialogModule,
    MatProgressBarModule,
    MatDividerModule,
    MatSelectModule,
    MatChipsModule,
    AppRoutingModule,
    AppLoadModule,
    MatProgressButtonsModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
