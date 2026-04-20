import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Instrucciones Captura Facial', loadComponent: () => import('@modules/liveness-guide/pages/liveness-guide.component').then(mod => mod.LivenessGuideComponent)}]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LivenessGuideRoutingModule { }
