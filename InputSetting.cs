using System.IO;
using System.Threading.Tasks;
using NSwag;

namespace code_gen
{
    public static class InputSetting
    {

        #region GeneralSetting

        public static readonly string ClassName = "";
        public static readonly string NameSpace = "";

        public static readonly bool FileOrUrl = true; //file true, url false
        public static readonly bool JsonOrYaml = true; // json true, yaml false

        public static readonly string Location = ""; // url or path location

        #endregion


        
        private static string GetFileTxt()
        {
            if (FileOrUrl)
            {
                //file
                return File.ReadAllText(Location);
            }
            //url
            var wclient = new System.Net.WebClient();
            return wclient.DownloadString(Location);
        }

        /// <summary>
        /// Create the openapi document file, based on the input. 
        /// </summary>
        /// <returns></returns>
        public  static async Task<OpenApiDocument> GetOpenApiDoc()
        {
            var fileTxt = GetFileTxt();
            if (JsonOrYaml)
            {
                return await OpenApiDocument.FromJsonAsync(fileTxt);
            }
            return await OpenApiYamlDocument.FromYamlAsync(fileTxt);
        }
    }
}
