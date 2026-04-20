export interface saveBiometricResponse {
  data:  SaveBiometricData;
  code: number;            // 200
  codeName: string;        // "OK"
}

export interface  SaveBiometricData {
  isHomologation: boolean;
  isSuccessful: boolean;
  transactionError: string[];
}