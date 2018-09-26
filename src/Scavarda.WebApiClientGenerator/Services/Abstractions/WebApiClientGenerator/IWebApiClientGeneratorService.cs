using System.IO;

namespace Scavarda.WebApiClientGenerator.Services.Abstractions.WebApiClientGenerator
{
    /// <summary>
    /// Interface for WebApiClientGeneratorService
    /// </summary>
    public interface IWebApiClientGeneratorService
    {
        /// <summary>
        /// Build package from source code
        /// </summary>
        /// <param name="language">Client implementation language</param>
        /// <param name="sourceCodeDir">Source code directory</param>
        /// <param name="packageName">Package name</param>
        /// <param name="outputDir">Target directory for compiled package (nupkg, etc..)</param>
        FileInfo BuildPackage(WebApiClientGeneratorLanguage language, string sourceCodeDir, string packageName, string outputDir);

        /// <summary>
        /// Build package from source code
        /// </summary>
        /// <param name="inputFile">OpenApi Document (swagger.json) full path</param>
        /// <param name="outputDir">Target directory for compiled package (nupkg, etc..)</param>
        /// <param name="language">Client implementation language</param>
        /// <param name="packageName">Package name</param>
        /// <param name="packageVersion">Package version</param>
        FileInfo BuildPackage(string inputFile, string outputDir, WebApiClientGeneratorLanguage language, string packageName, string packageVersion = null);

        /// <summary>
        /// Generate client source code
        /// </summary>
        /// <param name="inputFile">OpenApi Document (swagger.json) full path</param>
        /// <param name="outputDir">Target directory for generated source code<param>
        /// <param name="language">Client implementation language</param>
        /// <param name="packageName">Package name</param>
        /// <param name="packageVersion">Package version</param>
        void GenerateClientSourceCode(string inputFile, string outputDir, WebApiClientGeneratorLanguage language, string packageName, string packageVersion = null);
    }
}