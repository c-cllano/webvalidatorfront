import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { checkStateGuard } from 'src/app/core/guards/State.guard';



const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Bienvenido', loadComponent: () => import('@welcome/pages/welcome.component').then(mod => mod.WelcomeComponent),canActivate:[checkStateGuard] },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class welcomeRoutingModule { }