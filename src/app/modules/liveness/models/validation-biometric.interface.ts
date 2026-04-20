// Puedes usar 'string' para GUIDs si el backend los envía como texto
export type Guid = string;

export interface ValidateBiometric {
  CitizenGUID: Guid;                 
  ValidationProcessId: number;       
  Format: string;                    
  SubType: string;                   
  ServiceId: number;                 
  Biometric: string;                 
  BiometricGesture?: string | null;  
  FormatGesture?: string | null;     
}
 