import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';
import { tap, mergeMap } from 'rxjs/operators';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { AppUserModel } from '../../models/appUserModel';
import { CatProcedureResult } from '../../infrastructure/webApi/catProcedureResult';
import { UsersService } from '../../services/identity/users.service';
import { MatSnackBar } from '@angular/material';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from 'src/app/controls/dialogs/confirm-dialog/confirm-dialog.component';
import { EditUserDialogComponent } from '../../controls/dialogs/edit-user-dialog/edit-user-dialog.component';

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
    let addRes: CatProcedureResult;
    this.dialog.open(EditUserDialogComponent, {
      autoFocus: false,
      minWidth: '63vw',
      data: {
        addMode: true,
        agentFn: (user: AppUserModel) => {
          return this.usersSvc.addUser(user, [`System error occured, could not add user ${user.UserName} to the system!`]).pipe(
            mergeMap((result) => {
              addRes = result;
              return this.updateGrid(addRes.IsSuccess, addRes.IsSuccess);
            }),
            tap(_ => {
              const snackMsg = addRes.IsSuccess ? `User ${user.UserName} added to the system successfully.` : addRes.ErrorMsgs.join(' ');
              this.snackBar.open(snackMsg, 'Ok', { duration: 5000 });
            })
          );
        }
      }
    });
  }

  editUser(user: AppUserModel) {
    let editRes: CatProcedureResult;
    this.dialog.open(EditUserDialogComponent, {
      autoFocus: false,
      minWidth: '63vw',
      data: {
        addMode: false,
        userId: user.Id,
        agentFn: (user: AppUserModel) => {
          return this.usersSvc.updateUser(user, [`System error occured, could not update user!`]).pipe(
            mergeMap((result) => {
              editRes = result;
              return this.updateGrid(editRes.IsSuccess, editRes.IsSuccess);
            }),
            tap(_ => {
              const snackMsg = editRes.IsSuccess ? `User ${user.UserName} updated successfully.` : editRes.ErrorMsgs.join(' ');
              this.snackBar.open(snackMsg, 'Ok', { duration: 5000 });
            })
          );
        }
      }
    });
  }

  removeUser(user: AppUserModel) {
    let removeRes: CatProcedureResult;
    this.dialog.open(ConfirmDialogComponent, {
      autoFocus: false,
      data: {
        header: `Remove User`,
        contentHtml: `<p>Are you sure you want to remove <strong class="accent-color">${user.UserName}</strong>? This action cannot be reversed.</p>`,
        useAgent: true,
        agent: this.usersSvc.removeUser(user.Id, [`System error occured, could not remove user ${user.UserName} from the system!`]).pipe(
          mergeMap((result) => {
            removeRes = result;
            return this.updateGrid(removeRes.IsSuccess, removeRes.IsSuccess);
          }),
          tap(_ => {
            const snackMsg = removeRes.IsSuccess ? `User ${user.UserName} removed from the system successfully.` : removeRes.ErrorMsgs.join(' ');
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
