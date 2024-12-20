import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDeleteModalComponent } from '../modal/delete-modal/delete-modal.component';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  dialog = inject(MatDialog);
  model: any = {}

  login() {
    if (this.model.login == null || this.model.password == null)
      this.toastr.error("Necessário informar login e senha.");
    else {
      this.accountService.login(this.model).subscribe({
        next: () => {
          this.router.navigateByUrl('/contacts');
          this.toastr.success('Logado com sucesso!');
        },
        error: error => {
          error.handled = true; 
          this.toastr.error(error.error);
        }
      });
    }
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }

  deletarConta() {
    const dialogRef = this.dialog.open(ConfirmDeleteModalComponent, {
      width: 'auto',
      height: 'auto',
      data: { action: 'delete-account' },
      disableClose: true
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.accountService.deleteAccount(result);
      }
    });
  }
}
