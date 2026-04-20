namespace Process.Domain.Utilities
{
    public static class ConstantsIbeta
    {
        private static readonly string UnknownError = "Código de error desconocido";

        private static readonly Dictionary<string, string> _messagesIbeta1 = new()
        {
            { "0001", "La solicitud no se hizo con un archivo de imagen valido" },
            { "0002", "La extension del archivo no se encuentra en la lista de extensiones de imagen permitidas" },
            { "0003", "La verificacion de la integridad de la imagen no fue exitosa" },
            { "0004", "La correccion de orientacion de la imagen no fue exitosa" },
            { "0005", "La imagen no se ajusta en alguno de sus dimensiones al tamaño minimo requerido (usar una mejor camara)" },
            { "0006", "Ocurrio un error al reducir el tamaño de la imagen" },
            { "0007", "La imagen no se puede convertir a base 64" },
            { "0000", "La imagen cumple con los criterios minimos de integridad y dimensiones" },
            { "0102", "No se logro detectar un rostro en la imagen" },
            { "0103", "Se detectaron multiples rostros en la imagen" },
            { "0104", "La cara no se encuentra centrada en la imagen" },
            { "0105", "La cara se encuentra demasiado lejos de la camara" },
            { "0106", "La proporcion del ancho de la cara con respecto al ancho de la imagen es muy pequeña" },
            { "0107", "La proporcion del alto de la cara con respecto al alto de la imagen es muy pequeña" },
            { "0108", "La cara no esta orientada hacia adelante" },
            { "0109", "La usuera tiene los ojos cerrados (se omite cuando smiling=true)" },
            { "0110", "La usuera tiene la boca abierta (se omite cuando smiling=true)" },
            { "0111", "La proporcion de la distancia entre los ojos con respecto al ancho de la imagen es muy pequeña (ICAO usuera muy lejos)" },
            { "0112", "La usuera tiene una iluminacion no homogenea en la cara." },
            { "0113", "La camara posiblemente tiene algun filtro activado." },
            { "0114", "La cara esta demasiado cerca a la camara." },
            { "0115", "El usuario no debe taparse la cara con las manos, tampoco debe tener cerca las manos de la cara" },
            { "0101", "Error interno" },
            { "0100", "Rostro aceptable." },
            { "0202", "Las gafas de la usuera reflejan y no permiten capturar los ojos." },
            { "0203", "La usuera tiene lentes de sol." },
            { "0204", "La usuera tiene tapabocas." },
            { "0205", "La usuera usa sombrero o capucha" },
            { "0201", "Error interno" },
            { "0200", "Rotro sin accesorios" },
            { "0302", "Calidad de imagen no aceptable, verifica las condiciones de iluminacion y que el lente de la camara este limpio." },
            { "0303", "Los colores de la imagen no son aceptables." },
            { "0304", "La cara no tiene una iluminacion aceptable." },
            { "0305", "La cara tiene un contraste no aceptable." },
            { "0306", "La imagen no es nitida" },
            { "0301", "Error interno" },
            { "0300", "Calidad aceptable" },
            { "1402", "La cara es una suplantacion de identidad" },
            { "1403", "No se puede determinar si la imagen es real o no" },
            { "1401", "Error interno" },
            { "1400", "La cara es real" },
            { "2001", "Error interno" },
            { "2000", "Transmision interna de guardado exitosa" }
        };

        private static readonly Dictionary<string, string> _messagesIbeta2 = new()
        {
            { "0001", "imagen no encontrada" },
            { "0002", "extension de imagen no permitida" },
            { "0003", "imagen corrupta" },
            { "0004", "correcion de orientacion fallida" },
            { "0005", "imagen pequeña" },
            { "0006", "error redimensionando imagen" },
            { "0007", "error conversion imagen" },
            { "0000", "imagen aceptable integridad y dimensiones" },
            { "0102", "imagen no encontrada" },
            { "0103", "multiples rostros en la imagen" },
            { "0101", "error interno detector facial" },
            { "0100", "imagen aceptable detector facial" },
            { "0402", "rostro no centrado" },
            { "0403", "rostro muy lejos" },
            { "0404", "rostro muy lejos" },
            { "0405", "rostro muy lejos" },
            { "0406", "mirar al frente" },
            { "0407", "ojos cerrados" },
            { "0408", "boca abierta" },
            { "0409", "rostro muy lejos" },
            { "0410", "la iluminacion en la cara no es homogenea" },
            { "0411", "la imagen no tiene colores adecuados" },
            { "0412", "rostro muy lejos" },
            { "0414", "cara ocluida por objetos" },
            { "0415", "mirar al frente" },
            { "0416", "fondo no uniforme" },
            { "0417", "ojos rojos" },
            { "0401", "error interno validacion ICAO" },
            { "0400", "imagen aceptable validacion ICAO" },
            { "0202", "manos en la imagen" },
            { "0203", "accesorios en la imagen" },
            { "0204", "tapabocas en la imagen" },
            { "0205", "gafas de sol en la imagen" },
            { "0201", "error interno validacion accesorios" },
            { "0200", "imagen aceptable validacion accesorios" },
            { "0302", "la imagen esta borrosa" },
            { "0303", "la imagen esta en blanco y negro" },
            { "0304", "la imagen esta obscura o muy brillante" },
            { "0305", "la imagen esta opaca" },
            { "0306", "la imagen no es nitida" },
            { "0301", "error interno validacion calidad" },
            { "0300", "imagen aceptable validacion calidad" },
            { "1402", "la imagen es una suplantacion de identidad" },
            { "1403", "no se puede concluir si la imagen es real o falsa" },
            { "1401", "error interno antispoofing" },
            { "1400", "la imagen es de una persona real" },
            { "2001", "error interno base de datos" },
            { "2000", "base de datos OK" },
            { "3000", "la imagen es de una persona real" },
            { "3001", "error interno geometry check" },
            { "3002", "la imagen es una suplantacion de identidad" }
        };

        public static readonly HashSet<string> ValidCodesOkIbeta1 = ["0000", "0100", "0300", "1400", "2000"];
        public static readonly HashSet<string> ValidCodesOkIbeta2 = ["0000", "0100", "0200", "0300", "1400", "2000", "3000"];

        public static string GetMessageIbeta1(string code)
        => _messagesIbeta1.TryGetValue(code, out var message)
            ? message
            : UnknownError;

        public static string GetMessageIbeta2(string code)
        => _messagesIbeta2.TryGetValue(code, out var message)
            ? message
            : UnknownError;
    }
}
