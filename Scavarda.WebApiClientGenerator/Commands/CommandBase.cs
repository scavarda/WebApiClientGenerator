using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;

namespace Scavarda.WebApiClientGenerator.Commands
{
    /// <summary>
    /// This base type provides shared functionality.
    /// Also, declaring <see cref="HelpOptionAttribute"/> on this type means all types that inherit from it
    /// will automatically support '--help'
    /// </summary>
    [HelpOption("-h|--help")]
    abstract class CommandBase
    {
        public abstract List<string> CreateArgs();

        protected virtual int OnExecute(CommandLineApplication app)
        {
            var args = CreateArgs();

            Console.WriteLine("Arguments: " + ArgumentEscaper.EscapeAndConcatenate(args));
            return 0;
        }
    }
}
