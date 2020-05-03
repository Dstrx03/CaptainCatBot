import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-holddown-action-icon',
  templateUrl: './holddown-action-icon.component.html',
  styleUrls: ['./holddown-action-icon.component.scss']
})
export class HolddownActionIconComponent implements OnInit {

  @Input() fontSet: string;
  @Input() fontIconActive: string;
  @Input() fontIcon: string;
  @Input() isActive: boolean;

  @Output() onActivate: EventEmitter<any> = new EventEmitter();
  @Output() onDeactivate: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  activate() {
    this.onActivate.emit();
    this.isActive = false;
  }

  deactivate() {
    this.onDeactivate.emit();
    this.isActive = true;
  }

}
