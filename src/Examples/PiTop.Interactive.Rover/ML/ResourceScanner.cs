using System;
using System.IO;

using lobe;
using lobe.ImageSharp;

using PiTop.Camera;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTop.Interactive.Rover.ML
{
    public class ResourceScanner
    {
        private ImageClassifier _classifier;
        public Func<Image> CaptureImage { get; set; }

        public double Threshold { get; set; }

        public void CaptureFromCamera(IFrameSource<Image> camera)
        {
            CaptureImage = camera.GetFrame;
            Threshold = 0.9;
        }

        public void LoadModel(DirectoryInfo modelLocation)
        {
            _classifier?.Dispose();
            _classifier = ImageClassifier.CreateFromSignatureFile(
                 new FileInfo(Path.Combine(modelLocation.FullName, "signature.json")), format: "onnx", modelFileName: "saved_model.onnx");
        }

        public ClassificationResults Scan()
        {
            var image = CaptureImage?.Invoke();
            return AnalyseFrame(image);
        }

        public ClassificationResults AnalyseFrame(Image frame)
        {
            var classificationResults = frame != null ? _classifier?.Classify(frame.CloneAs<Rgb24>()) : null;
            return classificationResults?.Classification?.Confidence > Threshold ? classificationResults : null;
        }
    }
}
