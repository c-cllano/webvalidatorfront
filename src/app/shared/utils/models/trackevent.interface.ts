export interface EventTraffic {
  Actions: ActionEvent[];
  Devices: EventDeviceInfo[];
  sessionDurationSeconds: number
}

export interface ActionEvent {
  guidagreementprocess?: string;
  componente?: string;
  accion?: string;
  user?: string;
  agreement?: string;
  passed?: number;
  error?: boolean;
  errormessage?: string;
  date?: string;
  status?: string;
}

export interface EventDeviceInfo {
  devicetype?: string;
  browser?: string;
  browserVersion?: string;
  screenResolution?: string;
  screenOrientation?: string;
  language?: string;
  online?: boolean;
  cameraLabel?: string | null;
  cameraWidth?: number | null;
  cameraHeight?: number | null;
  os?: string; 
  osVersion?: string;
  ip?: string;
}
