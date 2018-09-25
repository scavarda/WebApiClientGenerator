using Newtonsoft.Json;

namespace Scavarda.WebApiClientGenerator.Services.SwaggerCodeGenerator
{
    /// <summary>
    /// SwaggerCodeGenerator Config Options
    /// </summary>
    public struct SwaggerCodeGeneratorConfigOptions
    {
        [JsonProperty("gitRepoId")]
        public string GitRepoId;

        [JsonProperty("gitUserId")]
        public string GitUserId;

        //public string apiPackage;   -> specific namespace for lib/Api
        //public string modelPackage; -> specific namespace for lib/Model

        [JsonProperty("invokerPackage")]
        public string InvokerPackage;

        [JsonProperty("modelPropertyNaming")]
        public string ModelPropertyNaming;

        [JsonProperty("netCoreProjectFile")]
        public bool NetCoreProjectFile;

        [JsonProperty("packageName")]
        public string PackageName;

        [JsonProperty("packageVersion")]
        public string PackageVersion;

        [JsonProperty("targetFramework")]
        public string TargetFramework;
    }
}