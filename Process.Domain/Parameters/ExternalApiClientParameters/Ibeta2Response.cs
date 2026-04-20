using System.Text.Json.Serialization;

namespace Process.Domain.Parameters.ExternalApiClientParameters
{
    public class Ibeta2Response
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

        [JsonPropertyName("with_accessories")]
        public bool WithAccessories { get; set; }

        [JsonPropertyName("with_iqa")]
        public bool WithIqa { get; set; }

        [JsonPropertyName("with_icao")]
        public bool WithIcao { get; set; }

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
        public Summary Summary { get; set; } = default!;

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; } = default!;

        [JsonPropertyName("user_data")]
        public object UserData { get; set; } = default!;

        [JsonPropertyName("with_geometry_check")]
        public bool WithGeometryCheck { get; set; }

        [JsonPropertyName("http_code")]
        public double HttpCode { get; set; }

        [JsonPropertyName("total_time_ms")]
        public double TotalTimeMs { get; set; }

        [JsonPropertyName("db_manager")]
        public string DbManager { get; set; } = string.Empty;
    }

    public partial class Metadata
    {
        [JsonPropertyName("near_img")]
        public ArImg NearImg { get; set; } = default!;

        [JsonPropertyName("far_img")]
        public ArImg FarImg { get; set; } = default!;

        [JsonPropertyName("geometry_check")]
        public GeometryCheck GeometryCheck { get; set; } = default!;
    }

    public partial class ArImg
    {
        [JsonPropertyName("integrity_dims")]
        public IntegrityDims IntegrityDims { get; set; } = default!;

        [JsonPropertyName("accessory_detector")]
        public AccessoryDetector AccessoryDetector { get; set; } = default!;

        [JsonPropertyName("face_detector")]
        public FaceDetector FaceDetector { get; set; } = default!;

        [JsonPropertyName("liqe")]
        public AccessoryDetector Liqe { get; set; } = default!;

        [JsonPropertyName("classic_metrics")]
        public ClassicMetrics ClassicMetrics { get; set; } = default!;

        [JsonPropertyName("iqa_heuristics")]
        public IqaHeuristics IqaHeuristics { get; set; } = default!;

        [JsonPropertyName("as_35_selfies")]
        public As35__Selfies As35_Selfies { get; set; } = default!;

        [JsonPropertyName("ibeta2_crops")]
        public As35__Selfies Ibeta2Crops { get; set; } = default!;

        [JsonPropertyName("ibeta2_full")]
        public As35__Selfies Ibeta2Full { get; set; } = default!;

        [JsonPropertyName("ibeta2_clip")]
        public As35__Selfies Ibeta2Clip { get; set; } = default!;

        [JsonPropertyName("as_heuristics")]
        public AsHeuristics AsHeuristics { get; set; } = default!;
    }

    public partial class AccessoryDetector
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public AccessoryDetectorResults Results { get; set; } = default!;
    }

    public partial class AccessoryDetectorResults
    {
        [JsonPropertyName("predictions")]
        public PurplePredictions Predictions { get; set; } = default!;
    }

    public partial class PurplePredictions
    {
        [JsonPropertyName("accessories")]
        public Accessories Accessories { get; set; } = default!;

        [JsonPropertyName("preprocess_time_ms")]
        public double PreprocessTimeMs { get; set; }

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }

        [JsonPropertyName("queue_time_ms")]
        public double QueueTimeMs { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }
    }

    public partial class Accessories
    {
        [JsonPropertyName("face")]
        public Face Face { get; set; } = default!;
    }

    public partial class Face
    {
        [JsonPropertyName("bbox")]
        public List<double> Bbox { get; set; } = default!;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }

    public partial class As35__Selfies
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public As35_SelfiesResults Results { get; set; } = default!;
    }

    public partial class As35_SelfiesResults
    {
        [JsonPropertyName("predictions")]
        public FluffyPredictions Predictions { get; set; } = default!;
    }

    public partial class FluffyPredictions
    {
        [JsonPropertyName("class")]
        public string Class { get; set; } = string.Empty;

        [JsonPropertyName("prob")]
        public double Prob { get; set; }

        [JsonPropertyName("preprocess_time_ms")]
        public double PreprocessTimeMs { get; set; }

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }

        [JsonPropertyName("queue_time_ms")]
        public double QueueTimeMs { get; set; }
    }

    public partial class AsHeuristics
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public AsHeuristicsResults Results { get; set; } = default!;
    }

    public partial class AsHeuristicsResults
    {
        [JsonPropertyName("predictions")]
        public TentacledPredictions Predictions { get; set; } = default!;

        [JsonPropertyName("config")]
        public PurpleConfig Config { get; set; } = default!;
    }

    public partial class PurpleConfig
    {
        [JsonPropertyName("ibeta2_full_thr")]
        public double Ibeta2FullThr { get; set; }

        [JsonPropertyName("ibeta2_crops_thr")]
        public double Ibeta2CropsThr { get; set; }

        [JsonPropertyName("ibeta2_clip_thr")]
        public double Ibeta2ClipThr { get; set; }

        [JsonPropertyName("as_3.5_selfies_thr")]
        public double As35_SelfiesThr { get; set; }

        [JsonPropertyName("as_heuristics_thr")]
        public double AsHeuristicsThr { get; set; }
    }

    public partial class TentacledPredictions
    {
        [JsonPropertyName("prob")]
        public double Prob { get; set; }

        [JsonPropertyName("black_region_area")]
        public double BlackRegionArea { get; set; }

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }
    }

    public partial class ClassicMetrics
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public ClassicMetricsResults Results { get; set; } = default!;
    }

    public partial class ClassicMetricsResults
    {
        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }

        [JsonPropertyName("sharpness")]
        public double Sharpness { get; set; }

        [JsonPropertyName("colorfulness")]
        public double Colorfulness { get; set; }

        [JsonPropertyName("contrast")]
        public double Contrast { get; set; }

        [JsonPropertyName("brightness")]
        public double Brightness { get; set; }

        [JsonPropertyName("blur_score")]
        public double BlurScore { get; set; }
    }

    public partial class FaceDetector
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public FaceDetectorResults Results { get; set; } = default!;
    }

    public partial class FaceDetectorResults
    {
        [JsonPropertyName("predictions")]
        public StickyPredictions Predictions { get; set; } = default!;

        [JsonPropertyName("icao_warnings")]
        public List<object> IcaoWarnings { get; set; } = default!;

        [JsonPropertyName("config")]
        public FluffyConfig Config { get; set; } = default!;
    }

    public partial class FluffyConfig
    {
        [JsonPropertyName("exp_factor_w")]
        public double ExpFactorW { get; set; }

        [JsonPropertyName("exp_factor_h")]
        public double ExpFactorH { get; set; }
    }

    public partial class StickyPredictions
    {
        [JsonPropertyName("faces")]
        public Faces Faces { get; set; } = default!;

        [JsonPropertyName("blendshapes")]
        public List<Dictionary<string, double>> Blendshapes { get; set; } = default!;

        [JsonPropertyName("hands")]
        public Hands Hands { get; set; } = default!;

        [JsonPropertyName("preprocess_time_ms")]
        public double PreprocessTimeMs { get; set; }

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }

        [JsonPropertyName("queue_time_ms")]
        public double QueueTimeMs { get; set; }
    }

    public partial class Faces
    {
        [JsonPropertyName("bboxes")]
        public List<List<double>> Bboxes { get; set; } = default!;

        [JsonPropertyName("landmarks")]
        public List<Dictionary<string, List<double>>> Landmarks { get; set; } = default!;

        [JsonPropertyName("bboxes_raw")]
        public List<List<double>> BboxesRaw { get; set; } = default!;

        [JsonPropertyName("axis_rot_angles")]
        public List<List<double>> AxisRotAngles { get; set; } = default!;

        [JsonPropertyName("roll_pitch_yaw")]
        public List<List<double>> RollPitchYaw { get; set; } = default!;
    }

    public partial class Hands
    {
        [JsonPropertyName("bboxes")]
        public List<object> Bboxes { get; set; } = default!;

        [JsonPropertyName("landmarks")]
        public List<object> Landmarks { get; set; } = default!;
    }

    public partial class IntegrityDims
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public IntegrityDimsResults Results { get; set; } = default!;
    }

    public partial class IntegrityDimsResults
    {
        [JsonPropertyName("file_ext")]
        public string FileExt { get; set; } = string.Empty;

        [JsonPropertyName("raw_img_size")]
        public List<double> RawImgSize { get; set; } = default!;

        [JsonPropertyName("resized")]
        public bool Resized { get; set; }
    }

    public partial class IqaHeuristics
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public IqaHeuristicsResults Results { get; set; } = default!;
    }

    public partial class IqaHeuristicsResults
    {
        [JsonPropertyName("icao_warnings")]
        public List<object> IcaoWarnings { get; set; } = default!;

        [JsonPropertyName("config")]
        public TentacledConfig Config { get; set; } = default!;

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }
    }

    public partial class TentacledConfig
    {
        [JsonPropertyName("liqe_thr")]
        public double LiqeThr { get; set; }

        [JsonPropertyName("liqe_icao_thr")]
        public double LiqeIcaoThr { get; set; }

        [JsonPropertyName("sharpness_thr")]
        public double SharpnessThr { get; set; }

        [JsonPropertyName("brightness_range")]
        public List<double> BrightnessRange { get; set; } = default!;

        [JsonPropertyName("brightness_range_icao")]
        public List<double> BrightnessRangeIcao { get; set; } = default!;

        [JsonPropertyName("colofulness_range")]
        public List<double> ColofulnessRange { get; set; } = default!;

        [JsonPropertyName("colofulness_range_icao")]
        public List<double> ColofulnessRangeIcao { get; set; } = default!;

        [JsonPropertyName("contrast_range")]
        public List<double> ContrastRange { get; set; } = default!;

        [JsonPropertyName("contrast_range_icao")]
        public List<double> ContrastRangeIcao { get; set; } = default!;

        [JsonPropertyName("blur_score_thr")]
        public double BlurScoreThr { get; set; }

        [JsonPropertyName("blur_score_icao_thr")]
        public double BlurScoreIcaoThr { get; set; }
    }

    public partial class GeometryCheck
    {
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("results")]
        public GeometryCheckResults Results { get; set; } = default!;
    }

    public partial class GeometryCheckResults
    {
        [JsonPropertyName("predictions")]
        public IndigoPredictions Predictions { get; set; } = default!;

        [JsonPropertyName("config")]
        public StickyConfig Config { get; set; } = default!;
    }

    public partial class StickyConfig
    {
        [JsonPropertyName("image_difference_threshold")]
        public double ImageDifferenceThreshold { get; set; }

        [JsonPropertyName("handcrafted_threshold")]
        public double HandcraftedThreshold { get; set; }

        [JsonPropertyName("neural_threshold")]
        public double NeuralThreshold { get; set; }
    }

    public partial class IndigoPredictions
    {
        [JsonPropertyName("neural_score")]
        public double NeuralScore { get; set; }

        [JsonPropertyName("preprocess_time_ms")]
        public double PreprocessTimeMs { get; set; }

        [JsonPropertyName("time_ms")]
        public double TimeMs { get; set; }

        [JsonPropertyName("queue_time_ms")]
        public double QueueTimeMs { get; set; }

        [JsonPropertyName("image_difference")]
        public double ImageDifference { get; set; }

        [JsonPropertyName("handcrafted_score")]
        public double HandcraftedScore { get; set; }

        [JsonPropertyName("delta")]
        public double Delta { get; set; }

        [JsonPropertyName("insufficient_delta")]
        public bool InsufficientDelta { get; set; }
    }

    public partial class Summary
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        [JsonPropertyName("desc")]
        public string Desc { get; set; } = string.Empty;
    }
}
