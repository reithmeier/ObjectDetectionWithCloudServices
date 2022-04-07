using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using DetectedObject = Domain.DetectedObject;

namespace Logic.Cv {
  public class CognitiveComputerVisionLogic : IComputerVisionLogic {

    private static string subscriptionKey = ConfigurationManager.AppSettings["SubscriptionKey"];
    private static string serviceEndpoint = ConfigurationManager.AppSettings["ServiceEndpoint"];

    private ComputerVisionClient Client { get; set; }

    /*
     * AUTHENTICATE
     * Creates a Computer Vision client used by each example.
     */
    public static ComputerVisionClient Authenticate(string endpoint, string key) {
      ComputerVisionClient client =
        new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
      return client;
    }

    public CognitiveComputerVisionLogic() {
      Client = Authenticate(serviceEndpoint, subscriptionKey);
    }

    public async Task<Image> AnalyzeAsync(Image data) {
      var features = new List<VisualFeatureTypes>()
      {
        VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
        VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
        VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
        VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
        VisualFeatureTypes.Objects
      };
      Stream imageStream = new MemoryStream(data.ImageData);
      var analysisResults = await Client.AnalyzeImageInStreamAsync(imageStream, features);
      data.AnalysisResults = new List<DetectedObject>();
      foreach (var detectedObject in analysisResults.Objects) {
        var newDetectedObject = new DetectedObject {
          Confidence = detectedObject.Confidence,
          ObjectProperty = detectedObject.ObjectProperty,
          X = detectedObject.Rectangle.X,
          Y = detectedObject.Rectangle.Y,
          W = detectedObject.Rectangle.W,
          H = detectedObject.Rectangle.H
        };
        data.AnalysisResults.Add(newDetectedObject);
      }
      return data;
    }
  }
}