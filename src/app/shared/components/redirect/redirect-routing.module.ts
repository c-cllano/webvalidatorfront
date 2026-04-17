import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { checkStateGuard } from 'src/app/core/guards/State.guard';



const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Redirigir', loadComponent: () => import('@components/redirect/redirect.component').then(mod => mod.RedirectComponent), canActivate: [checkStateGuard] },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RedirectRoutingModule { }