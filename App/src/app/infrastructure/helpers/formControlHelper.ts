import { FormControl } from '@angular/forms';

export class FormControlHelper {

    private formCollection: FormControl[];
    private errorMsgsCollection: {error: string, msg: string}[];

    constructor(forms?: FormControl[], addErrorMsgs?: {error: string, msg: string}[]) {
        if (forms !== undefined) this.formCollection = forms;
        else this.formCollection = [];
        this.errorMsgsCollection = this.defaultErrorMessages();
        if (addErrorMsgs != undefined) this.addErrorMessages(addErrorMsgs);
    }

    disable() {
        this.formCollection.forEach(x => x.disable());
    }

    enable() {
        this.formCollection.forEach(x => x.enable());
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

    setErrorMessages(errors: {error: string, msg: string}[]) {
        this.errorMsgsCollection = errors;
    }

    addErrorMessages(errors: {error: string, msg: string}[]) {
        errors.forEach(x => this.setErrorMessage(x.error, x.msg));
    }

    setErrorMessage(error: string, msg: string) {
        const i = this.errorMsgsCollection.findIndex(err => err.error === error);
        if (i !== -1) this.errorMsgsCollection[i].msg = msg;
        else this.errorMsgsCollection.push({error, msg});
    }

    private defaultErrorMessages() {
        return [
            {error: 'default', msg: 'This field is invalid.'},
            {error: 'required', msg: 'This field is <strong>required</strong>.'},
            {error: 'email', msg: 'Email format is incorrect.'}
        ];
    }

}
