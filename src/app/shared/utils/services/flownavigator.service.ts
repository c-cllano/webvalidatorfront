import { inject, Injectable, PLATFORM_ID, signal } from "@angular/core";
import { Pages, State, typeCurrent, TypePermission, UtilsService } from "@utils/services/utils.service";
import { StateService } from "@utils/services/state.service";
import { LocalStorageService } from "ngx-localstorage";
import { TrackEventService } from '@utils/services/trackevent.service';
import { ActionEvent } from '@utils/models/trackevent.interface';
import { Alerpermitdenied } from '@utils/models/utils.interface';
import { Router } from "@angular/router";
import { ParameterizationConsumeService } from '@utils/services/parameterization-consume.service';
import { AlertMessageService } from '@utils/services/alert.service';
import { isPlatformBrowser } from "@angular/common";
import { Conditional } from '@utils/models/parameterization.interface';



@Injectable({ providedIn: 'root' })
export class FlowNavigatorService {

  public stateService = inject(StateService);
  private storageService = inject(LocalStorageService);
  private traficSrv = inject(TrackEventService);
  private router = inject(Router);
  private utils = inject(UtilsService);
  private parameterizationConsumeService = inject(ParameterizationConsumeService);
  private currentStep = signal<string>('');
  private internalPages = signal<string[]>([]);
  private currentInternalIndex = signal<number>(0);
  private initialized = signal<boolean>(false);
  private currentpage = signal<string>("");
  private hasPermission = signal<boolean>(true);
  private hasPermissionPermitButton = signal<boolean>(false);
  private hasConditional = signal<boolean>(false);

  private Alert = inject(AlertMessageService);
  private platformId = inject<Object>(PLATFORM_ID);

  constructor() {
    this.restoreFromStorage();
    if (isPlatformBrowser(this.platformId)) {
      document.addEventListener(
        'click',
        async (event: MouseEvent) => {
          console.warn(event);
          this.hasPermission.set(await this.executePermission());
          if (!this.hasPermission()) {
            event.preventDefault();
            event.stopPropagation();
          }
        },
        true 
      );

    }
  }

  // Inicializa el flujo y devuelve la primera pantalla interna real
  public async init(): Promise<string | null> {
    const step = typeCurrent.Inicio;
    this.initialized.set(true);
    this.SaveInitialized(this.initialized());
    this.stateService.setCurrentIndexSignal(0);
    await this.loadStep(this.stateService.process()?.data.datos.workflowId ?? 0, this.stateService.guidCon(), step);
    if (this.internalPages().length === 0) {
      return null;
    }
    const firstPage = this.internalPages()[0];
    this.currentpage.set(firstPage);
    if (firstPage === Pages.permit) {
      this.hasPermissionPermitButton.set(true);
      this.hasPermission.set(await this.executePermission());
    }
    if (this.hasPermission()) {
      this.Savecurrentpage(this.currentpage())
      return this.currentpage();
    }
    return null;
  }

  // Avanza automáticamente al siguiente step o página interna
  public async goNext(): Promise<string | null> {
    if (this.currentInternalIndex() < this.internalPages().length - 1) {
      this.currentInternalIndex.update(v => v + 1);
      this.SavecurrentInternalIndex(this.currentInternalIndex());
      if (this.currentpage() === Pages.permit) {
        this.hasPermissionPermitButton.set(true);
        this.hasPermission.set(await this.executePermission());
      }
      if (this.hasPermission()) {
        const nextPage = this.internalPages()[this.currentInternalIndex()];
        this.currentpage.set(nextPage);
        if (this.stateService.currentIndex() < this.stateService.totalSteps() - 1) {
          this.stateService.setCurrentIndexSignal(this.stateService.currentIndex() + 1);
        }
        this.Savecurrentpage(this.currentpage())
        return this.currentpage();
      }
      return null;
    }
    const nextStep = await this.getNextStep(this.stateService.process()?.data.datos.workflowId ?? 0, this.stateService.guidCon());
    if (!nextStep) return null;
    await this.loadStep(this.stateService.process()?.data.datos.workflowId ?? 0, this.stateService.guidCon(), nextStep);
    if (this.internalPages().length === 0) {
      return null;
    }
    if (this.currentpage() === Pages.permit) {
      this.hasPermissionPermitButton.set(true);
      this.hasPermission.set(await this.executePermission());
    }
    if (this.hasPermission()) {
      const firstPage = this.internalPages()[0];
      this.currentpage.set(firstPage);
      if (this.stateService.currentIndex() < this.stateService.totalSteps() - 1) {
        this.stateService.setCurrentIndexSignal(this.stateService.currentIndex() + 1);
      }
      this.Savecurrentpage(this.currentpage())
      return this.currentpage();
    }
    return null;
  }

