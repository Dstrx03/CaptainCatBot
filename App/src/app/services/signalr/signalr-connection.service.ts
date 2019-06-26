import { EventEmitter, Injectable, NgZone, OnDestroy } from "@angular/core";
import { SignalrHub } from './signalr-hub.service';

import 'jquery'; 
import 'signalr';
import { GlobalService } from 'src/app/infrastructure/global.service';

declare var jQuery: any;
declare var $: any;


export enum HubConnectionState {
  Connecting = 1,
  Connected = 2,
  Reconnecting = 3,
  Disconnected = 4
}

@Injectable({
  providedIn: 'root'
})
export class SignalrConnectionService implements OnDestroy {

  private jQuery = $;
  
  private hubConnection: any = null;
  private hubLookup = new Map<string, SignalrHub>();

  public constructor(private zone: NgZone, private globalSvc: GlobalService) {

      if (typeof this.jQuery === "undefined") {
          throw new Error("jQuery is not defined on the global object.");
      }
      if (this.jQuery.hubConnection === null) {
          throw new Error("SignalR is not defined on the jQuery object.");
      }
      this.hubConnection = this.jQuery.hubConnection(`${this.globalSvc.baseUrl}signalr`, { useDefaultPath: false });

      this.hubConnection.stateChanged((state: any) => {
          const newState = this.getConnectionState(state);
          this.connectionStateChange.next(newState);
          if (newState === HubConnectionState.Disconnected) {
              //this.disconnect();
          }
      });

      this.hubConnection.error((error: any) => {
          this.connectionError.next(error);
      });
  }

  private getConnectionState(state: any): HubConnectionState {
      switch (state.newState) {
          case 0:
              return HubConnectionState.Connecting;
          case 1:
              return HubConnectionState.Connected;
          case 2:
              return HubConnectionState.Reconnecting;
          case 4:
              return HubConnectionState.Disconnected;
          default:
              return HubConnectionState.Connecting;
      }
  }

  public ngOnDestroy(): void {
      this.connectionStateChange.complete();
      this.connectionError.complete();
      this.disconnect();
  }

  public connectionStateChange = new EventEmitter<HubConnectionState>();

  public connectionError = new EventEmitter<Error>();

  public createHub(hubName: string): SignalrHub {
      if (this.hubLookup.has(hubName)) {
          return this.hubLookup.get(hubName);
      }
      const hub = new SignalrHub(this.zone, this.hubConnection, hubName);
      this.hubLookup.set(hubName, hub);
      return hub;
  }

  public disconnect(): void {
      this.hubLookup.forEach((hub) => hub.disconnect());
      this.hubLookup.clear();
      this.safeDisconnect();
  }

  private safeDisconnect(): void {
      if (this.hubConnection.state === 4) {
          return;
      }
      try {
          this.hubConnection.stop(true, true);
      } catch (e) {
          return;
      }
  }

}
