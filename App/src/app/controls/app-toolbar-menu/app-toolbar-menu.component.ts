import { Component, OnInit, Input, Injector, TemplateRef, ContentChild, QueryList, ContentChildren } from '@angular/core';
import { TemplatesRegistryService } from 'src/app/infrastructure/templates/templates-registry/templates-registry.service';

@Component({
  selector: 'app-toolbar-menu',
  templateUrl: './app-toolbar-menu.component.html',
  styleUrls: ['./app-toolbar-menu.component.scss']
})
export class AppToolbarMenuComponent implements OnInit {

  @Input() menuItems: AppToolbarMenuItem[];

  templateRef: TemplateRef<any>;
  useTemplate = false;

  constructor(private injector: Injector, private templatesRegistrySvc: TemplatesRegistryService) { }

  onItemClick($event: any, item: AppToolbarMenuItem) {
    $event.stopPropagation();
    if (item.useTemplate()) { 
      this.templateRef = this.templatesRegistrySvc.getTemplateRef(item.TemplateId);
      this.useTemplate = this.templateRef !== null;
    }
    else item.OnClick(this.injector);
  }

  onInnerBackClick($event: any) {
    $event.stopPropagation();
    this.templateRef = null;
    this.useTemplate = false;
  }

  ngOnInit() {
  }

  static BuildOnClickMenuItem(caption: string, iconStr: string, onClick: (injector: Injector) => any): AppToolbarMenuItem {
    return new AppToolbarMenuItem(caption, iconStr, onClick, undefined);
  }

  static BuildTemplateMenuItem(caption: string, iconStr: string, templateId: string): AppToolbarMenuItem {
    return new AppToolbarMenuItem(caption, iconStr, undefined, templateId);
  }

}

export class AppToolbarMenuItem {

  Caption: string;
  Icon: AppToolbarMenuItemIcon;
  OnClick: (injector: Injector) => any;
  TemplateId: string;
  
  constructor(caption: string, iconStr: string, onClick: (injector: Injector) => any, templateId: string) {
    this.Caption = caption;
    let a = iconStr.split(" ");
    this.Icon = new AppToolbarMenuItemIcon(a[0], a[1]);
    this.OnClick = onClick;
    this.TemplateId = templateId;
  }

  useTemplate(): boolean {
    return this.TemplateId !== undefined && this.TemplateId !== null;
  }
}

export class AppToolbarMenuItemIcon {

  FontSet: string;
  FontIcon: string

  constructor(fontSet: string, fontIcon: string) {
    this.FontSet = fontSet;
    this.FontIcon = fontIcon;
  }
}