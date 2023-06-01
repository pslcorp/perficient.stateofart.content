Add Credentials to `appsettings.json`

Add call to Azure Cognitive Services to `ImageAnalysisService`:

```csharp
return await computerVisionClient.AnalyzeImageInStreamAsync(imageStream, features);
```

Copy the following assignments to the `ExtractAnalysisResults` method in the `Index.razor` page:

```csharp

//get image captions
ImageCaptions = analysis!.Description.Captions;

// Get image tags
ImageTags = analysis.Tags;

// Get image categories
Landmarks = new List<LandmarksModel>();
Categories = analysis.Categories;

foreach (var category in Categories)
{
    //Get landmarks in this category
    if (category.Detail?.Landmarks != null)
    {
        foreach (var landmark in category.Detail.Landmarks)
        {
            if (!Landmarks.Any(item => item.Name == landmark.Name))
            {
                Landmarks.Add(landmark);
            }
        }
    }
}

// Get brands in the image
Brands = analysis.Brands;

//Get Objects in image
Objects = analysis.Objects;

//Get Detected Faces
Faces = analysis.Faces;

// Get moderation ratings
IsAdultContent = analysis.Adult.IsAdultContent;
IsRacyContent = analysis.Adult.IsRacyContent;
IsGoryContent = analysis.Adult.IsGoryContent;

```