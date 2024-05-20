import { HttpClient } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';

export class UserService {
  apiEndpoint: string = '';
  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
  ) {
    this.apiEndpoint = "http://localhost:8080/api"
  }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Email: ['', Validators.email],
    FullName: [''],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
    }),
  });

  login(formData: any) {
    return this.http.post(this.apiEndpoint + '/user/login', formData);
  }

  public logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('permissions');
    localStorage.removeItem('tenant');
    localStorage.removeItem('userName');
    localStorage.removeItem('role');
    localStorage.removeItem('lastAccess');
    this.router.navigate(['/auth']);
  }
}
