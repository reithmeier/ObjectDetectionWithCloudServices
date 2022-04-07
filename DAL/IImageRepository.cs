using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
  public interface IImageRepository
  {
    Task<IEnumerable<Image>> FindAllAsync();
    Task<Image> FindByIdAsync(string id);
    Task<string> InsertAsync(Image data);
    Task<string> UpsertAsync(Image data);
  }
}