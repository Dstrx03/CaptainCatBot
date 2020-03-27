import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { GlobalService } from '../global.service';

@Injectable({
  providedIn: 'root'
})
export class UserPreferencesService {

  private readonly userPreferencesCookieName = `${this.globalSvc.appTitle}:userPreferences`;

  constructor(private cookieSvc: CookieService, private globalSvc: GlobalService) { }

  get<T extends string | number | boolean>(key: string): T {
    let value = this.getAny(key);
    if (value !== null && 
        typeof value !== "string" && 
        typeof value !== "number" && 
        typeof value !== "boolean") {
      this.remove(key);
      return null;
    }
    return value as T;
  }

  set(key: string, value: string | number | boolean): void {
    this.setAny(key, value);
  }

  remove(key: string): void {
    this.checkCookie();
    let preferences = this.getPreferences();
    if (preferences === null) return;
    preferences = preferences.filter(x => x.key !== key);
    this.setPreferences(preferences);
  }

  initUserPreferences(cookieCheck?: boolean): void {
    let cookieChecked = (cookieCheck !== undefined && cookieCheck !== null) ? cookieCheck : this.cookieSvc.check(this.userPreferencesCookieName);
    let preferences: { key: string, value: any }[] = cookieChecked ? this.getPreferences() : [];
    if (preferences === null) preferences = [];
    this.setPreferences(preferences);
  }

  private getAny(key: string): any {
    if (!this.checkCookie()) return null;
    let preferences = this.getPreferences();
    if (preferences === null) return null;
    let preferencesItem = preferences.find(x => x.key === key);
    return (preferencesItem === undefined) ? null : preferencesItem.value;
  }

  private setAny(key: string, value: any): void {
    this.checkCookie();
    let preferences = this.getPreferences();
    if (preferences === null) return;
    let preferencesItem = preferences.find(x => x.key === key);
    if (preferencesItem === undefined) preferences.push({ key: key, value: value });
    else preferencesItem.value = value;
    this.setPreferences(preferences);
  }

  private checkCookie(): boolean {
    let cookieCheck = this.cookieSvc.check(this.userPreferencesCookieName);
    if (!cookieCheck) this.initUserPreferences(cookieCheck);
    return cookieCheck;
  }

  private getPreferences(): { key: string, value: any }[] {
    let preferencesJson = this.cookieSvc.get(this.userPreferencesCookieName);
    try {
      let preferences: {key: string, value: any}[] = JSON.parse(preferencesJson);
      return (preferences === undefined || preferences === null) ? null : preferences;
    } catch(e) {
      console.error(e);
      return null;
    }
  }

  private setPreferences(preferences: { key: string, value: any }[]): void {
    this.cookieSvc.set(this.userPreferencesCookieName, JSON.stringify(preferences), 180, "/", null, false, "Strict");
  }

}