import { Injectable, TemplateRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TemplatesRegistryService {

  private templates: Array<TemplatesRegistryEntry>;

  constructor() {
    this.templates = new Array();
  }

  register(id: string, templateRef: TemplateRef<any>) {
    this.templates.push(new TemplatesRegistryEntry(id, templateRef));
  }

  getTemplateRef(templateId: string) : TemplateRef<any> {
    let template = this.templates.find((template) => template.Id === templateId);
    return template ? template.TemplateRef : null;
  }

}

export class TemplatesRegistryEntry {
  Id: string;
  TemplateRef: TemplateRef<any>;

  constructor(id: string, templateRef: TemplateRef<any>) {
    this.Id = id;
    this.TemplateRef = templateRef;
  }
}
