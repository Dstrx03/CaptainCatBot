import { Component, OnInit, Input, ElementRef } from '@angular/core';

@Component({
  selector: 'app-spacer',
  templateUrl: './spacer.component.html',
  styleUrls: ['./spacer.component.scss']
})
export class SpacerComponent implements OnInit {

  @Input() height: string;
  @Input() heightClass: string;
  @Input() inheritHeightFromClass: boolean;
  @Input() displayClass: string;
  
  render() {
    return this.height !== undefined;
  }

  constructor(private el: ElementRef) { }

  ngOnInit() {
    if (!this.inheritHeightFromClass) return;

    this.el.nativeElement.classList = this.heightClass;
    this.height = "inherit";
  }

}
