using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;
using Vlingo.UUID;

namespace Logic.Db {
  public class DbImageLogic : IImageLogic {
    private IImageRepository imageRepo;
    private RandomBasedGenerator uuidGenerator;

    public DbImageLogic(IImageRepository imageRepo) {
      this.imageRepo = imageRepo;
      this.uuidGenerator = new RandomBasedGenerator();
    }


    public Task<Image> GetImageByIdAsync(string id) {
      return imageRepo.FindByIdAsync(id);
    }

    public Task<IEnumerable<Image>> GetImagesAsync() {
      return imageRepo.FindAllAsync();
    }

    public Task<string> InsertImageAsync(Image data) {
      if (data.Id == null)
        data.Id = uuidGenerator.GenerateGuid().ToString();
      if (string.IsNullOrEmpty(data.Name)) {
        data.Name = "unknown";
      }
      if (string.IsNullOrEmpty(data.Type)) {
        data.Type = "unknown";
      }
      return imageRepo.InsertAsync(data);
    }

    public async Task<bool> ImageExistsAsync(string id) {
      return await imageRepo.FindByIdAsync(id) != null;
    }

    public async Task<string> UpsertImageAsync(Image data) {
      if (data.Id == null)
        data.Id = uuidGenerator.GenerateGuid().ToString();
      if (string.IsNullOrEmpty(data.Name)) {
        data.Name = "unknown";
      }
      if (string.IsNullOrEmpty(data.Type)) {
        data.Type = "unknown";
      }
      return await imageRepo.UpsertAsync(data);
    }
  }
}