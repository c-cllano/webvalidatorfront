
using System.ComponentModel;

namespace Domain.Enums
{
    public class Enumerations
    {
        public enum TipoConvenio : int
        {
            WEB = 1,
            MOVIL = 2,
            AUTHID = 3
        }
        public enum TipoOTP : byte
        {
            SMS = 1,
            MAIL = 2
        }
        public enum TipoEntrada : byte
        {
            GuidCiudadano = 1,
            GuidConvenio = 2
        }

        public enum TipoDocumentos : byte
        {
            CC = 1,
            TI = 2,
            CURP = 19,
            CCE = 20
        }

        public enum ProveedorSMS : int
        {
            ProxyOlimpiaSMS = 7,
            OlimpiaSMS = 8
        }

        public enum ServiciosEnum : long
        {
            ATDP = 1,
            OTP_MAIL = 2,
            OTP_SMS = 3,
            BIOMETRIA_DOCUMENTO = 4,
            BIOMETRIA_FACIAL = 5,
            PSD = 6,
            NOTIFICACION_SOL_1 = 8,
            SISEC_VAL = 9,
            SISEC_POS_SINCRONO = 10,
            SISEC_POS_ASINCRONO = 11,
            FIRMA_ENROLAMIENTO = 14,
            FIRMA_VALIDACION = 15,
            MODO_INSTRUCTOR = 18,
            ANI = 21,
            FUENTES_ABIERTAS = 22,
            GROUP_MATCH = 25,
            BILL = 28,
            NOTIFICAR_WHATSAAP = 30,
            ANTISPOOFING = 32,
            CALIDAD_IMAGEN = 33,
            DRIVINGLICENSE = 34,
            FEDERALLICENSE = 35,
            LOCALIZATION = 37,
            CARPETADIGITAL = 38,
            DOCUMENTOPENSOURCE = 39,
            FACERISK = 40,
            MEETINGID = 41,
            ONBOARDING_WHATSAAP = 42,
            CALIDAD_DOCUMENTOS = 44,
            EMAIL_AGE = 47,
            INTEGRACIONJUMIO = 48,
            SERVICIOVERID = 49
        }

        public enum ServicioSubTipoEnum : long
        {
            FRONTAL = 1,
            ANVERSO = 2,
            REVERSO = 3,
            BARCODE = 4,
            ROSTRO_VIVO = 5,
            FIRMA_ELECTRONICA = 6,
            CODIGO_QR = 7,
            CALIDAD_DOCUMENTO = 8,
            CALIDAD_SELFIE = 9,
            DIRECTO = 10,
            INTEGRADO = 11,
            SELFIE = 12,
            GESTURE = 13,
            VIDEO = 14
        }

        public enum ProvidersEnum : long
        {
            ANI = 1,
            ARES = 2,
            OTP = 4,
            XPERIAN = 5,
            REINTENTOS = 6,
            OLIMPIA = 8,
            SISEC = 12,
            AWS = 13,
            MEGVI = 14,
            HUBSMS = 15,
            REGULA = 16,
            INLITE = 17,
            OTPRECONOSER = 18,
            AMAZON_OCR = 19,
            SDK_MOVIL = 20,
            VERID = 21,
            JUMIO = 22
        }

        public enum ProvidersOTPEnum : long
        {
            ReconoserID = 1,
            Masivian = 2,

        }

        public enum ResultadoBiometriaEnum : byte
        {
            OK = 1,
            ZONA_GRIS,
            FAIL
        }

        public enum LogBiometriaTypeEnum : byte
        {
            ENROLAMIENTO = 1,
            VALIDACION
        }

        public enum BusinessError
        {
            ContacteAdministrador,
            ConvenioNotFound,
            NotFoundProcess,
        }

