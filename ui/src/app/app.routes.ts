import { Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { ProductsComponent } from './products/products.component';
import { AuthGuard } from './utils/auth/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/auth',
    pathMatch: 'full',
  },
  { path: 'auth', component: AuthComponent },
  { path: 'products', component: ProductsComponent, canActivate: [AuthGuard] },
];
