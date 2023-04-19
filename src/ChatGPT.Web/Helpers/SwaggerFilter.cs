////-----------------------------------------------------------------------
//// <copyright file="CustomSwaggerFilter.cs" company="Luppes Consulting, Inc.">
//// Copyright 2023, Luppes Consulting, Inc. All rights reserved.
//// </copyright>
//// <summary>
//// Custom Swagger Filter
//// </summary>
////-----------------------------------------------------------------------
//namespace ChatGPT.Web.Web;

///// <summary>
///// Swagger Filters
///// </summary>
//[ExcludeFromCodeCoverage]
//public class CustomSwaggerFilter : IDocumentFilter
//{
//    /// <summary>
//    /// Apply the filters to remove extraneous API routes that I don't want shown
//    /// </summary>
//    /// <param name="swaggerDoc">Doc</param>
//    /// <param name="context">Context</param>
//    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
//    {
//        var identityRoutes = swaggerDoc.Paths
//            .Where(x => x.Key.ToLower().Contains("microsoftidentity"))
//            .ToList();
//        identityRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
//    }
//}