using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class VerIdResponse
    {
        [JsonPropertyName("api_hash")]
        public string ApiHash { get; set; } = string.Empty;

        [JsonPropertyName("api_version")]
        public string ApiVersion { get; set; } = string.Empty;

        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;

        [JsonPropertyName("service")]
        public string Service { get; set; } = string.Empty;

        [JsonPropertyName("api_gateway_id")]
        public string ApiGatewayId { get; set; } = string.Empty;

        [JsonPropertyName("file_token")]
        public string FileToken { get; set; } = string.Empty;

        [JsonPropertyName("with_iqa")]
        public bool WithIqa { get; set; }

        [JsonPropertyName("with_antispoofing")]
        public bool WithAntispoofing { get; set; }

        [JsonPropertyName("save_azure")]
        public bool SaveAzure { get; set; }

        [JsonPropertyName("save_mongo")]
        public bool SaveMongo { get; set; }

        [JsonPropertyName("ts")]
        public double Ts { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("summary")]
        public SummaryVerId Summary { get; set; } = default!;

        [JsonPropertyName("global_result")]
        public GlobalResult GlobalResult { get; set; } = default!;

        [JsonPropertyName("metadata")]
        public MetadataVerId Metadata { get; set; } = default!;

        [JsonPropertyName("user_data")]
        public object UserData { get; set; } = default!;

        [JsonPropertyName("http_code")]
        public object HttpCode { get; set; } = default!;

        [JsonPropertyName("total_time_ms")]
        public object TotalTimeMs { get; set; } = default!;

        [JsonPropertyName("db_manager")]
        public string DbManager { get; set; } = string.Empty;
    }

    public class GlobalResult
    {
        [JsonPropertyName("extracted_data")]
        public ExtractedData ExtractedData { get; set; } = default!;

        [JsonPropertyName("document_comparison")]
        public DocumentComparison DocumentComparison { get; set; } = default!;
    }

    public class DocumentComparison
    {
        [JsonPropertyName("type_validation")]
        public TypeValidation TypeValidation { get; set; } = default!;

        [JsonPropertyName("field_matching_results")]
        public FieldMatchingResults FieldMatchingResults { get; set; } = default!;

        [JsonPropertyName("error_5008")]
        public bool Error5008 { get; set; }

        [JsonPropertyName("error_5010")]
        public bool Error5010 { get; set; }

        [JsonPropertyName("error_5009")]
        public bool Error5009 { get; set; }
    }

    public class FieldMatchingResults
    {
        [JsonPropertyName("Number")]
        public DateOfBirth Number { get; set; } = default!;

        [JsonPropertyName("GivenName")]
        public DateOfBirth GivenName { get; set; } = default!;

        [JsonPropertyName("Surname")]
        public DateOfBirth Surname { get; set; } = default!;

        [JsonPropertyName("DateOfBirth")]
        public DateOfBirth DateOfBirth { get; set; } = default!;

        [JsonPropertyName("DateOfExpiry")]
        public DateOfBirth DateOfExpiry { get; set; } = default!;
    }

    public class DateOfBirth
    {
        [JsonPropertyName("TextField")]
        public string TextField { get; set; } = string.Empty;

        [JsonPropertyName("MRZ")]
        public string Mrz { get; set; } = string.Empty;

        [JsonPropertyName("SimilarityScore")]
        public object SimilarityScore { get; set; } = default!;
    }

    public class TypeValidation
    {
        [JsonPropertyName("front_img")]
        public Img FrontImg { get; set; } = default!;

        [JsonPropertyName("back_img")]
        public Img BackImg { get; set; } = default!;

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }

    public class Img
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
    }

    public class ExtractedData
    {
        [JsonPropertyName("extracted_ocr")]
        public ExtractedOcr ExtractedOcr { get; set; } = default!;

        [JsonPropertyName("extracted_bcr")]
        public ExtractedBcr ExtractedBcr { get; set; } = default!;

        [JsonPropertyName("extracted_mrz")]
        public ExtractedMrz ExtractedMrz { get; set; } = default!;
    }

    public class ExtractedBcr
    {
        [JsonPropertyName("back_img")]
        public PredictionsClass BackImg { get; set; } = default!;
    }

    public class PredictionsClass
    {
    }

    public class ExtractedMrz
    {
        [JsonPropertyName("back_img")]
        public Dictionary<string, string> BackImg { get; set; } = default!;
    }

    public class ExtractedOcr
    {
        [JsonPropertyName("front_img")]
        public ResClass FrontImg { get; set; } = default!;

        [JsonPropertyName("back_img")]
        public ExtractedOcrBackImg BackImg { get; set; } = default!;
    }

    public class ExtractedOcrBackImg
    {
        [JsonPropertyName("codigo_lateral")]
        public string CodigoLateral { get; set; } = string.Empty;

        [JsonPropertyName("global_inference")]
        public List<string> GlobalInference { get; set; } = default!;
    }

    public class ResClass
    {
        [JsonPropertyName("primer_apellido")]
        public string PrimerApellido { get; set; } = string.Empty;

        [JsonPropertyName("segundo_apellido")]
        public string SegundoApellido { get; set; } = string.Empty;

        [JsonPropertyName("estatura")]
        public string Estatura { get; set; } = string.Empty;

        [JsonPropertyName("fecha_expiracion")]
        public string FechaExpiracion { get; set; } = string.Empty;

        [JsonPropertyName("fecha_nacimiento")]
        public string FechaNacimiento { get; set; } = string.Empty;

        [JsonPropertyName("a_s_preim_fech_lug_exp")]
        public string ASPreimFechLugExp { get; set; } = string.Empty;

        [JsonPropertyName("fecha_expedicion")]
        public string FechaExpedicion { get; set; } = string.Empty;

        [JsonPropertyName("lugar_expedicion")]
        public string LugarExpedicion { get; set; } = string.Empty;

        [JsonPropertyName("gs_rh")]
        public string GsRh { get; set; } = string.Empty;

        [JsonPropertyName("lugar_nacimiento")]
        public string LugarNacimiento { get; set; } = string.Empty;

        [JsonPropertyName("nacionalidad")]
        public string Nacionalidad { get; set; } = string.Empty;

        [JsonPropertyName("primer_nombre")]
        public string PrimerNombre { get; set; } = string.Empty;

        [JsonPropertyName("segundo_nombre")]
        public string SegundoNombre { get; set; } = string.Empty;

        [JsonPropertyName("a_s_numero")]
        public string ASNumero { get; set; } = string.Empty;

        [JsonPropertyName("numero")]
        public string Numero { get; set; } = string.Empty;

        [JsonPropertyName("sexo")]
        public string Sexo { get; set; } = string.Empty;

        [JsonPropertyName("global_inference")]
        public List<string> GlobalInference { get; set; } = default!;
    }

    public class MetadataVerId
    {
        [JsonPropertyName("front_img")]
        public MetadataFrontImg FrontImg { get; set; } = default!;

        [JsonPropertyName("back_img")]
        public MetadataBackImg BackImg { get; set; } = default!;
    }

    public class MetadataBackImg
    {
        [JsonPropertyName("integrity_dims")]
        public IntegrityDims IntegrityDims { get; set; } = default!;

        [JsonPropertyName("doc_segmentation")]
        public DocSegmentation DocSegmentation { get; set; } = default!;

        [JsonPropertyName("doc_homography")]
        public DocHomography DocHomography { get; set; } = default!;

        [JsonPropertyName("classic_metrics")]
        public ClassicMetrics ClassicMetrics { get; set; } = default!;

        [JsonPropertyName("iqa_heuristics")]
        public IqaHeuristics IqaHeuristics { get; set; } = default!;

        [JsonPropertyName("doc_field_detection")]
        public DocFieldDetection DocFieldDetection { get; set; } = default!;

        [JsonPropertyName("doc_field_extraction")]
        public BackImgDocFieldExtraction DocFieldExtraction { get; set; } = default!;

        [JsonPropertyName("doc_ocr")]
        public BackImgDocOcr DocOcr { get; set; } = default!;

        [JsonPropertyName("gender_gs_classification")]
        public GenderGsClassification GenderGsClassification { get; set; } = default!;

        [JsonPropertyName("ocr_quality_metrics")]
        public BackImgOcrQualityMetrics OcrQualityMetrics { get; set; } = default!;

        [JsonPropertyName("doc_bcr")]
        public DocBcr DocBcr { get; set; } = default!;

        [JsonPropertyName("doc_antispoofing")]
        public DocAntispoofing DocAntispoofing { get; set; } = default!;

        [JsonPropertyName("r_classification")]
        public Classification RClassification { get; set; } = default!;

        [JsonPropertyName("signature_classification")]
        public EClassification SignatureClassification { get; set; } = default!;

        [JsonPropertyName("barcode_classification")]
        public EClassification BarcodeClassification { get; set; } = default!;

        [JsonPropertyName("img_position_classification")]
        public Classification ImgPositionClassification { get; set; } = default!;
    }

    public class EClassification
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public PurpleResults Results { get; set; } = default!;
    }

    public class PurpleResults
    {
        [JsonPropertyName("predictions")]
        public MulticlassClass Predictions { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;
    }

    public class MulticlassClass
    {
        [JsonPropertyName("class")]
        public string Class { get; set; } = string.Empty;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }

    public class ClassicMetricsVerId
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public ClassicMetricsResultsVerId Results { get; set; } = default!;
    }

    public class ClassicMetricsResultsVerId
    {
        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("colorfulness")]
        public double Colorfulness { get; set; }

        [JsonPropertyName("white_area")]
        public double WhiteArea { get; set; }

        [JsonPropertyName("variance_of_laplacian")]
        public double VarianceOfLaplacian { get; set; }
    }

    public class DocAntispoofing
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public DocAntispoofingResults Results { get; set; } = default!;
    }

    public class DocAntispoofingResults
    {
        [JsonPropertyName("predictions")]
        public FluffyPredictionsVerId Predictions { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;
    }

    public class FluffyPredictionsVerId
    {
        [JsonPropertyName("multiclass")]
        public MulticlassClass Multiclass { get; set; } = default!;

        [JsonPropertyName("photo")]
        public MulticlassClass Photo { get; set; } = default!;
    }

    public class DocBcr
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public DocBcrResults Results { get; set; } = default!;
    }

    public class DocBcrResults
    {
        [JsonPropertyName("res")]
        public ResultsRes Res { get; set; } = default!;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("preprocessing_technique")]
        public string PreprocessingTechnique { get; set; } = string.Empty;
    }

    public class ResultsRes
    {
        [JsonPropertyName("primer_apellido")]
        public object PrimerApellido { get; set; } = default!;

        [JsonPropertyName("segundo_apellido")]
        public object SegundoApellido { get; set; } = default!;

        [JsonPropertyName("finger_card")]
        public object FingerCard { get; set; } = default!;

        [JsonPropertyName("numero")]
        public object Numero { get; set; } = default!;

        [JsonPropertyName("afis_code")]
        public object AfisCode { get; set; } = default!;

        [JsonPropertyName("primer_nombre")]
        public object PrimerNombre { get; set; } = default!;

        [JsonPropertyName("segundo_nombre")]
        public object SegundoNombre { get; set; } = default!;

        [JsonPropertyName("sexo")]
        public object Sexo { get; set; } = default!;

        [JsonPropertyName("fecha_nacimiento")]
        public object FechaNacimiento { get; set; } = default!;

        [JsonPropertyName("municipio")]
        public object Municipio { get; set; } = default!;

        [JsonPropertyName("departamento")]
        public object Departamento { get; set; } = default!;

        [JsonPropertyName("gs_rh")]
        public object GsRh { get; set; } = default!;
    }

    public class DocFieldDetection
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public DocFieldDetectionResults Results { get; set; } = default!;
    }

    public class DocFieldDetectionResults
    {
        [JsonPropertyName("detections")]
        public PurpleDetections Detections { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;
    }

    public class PurpleDetections
    {
        [JsonPropertyName("verid_results")]
        public VeridResults VeridResults { get; set; } = default!;

        [JsonPropertyName("DocumentType")]
        public DocumentTypeVerId DocumentType { get; set; } = default!;

        [JsonPropertyName("MarginValidation")]
        public bool MarginValidation { get; set; }

        [JsonPropertyName("margin_occlusions_metadata")]
        public PredictionsClass MarginOcclusionsMetadata { get; set; } = default!;

        [JsonPropertyName("fields_checks")]
        public List<object> FieldsChecks { get; set; } = default!;
    }

    public class DocumentTypeVerId
    {
        [JsonPropertyName("DocumentName")]
        public string DocumentName { get; set; } = string.Empty;

        [JsonPropertyName("FDSIDList")]
        public FdsidList FdsidList { get; set; } = default!;
    }

    public class FdsidList
    {
        [JsonPropertyName("ICAOCode")]
        public string IcaoCode { get; set; } = string.Empty;

        [JsonPropertyName("dCountryName")]
        public string DCountryName { get; set; } = string.Empty;

        [JsonPropertyName("dDescription")]
        public string DDescription { get; set; } = string.Empty;

        [JsonPropertyName("dType")]
        public object DType { get; set; } = default!;

        [JsonPropertyName("dYear")]
        public object DYear { get; set; } = default!;
    }

    public class VeridResults
    {
        [JsonPropertyName("homography_image")]
        public HomographyImage HomographyImage { get; set; } = default!;
    }

    public class HomographyImage
    {
        [JsonPropertyName("cls")]
        public List<object> Cls { get; set; } = default!;

        [JsonPropertyName("boxes")]
        public List<List<double>> Boxes { get; set; } = default!;
    }

    public class BackImgDocFieldExtraction
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public FluffyResults Results { get; set; } = default!;
    }

    public class FluffyResults
    {
        [JsonPropertyName("detections")]
        public FluffyDetections Detections { get; set; } = default!;
    }

    public class FluffyDetections
    {
        [JsonPropertyName("image_fields_results")]
        public PurpleImageFieldsResults ImageFieldsResults { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;
    }

    public class PurpleImageFieldsResults
    {
        [JsonPropertyName("foto_fantasma")]
        public List<double> FotoFantasma { get; set; } = default!;

        [JsonPropertyName("barcode_qr")]
        public List<double> BarcodeQr { get; set; } = default!;
    }

    public class DocHomography
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public DocHomographyResults Results { get; set; } = default!;
    }

    public class DocHomographyResults
    {
        [JsonPropertyName("detections")]
        public TentacledDetections Detections { get; set; } = default!;
    }

    public class TentacledDetections
    {
        [JsonPropertyName("DocumentTransformation")]
        public DocumentTransformation DocumentTransformation { get; set; } = default!;

        [JsonPropertyName("NewDocumentPosition")]
        public DocumentPosition NewDocumentPosition { get; set; } = default!;

        [JsonPropertyName("homography_dimensions")]
        public HomographyDimensions HomographyDimensions { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;
    }

    public class DocumentTransformation
    {
        [JsonPropertyName("Rotate90")]
        public bool Rotate90 { get; set; }

        [JsonPropertyName("Rotate180")]
        public bool Rotate180 { get; set; }

        [JsonPropertyName("Rotate270")]
        public bool Rotate270 { get; set; }

        [JsonPropertyName("MaskRotated")]
        public List<List<object>> MaskRotated { get; set; } = default!;

        [JsonPropertyName("Matrix")]
        public List<List<double>> Matrix { get; set; } = default!;

        [JsonPropertyName("Vertices")]
        public List<List<object>> Vertices { get; set; } = default!;
    }

    public class HomographyDimensions
    {
        [JsonPropertyName("height")]
        public object Height { get; set; } = default!;

        [JsonPropertyName("width")]
        public object Width { get; set; } = default!;
    }

    public class DocumentPosition
    {
        [JsonPropertyName("HeightBBox")]
        public double HeightBBox { get; set; }

        [JsonPropertyName("WidthBBox")]
        public double WidthBBox { get; set; }

        [JsonPropertyName("CenterBBox")]
        public CenterBBox CenterBBox { get; set; } = default!;

        [JsonPropertyName("Orientation")]
        public string Orientation { get; set; } = string.Empty;
    }

    public class CenterBBox
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }
    }

    public class BackImgDocOcr
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public TentacledResults Results { get; set; } = default!;
    }

    public class TentacledResults
    {
        [JsonPropertyName("detections")]
        public StickyDetections Detections { get; set; } = default!;
    }

    public class StickyDetections
    {
        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;

        [JsonPropertyName("ocr_result")]
        public PurpleOcrResult OcrResult { get; set; } = default!;

        [JsonPropertyName("mrz_result")]
        public MrzResult MrzResult { get; set; } = default!;
    }

    public class MrzResult
    {
        [JsonPropertyName("res")]
        public Dictionary<string, string> Res { get; set; } = default!;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    public class PurpleOcrResult
    {
        [JsonPropertyName("res")]
        public OcrResultRes Res { get; set; } = default!;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    public class OcrResultRes
    {
        [JsonPropertyName("codigo_lateral")]
        public string CodigoLateral { get; set; } = string.Empty;

        [JsonPropertyName("global_inference")]
        public List<string> GlobalInference { get; set; } = default!;

        [JsonPropertyName("gs_rh")]
        public object GsRh { get; set; } = default!;

        [JsonPropertyName("sexo")]
        public object Sexo { get; set; } = default!;
    }

    public class DocSegmentation
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public DocSegmentationResults Results { get; set; } = default!;
    }

    public class DocSegmentationResults
    {
        [JsonPropertyName("detections")]
        public IndigoDetections Detections { get; set; } = default!;
    }

    public class IndigoDetections
    {
        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;

        [JsonPropertyName("DocumentPosition")]
        public DocumentPosition DocumentPosition { get; set; } = default!;

        [JsonPropertyName("Mask")]
        public List<List<List<double>>> Mask { get; set; } = default!;
    }

    public class GenderGsClassification
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public GenderGsClassificationResults Results { get; set; } = default!;
    }

    public class GenderGsClassificationResults
    {
        [JsonPropertyName("predictions")]
        public TentacledPredictionsVerId Predictions { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;
    }

    public class TentacledPredictionsVerId
    {
        [JsonPropertyName("gs_rh")]
        public string GsRh { get; set; } = string.Empty;

        [JsonPropertyName("sexo")]
        public string Sexo { get; set; } = string.Empty;
    }

    public class Classification
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public ImgPositionClassificationResults Results { get; set; } = default!;
    }

    public class ImgPositionClassificationResults
    {
        [JsonPropertyName("predictions")]
        public PredictionsClass Predictions { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;
    }

    public class IntegrityDimsVerId
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public IntegrityDimsResultsVerId Results { get; set; } = default!;
    }

    public class IntegrityDimsResultsVerId
    {
        [JsonPropertyName("file_ext")]
        public string FileExt { get; set; } = string.Empty;

        [JsonPropertyName("raw_img_size")]
        public List<object> RawImgSize { get; set; } = default!;

        [JsonPropertyName("resized")]
        public bool Resized { get; set; }

        [JsonPropertyName("new_img_size")]
        public List<object> NewImgSize { get; set; } = default!;
    }

    public class IqaHeuristicsVerId
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public IqaHeuristicsResultsVerId Results { get; set; } = default!;
    }

    public class IqaHeuristicsResultsVerId
    {
        [JsonPropertyName("config")]
        public FluffyConfigVerId Config { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }
    }

    public class FluffyConfigVerId
    {
        [JsonPropertyName("colofulness_range")]
        public List<double> ColofulnessRange { get; set; } = default!;

        [JsonPropertyName("glare_thr")]
        public double GlareThr { get; set; }

        [JsonPropertyName("laplacian_blur_thr")]
        public object LaplacianBlurThr { get; set; } = default!;
    }

    public class BackImgOcrQualityMetrics
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public StickyResults Results { get; set; } = default!;
    }

    public class StickyResults
    {
        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("ocr_quality_score")]
        public object OcrQualityScore { get; set; } = default!;

        [JsonPropertyName("ocr_qulity_results")]
        public OcrQulityResults OcrQulityResults { get; set; } = default!;
    }

    public class OcrQulityResults
    {
        [JsonPropertyName(".CO")]
        public object Co { get; set; } = default!;

        [JsonPropertyName("REGISTRADOR")]
        public object Registrador { get; set; } = default!;

        [JsonPropertyName("NACIONAL")]
        public object Nacional { get; set; } = default!;
    }

    public class MetadataFrontImg
    {
        [JsonPropertyName("integrity_dims")]
        public IntegrityDimsVerId IntegrityDims { get; set; } = default!;

        [JsonPropertyName("doc_segmentation")]
        public DocSegmentation DocSegmentation { get; set; } = default!;

        [JsonPropertyName("doc_homography")]
        public DocHomography DocHomography { get; set; } = default!;

        [JsonPropertyName("classic_metrics")]
        public ClassicMetricsVerId ClassicMetrics { get; set; } = default!;

        [JsonPropertyName("iqa_heuristics")]
        public IqaHeuristicsVerId IqaHeuristics { get; set; } = default!;

        [JsonPropertyName("doc_field_detection")]
        public DocFieldDetection DocFieldDetection { get; set; } = default!;

        [JsonPropertyName("doc_field_extraction")]
        public FrontImgDocFieldExtraction DocFieldExtraction { get; set; } = default!;

        [JsonPropertyName("doc_ocr")]
        public FrontImgDocOcr DocOcr { get; set; } = default!;

        [JsonPropertyName("gender_gs_classification")]
        public GenderGsClassification GenderGsClassification { get; set; } = default!;

        [JsonPropertyName("ocr_quality_metrics")]
        public FrontImgOcrQualityMetrics OcrQualityMetrics { get; set; } = default!;

        [JsonPropertyName("doc_bcr")]
        public DocBcr DocBcr { get; set; } = default!;

        [JsonPropertyName("doc_antispoofing")]
        public DocAntispoofing DocAntispoofing { get; set; } = default!;

        [JsonPropertyName("r_classification")]
        public Classification RClassification { get; set; } = default!;

        [JsonPropertyName("signature_classification")]
        public Classification SignatureClassification { get; set; } = default!;

        [JsonPropertyName("barcode_classification")]
        public Classification BarcodeClassification { get; set; } = default!;

        [JsonPropertyName("img_position_classification")]
        public Classification ImgPositionClassification { get; set; } = default!;

        [JsonPropertyName("as_heuristics")]
        public AsHeuristicsVerId AsHeuristics { get; set; } = default!;
    }

    public class AsHeuristicsVerId
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public AsHeuristicsResults Results { get; set; } = default!;
    }

    public class FrontImgDocFieldExtraction
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public IndigoResults Results { get; set; } = default!;
    }

    public class IndigoResults
    {
        [JsonPropertyName("detections")]
        public IndecentDetections Detections { get; set; } = default!;
    }

    public class IndecentDetections
    {
        [JsonPropertyName("image_fields_results")]
        public FluffyImageFieldsResults ImageFieldsResults { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;
    }

    public class FluffyImageFieldsResults
    {
        [JsonPropertyName("foto")]
        public List<double> Foto { get; set; } = default!;

        [JsonPropertyName("firma")]
        public List<double> Firma { get; set; } = default!;

        [JsonPropertyName("foto_fantasma")]
        public List<double> FotoFantasma { get; set; } = default!;
    }

    public class FrontImgDocOcr
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public IndecentResults Results { get; set; } = default!;
    }

    public class IndecentResults
    {
        [JsonPropertyName("detections")]
        public HilariousDetections Detections { get; set; } = default!;
    }

    public class HilariousDetections
    {
        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("queue_time_ms")]
        public object QueueTimeMs { get; set; } = default!;

        [JsonPropertyName("ocr_result")]
        public FluffyOcrResult OcrResult { get; set; } = default!;

        [JsonPropertyName("mrz_result")]
        public MrzResult MrzResult { get; set; } = default!;
    }

    public class FluffyOcrResult
    {
        [JsonPropertyName("res")]
        public ResClass Res { get; set; } = default!;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    public class FrontImgOcrQualityMetrics
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = default!;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public HilariousResults Results { get; set; } = default!;
    }

    public class HilariousResults
    {
        [JsonPropertyName("time_ms")]
        public object TimeMs { get; set; } = default!;

        [JsonPropertyName("ocr_quality_score")]
        public object OcrQualityScore { get; set; } = default!;

        [JsonPropertyName("ocr_qulity_results")]
        public Dictionary<string, double> OcrQulityResults { get; set; } = default!;
    }

    public class SummaryVerId
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("risk_score")]
        public float RiskScore { get; set; } = default!;

        [JsonPropertyName("risk_warnings")]
        public List<object> RiskWarnings { get; set; } = default!;

        [JsonPropertyName("match_score")]
        public object MatchScore { get; set; } = default!;

        [JsonPropertyName("match_warnings")]
        public List<object> MatchWarnings { get; set; } = default!;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = string.Empty;

        [JsonPropertyName("from")]
        public string From { get; set; } = string.Empty;
    }
}
