using System.Collections.Generic;
using System.Linq;
using Yolov5Net.Scorer.Models.Abstract;

namespace Yolov5Net.Scorer.Models
{
    public class UniversalModel : YoloModel
    {
        public string ModelName { get; set; } = "Default_Reds_UniversalModel";
        public bool AutoDimensionsCount { get; set; } = true;

        public override int Width { get; set; } = 640;
        public override int Height { get; set; } = 640;
        public override int Depth { get; set; } = 3;

        public override int Dimensions { get; set; } = 0; //Lables count + 5 (idk why)

        public override int[] Strides { get; set; } = new int[] { 8, 16, 32 };

        public override int[][][] Anchors { get; set; } = new int[][][]
        {
            new int[][] { new int[] { 010, 13 }, new int[] { 016, 030 }, new int[] { 033, 023 } },
            new int[][] { new int[] { 030, 61 }, new int[] { 062, 045 }, new int[] { 059, 119 } },
            new int[][] { new int[] { 116, 90 }, new int[] { 156, 198 }, new int[] { 373, 326 } }
        };

        public override int[] Shapes { get; set; } = new int[] { 80, 40, 20 };

        public override float Confidence { get; set; } = 0.20f;
        public override float MulConfidence { get; set; } = 0.25f;
        public override float Overlap { get; set; } = 0.45f;

        public override string[] Outputs { get; set; } = new[] { "output" };

        public override List<YoloLabel> Labels { get; set; } = new List<YoloLabel>()
        {
            new YoloLabel { Id = 0, Name = "object0" },
        };

        public override bool UseDetect { get; set; } = true;

        public UniversalModel()
        {
            if (AutoDimensionsCount)
            {
                Dimensions = Labels.Count + 5;
            }
        }

        public UniversalModel(UniversalModelConfig universalModelConfig)
        {
            YoloTypeConverter.StrangerStrangerSetTo<UniversalModelConfig, UniversalModel>(universalModelConfig, this);

            if (AutoDimensionsCount)
            {
                Dimensions = Labels.Count + 5;
            }
        }
    }

    public class UniversalModelConfig
    {
        public string ModelName { get; set; }
        public bool AutoDimensionsCount { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }

        public int Dimensions { get; set; }

        public int[] Strides { get; set; }
        public int[][][] Anchors { get; set; }
        public int[] Shapes { get; set; }

        public float Confidence { get; set; }
        public float MulConfidence { get; set; }
        public float Overlap { get; set; }

        public string[] Outputs { get; set; }
        public List<YoloLabel> Labels { get; set; }
        public bool UseDetect { get; set; }

        public UniversalModelConfig(UniversalModel universalModel)
        {
            YoloTypeConverter.StrangerStrangerSetTo<UniversalModel, UniversalModelConfig>(universalModel, this);
        }

        public UniversalModelConfig() { }
    }

    public static class YoloTypeConverter
    {
        public static void StrangerStrangerSetTo<TInput, TOutput>(TInput input, TOutput output)
        {
            string[] inputProps = typeof(TInput).GetProperties().Select(x => x.Name).ToArray();
            string[] outputProps = typeof(TOutput).GetProperties().Select(x => x.Name).ToArray();

            string[] commonProps = inputProps.Intersect(outputProps).ToArray();

            foreach (string prop in commonProps)
            {
                object inputValue = typeof(TInput).GetProperty(prop).GetValue(input);
                typeof(TOutput).GetProperty(prop).SetValue(output, inputValue);
            }
        }
    }
}
