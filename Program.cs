using System.IO;
using System.Threading.Tasks;
using NSwag.CodeGeneration.CSharp;

namespace code_gen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var document = await InputSetting.GetOpenApiDoc();

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = InputSetting.ClassName,
                CSharpGeneratorSettings =
                {
                    Namespace = InputSetting.NameSpace
                }
            };

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();
            await File.WriteAllTextAsync($"{settings.ClassName}.cs", code);
        }
    }
}