  // Retrocede automáticamente
  public async goBack(): Promise<string | null> {
    if (this.stateService.currentIndex() > 0) {
      this.stateService.setCurrentIndexSignal(this.stateService.currentIndex() - 1);
    }
    if (this.currentInternalIndex() > 0) {
      this.currentInternalIndex.update(v => v - 1);
      this.SavecurrentInternalIndex(this.currentInternalIndex());
      this.currentpage.set(this.internalPages()[this.currentInternalIndex()]);
      if (this.currentpage() === Pages.permit) {
        this.hasPermissionPermitButton.set(true);
        this.hasPermission.set(await this.executePermission());
      }
      this.Savecurrentpage(this.currentpage())
      return this.currentpage();
    }
    const prevStep = await this.getPreviousStep(this.stateService.process()?.data.datos.workflowId ?? 0, this.stateService.guidCon());
    if (!prevStep) return null;
    await this.loadStep(this.stateService.process()?.data.datos.workflowId ?? 0, this.stateService.guidCon(), prevStep);
    this.currentInternalIndex.set(this.internalPages().length - 1);
    this.SavecurrentInternalIndex(this.currentInternalIndex());
    this.currentpage.set(this.internalPages()[this.currentInternalIndex()]);
    if (this.currentpage() === Pages.permit) {
      this.hasPermissionPermitButton.set(true);
      this.hasPermission.set(await this.executePermission());
    }
    this.Savecurrentpage(this.currentpage())
    return this.currentpage();
  }

  ///Avanza a la  siguiente página interna
  public async goToPage(pages: Pages): Promise<string | null> {
    const index = this.internalPages().indexOf(pages);
    if (index === -1) {
      this.Alert.Warning(`La página ${pages} no existe en el flujo actual`);
      return null;
    }
    this.currentInternalIndex.set(index);
    this.SavecurrentInternalIndex(index);

    if (this.hasPermission()) {
      if (this.stateService.currentIndex() < this.stateService.totalSteps() - 1) {
        this.stateService.setCurrentIndexSignal(this.stateService.currentIndex() + 1);
      }
      this.currentpage.set(pages);
      this.Savecurrentpage(pages);
      return pages;
    }
    return null;
  }


  // Carga un step completo desde la API y setea las páginas internas activas
  private async loadStep(workFlowId: number, agreementId: string, step: string) {
    /// verificar bien la condicion
    const Conditional: Conditional = {
      value: "0"
    }
    const flowData = await this.parameterizationConsumeService.LoadGetProcessFlow(workFlowId, agreementId, step, Conditional);
    if (!flowData) {
      this.internalPages.set([]);
      return;
    }
    if (step === typeCurrent.Inicio) {
      this.stateService.setTotalStepsSignal(flowData.data.countPages);
    }
    this.currentStep.set(flowData.data.currentStep);
    this.SavecurrentStep(this.currentStep());
    this.internalPages.set(Object.keys(flowData.data.dataConfiguration.pages)
      .filter(k => flowData.data.dataConfiguration.pages[k] === true));
    this.SaveinternalPages(this.internalPages())
    this.currentInternalIndex.set(0);
    this.SavecurrentInternalIndex(this.currentInternalIndex());
  }

  // Obtiene siguiente paso principal desde la API
  private async getNextStep(workFlowId: number, agreementId: string): Promise<string | null> {
    /// verificar bien la condicion
    const Conditional: Conditional = {
      value: "0"
    }
    const flowData = await this.parameterizationConsumeService.LoadGetProcessFlow(workFlowId, agreementId, this.currentStep(), Conditional);
    if (!flowData) return null;
    this.hasConditional.set(flowData.data.conditional ?? false);
    return flowData.data.nextStep[0] || null;
  }

  // Obtiene paso anterior principal desde la API
  private async getPreviousStep(workFlowId: number, agreementId: string): Promise<string | null> {
    /// verificar bien la condicion
    const Conditional: Conditional = {
      value: "0"
    }
    const flowData = await this.parameterizationConsumeService.LoadGetProcessFlow(workFlowId, agreementId, this.currentStep(), Conditional);
    if (!flowData) return null;
    this.hasConditional.set(flowData.data.conditional ?? false);
    return flowData.data.backStep[0] || null;
  }


  public isInitialized(): boolean {
    return this.initialized();
  }


  public isPageActive(page: string): boolean {
    const excludeKeys = [Pages.permitdenied, Pages.flowcompletion];
    if (!excludeKeys.includes(page as Pages)) {
      return this.currentpage().includes(page);
    }
    return true
  }

