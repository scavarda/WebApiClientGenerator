using McMaster.Extensions.CommandLineUtils;
using Scavarda.WebApiClientGenerator.Commands;

namespace Scavarda.WebApiClientGenerator
{
    class Program
    {
        public static void Main(string[] args) => CommandLineApplication.Execute<OpenApiCommand>(args);
    }
}
