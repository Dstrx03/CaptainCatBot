import { NgZone } from "@angular/core";
import { Observable, Observer, ReplaySubject, Subject } from "rxjs";

export class SignalrHub {

    private hubProxy: any = null;
    private subscriptions = new Map<string, Subject<any>>();

    public constructor(private zone: NgZone, private hubConnection: any, private hubName: string) {
    }

    public disconnect(): void {
        if (this.hubProxy === null) {
            return;
        }
        this.subscriptions.forEach((subject, eventName) => {
            subject.complete();
            this.hubProxy.off(eventName);
        });
        this.subscriptions.clear();
        this.hubProxy = null;
    }

    public async invoke(actionName: string, ...data: any[]): Promise<any> {
        await this.prepareHubProxy((proxy) => proxy.on("$$__FAKE_EVENT", () => { return; }));
        const result = await this.hubProxy.invoke(actionName, ...data);
        return result;
    }

    public async listen(eventName: string): Promise<Observable<any>> {
        const subject = await this.getCachedSubject(eventName);
        return this.getZonedObservable(subject);
    }

    private async getCachedSubject(eventName: string): Promise<Subject<any>> {
        if (this.subscriptions.has(eventName)) {
            return this.subscriptions.get(eventName);
        }
        const subject = new ReplaySubject<any>();
        await this.listenAsync(eventName, (...data: any[]) => {
            subject.next(data);
        });
        this.subscriptions.set(eventName, subject);
        return subject;
    }

    private getZonedObservable<T>(observable: Observable<T>): Observable<T> {
        return Observable.create((observer: Observer<T>) => {
            const onNext = (value: T) => this.zone.run(() => observer.next(value));
            const onError = (error: any) => this.zone.run(() => observer.error(error));
            const onComplete = () => this.zone.run(() => observer.complete());
            const subscription = observable.subscribe(onNext, onError, onComplete);
            return () => subscription.unsubscribe();
        });
    }

    private async listenAsync(eventName: string, handler: (...data: any[]) => void): Promise<any> {
        const result = await this.prepareHubProxy((proxy) => proxy.on(eventName, handler));
        return result;
    }

    private async prepareHubProxy(listener: (proxy: any) => void): Promise<any> {
        let isNewHub = false;
        if (this.hubProxy === null) {
            isNewHub = true;
            this.hubProxy = this.hubConnection.createHubProxy(this.hubName);
        }
        listener(this.hubProxy);
        if (this.hubConnection.state === 4) {
            const result = await this.hubConnection.start();
            return result;
        } else {
            if (!isNewHub) return null;
            try{
                await this.hubConnection.stop(true, true);
            } catch(e) {
                console.error(e);
            }
            const result = await this.hubConnection.start();
            return result;
        }
    }
}
