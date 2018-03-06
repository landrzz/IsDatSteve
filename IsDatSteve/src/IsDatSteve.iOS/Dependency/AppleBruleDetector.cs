using System;
using Foundation;
using IsDatSteve.Interfaces;
using IsDatSteve.iOS.Dependency;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using System.Diagnostics;
using Vision;
using CoreML;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AppleBruleDetector))]
namespace IsDatSteve.iOS.Dependency
{
    public class AppleBruleDetector : IBruleDetector
    {
        VNCoreMLModel model;
        VNRequest[] classificationRequests;
        public Tuple<string, string> picResults { get; set; }

        public AppleBruleDetector()
        {
        }

        public void CollectImageData(MediaFile file)
        {
            var imagedata = NSData.FromStream(file.GetStream());
            var requestHandler = new VNImageRequestHandler(imagedata, new VNImageOptions());
            requestHandler.Perform(ClassificationRequest, out NSError error);


            if (error != null)
                Debug.WriteLine($"Error identifying {error}");
        }

        public VNRequest[] ClassificationRequest
        {
            get
            {
                if (model == null)
                {
                    var modelPath = NSBundle.MainBundle.GetUrlForResource("BestofBrule", "mlmodel");
                    var compiledPath = MLModel.CompileModel(modelPath, out NSError compileError);
                    var mlModel = MLModel.Create(compiledPath, out NSError createError);

                    model = VNCoreMLModel.FromMLModel(mlModel, out NSError mlError);
                }

                if (classificationRequests == null)
                {
                    var classificationRequest = new VNCoreMLRequest(model, HandleClassificationRequest);
                    classificationRequests = new[] { classificationRequest };
                }

                return classificationRequests;
            }
        }

        public void HandleClassificationRequest(VNRequest request, NSError error)
        {
            var observations = request.GetResults<VNClassificationObservation>();
            var best = observations?[0];

            var bestTag = best.Identifier.Trim();
            var confidence = $"{best.Confidence:P0}";
            picResults = new Tuple<string, string>(bestTag, confidence);
        }

        public Tuple<string, string> GetBrulesThoughts(MediaFile file)
        {
            CollectImageData(file);
            return picResults;
        }

    }
}
