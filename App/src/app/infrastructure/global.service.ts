import { Injectable } from '@angular/core';
import { Meta } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class GlobalService {

  public appTitle: string;
  public appVersion: string;
  public baseUrl: string;
  public serverTimezoneIana: string;
  public gridSchemesJson: string;

  constructor(private meta: Meta) {
    this.appTitle = this.meta.getTag('name=appTitle').content;
    this.baseUrl = this.meta.getTag('name=baseUrl').content;
    this.serverTimezoneIana = this.meta.getTag('name=serverTimezoneIana').content;
    this.gridSchemesJson = this.meta.getTag('name=gridSchemesJson').content;
    this.appVersion = this.meta.getTag('name=appVersion').content;
  }

}
