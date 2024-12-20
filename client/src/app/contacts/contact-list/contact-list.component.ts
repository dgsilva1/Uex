import { Component, OnInit, inject } from '@angular/core';
import { ContactsService } from '../../_services/contact.service';
import { RouterLink } from '@angular/router';
import { Contact } from '../../_models/contact';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, NgStyle } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ModalComponent } from '../../modal/cadastro-contact/modal.component';
import { MatIconModule } from '@angular/material/icon';
import { MapsComponent } from '../../maps/maps.component';
import { MapService } from '../../_services/map.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [RouterLink, FormsModule, NgFor, NgStyle, NgIf, MatIconModule, ModalComponent, MapsComponent],
  templateUrl: './contact-list.component.html',
  styleUrl: './contact-list.component.css'
})
export class ContactListComponent implements OnInit{
  contacts: Contact[] = []; 
  filteredContacts: Contact[] = []; 
  searchText: string = '';
  contactsService = inject(ContactsService);
  dialog = inject(MatDialog);
  private toastr = inject(ToastrService);
  private mapService = inject(MapService);

  ngOnInit(): void {
      this.loadContacts();
  }

  openDialog(contact?: Contact) {
    const dialogRef = this.dialog.open(ModalComponent, {
      width: 'auto',
      height: 'auto',
      data: contact,
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const msg = contact == null ? "Usuário cadastrado com sucesso." : "Usuário alterado com sucesso.";
        this.toastr.success(msg);
        this.loadContacts();
      }
    });
  }

  addContact() {
    this.openDialog();
  }

  editContact(contact: Contact) {
    this.openDialog(contact);
  }

  loadContacts() {
    this.contactsService.fetchContacts().subscribe({
      next: (response) => {
        this.contactsService.contacts.set(response);
        this.filteredContacts = response;
      },
      error: (error) => {
        console.error('Erro ao carregar os contatos', error);
      }
    });
  }

  filterContacts(): void {
    const lowerText = this.searchText.toLowerCase();
    this.filteredContacts = this.contactsService.contacts()
      .filter(contact =>
        contact.name.toLowerCase().includes(lowerText) ||
        contact.cpf.includes(lowerText)
      );
  }

  deleteContact(id: number) {
    this.contactsService.deleteContact(id).subscribe(() => {
      this.loadContacts();
    });
  }

  clearSearch(): void {
    this.searchText = '';
    this.filteredContacts = [...this.contactsService.contacts()];
  }

  selectContact(contact: Contact): void {
    if (contact.latitude && contact.longitude) {
      this.mapService.setCenter(contact.latitude, contact.longitude, 17);
    }
  }
}
