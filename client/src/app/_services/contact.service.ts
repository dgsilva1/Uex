import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Contact } from '../_models/contact';
import { tap } from 'rxjs';
import { MapsComponent } from '../maps/maps.component';

@Injectable({
  providedIn: 'root'
})
export class ContactsService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  contacts = signal<Contact[]>([]);

  getContacts() {
    return this.http.get<Contact[]>(this.baseUrl + 'contact/contacts').subscribe({
      next: contacts => this.contacts.set(contacts)
    });
  }

  getContact() {
    return this.http.get<Contact[]>(this.baseUrl + 'contact/contacts');
  }

  fetchContacts() {
    return this.http.get<Contact[]>(this.baseUrl + 'contact/contacts');
  }

  updateContact(contact: Contact) {
    return this.http.put(this.baseUrl + 'contact/update-contact', contact).pipe(
      tap(() => {
        this.contacts.update(contacts => contacts.map(m => m.contactId == contact.contactId
          ? contact : m))
      })
    )
  }

  register(contact: Contact) {
    return this.http.post(this.baseUrl + 'contact/register', contact).pipe(
      tap(() => {
        if (this.contacts.length == 0) { 
          this.getContact().subscribe({
            next: (contacts) => {
              this.contacts.set(contacts);
              this.contacts.update(contacts => contacts.map(m => m.contactId === contact.contactId
                ? contact
                : m));
            },
          });
        } else {
          this.contacts.update(contacts => contacts.map(m => m.contactId === contact.contactId
            ? contact
            : m));
        }
      })
    )
  }

  searchCep(cep: string) {
    return this.http.get(this.baseUrl + 'contact/search/'+ cep);
  }

  deleteContact(contactId: number) {
    return this.http.delete(this.baseUrl + 'contact/delete-contact/' + contactId);
  }

}
