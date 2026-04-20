import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { checkStateGuard } from 'src/app/core/guards/State.guard';


const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Permisos Denegados', loadComponent: () => import('@permitdenied/pages/permitdenied.component').then(mod => mod.PermitdeniedComponent),canActivate:[checkStateGuard] },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PermitdeniedRoutingModule { }
