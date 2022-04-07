using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Logic {
  public interface IImageLogic {
    Task<Image> GetImageByIdAsync(string id);
    Task<IEnumerable<Image>> GetImagesAsync();
    Task<string> InsertImageAsync(Image data);
    Task<bool> ImageExistsAsync(string id);
    Task<string> UpsertImageAsync(Image image);
  }
}
