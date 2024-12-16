import { Component, Inject, ViewChild, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { Contact } from '../../_models/contact';
import { FormsModule, NgForm } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ContactsService } from '../../_services/contact.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css'],
  imports:[MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, FormsModule]
})
export class ModalComponent {
  model: any = {}
  contactService = inject(ContactsService);
  private toastr = inject(ToastrService);

  @ViewChild('contactForm') contactForm!: NgForm;

  constructor(
    public dialogRef: MatDialogRef<ModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Contact
  ) {
    this.model = { ...data };
  }

  saveContact() {
    if (this.contactForm?.valid) {
      if (this.model.contactId == null) {
        this.contactService.register(this.model).subscribe({
          next: response => {
            this.dialogRef.close(true);
          }
        })
      } else {
        this.contactService.updateContact(this.model).subscribe({
          next: response => {
            this.dialogRef.close(true);
          }
        })
      }
    }
  }

  fetchAdress() {
    if (this.model.cep && this.model.cep.length === 8) {
      this.contactService.searchCep(this.model.cep).subscribe({
        next: (response: any) => {
          if (response.erro) {
            this.toastr.error('CEP inválido!');
            return;
          }

          this.model.street = response.logradouro;
          this.model.district = response.bairro;
          this.model.city = response.localidade;
          this.model.state = response.uf;
        },
        error: (error) => {
          console.error('Erro ao buscar dados no Via Cep:', error);
          this.toastr.error('Erro ao buscar dados no Via Cep');
        },
      });
    } else {
      this.toastr.error('Digite um CEP válido!');
    }
  }

  cancel() {
    this.dialogRef.close();
  }
}

