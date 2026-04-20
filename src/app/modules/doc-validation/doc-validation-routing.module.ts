import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { checkStateGuard } from 'src/app/core/guards/State.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Validación del Documento', loadComponent: () => import('@doc-validation/pages/doc-validation.component').then(mod => mod.DocValidationComponent), canActivate: [checkStateGuard] },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocValidationRoutingModule { }
