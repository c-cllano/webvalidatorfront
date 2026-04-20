import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { StateService } from '@utils/services/state.service';
import { State } from '@utils/services/utils.service';


export const checkStateGuard: CanActivateFn = () => {
    const stateService = inject(StateService);
    return stateService.state() === State.start || stateService.state() === State.process;
}