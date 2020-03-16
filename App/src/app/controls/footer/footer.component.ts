import { Component, OnInit } from '@angular/core';
import { ThemeService } from 'src/app/infrastructure/theme/theme.service';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  isDarkTheme: boolean = undefined;

  constructor(private themeSvc: ThemeService) { }

  ngOnInit() {
    this.themeSvc.currentIsDarkMode()
      .subscribe(currentIsDarkTheme => this.isDarkTheme = currentIsDarkTheme);
  }

}