export interface GetResponse {
  code: number;
  codeName: string;
  data: any;
}

export interface Alerpermitdenied {
    permitdenied: boolean;
    locationdenied: boolean;
    cameradenied:boolean
}