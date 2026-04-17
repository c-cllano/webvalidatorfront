import { computed, inject, Injectable, signal } from '@angular/core';
import { GetConsultProcessResponse } from '@utils/models/parameterization.interface';
import { State, UtilsService } from '@utils/services/utils.service';
import { LocalStorageService } from 'ngx-localstorage';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private utils = inject(UtilsService);
  private storageService = inject(LocalStorageService);
  private globalStylesSignal = signal<any>(null);
  private totalStepsSignal = signal<number>(0);
  private stateSignal = signal<State>(State.start);
  private currentIndexSignal = signal<number>(0);
  private GuidProSignal = signal<string>("");
  private GuidConSignal = signal<string>("");
  private ProcessSignal = signal<GetConsultProcessResponse | null>(null);
  readonly globalStyles = computed(() => this.globalStylesSignal());
  readonly totalSteps = computed(() => this.totalStepsSignal());
  readonly state = computed(() => this.stateSignal());
  readonly currentIndex = computed(() => this.currentIndexSignal());
  readonly guidPro = computed(() => this.GuidProSignal());
  readonly guidCon = computed(() => this.GuidConSignal());
  readonly process = computed(() => this.ProcessSignal());


  constructor() {
    this.restoreFromStorage();
  }

  public setGlobalStyles(value: any) {
    this.globalStylesSignal.set(value);
  }


  public setTotalStepsSignal(value: number) {
    this.totalStepsSignal.set(value);
  }

  public setStateSignal(state: State) {
    this.stateSignal.set(state);
  }

  public setCurrentIndexSignal(value: number) {
    this.SaveCurrentIndex(value);
    this.currentIndexSignal.set(value);
  }

  public setGuidProSignal(value: string) {
    this.SaveGuidPro(value);
    this.GuidProSignal.set(value);
  }
  public setGuidConSignal(value: string) {
    this.SaveGuidCon(value);
    this.GuidConSignal.set(value);
  }

  public setProcessSignal(value: GetConsultProcessResponse) {
    this.ProcessSignal.set(value);
  }

  private SaveGuidCon(value: string): void {
    this.storageService.set('GuidCon', this.utils.encrypt(value!));
  }
  private SaveGuidPro(value: string): void {
    this.storageService.set('GuidPro', this.utils.encrypt(value!));
  }

  private SaveCurrentIndex(value: number): void {
    this.storageService.set('CurrentIndex', this.utils.encrypt(value.toString()!));
  }

  private restoreFromStorage(): void {
    const GuidCon = this.utils.decrypt<string>(this.storageService.get('GuidCon'));
    if (GuidCon) {
      try {
        this.GuidConSignal.set(GuidCon);
      } catch (e) {
        this.GuidConSignal.set("");
      }
    }

    const GuidPro = this.utils.decrypt<string>(this.storageService.get('GuidPro'));
    if (GuidPro) {
      try {
        this.GuidProSignal.set(GuidPro);
      } catch (e) {
        this.GuidProSignal.set("");
      }
    }

    const CurrentIndex = this.utils.decrypt<number>(this.storageService.get('CurrentIndex'));
    if (CurrentIndex) {
      try {
        this.currentIndexSignal.set(CurrentIndex);
      } catch (e) {
        this.currentIndexSignal.set(0);
      }
    }
  }

}
