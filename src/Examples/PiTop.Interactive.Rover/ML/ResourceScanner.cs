using System;
using System.IO;

using lobe;
using lobe.Http;
using lobe.ImageSharp;

using PiTop.Camera;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTop.Interactive.Rover.ML
{
    public class ResourceScanner
    {
        private ImageClassifier _classifier;
        private Uri _predictionEndpoint;
        private LobeClient _client;
        public Func<Image> CaptureImage { get; set; }

        public double Threshold { get; set; }

        public void CaptureFromCamera(IFrameSource<Image> camera)
        {
            CaptureImage = camera.GetFrame;
            Threshold = 0.9;
        }

        public void UseModel(DirectoryInfo modelLocation)
        {
            _predictionEndpoint = null;
            _classifier?.Dispose();
            _classifier = ImageClassifier.CreateFromSignatureFile(
                 new FileInfo(Path.Combine(modelLocation.FullName, "signature.json")));
        }

        public void UseUri(Uri predictionEndpoint)
        {
            _classifier?.Dispose();
            _classifier = null;
            _predictionEndpoint = predictionEndpoint;
            _client = new LobeClient(_predictionEndpoint);
        }

        public ClassificationResults Scan()
        {
            var image = CaptureImage?.Invoke();
            return AnalyseFrame(image);
        }

        public ClassificationResults AnalyseFrame(Image frame)
        {
            ClassificationResults results = null;
            if (_classifier != null)
            {
                results = UseClassifier(frame);
            }
            else if (_predictionEndpoint != null)
            {
                results = UseService(frame);
            }

            return results;

        }

        private ClassificationResults UseService(Image frame)
        {
            var classificationResults = frame != null ? _client?.Classify(frame.CloneAs<Rgb24>()) : null;
            var results = classificationResults?.Prediction?.Confidence > Threshold ? classificationResults : null;
            return results;
        }

        private ClassificationResults UseClassifier(Image frame)
        {
            var classificationResults = frame != null ? _classifier?.Classify(frame.CloneAs<Rgb24>()) : null;
            var results = classificationResults?.Prediction?.Confidence > Threshold ? classificationResults : null;
            return results;
        }
    }
}
