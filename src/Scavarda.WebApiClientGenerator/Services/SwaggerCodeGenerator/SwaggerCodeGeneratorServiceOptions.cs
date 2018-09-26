namespace Scavarda.WebApiClientGenerator.Services.SwaggerCodeGenerator
{
    /// <summary>
    /// SwaggerCodeGeneratorService Options for Options pattern
    /// </summary>
    public class SwaggerCodeGeneratorServiceOptions
    {
        /// <summary>
        /// DotNetCommandPath
        /// </summary>
        public string DotNetCommandPath { get; set; }

        /// <summary>
        /// JavaCommandPath
        /// </summary>
        public string JavaCommandPath { get; set; }

        /// <summary>
        /// SwaggerCodeGeneratorJarPath
        /// </summary>
        public string SwaggerCodeGeneratorJarPath { get; set; }
    }
}