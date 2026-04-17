


export interface ClientCredentialEntry {
    grant_type: string;
    client_id: string;
    username: string;
    password: string;
}

export interface GetTokenResponse {
    access_token: string;
    token_type: string;
    expires_in: number;
    scope: string;
    id_token: string;
    refresh_token: string;

}


export interface SaveTransactionRequest {
    atdpVersionID: number;
    documentTypeID: number;
    documentNumber: string;
    transactionID: string;
    commerce: string;
    firstName: string;
    secondName: string;
    firstLastName: string;
    secondLastName: string;
    email: string;
    file: string;
    signature: boolean;
    date: string;
    isApproved: boolean;
}


export interface GetConsultProcessResponse {
    code: number;
    codeName: string
    data: {
        datos: {
            guidConv: string;
            procesoConvenioGuid: string;
            guidCiu: string;
            primerNombre: string;
            segundoNombre: string;
            primerApellido: string;
            segundoApellido: string;
            infCandidato: string;
            tipoDoc: string;
            numDoc: string;
            email: string;
            celular: string;
            asesor: string;
            sede: string;
            nombreSede: string;
            codigoCliente: string;
            ejecutarEnMovil: boolean;
            estadoProceso: number,
            finalizado: boolean,
            estadoForense: number;
            validacion: boolean;
            fechaRegistro: string;
            fechaFinalizacion: string;
            fechaExpedicion: string;
            lugarExpedicion: string;
            validationProcessId: number;
            validationProcessExecutionId: number;
            lastStep: string;
            workflowId: number;
        }

    }

}

export interface Conditional {
    value: string;
}


