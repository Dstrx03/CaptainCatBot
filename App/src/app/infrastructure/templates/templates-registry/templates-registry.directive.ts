import { Directive, Input, TemplateRef } from '@angular/core';
import { TemplatesRegistryService } from './templates-registry.service';

@Directive({
  selector: '[templatesRegistry]'
})
export class TemplatesRegistryDirective {

  @Input('templatesRegistry') templateId;

  constructor(private host: TemplateRef<any>, private templatesRegistrySvc: TemplatesRegistryService) { }

  ngOnInit() {
    this.templatesRegistrySvc.register(this.templateId, this.host);
  }

}
