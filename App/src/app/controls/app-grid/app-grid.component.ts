import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { GridSchemesService } from '../../infrastructure/grids-schemes/grid-schemes.service';
import { GridScheme } from '../../models/gridScheme';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-app-grid',
  templateUrl: './app-grid.component.html',
  styleUrls: ['./app-grid.component.scss']
})
export class AppGridComponent implements OnInit {

  @Input() data: Observable<object[]>;
  @Input() schemeName: string;
  @Input() search: Observable<string>;

  @Output() public actionApplied = new EventEmitter<object>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  gridScheme: GridScheme;
  displayedColumns: string[];
  searchColumns: string[];
  dataSource = new MatTableDataSource<object>();

  constructor(private gridSchemesSvc: GridSchemesService) { }

  ngOnInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.gridScheme = this.gridSchemesSvc.getScheme(this.schemeName);
    this.displayedColumns = this.gridScheme.Columns.filter(x => x.Visible === true).map(({ColumnId}) => ColumnId);
    this.searchColumns = this.gridScheme.Columns.filter(x => x.Searchable === true).map(({ColumnId}) => ColumnId);
    this.dataSource.filterPredicate = (data, filter: string): boolean => {
      for (const searchCol of this.searchColumns) {
        if (data[searchCol] === null) continue;
        else if (data[searchCol].toString().toLowerCase().includes(filter)) return true;
      }
      return false;
    };
    this.search.subscribe((search: string) => {
      this.dataSource.filter = search.trim().toLowerCase();
    });
    this.data.subscribe((data: object[]) => {
      this.dataSource.data = data;
      this.dataSource.paginator.firstPage();
    });
  }

  applyAction(item: any, actionName: string): any {
    this.actionApplied.emit({item, actionName});
  }

}
