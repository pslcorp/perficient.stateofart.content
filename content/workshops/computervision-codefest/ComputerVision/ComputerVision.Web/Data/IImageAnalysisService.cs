using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ComputerVision.Web.Data
{
    public interface IImageAnalysisService
    {
        Task<ImageAnalysis> AnalyzeImage(Stream imageStream, IList<VisualFeatureTypes?> features);
        Task<Stream> GetThumbnail(Stream imageStream, int width, int height);
    }
}