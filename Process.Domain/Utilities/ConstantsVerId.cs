namespace Process.Domain.Utilities
{
    public static class ConstantsVerId
    {
        private static readonly string UnknownError = "Código de error desconocido";

        private static readonly Dictionary<string, string> _messages = new()
        {
            { "0001", "La solicitud no se hizo con un archivo de imagen valido" },
            { "0002", "La extension del archivo no se encuentra en la lista de extensiones de imagen permitidas" },
            { "0003", "La verificacion de la integridad de la imagen no fue exitosa" },
            { "0004", "La correcion de orientacion de la imagen no fue exitosa" },
            { "0005", "La imagen no se ajusta en alguna de sus dimensiones al tamaño minimo requerido (usar una mejor camara)" },
            { "0006", "Ocurrio un error al reducir el tamaño de la imagen" },
            { "0007", "La imagen no se puede convertir a base 64" },
            { "0000", "La imagen cumple con los criterios minimos de integridad y dimensiones" },
            { "0102", "No se logro detectar un documento en la imagen" },
            { "0103", "Se detectaron multiples documentos en la imagen" },
            { "0101", "Error Interno" },
            { "0100", "Documento aceptable." },
            { "0202", "Documento no permitido." },
            { "0203", "El reverso del documento no está permitido como primer archivo" },
            { "0204", "El documento no cumple con los requisitos mínimos de margen (espacio alrededor del documento)." },
            { "0201", "Error Interno" },
            { "0200", "Documento aceptable." },
            { "0302", "El ángulo de inclinación del documento no debe exceder los grados aceptables en cualquier dirección (horizontal o vertical)." },
            { "0303", "El objeto detectado no cumple con la forma esperada de un documento de identidad" },
            { "0304", "Error procesando la información" },
            { "0301", "Error Interno" },
            { "0300", "Transformación homográfica exitosa" },
            { "1002", "El nivel de color de la imagen indica intento de suplantación de identidad" },
            { "1003", "El documento no tiene una iluminiacion aceptable." },
            { "1004", "El documento tiene un contraste no aceptable." },
            { "1005", "La imagen no es nitida" },
            { "1006", "La imagen presenta áreas borrosas o se observan destellos de luz." },
            { "1007", "Error Interno" },
            { "1008", "Calidad aceptable" },
            { "2001", "Error Interno" },
            { "2000", "Datos extraídos correctamente" },
            { "2101", "Error Interno" },
            { "2100", "Campos gráficos extraídos correctamente" },
            { "2002", "El documento es una suplantación de identidad." },
            { "3001", "Error Interno" },
            { "3000", "El documento es original" },
            { "4001", "Error Interno" },
            { "4000", "Transmision interna de guardado exitosa" },
            { "5001", "Error en procesamiento interno" },
            { "5002", "Las caras del documento no corresponden al mismo tipo de documento, año y/o anverso/reverso" },
            { "5003", "(DOC 2000- 2020) El número del anverso no coincide con el número del reverso" },
            { "5004", "El nombre completo del anverso no coincide con el nombre completo del reverso" },
            { "5005", "La fecha de nacimiento del OCR no coincide contra BCR/MRZ" },
            { "5006", "La fecha de expiración en el mrz no coincide con el numero detectado en OCR" },
            { "5007", "Emparejamiento de campos exitoso." },
            { "6002", "Fecha de impresion no detectada en el tren de datos" },
            { "6003", "El primer caracter de tren de datos no esta en la lista de caracteres permitidos." },
            { "6004", "El caracter del genero en el tren de datos no coincide con el genero detectado." },
            { "6005", "Puntos en el numero de documento no coincide con la fecha de impresion." },
            { "6006", "Las tildes en documento no coinciden con la fecha de impresion." },
            { "6007", "El numero de documento en el tren de datos no coincide con el numero detectado." },
            { "6008", "La firma no coincide con la fecha de impresion." },
            { "6009", "El codigo de barras no coincide con la fecha de impresion." },
            { "6010", "Posicion de foto no coincide con la fecha de impresion." },
            { "6011", "Alineación de rh no coincide con la fecha de impresion." },
            { "6012", "La zona de lectura mecanica (MRZ) no tiene la longitud o caracteres permitidos" },
            { "6013", "La formula de seguridad aplicado a las fechas del MRZ no coinciden" },
            { "6014", "El numero lateral en el MRZ no coincide con el numero detectado en OCR" },
            { "6015", "El número (NUIP) no tiene puntos de mil" },
            { "6016", "Registrador no permitido" },
            { "6017", "El formato de fecha es inválido" },
            { "6018", "Seguridad del trio ( tipografia caracteres 1-G-R) no coinciden" },
            { "6019", "Fotografía a blanco y negro con la ubicación invertida a la fotografía (selfie) del documento a color." },
            { "6020", "Tonalidad de la mariposa cambia respecto a la posicion del documento" },
            { "6001", "internal error" },
            { "6000", "El documento es original" }
        };

        public static readonly HashSet<string> ValidCodesOk = ["0000", "0100", "0200", "0300", "1000", "2000", "2100", "3000", "4000", "5000", "6000"];

        public static string GetMessage(string code)
        => _messages.TryGetValue(code, out var message)
            ? message
            : UnknownError;
    }
}
