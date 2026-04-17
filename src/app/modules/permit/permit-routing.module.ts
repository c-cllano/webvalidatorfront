import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { checkStateGuard } from 'src/app/core/guards/State.guard';



const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Permisos', loadComponent: () => import('@permit/pages/permit.component').then(mod => mod.PermitComponent),canActivate:[checkStateGuard] },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PermitRoutingModule { }
