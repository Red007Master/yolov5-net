using SixLabors.ImageSharp;
using Newtonsoft.Json;
using CustomJsonConverter;

namespace Yolov5Net.Scorer
{
    /// <summary>
    /// Label of detected object.
    /// </summary>
    public class YoloLabel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public YoloLabelKind Kind { get; set; }
        [JsonConverter(typeof(ColorConverter))]
        public Color Color { get; set; }

        public YoloLabel()
        {
            Color = Color.Red;
        }
    }
}
