

using Plot.Data.Models.Email; 
using RazorLight;

namespace Plot.Services
{
    public class RazorEmailRenderer
    {
        private readonly RazorLightEngine _razorEngine;

        public RazorEmailRenderer()
        {
            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Directory.GetCurrentDirectory() + "/Templates/")
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderEmailAsync(EmailTemplate emailModel)
        {
            string templatePath = "EmailTemplate.cshtml";
            return await _razorEngine.CompileRenderAsync(templatePath, emailModel);
        }
    }
}