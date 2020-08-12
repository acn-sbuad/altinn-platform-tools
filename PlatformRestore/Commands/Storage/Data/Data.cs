using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace PlatformRestore.Commands.Storage.Data
{
    /// <summary>
    /// Data command
    /// </summary>
    [Command(
      Name = "data",
      OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
      UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    [Subcommand(typeof(List), typeof(Info))]
    public class Data : IBaseCmd
    {
        /// <inheritdoc/>    
        protected override Task OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.CompletedTask;
        }
    }
}
