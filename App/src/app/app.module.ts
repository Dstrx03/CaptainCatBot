import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { ReactiveFormsModule } from '@angular/forms';

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
    EditUserDialogComponent
  ],
  entryComponents: [
    ConfirmDialogComponent,
    EditUserDialogComponent
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
    AppRoutingModule,
    AppLoadModule,
    MatProgressButtonsModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
