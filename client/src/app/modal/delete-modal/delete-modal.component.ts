import { Component, Inject, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-delete-modal',
  templateUrl: './delete-modal.component.html',
  styleUrls: ['./delete-modal.component.css'],
  imports: [
    MatDialogModule,
    FormsModule
  ],
})
export class ConfirmDeleteModalComponent {
  password: string = '';
  accountService = inject(AccountService);


  constructor(
    public dialogRef: MatDialogRef<ConfirmDeleteModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private toastr: ToastrService
  ) { }

  confirmDeletion() {
    if (!this.password) {
      this.toastr.error('Por favor, digite sua senha');
      return;
    }

    this.accountService.deleteAccount(this.password).subscribe({
      next: () => {
        this.toastr.success('Conta excluída com sucesso');
        this.accountService.logout();
        this.dialogRef.close(true);
      }
    })
  }

  cancel() {
    this.dialogRef.close();
  }
}
