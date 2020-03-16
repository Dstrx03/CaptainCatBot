import { Injectable, Renderer2, Inject, RendererFactory2 } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {

  private readonly lightThemeClass: string = 'light-theme';
  private readonly darkThemeClass: string = 'dark-theme';
  private readonly defaultDarkMode: boolean = false;

  private renderer: Renderer2;

  private isDarkMode: BehaviorSubject<boolean>;

  currentIsDarkMode(): Observable<boolean> {
    return this.isDarkMode.asObservable();
  }

  constructor(@Inject(DOCUMENT) private document: Document, rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.isDarkMode = new BehaviorSubject<boolean>(undefined);
  }

  initTheme() {
    // todo: maybe use cookies to store last applied theme?
    this.applyTheme(this.defaultDarkMode);
  }

  toggleThemeMode() {
    this.applyTheme(!this.isDarkMode.value);
  }

  private applyTheme(darkMode: boolean) {
    this.isDarkMode.next(darkMode);
    this.renderer.removeClass(this.document.body, darkMode ? this.lightThemeClass : this.darkThemeClass);
    this.renderer.addClass(this.document.body, darkMode ? this.darkThemeClass : this.lightThemeClass);
  }
}