  private restoreFromStorage(): void {
    const currentStep = this.utils.decrypt<string>(this.storageService.get('currentStep'));
    if (currentStep) {
      try {
        this.currentStep.set(currentStep);
      } catch (e) {
        this.currentStep.set('');
      }
    }

    const internalPages = this.utils.decrypt<string[]>(this.storageService.get('internalPages'));
    if (internalPages) {
      try {
        this.internalPages.set(internalPages);
      } catch (e) {
        this.internalPages.set([]);
      }
    }
    const currentInternalIndex = this.utils.decrypt<number>(this.storageService.get('currentInternalIndex'));
    if (currentInternalIndex) {
      try {
        this.currentInternalIndex.set(currentInternalIndex);
      } catch (e) {
        this.currentInternalIndex.set(0);
      }
    }

    const currentpage = this.utils.decrypt<string>(this.storageService.get('currentpage'));
    if (currentpage) {
      try {
        this.currentpage.set(currentpage);
      } catch (e) {
        this.currentpage.set("");
      }
    }

    const isInit = this.utils.decrypt<boolean>(this.storageService.get('initialized'));
    this.initialized.set(isInit!);
  }

  private SavecurrentStep(value: string): void {
    this.storageService.set('currentStep', this.utils.encrypt(value!));
  }

  private SaveinternalPages(value: string[]): void {
    this.storageService.set('internalPages', this.utils.encrypt(JSON.stringify(value)!));
  }

  private SavecurrentInternalIndex(value: number): void {
    this.storageService.set('currentInternalIndex', this.utils.encrypt(value.toString()!));
  }

  private Savecurrentpage(value: string): void {
    this.storageService.set('currentpage', this.utils.encrypt(value!));
  }

  private SaveInitialized(value: boolean): void {
    this.storageService.set('initialized', this.utils.encrypt(value.toString()!));
  }

  // 🔹 Centralizamos la consulta de permisos
  private async queryPermission(name: PermissionName): Promise<PermissionStatus | null> {
    try {
      return await navigator.permissions.query({ name });
    } catch (error) {
      this.Alert.Warning(`Permissions API not supported:${error}`);
      return null;
    }
  }


  // 🔹 Traemos ambos permisos al mismo tiempo
  private async checkAllPermissions(): Promise<{ location: string; camera: string }> {
    const [location, camera] = await Promise.all([
      this.queryPermission("geolocation" as PermissionName),
      this.queryPermission("camera" as PermissionName),
    ]);
    return {
      location: location?.state ?? "denied",
      camera: camera?.state ?? "denied",
    };
  }

  // 🔹 Lógica principal de validación
  private async executePermission(): Promise<boolean> {
    const excludedPages = [Pages.permitdenied, Pages.permit, Pages.welcome, Pages.redirect];
    if (excludedPages.includes(this.currentpage() as Pages) && !this.hasPermissionPermitButton()) return true;
    this.track("Solicitud de permisos", this.currentpage());
    const { location, camera } = await this.checkAllPermissions();
    let deniedType: TypePermission | null = null;
    const needsValidation =
      ["denied", "prompt"].includes(location) || ["denied", "prompt"].includes(camera);
    if (needsValidation) {
      const alertPermission = await this.validatePermission();
      deniedType = this.getDeniedPermissionType(alertPermission);
      if (deniedType) {
        this.navigateToDenied(deniedType, this.currentpage());
        return false;
      }
      //this.reloadCurrentPage(deniedType!);
    } else if (location === "granted" && camera === "granted") {
      this.reloadCurrentPage(deniedType!);
    }
    return true;
  }

  // 🔹 Obtiene el tipo de permiso denegado
  private getDeniedPermissionType(alertPermission: Alerpermitdenied): TypePermission | null {
    if (alertPermission.permitdenied) return TypePermission.permitdenied;
    if (alertPermission.locationdenied) return TypePermission.locationdenied;
    if (alertPermission.cameradenied) return TypePermission.cameradenied;
    return null;
  }

  // 🔹 Navegar a "denied"
  private navigateToDenied(type: TypePermission, page: string): void {
    this.router.navigate(
      [`/${Pages.permitdenied}`, this.stateService.guidPro()],
      { queryParams: { TypePermission: type, state: State.process } }
    );
    this.track(`Solicitud de permisos: ${type} denegado`, page);
  }

  // 🔹 Recargar página actual
  private reloadCurrentPage(type: TypePermission): void {
    this.router.navigate(
      [`/${this.currentpage()}`, this.stateService.guidPro()],
      { queryParams: { state: State.process } }
    );
    this.track(`Solicitud de permisos : ${type} habilitado`, this.currentpage());
    this.hasPermissionPermitButton.set(false);
  }

  // 🔹 Validación unificada
  private async validatePermission(): Promise<Alerpermitdenied> {
    let denied: Alerpermitdenied = { cameradenied: false, locationdenied: false, permitdenied: false };
    const hasLocation = await this.utils.handlePermission(TypePermission.locationdenied);
    if (!hasLocation) denied.locationdenied = true;
    const hasCamera = await this.utils.handlePermission(TypePermission.cameradenied);
    if (!hasCamera) denied.cameradenied = true;
    if (!hasLocation && !hasCamera) denied.permitdenied = true;
    return denied;
  }

  private track(action: string, Page: string): void {
    this.traficSrv.RegisterEvent(<Partial<ActionEvent>>{
      componente: `${Page}Component`,
      accion: action
    }, true);
  }


}
