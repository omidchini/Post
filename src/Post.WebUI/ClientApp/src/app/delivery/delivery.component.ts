import { Component, TemplateRef } from '@angular/core';
import {
  DeliveryClient, CreateDeliveryCommand, DeliveryDto, UpdateDeliveryCommand,
  DeliveriesVm, ZoneClient, ZoneDto, CreateZoneCommand, UpdateZoneCommand,
  UpdateDeliveryDetailCommand
} from '../web-api-client';
import { faPlus, faEllipsisH } from '@fortawesome/free-solid-svg-icons';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-todo-component',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.scss']
})
export class DeliveryComponent {

  debug = false;

  vm: DeliveriesVm;

  selectedZone: ZoneDto;
  selectedDelivery: DeliveryDto;

  newZoneEditor: any = {};
  zoneOptionsEditor: any = {};
  deliveryDetailsEditor: any = {};

  newZoneModalRef: BsModalRef;
  zoneOptionsModalRef: BsModalRef;
  deleteZoneModalRef: BsModalRef;
  deliveryDetailsModalRef: BsModalRef;

  faPlus = faPlus;
  faEllipsisH = faEllipsisH;

  constructor(private zoneClient: ZoneClient, private deliveryClient: DeliveryClient, private modalService: BsModalService) {
    zoneClient.get().subscribe(
      result => {
        this.vm = result;
        if (this.vm.zones.length) {
          this.selectedZone = this.vm.zones[0];
        }
      },
      error => console.error(error)
    );
  }

  // Lists
  remainingDeliveries(zone: ZoneDto): number {
    return zone.items.filter(t => !t.done).length;
  }

  showNewZoneModal(template: TemplateRef<any>): void {
    this.newZoneModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById("title").focus(), 250);
  }

  newZoneCancelled(): void {
    this.newZoneModalRef.hide();
    this.newZoneEditor = {};
  }

  addZone(): void {
    let zone = ZoneDto.fromJS({
      id: 0,
      title: this.newZoneEditor.title,
      items: [],
    });

    this.zoneClient.create(<CreateZoneCommand>{ title: this.newZoneEditor.title }).subscribe(
      result => {
        zone.id = result;
        this.vm.zones.push(zone);
        this.selectedZone = zone;
        this.newZoneModalRef.hide();
        this.newZoneEditor = {};
      },
      error => {
        let errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newZoneEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById("title").focus(), 250);
      }
    );
  }

  showZoneOptionsModal(template: TemplateRef<any>) {
    this.zoneOptionsEditor = {
      id: this.selectedZone.id,
      title: this.selectedZone.title,
    };

    this.zoneOptionsModalRef = this.modalService.show(template);
  }

  updateZoneOptions() {
    this.zoneClient.update(this.selectedZone.id, UpdateZoneCommand.fromJS(this.zoneOptionsEditor))
      .subscribe(
        () => {
          this.selectedZone.title = this.zoneOptionsEditor.title,
            this.zoneOptionsModalRef.hide();
          this.zoneOptionsEditor = {};
        },
        error => console.error(error)
      );
  }

  confirmDeleteZone(template: TemplateRef<any>) {
    this.zoneOptionsModalRef.hide();
    this.deleteZoneModalRef = this.modalService.show(template);
  }

  deleteZoneConfirmed(): void {
    this.zoneClient.delete(this.selectedZone.id).subscribe(
      () => {
        this.deleteZoneModalRef.hide();
        this.vm.zones = this.vm.zones.filter(t => t.id != this.selectedZone.id)
        this.selectedZone = this.vm.zones.length ? this.vm.zones[0] : null;
      },
      error => console.error(error)
    );
  }

  // Items

  showDeliveryDetailsModal(template: TemplateRef<any>, item: DeliveryDto): void {
    this.selectedDelivery = item;
    this.deliveryDetailsEditor = {
      ...this.selectedDelivery
    };

    this.deliveryDetailsModalRef = this.modalService.show(template);
  }

  updateDeliveryDetails(): void {
    this.deliveryClient.updateItemDetails(this.selectedDelivery.id, UpdateDeliveryDetailCommand.fromJS(this.deliveryDetailsEditor))
      .subscribe(
        () => {
          if (this.selectedDelivery.zoneId != this.deliveryDetailsEditor.zoneId) {
            this.selectedZone.items = this.selectedZone.items.filter(i => i.id != this.selectedDelivery.id);
            let listIndex = this.vm.zones.findIndex(l => l.id == this.deliveryDetailsEditor.zoneId);
            this.selectedDelivery.zoneId = this.deliveryDetailsEditor.zoneId;
            this.vm.zones[listIndex].items.push(this.selectedDelivery);
          }

          this.selectedDelivery.priority = this.deliveryDetailsEditor.priority;
          this.selectedDelivery.note = this.deliveryDetailsEditor.note;
          this.deliveryDetailsModalRef.hide();
          this.deliveryDetailsEditor = {};
        },
        error => console.error(error)
      );
  }

  addDelivery() {
    let item = DeliveryDto.fromJS({
      id: 0,
      zoneId: this.selectedZone.id,
      priority: this.vm.priorityLevels[0].value,
      title: '',
      done: false
    });

    this.selectedZone.items.push(item);
    let index = this.selectedZone.items.length - 1;
    this.editDelivery(item, 'itemTitle' + index);
  }

  editDelivery(item: DeliveryDto, inputId: string): void {
    this.selectedDelivery = item;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateDelivery(item: DeliveryDto, pressedEnter: boolean = false): void {
    let isNewItem = item.id == 0;

    if (!item.title.trim()) {
      this.deleteDelivery(item);
      return;
    }

    if (item.id == 0) {
      this.deliveryClient.create(CreateDeliveryCommand.fromJS({ ...item, listId: this.selectedZone.id }))
        .subscribe(
          result => {
            item.id = result;
          },
          error => console.error(error)
        );
    } else {
      this.deliveryClient.update(item.id, UpdateDeliveryCommand.fromJS(item))
        .subscribe(
          () => console.log('Update succeeded.'),
          error => console.error(error)
        );
    }

    this.selectedDelivery = null;

    if (isNewItem && pressedEnter) {
      this.addDelivery();
    }
  }

  // Delete item
  deleteDelivery(item: DeliveryDto) {
    if (this.deliveryDetailsModalRef) {
      this.deliveryDetailsModalRef.hide();
    }

    if (item.id == 0) {
      let itemIndex = this.selectedZone.items.indexOf(this.selectedDelivery);
      this.selectedZone.items.splice(itemIndex, 1);
    } else {
      this.deliveryClient.delete(item.id).subscribe(
        () => this.selectedZone.items = this.selectedZone.items.filter(t => t.id != item.id),
        error => console.error(error)
      );
    }
  }
}
