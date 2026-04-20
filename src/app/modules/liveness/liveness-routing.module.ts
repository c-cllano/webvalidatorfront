import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Captura Facial', loadComponent: () => import('@modules/liveness/pages/liveness.component').then(mod => mod.LivenessComponent)}]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LivenessRoutingModule { }
