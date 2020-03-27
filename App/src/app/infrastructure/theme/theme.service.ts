import { Injectable, Renderer2, Inject, RendererFactory2 } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserPreferencesService } from '../user-preferences/user-preferences.service';


@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private readonly lightThemeClass: string = 'light-theme';
  private readonly darkThemeClass: string = 'dark-theme';
  private readonly defaultDarkMode: boolean = false;
  private readonly useDarkModePreferenceKey: string = "useDarkMode"

  private renderer: Renderer2;

  private isDarkMode: BehaviorSubject<boolean>;

  currentIsDarkMode(): Observable<boolean> {
    return this.isDarkMode.asObservable();
  }

  constructor(@Inject(DOCUMENT) private document: Document, 
      private rendererFactory: RendererFactory2,
      private userPreferencesSvc: UserPreferencesService) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.isDarkMode = new BehaviorSubject<boolean>(undefined);
  }

  initTheme() {
    let isDarkModePreference = this.userPreferencesSvc.get<boolean>(this.useDarkModePreferenceKey);
    let isDarkMode: boolean = isDarkModePreference ? isDarkModePreference : this.defaultDarkMode;
    this.applyTheme(isDarkMode);
  }

  toggleThemeMode() {
    let isDarkMode = !this.isDarkMode.value
    this.applyTheme(isDarkMode);
    this.userPreferencesSvc.set(this.useDarkModePreferenceKey, isDarkMode);
  }

  private applyTheme(darkMode: boolean) {
    this.isDarkMode.next(darkMode);
    this.renderer.removeClass(this.document.body, darkMode ? this.lightThemeClass : this.darkThemeClass);
    this.renderer.addClass(this.document.body, darkMode ? this.darkThemeClass : this.lightThemeClass);
  }

}
