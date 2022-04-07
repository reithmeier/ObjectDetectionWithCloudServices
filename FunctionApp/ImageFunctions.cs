using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DAL;
using Domain;
using Logic;
using Logic.Cv;
using Logic.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp {
  public class ImageFunctions {

    private IImageLogic ImageLogic { get; set; }
    private CognitiveComputerVisionLogic CvLogic { get; set; }

    public ImageFunctions(IImageLogic imageLogic, IComputerVisionLogic cvLogic) {
      ImageLogic = imageLogic;
      CvLogic = new CognitiveComputerVisionLogic();
    }

    [FunctionName("Hello")]
    public async Task<IActionResult> Hello(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hello")] HttpRequest req,
      ILogger log
    ) {
      log.LogInformation("hello");
      return new OkObjectResult("world");
    }

    [FunctionName("GetImages")]
    public async Task<IActionResult> GetImages(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images")] HttpRequest req,
      ILogger log
      ) {
      log.LogInformation("GetImages");
      return new OkObjectResult(await ImageLogic.GetImagesAsync());
    }

    [FunctionName("GetImageById")]
    public async Task<IActionResult> GetImageById(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "images/{id}")] HttpRequest req,
      string id,
      ILogger log
      ) {
      log.LogInformation($"GetImageById({id})");
      if (!await ImageLogic.ImageExistsAsync(id)) {
        return new NotFoundResult();
      }
      return new OkObjectResult(await ImageLogic.GetImageByIdAsync(id));
    }

    [FunctionName("InsertImage")]
    public async Task<IActionResult> InsertImage(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "images")] HttpRequest req,
      ILogger log
      ) {
      var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      var data = JsonConvert.DeserializeObject<Image>(requestBody);

      log.LogInformation($"InsertImage({data}");

      await ImageLogic.InsertImageAsync(data);
      return new CreatedResult($"{req.Scheme}://{req.Host}{req.Path}/{data.Id}", data);
    }

    [FunctionName("AnalyzeImage")]
    public async Task<IActionResult> AnalyzeImage(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "analyze/{id}")] HttpRequest req,
      string id,
      ILogger log
      ) {

      log.LogInformation($"AnalyzeImage({id}");

      var image = await ImageLogic.GetImageByIdAsync(id);
      var newImage = await CvLogic.AnalyzeAsync(image);
      await ImageLogic.UpsertImageAsync(newImage);
      return new CreatedResult($"{req.Scheme}://{req.Host}{req.Path}/{image.Id}", image);
    }


    [FunctionName("AnalyzeAndUploadImage")]
    public async Task<IActionResult> AnalyzeAndUploadImage(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "images/analyze")] HttpRequest req,
      ILogger log
    ) {
      var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
      var data = JsonConvert.DeserializeObject<dynamic>(requestBody);
      var image = new Image { Name = data.name, Type = data.type };

      //parse bytes
      List<byte> buffer = new List<byte>();
      foreach (var b in data.imageData) {
        byte val = (byte)b.First.Value;
        buffer.Add(val);
      }

      image.ImageData = buffer.ToArray();

      log.LogInformation($"AnalyzeAndUploadImage");

      if (image.ImageData == null) {
        return new BadRequestResult();
      }

      Image analyzedImage = await CvLogic.AnalyzeAsync(image);
      await ImageLogic.UpsertImageAsync(analyzedImage);
      return new CreatedResult($"{req.Scheme}://{req.Host}{req.Path}/{analyzedImage.Id}", analyzedImage);
    }
  }
}
