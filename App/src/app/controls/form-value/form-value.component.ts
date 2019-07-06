import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-form-value',
  templateUrl: './form-value.component.html',
  styleUrls: ['./form-value.component.scss']
})
export class FormValueComponent implements OnInit {

  @Input() caption: string;
  @Input() captionClass: string;
  @Input() contentClass: string;

  constructor() {
    if (this.captionClass === undefined || this.captionClass === null || this.captionClass.length === 0) this.captionClass = 'col-4';
    if (this.contentClass === undefined || this.contentClass === null || this.contentClass.length === 0) this.contentClass = 'col';
  }

  ngOnInit() {
  }

}
