using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Scavarda.WebApiClientGenerator.Services.Abstractions.WebApiClientGenerator;
using Scavarda.WebApiClientGenerator.Services.SwaggerCodeGenerator;

namespace Scavarda.WebApiClientGenerator.Commands
{
    [Command(Description = "Generate WebApi Client from swagger.json file")]
    class OpenApiGenerateClientCommand : CommandBase
    {
        [Option("-i|--input <path>", "Full path of swagger.json file", CommandOptionType.SingleValue)]
        [Required]
        [FileExists]
        public string InputFile { get; set; }

        [Option("-l|--language <language>", "WebApi client target language (One of: all, dotnet4, dotnet45, java, netcore2, php, typescript-angular)", CommandOptionType.SingleValue)]
        [Required]
        [AllowedValues("all", "dotnet4", "dotnet45", "java", "netcore2", "php", "typescript-angular")]
        public string Language { get; set; }

        [Option("-o|--output <output_folder>", "WebApi client source code output folder", CommandOptionType.SingleValue)]
        [LegalFilePath]
        public string OutputFolder { get; set; }

        [Option("-n|--package-name <package_name>", "WebApi client package name", CommandOptionType.SingleValue)]
        [Required]
        public string PackageName { get; set; }

        [Option("-v|--package-version <package_version>", "WebApi client package version", CommandOptionType.SingleValue)]
        public string PackageVersion { get; set; }

        /// <summary>
        /// You can use this pattern when the parent command may have options or methods you want to use from sub-commands.
        /// This will automatically be set before OnExecute is invoked
        /// </summary>
        private OpenApiCommand Parent { get; set; }

        private IWebApiClientGeneratorService _swaggerCodeGeneratorService;

        public override List<string> CreateArgs()
        {
            var args = Parent.CreateArgs();
            args.Add("generate-client");

            return args;
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            _swaggerCodeGeneratorService = new SwaggerCodeGeneratorService(new SwaggerCodeGeneratorServiceOptions
            {
                DotNetCommandPath = "dotnet",
                JavaCommandPath = "java",
                SwaggerCodeGeneratorJarPath = @"C:\Program Files\SwaggerCodGen\swagger-codegen-cli-2.4.0-20180919.042154-327.jar",
            });

            var language = WebApiClientGeneratorLanguage.All;
            if (Language == "all") language = WebApiClientGeneratorLanguage.All;
            if (Language == "dotnet4") language = WebApiClientGeneratorLanguage.DotNet4;
            if (Language == "dotnet45") language = WebApiClientGeneratorLanguage.DotNet45;
            if (Language == "java") language = WebApiClientGeneratorLanguage.Java;
            if (Language == "netcore2") language = WebApiClientGeneratorLanguage.NetCoreStandard2;
            if (Language == "php") language = WebApiClientGeneratorLanguage.Php;
            if (Language == "typescript-angular") language = WebApiClientGeneratorLanguage.TypescriptAngular;

            _swaggerCodeGeneratorService.GenerateClientSourceCode(InputFile, OutputFolder, language, PackageName, PackageVersion);

            return base.OnExecute(app);
        }
    }
}
