using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ComputerVision.Web.Data;

public class ImageAnalysisService : IImageAnalysisService
{
    private readonly IComputerVisionClient computerVisionClient;

    public ImageAnalysisService(IComputerVisionClient computerVisionClient)
    {
        this.computerVisionClient = computerVisionClient;
    }

    public async Task<ImageAnalysis> AnalyzeImage(Stream imageStream, IList<VisualFeatureTypes?> features)
    {
        //Call the computer vision client's analysis method
        throw new NotImplementedException();
    }

    public async Task<Stream> GetThumbnail(Stream imageStream, int width, int height)
    {
        throw new NotImplementedException();
    }
}
