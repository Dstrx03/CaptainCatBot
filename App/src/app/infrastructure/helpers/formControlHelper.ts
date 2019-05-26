import { FormControl } from '@angular/forms';
import { Observable, of, BehaviorSubject } from 'rxjs';

export class FormControlHelper {

    private formCollection: FormControl[];
    private errorMsgsCollection: {error: string, msg: string}[];

    constructor(forms?: FormControl[]) {
        if (forms !== undefined) this.formCollection = forms;
        else this.formCollection = [];
        this.errorMsgsCollection = this.defaultErrorMessages();
    }

    isError(): boolean {
        return this.formCollection.filter(fc => fc.invalid).length > 0;
    }

    errorMessage(formControl: FormControl): string {
        const res = this.errorMsgsCollection.find(err => formControl.hasError(err.error));
        return res !== undefined ?
            res.msg :
            this.errorMsgsCollection.find(err => err.error === 'default').msg;
    }

    setErrorMessages(errors: {error: string, msg: string}[]){
        this.errorMsgsCollection = errors;
    }

    setErrorMessage(error: string, msg: string) {
        const i = this.errorMsgsCollection.findIndex(err => err.error === error);
        if (i !== -1) this.errorMsgsCollection[i].msg = msg;
        else this.errorMsgsCollection.push({error, msg});
    }

    private defaultErrorMessages() {
        return [
            {error: 'default', msg: 'This field is invalid.'},
            {error: 'required', msg: 'This field is required.'},
            {error: 'email', msg: 'Email is in wrong format.'}
        ];
    }

}
