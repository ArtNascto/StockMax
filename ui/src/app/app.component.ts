import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { MenuItem } from 'primeng/api';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { CheckboxModule } from 'primeng/checkbox';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { TableModule } from 'primeng/table';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MenubarModule,
    MatButtonModule,
    TableModule,
    CardModule,
    NgxSpinnerModule,
    ButtonModule,
    CheckboxModule,
    InputTextModule,
    PasswordModule,
    MatCardModule,
    MatInputModule,
    MatFormFieldModule,
    DialogModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  items: MenuItem[] | undefined;
  constructor(private router: Router) {
    let token = localStorage.getItem('token');
    if (token && token != '') {
      this.router.navigateByUrl('/products');
    }
  }
  islogged(): boolean {
    let token = localStorage.getItem('token');
    if (token && token != '') {
      return true;
    }
    return false;
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('permissions');
    localStorage.removeItem('tenant');
    localStorage.removeItem('userName');
    localStorage.removeItem('role');
    localStorage.removeItem('lastAccess');
    this.router.navigate(['/auth']);
  }
  ngOnInit() {
    this.items = [
      {
        label: 'Produtos',
        icon: 'pi pi-box',
      }
    ];
  }
}
