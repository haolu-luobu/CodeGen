using System.IO;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.CSharp;

namespace code_gen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Net.WebClient wclient = new System.Net.WebClient();         

            // OpenApiDocument document = await OpenApiYamlDocument.FromYamlAsync(
                // wclient.DownloadString("http://172.19.0.7/doc/Services/ModerationDBService.yml"));
            OpenApiDocument document = await OpenApiDocument.FromJsonAsync(
                // wclient.DownloadString("http://172.19.0.44/swagger/v1/swagger.json")
                File.ReadAllText("/Users/hlu/rsrc/swagger/code_gen/code_gen/AIModerationClient.json")
                );
            wclient.Dispose();

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = "AIModerationClient", 
                CSharpGeneratorSettings = 
                {
                    Namespace = "Luobo.AIModerationClient.Client"
                }
            };

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            await File.WriteAllTextAsync($"{settings.ClassName}.cs", code);
        }
    }
}
