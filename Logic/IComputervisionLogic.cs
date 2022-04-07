using System.Threading.Tasks;
using Domain;

namespace Logic {
  public interface IComputerVisionLogic {
    Task<Image> AnalyzeAsync(Image data);
  }
}