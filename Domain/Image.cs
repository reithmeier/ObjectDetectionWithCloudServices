using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Domain {

  public class DetectedObject {
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int H { get; set; }
    public string ObjectProperty { get; set; }
    public double Confidence { get; set; }

    public override string ToString() {
      return JsonConvert.SerializeObject(this);
    }
  }

  public class Image {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string Name { get; set; }

    public byte[] ImageData { get; set; }

    public string Type { get; set; }

    public List<DetectedObject> AnalysisResults { get; set; }

    public override string ToString() {
      return JsonConvert.SerializeObject(this);
    }

  }
}
