using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace PlatformRestore.Commands.Storage
{
    /// <summary>
    /// Storage root command.
    /// </summary>
    [Command(
       Name = "storage",
       OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
       UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    [Subcommand(typeof(Data.Data), typeof(Instance.Instance))]
    public class Storage : IBaseCmd
    {
        /// <inheritdoc/>    
        protected override Task OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.CompletedTask;
        }
    }
}
