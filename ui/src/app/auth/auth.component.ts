import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import {
  FormGroup,
  FormControl,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MessageService } from 'primeng/api';

import { PasswordModule } from 'primeng/password';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [
    NgxSpinnerModule,
    ButtonModule,
    CheckboxModule,
    InputTextModule,
    PasswordModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss',
})
export class AuthComponent {
  apiEndpoint: string = '';
  constructor(
    private spinner: NgxSpinnerService,
    private router: Router,
    private http: HttpClient,
    private messageService: MessageService
  ) {
    this.apiEndpoint = 'http://localhost:8080/api';
   
  }
  email?: string;
  password?: string;
  welcomeTxt = 'Bem vindo';
  submit() {
    this.spinner.show();
    this.login(this.email, this.password).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('email', this.email ?? '');
        localStorage.setItem('name', res.name ?? '');
        this.spinner.hide();
        this.router.navigateByUrl('/products');
      },
      (err) => {
        this.spinner.hide();
        if (err.status == 401)
          this.messageService.add({
            severity: 'error',
            summary: 'Erro ao logar.',
            detail: err.error?.errorMessage ?? 'Usuário ou senha incorreta.',
          });
        else {
          this.messageService.add({
            severity: 'error',
            summary: 'Erro ao logar.',
            detail: err.error?.errorMessage ?? 'Erro ao logar.',
          });
          console.log(err);
        }
      }
    );
  }
  @Input() error: string | null = null;

  @Output() submitEM = new EventEmitter();

  login(email?: string, password?: string) {
    return this.http.post(this.apiEndpoint + '/user/login', {
      email,
      password,
    });
  }
}
