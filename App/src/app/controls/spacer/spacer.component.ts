import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-spacer',
  templateUrl: './spacer.component.html',
  styleUrls: ['./spacer.component.css']
})
export class SpacerComponent implements OnInit {

  @Input() height: string;

  render() {
    return this.height !== undefined;
  }

  constructor() { }

  ngOnInit() {
  }

}
