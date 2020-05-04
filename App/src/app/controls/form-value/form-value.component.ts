import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-form-value',
  templateUrl: './form-value.component.html',
  styleUrls: ['./form-value.component.scss']
})
export class FormValueComponent implements OnInit {

  @Input() caption: string;
  @Input() captionClass?: string = 'col-4';
  @Input() contentClass?: string = 'col';
  @Input() captionTextRight?: boolean  = true;

  constructor() {
  }

  ngOnInit() {
  }

}
