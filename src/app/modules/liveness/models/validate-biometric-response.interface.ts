// Interfaces para mapear el response mostrado

export interface ValidateBiometricResponse {
  data:  ValidateBiometricData;
  code: number;            // 200
  codeName: string;        // "OK"
}

export interface  ValidateBiometricData {
  isValid: boolean; 
  result: string;
  score: number;          
  isHomologation: boolean;
  isSuccessful: boolean;
  transactionError: string[];
}