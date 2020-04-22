import { Component, OnInit } from '@angular/core';
import { AppRoutes } from 'src/app/infrastructure/navigation/models/appRoutes';

@Component({
  selector: 'app-telegram',
  templateUrl: './telegram.component.html',
  styleUrls: ['./telegram.component.scss']
})
export class TelegramComponent implements OnInit {

  appRoutes = AppRoutes;

  constructor() { }

  ngOnInit() {
  }

}
