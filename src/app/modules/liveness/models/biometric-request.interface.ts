// Puedes usar 'string' para GUIDs si el backend los envía como texto
export type Guid = string;

export interface BiometricRequest {
  citizenGUID: Guid;    
  serviceId: number; 
  subType: string;
  value: string;  
  format: string;   
  aditionalData: string;   
  user: string;   
  update: boolean;   
  codeParameter: string;     
  biometricGesture?: string | null;  
  formatGesture?: string | null;     
}
 