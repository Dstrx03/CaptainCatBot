import { Injectable } from '@angular/core';
import { Meta } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class GlobalService {

  public baseUrl: string;

  constructor(private meta: Meta) {
    this.baseUrl = this.meta.getTag('name=baseUrl').content;
  }

}
