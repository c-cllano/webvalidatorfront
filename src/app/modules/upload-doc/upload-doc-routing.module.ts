import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { checkStateGuard } from 'src/app/core/guards/State.guard';



const routes: Routes = [
  {
    path: '',
    children: [
      { path: ':Guid', title: 'Subir Documento', loadComponent: () => import('@upload-doc/pages/upload-doc.component').then(mod => mod.UploadDocComponent), canActivate: [checkStateGuard] },

    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UploadDocRoutingModule { }
