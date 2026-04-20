import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Flujo finalizado', loadComponent: () => import('@components/flow-completion/flow-completion.component').then(mod => mod.FlowCompletionComponent) },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FlowCompletionRoutingModule { }