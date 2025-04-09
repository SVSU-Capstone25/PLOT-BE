using Plot.Data.Models.Auth.Email;
using RazorLight;

namespace Plot.Services;

/// <summary>
/// Filename: RazorEmailRenderer.cs
/// Part of Project: PLOT/PLOT-BE/Plot/Services
/// 
/// File Purpose:
/// This file contains the implementation of the RazorEmailRenderer class,
/// which 
/// is responsible for rendering html email templates. 
///
/// Class Purpose:
/// The RazorEmailRenderer class is a utility service that renders 
/// html email templates. It uses the RazorLight engine to dynamically 
/// generate HTML emails by injecting model data into pre-defined templates.
///
/// Dependencies:
/// RazorLight: https://github.com/toddams/RazorLight
/// EmailTemplate: A model representing dynamic content for email templates.
/// 
/// Written by: Michael Polhill
/// </summary>
public class RazorEmailRenderer
{
    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // RazorLightEngine instance used to render email templates.
    private readonly RazorLightEngine _razorEngine;

    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----

    /// <summary>
    /// Constructor used for RazorEmailRenderer class.
    /// Initializes a new instance of the RazorEmailRenderer class.
    /// Configures the RazorLightEngine to use the file system for loading 
    /// templates.
    /// </summary>
    public RazorEmailRenderer()
    {
        _razorEngine = new RazorLightEngineBuilder()
            .UseFileSystemProject("/app/wwwroot/Templates")
                .UseMemoryCachingProvider()
                .Build();
    }

    /// <summary>
    /// Method used to render an email template asynchronously.
    /// Uses an html template and accompanying model data to generate an
    /// email.
    /// </summary>
    /// <param name="emailModel"> Model used to inject info into the
    /// template</param>
    /// <returns></returns>
    public async Task<string> RenderEmailAsync(EmailTemplate emailModel)
    {
        // Path to the html email template file.
        string templatePath = "EmailTemplate.cshtml";
        // Render the email template adding the data from the emailModel.
        return await _razorEngine.CompileRenderAsync(
            templatePath, emailModel);
    }
}