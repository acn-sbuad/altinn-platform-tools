using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace PlatformRestore.Commands.Storage.Instance
{
    /// <summary>
    /// Instance 
    /// </summary>
    [Command(
      Name = "instance",
      OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
      UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    [Subcommand(typeof(List))]
    public class Instance : IBaseCmd
    {
        /// <inheritdoc/>    
        protected override Task OnExecuteAsync(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.CompletedTask;
        }
    }
}