        public enum ParametroServicioEnum : long
        {
            NivelCaptura = 1,
            MetodoCaptura = 2,
            ContinuarProceso = 3,
            Reintentos = 4,
            EsperaReintentos = 5,
            Robot = 6,
            Listas = 7,
            Gestos = 8,
            AccountSidWhatsApp = 9,
            TokenAuthWhatsApp = 10,
            TelefonoWhatsApp = 11,
            TokenAntiSpoofing = 12,
            CodAni = 13,
            UsuarioConexionAni = 14,
            PassAni = 15,
            TokenFuentes = 16,
            ValorMinimoScore = 17,
            Limite = 18,
            RechazarProceso = 19,
            DiferenciaCaracteresNombre = 20,
            CargueAutomaticoFaceRisk = 21,
            RechazarEnrolamientoAutomatico = 22,
            TokenSigep = 23,
            ValidadSigep = 24,
            OTPProveedor = 45,
            OTPMasivianCodigoAplicacion = 46,
            OTPMasivianUsuario = 47,
            OTPMasivianContrasena = 48,
            BandaRiesgo = 50,
            IdCliente = 51,
            Usuario = 52,
            Clave = 53,
            Url = 56
        }


        public enum TiposTareaEnum : long
        {
            ValidacionSisec = 1,
            FuentesAbiertas = 2
        }

        public enum EstadoTareaEnum : byte
        {
            Pendiente = 1,
            Ejecucion,
            Finalizada
        }

        public enum MesesCedulaColombiana : int
        {
            ENE = 01,
            FEB,
            MAR,
            ABR,
            MAY,
            JUN,
            JUL,
            AGO,
            SEP,
            OCT,
            NOV,
            DIC
        }

        public enum eClaseServicio : byte
        {
            ENROLAMIENTO = 1,
            VALIDACION = 2,
            ENROLAMIENTO_VALIDACION = 3,
            INTERNO = 4
        }

        /// <summary>
        /// Personalizacions
        /// </summary>
        public enum PersonalizacionesEnum : long
        {
            ColorPrimario = 1,
            ColorSecundario = 2,
            Icono = 3,
            SoloRostroVivo = 4,
            NotificarEnrolamiento = 5,
            DesactivarOCR = 6,
            MailProcesoFinalizado = 7,
            SubirDocumento = 8,
            GestoSimple = 9,
            InterfazSimple = 10,
            MensajeSolicitudValidacion = 11,
            DesactivarLecturaBarcode = 12,
            CargaRapida = 13,
            MensajeSolicitudValidacionSMS = 14,
            TextoFinalValidador = 15,
            ContinuarMovil = 16,
            MensajeOtp = 17,
            ForzarRostroDocumento = 18,
            ColorTerciario = 32,
            MaximoTransaccionesDia = 33,
            MinimoScoreRostroDocumentoEnrolamiento = 34,
            HttpClient = 35,
            RiskThreshold = 36,
            MaxOtpLength = 37,
            JustOtpNumber = 38,
            MaxOtpTry = 39,
            SendMailAmazon = 42,
            EstadoAni = 43,
            ShortenLink = 44,
            MensajeSolicitudValidacionWhatsApp = 45,
            LinkExpiration = 47,
            SubjectMailValidation = 54,
            SubjectMailOtp = 55,
            OtpMailTemplate = 56,
            OpenFacialNativeCamera = 59,
            AuthIdTemplateMail = 60,
            SubjectAuthIdMail = 61,
            cancelarenrolamientos = 63,
            TemplateMailValidationOnboarding = 65,
            SendSmsTwilio = 66,
            RetryNotification = 67,
            ValidarProcesoActivoConsola = 69,
            MaximoTransaccionesMes = 70,
            ConvenioHibrido = 72,
            ErroresPersonalizados = 74,
            OcultarLinkValidacion = 78,
            FormartoRespuestaSoliciutdValidación = 81,
            PlataformaEasyTrack = 82,
            HidePersonalInfo = 83,
            MaxRevisionDactiloscopista = 84,
            MinimoScoreRostroDocumentoValidacion = 88,
            MaximoLoingAct = 85,
            AppId = 89,
            ValidarDataCiudadano = 90,
            ReintentosFinalzaProceso = 91,
            OkTimeDactyloscopy = 96,
            AlertTimeDactyloscopy = 97,
            DangerTimeDactyloscopy = 98,
            FinalizarSiSpoof = 99,
            RetornanInfoAniSolProcesoEst = 116,
            ValidacionConvenioPrefijoPais = 119,
            OtpMailAndSms = 122,
            NuevosErroresAni = 124,
            MaxTx = 125,
            CanalSeguro = 127,
            AntispoofingNuevaVersión = 128,
            RegExpDispAceptados = 129,
            RegExpDispRechazados = 130,
            DocValidateDocumentProvider = 201,
            UrlExclusiva = 202,
            AntispoofingIbeta2 = 203,
            UmbralSpoofDocumento = 206,
            VerIdCC = 207,
            JsonMensajesValidacionDoc = 214,
            AttemptPermissions = 216,
            BlockNavigator = 219
        }

