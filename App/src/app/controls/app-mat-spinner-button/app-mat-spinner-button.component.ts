import { Component, OnInit, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { MatProgressButtonOptions } from 'mat-progress-buttons';

@Component({
  selector: 'app-mat-spinner-button',
  templateUrl: './app-mat-spinner-button.component.html',
  styleUrls: ['./app-mat-spinner-button.component.scss']
})
export class AppMatSpinnerButtonComponent implements OnInit {

  @Input() id: string;
  @Input() options: MatProgressButtonOptions;
  @Output() btnClick: EventEmitter<MouseEvent> = new EventEmitter<MouseEvent>();
  @HostListener('click', ['$event'])
  public onClick(event: MouseEvent) {
    if (!this.options.disabled && !this.options.active) {
      this.btnClick.emit(event);
    }
  }

  constructor() { }

  ngOnInit() {
  }

}
