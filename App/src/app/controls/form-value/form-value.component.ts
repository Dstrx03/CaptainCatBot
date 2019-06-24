import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-form-value',
  templateUrl: './form-value.component.html',
  styleUrls: ['./form-value.component.scss']
})
export class FormValueComponent implements OnInit {

  @Input() caption: string;
  @Input() captionClass: string;
  @Input() value: string;
  @Input() valueClass: string;

  constructor() {
    if (this.captionClass == undefined) this.captionClass = 'col-4';
  }

  ngOnInit() {
  }

}
