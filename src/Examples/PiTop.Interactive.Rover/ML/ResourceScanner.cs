using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

using lobe;
using lobe.ImageSharp;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json.Linq;

using PiTop.Camera;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTop.Interactive.Rover.ML
{
    public class ResourceScanner
    {
        private ImageClassifier _classifier;
        private Uri _predictionEndpoint;
        private HttpClient _client;
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
                 new FileInfo(Path.Combine(modelLocation.FullName, "signature.json")), format: "onnx", modelFileName: "saved_model.onnx");
        }

        public void UseUri(Uri predictionEndpoint)
        {
            _classifier?.Dispose();
            _classifier = null;
            _predictionEndpoint = predictionEndpoint;
            _client = new HttpClient();
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
            var image = frame.CloneAs<Rgb24>();
            using var stream = new MemoryStream();
            image.Save(stream, new PngEncoder());
            stream.Flush();
            var data = stream.ToArray();
            var imageSource = $"{Convert.ToBase64String(data)}";

            var content = new StringContent($"{{ \"inputs\": {{ \"Image\":  \"{imageSource}\" }} }}", Encoding.UTF8,
                "application/json");
            var response = _client.PostAsync(_predictionEndpoint, content).Result;
            var body = response.Content.ReadAsStringAsync().Result;

            var classification = JObject.Parse(body);

            var classifications = classification.SelectToken("outputs.Labels").Values<JArray>()
                .Select(ja => new Classification(ja[0].Value<string>(), ja[1].Value<double>())).ToArray();


            var classificationResults =
                new ClassificationResults(
                    classifications.First(c => c.Label == classification.SelectToken("outputs.Prediction[0]").Value<string>()),
                    classifications);

            var results = classificationResults?.Classification?.Confidence > Threshold ? classificationResults : null;
            return results;
        }

        private ClassificationResults UseClassifier(Image frame)
        {
            var classificationResults = frame != null ? _classifier?.Classify(frame.CloneAs<Rgb24>()) : null;
            var results = classificationResults?.Classification?.Confidence > Threshold ? classificationResults : null;
            return results;
        }
    }
}
