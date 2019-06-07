import { Injectable } from '@angular/core';
import { GridScheme } from '../../models/gridScheme';
import { GlobalService } from '../global.service';

@Injectable({
  providedIn: 'root'
})
export class GridSchemesService {

  private schemes: GridScheme[];

  constructor(private globalSvc: GlobalService) {
    this.schemes = JSON.parse(this.globalSvc.gridSchemesJson) as GridScheme[];
  }

  getScheme(name: string): GridScheme {
    return this.schemes.find(x => x.Name === name);
  }
}