        /// <summary>
        /// Tipos de personalizaciones
        /// </summary>
        public enum TipoInput : int
        {
            COLOR = 1,
            ARCHIVO = 2,
            ENTERO = 3,
            BOOLEANO = 4, //0 o 1
            STRING = 5
        }

        public enum TipoProcesoMovil : int
        {
            MOVIL = 1,
            AUTHID = 2
        }

        public enum TipoForense : int
        {
            FORENSE = 1,
            SPOOFING = 2
        }

        public enum TipoFuentesAbiertas : long
        {
            Robot = 1,
            List = 2
        }

        public enum Motivo : long
        {
            Ok = 1,
            ValidacionFacialFail = 2,
            ValidacionFacialGris = 3,
            ComparacionRostroDocumentoFail = 4,
            DocumentoAnversoFail = 5,
            DocumentoReversoFail = 6,
            MaxReintentosFacial = 7,
            MaxReintentosPSD = 8,
            ComprobacionDomicilio = 9,
            NoHayPreguntasPSD = 10,
            DispositivoNosuperaResoluciónMinimaHorizontal320px = 11,
            DispositivoNosuperaResoluciónMinimaVertical452px = 12,
            RechazadoDactiloscopista = 13,
            LicenciaConduccion = 14,
            ValDocumentoExtranjero = 15,
            IPRisk = 16,
            FaceRiskEncontradoCentralRiesgo = 18,
            EnrolamientoNoPermitido = 19,
            SpoofFinished = 20,
            RejectedAtdp = 21,
            SeguridadDocumentos = 23,
            MaxReintentosDocumento = 24,
            ReglaAutenticidadLongitud = 25,
            ReglaAutenticidadDeteccionletra = 26,
            ReglaAutenticidadAlteraciondocumento = 27,
            ProcesofinalizadoPorseguridad = 30,
            DetecciónAdulteraciónMicrotexto = 31,
            MalaCalidadSelfie = 32,
            IncertidumbreAnalisisSelfie = 33,
            CancelDocument = 34,
            FraudeInfoSuministrada = 35,
            FiltroDetectado = 36,
            ProblemaEnRosto = 37,
            ProblemaEnImagen = 38,
            SpoofDocumento = 39,
            SuperoRiesgoConvenio = 40,
            MaxReintentosPermisosCam = 41
        }
        public enum TipoImagenEnum
        {
            Document = 2,
            Selfie = 1,
            DocumentPhoto = 3

        }
        public enum EstadoSolicitudTrustID
        {
            Creado,
            Aprobado,
            Rechazado
        }

        public enum TipoCodigoBarras
        {
            Ean8 = 0,
            Code93 = 1,
            Upce = 2,
            Ean13 = 3,
            Ucc128 = 4,
            Code32 = 5,
            Code128 = 6,
            Codabar = 7,
            Code39 = 8,
            Pdf417 = 9,
            QR = 10
        }

        public enum TipoArchivo
        {
            Imagen = 1,
            Video = 2
        }

        public enum TipoLogeo
        {
            Facial = 1,
            Huella = 2
        }

