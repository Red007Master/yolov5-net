using Newtonsoft.Json;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

class Program
{
    private static async Task Main(string[] args)
    {
        //Save config//
        // string config1 = JsonConvert.SerializeObject(new YoloCocoP5Model());
        // File.WriteAllText("Assets/Weights/saveExample.json", config1);

        //Load config//
        string config = File.ReadAllText("Assets/Weights/yolov5n.json"); //loading json string
        UniversalModelConfig universalModelConfig = JsonConvert.DeserializeObject<UniversalModelConfig>(config); //deserializeing json string to config instance


        using var image = await Image.LoadAsync<Rgba32>("Assets/test.jpg");
        {
            //creating YoloScorer without universalModelConfig
            //using var scorer = new YoloScorer<YoloCocoP5Model>("Assets/Weights/yolov5n.onnx");

            //creating YoloScorer by passing weights path and universalModelConfig (<UniversalModel> is required!! if you want to use config)
            using var scorer = new YoloScorer<UniversalModel>("Assets/Weights/yolov5n.onnx", universalModelConfig);
            {
                var predictions = scorer.Predict(image);

                var font = new Font(new FontCollection().Add("Assets/consolas.ttf"), 16);

                foreach (var prediction in predictions) // draw predictions
                {
                    var score = Math.Round(prediction.Score, 2);

                    var (x, y) = (prediction.Rectangle.Left - 3, prediction.Rectangle.Top - 23);

                    image.Mutate(a => a.DrawPolygon(new Pen(prediction.Label.Color, 1),
                        new PointF(prediction.Rectangle.Left, prediction.Rectangle.Top),
                        new PointF(prediction.Rectangle.Right, prediction.Rectangle.Top),
                        new PointF(prediction.Rectangle.Right, prediction.Rectangle.Bottom),
                        new PointF(prediction.Rectangle.Left, prediction.Rectangle.Bottom)
                    ));

                    image.Mutate(a => a.DrawText($"{prediction.Label.Name} ({score})", font, prediction.Label.Color, new PointF(x, y)));
                }

                await image.SaveAsync("Assets/result.jpg");
            }
        }
    }
}