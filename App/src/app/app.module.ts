import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { MatButtonModule, MatCheckboxModule } from '@angular/material';
import { MatToolbarModule } from '@angular/material/toolbar';

import { AppComponent } from './app.component';
import { LoginbuttonComponent } from './identity/loginbutton/loginbutton.component';
import { AppRoutingModule } from './navigation/app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginbuttonComponent,
    DashboardComponent,
    LoginComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    HttpClientModule,
    MatButtonModule,
    MatCheckboxModule,
    MatToolbarModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
