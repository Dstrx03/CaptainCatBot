import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';
import { tap, mergeMap } from 'rxjs/operators';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { AppUserModel } from '../../models/appUserModel';
import { UsersService } from '../../services/identity/users.service';
import { MatSnackBar } from '@angular/material';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/controls/dialogs/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  @ViewChild('searchBox') searchBox: ElementRef;

  users$ = new Observable<AppUserModel[]>();
  search$ = new Observable<string>();

  private userTerms = new Subject<AppUserModel[]>();
  private searchTerms = new Subject<string>();

  constructor(private usersSvc: UsersService, private snackBar: MatSnackBar, public dialog: MatDialog) { }

  ngOnInit() {
    this.users$ = this.userTerms.pipe(
      tap((users: AppUserModel[]) => of(users))
    );
    this.search$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) => of(term)),
    );
    this.updateGrid().subscribe();
  }

  private updateGrid(callHttp: boolean = true, resetControls: boolean = true): Observable<any> {
    const resetControlsFn = () => {
      this.searchBox.nativeElement.value = '';
      this.searchTerms.next('');
    };
    return callHttp ?
      this.usersSvc.getUsersList().pipe(
        tap((users: AppUserModel[]) => {
          this.userTerms.next(users);
          if (resetControls) resetControlsFn();
        })
      ) :
      of(this.users$).pipe(
        tap(_ => {
          if (resetControls) resetControlsFn();
        })
      );
  }

  search(term: string): void {
    this.searchTerms.next(term);
  }

  addUser() {
    this.updateGrid();
  }

  editUser(user: AppUserModel) {
    this.updateGrid();
  }

  removeUser(user: AppUserModel) {
    let removeRes: { Success: boolean, Error: string };
    this.dialog.open(ConfirmDialogComponent, {
      autoFocus: false,
      data: {
        header: `Remove User`,
        content: `Are you shure you want to remove ${user.Name}? This action cannot be reversed.`,
        useAgent: true,
        agent: this.usersSvc.removeUser(user.Id).pipe(
          mergeMap((result) => {
            removeRes = result;
            return this.updateGrid(removeRes.Success, removeRes.Success);
          }),
          tap(_ => {
            const snackMsg = removeRes.Success ? `${user.Name} successfully removed from the system!` : removeRes.Error;
            this.snackBar.open(snackMsg, 'Ok', { duration: 5000 });
          })
        )
      }
    });
  }

  actionApplied($event: any) {
    switch ($event.actionName) {
      case 'Edit': return this.editUser($event.item);
      case 'Remove': return this.removeUser($event.item);
    }
  }

}
