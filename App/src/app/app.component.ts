import { Component, Renderer2, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent  { 
  isLightTheme: boolean = true;
  
  constructor (
    @Inject(DOCUMENT) private document: Document,
    private renderer: Renderer2)
  {
    this.renderer.addClass(this.document.body, 'light-theme');
  }

}
