import { inject } from "@angular/core";
import { CanActivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { FlowNavigatorService } from "@utils/services/flownavigator.service";
import { getPageAndGuidFromUrl, Pages } from "@utils/services/utils.service";

export const flownavigatorGuard = (): CanActivateFn => {
    return (_route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
        const flowNavigatorService = inject(FlowNavigatorService);
        const router = inject(Router);
        const PageAndGuid = getPageAndGuidFromUrl(state.url);
        if (!flowNavigatorService.isInitialized()) {
            return router.parseUrl(`/${Pages.redirect}`);
        }
        return flowNavigatorService.isPageActive(PageAndGuid.page);
    };
};
