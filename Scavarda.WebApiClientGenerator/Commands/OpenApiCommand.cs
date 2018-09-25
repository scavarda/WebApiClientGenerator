using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using System.Reflection;

namespace Scavarda.WebApiClientGenerator.Commands
{
    [Command(Description = "Generate WebApi Client from swagger.json file")]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand("generate-client", typeof(OpenApiGenerateClientCommand))]
    [Subcommand("build-package", typeof(OpenApiBuildPackageCommand))]
    class OpenApiCommand : CommandBase
    {
        public override List<string> CreateArgs()
        {
            var args = new List<string>();
            return args;
        }

        protected override int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return 1;
        }

        private static string GetVersion()
            => typeof(OpenApiCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
