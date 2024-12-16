import { Component, OnInit, inject } from '@angular/core';
import { RegisterFormComponent } from '../register-form/register-form.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterFormComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  registerMode = false;

  registerToggle() {
    this.registerMode = !this.registerMode
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