        public enum TipoProcesoBackground : int
        {
            DELETE_FACE_RISK = 1,
            ADD_FACE_RISK = 2
        }

        public enum EstadoBackground : int
        {
            PENDING = 1,
            ERROR = 2,
            FINISH = 3
        }

        /// <summary>
        /// indica el tipo de PlataformaServicio en la tabla proceso convenio
        /// </summary>
        public enum PlataformaServicio : int
        {
            ONBOARDINGWHATSAPP = 1,
            WEB = 2
        }

        /// <summary>
        /// enumeracion que indica el tipo de envio
        /// de la notificacion
        /// </summary>
        public enum TypeNotifiation : int
        {
            MAIL = 1,
            SMS = 2,
            WSP = 3
        }

        /// <summary>
        /// enumeracion que indica el tipo de mensageroa
        /// de la notificacion
        /// </summary>
        public enum MailType
        {
            SMTP = 1,
            AMAZON = 2
        }

        public enum TipoCampoMiFirma : int
        {
            Fecha = 1,
            Texto = 2,
            Numerico = 3,
            Adjunto = 4
        }

        public enum DaviplatResource : int
        {
            token = 1,
            compra = 2,
            readOtp = 3,
            confirmarCompra = 4
        }

        public enum PositionPhotoEnum : int
        {
            text_abajo = 1,
            text_arriba = 2,
            non_conclusive = 3
        }

        public enum ProviderType
        {
            Regula,
            OlimpiaVerID,
            Jumio
        }

        public enum EstadoProceso
        {
            Pendiente = 0,
            Enrolamiento = 1,
            Validacion = 2
        }

        public enum ConnectionsName
        {
            OKeyConnection,
            DefaultConnection
        }

        public enum ApiName
        {
            ExternalApi,
            BaseUrlReconoser1,
            BaseUrlReconoser2,
            BaseUrlAdtp,
            BaseUrlAni,
            BaseUrlMegvi,
            BaseUrlIbeta1,
            BaseUrlIbeta2,
            BaseUrlVeriFace,
            BaseUrlVerId,
            BaseUrlTokenJumio,
            BaseUrlAccountJumio,
            BaseUrlWhatsapp,
            TokenReconoser,
            SaveDocumentBothSides,
            CompareFaces,
            SaveBiometric,
            SaveBiometricV3,
            StatusValidationRequest,
            ValidateBiometric,
            ValidateBiometricV3,
            ConsultValidation,
            ConsultAgreementProcess,
            ATDPGetFileByID,
            ATDPTransactionGetFileByID,
            ATDPTransactionSave,
            LoginAuthAni,
            ValidationDocumentAni,
            CompareFaceMegvi,
            Ibeta1Main,
            Ibeta2Main,
            GetTokenVeriface,
            VerIdMain,
            GetTokenJumio,
            CreateAccountJumio,
            LoginWhatsapp,
            CreateRequestWhatsapp,
            BaseUrlRecaptcha,
            GetValidateRecaptcha,
            GetTempKeys,
            CancelProcess
        }

        public enum RedirectUrl
        {
            RedirectValidationUrl
        }

        public enum AzureBlobStorageFolders
        {
            ValidationFiles,
            UIFiles
        }

        public enum UpdateStatusCode
        {
            None = 0,
            Process = 1,
            Finalized = 2,
            Canceled = 3,
            Validation = 4,
            Error = 5
        }

        public enum ScoresCode
        {
            [Description("LIVENESS")]
            Liveness,

            [Description("SELFIEVSENROL")]
            RostroSelfieVsRostroEnrolado,

            [Description("ENROLVSDOC")]
            RostroEnroladoVsRostroDocumento,

            [Description("SELFIEVSDOC")]
            RostroSelfieVsRostroDocumento,

            [Description("OCRVSDATA")]
            DatosDocumentoVsDatosOficiales,

            [Description("DOCSPOOF")]
            VerIdSpoofing,

            [Description("DOCSCORE")]
            VerIdScore
        }
    }
}