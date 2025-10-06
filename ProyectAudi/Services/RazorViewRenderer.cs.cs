using RazorLight;
using System.IO;
using System.Threading.Tasks;

namespace ProyectAudi.Services
{
    public class RazorViewRenderer
    {
        private readonly RazorLightEngine _engine;

        public RazorViewRenderer()
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Views"))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderAsync<T>(string viewPath, T model)
        {
            return await _engine.CompileRenderAsync(viewPath, model);
        }
    }
}
