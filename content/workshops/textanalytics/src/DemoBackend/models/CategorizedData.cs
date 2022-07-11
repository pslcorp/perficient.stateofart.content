public class CategorizedData
{
    //
        // Summary:
        //     Gets the entity text as it appears in the input document.
        public string? Text { get; set;}
        //
        // Summary:
        //     Gets the entity category inferred by the Text Analytics service's named entity
        //     recognition model. The list of available categories is described at https://docs.microsoft.com/azure/cognitive-services/Text-Analytics/named-entity-types.
        public string? Category { get; set;}
        //
        // Summary:
        //     Gets the sub category of the entity inferred by the Text Analytics service's
        //     named entity recognition model. This property may not have a value if a sub category
        //     doesn't exist for this entity. The list of available categories and subcategories
        //     is described at https://docs.microsoft.com/azure/cognitive-services/Text-Analytics/named-entity-types.
        public string? SubCategory { get; set;}
        //
        // Summary:
        //     Gets a score between 0 and 1, indicating the confidence that the text substring
        //     matches this inferred entity.
        public double ConfidenceScore { get; set;}
        //
        // Summary:
        //     Gets the starting position for the matching text in the input document.
        public int Offset { get; set;}
        //
        // Summary:
        //     Gets the length of the matching text in the input document.
        public int Length { get; set;}
}