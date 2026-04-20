import { Routes } from '@angular/router';
import { flownavigatorGuard } from '@guards/flownavigator.guard';
import { Pages } from '@utils/services/utils.service';

export const routes: Routes = [
    { path: 'welcome', loadChildren: () => import('@modules/welcome/welcome-routing.module').then(m => m.welcomeRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: 'permit', loadChildren: () => import('@modules/permit/permit-routing.module').then(m => m.PermitRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: 'permitdenied', loadChildren: () => import('@modules/permitdenied/permitdenied-routing.module').then(m => m.PermitdeniedRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: 'docvalidation', loadChildren: () => import('@modules/doc-validation/doc-validation-routing.module').then(m => m.DocValidationRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: 'uploaddoc', loadChildren: () => import('@modules/upload-doc/upload-doc-routing.module').then(m => m.UploadDocRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: 'redirect', loadChildren: () => import('@components/redirect/redirect-routing.module').then(m => m.RedirectRoutingModule) },
    { path: 'flow-completion', loadChildren: () => import('@components/flow-completion/flow-completion-routing.module').then(m => m.FlowCompletionRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: 'livenessguide', loadChildren: () => import('@modules/liveness-guide/liveness-guide-routing.module').then(m => m.LivenessGuideRoutingModule)},
    { path: 'liveness', loadChildren: () => import('@modules/liveness/liveness-routing.module').then(m => m.LivenessRoutingModule), canActivate: [flownavigatorGuard()] },
    { path: '**', pathMatch: 'full', redirectTo: 'redirect' },

];
