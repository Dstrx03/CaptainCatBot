import { Component, OnInit } from '@angular/core';
import { AppRoutes } from 'src/app/infrastructure/navigation/models/appRoutes';

@Component({
  selector: 'app-system',
  templateUrl: './system.component.html',
  styleUrls: ['./system.component.scss']
})
export class SystemComponent implements OnInit {

  appRoutes = AppRoutes;

  constructor() { }

  ngOnInit() {
  }

}
