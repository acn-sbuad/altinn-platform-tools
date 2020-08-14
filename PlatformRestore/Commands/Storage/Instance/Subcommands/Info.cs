using System;
using McMaster.Extensions.CommandLineUtils;

namespace PlatformRestore.Commands.Storage.Instance
{
    /// <summary>
    /// Info command handler. Returns metadata about an instance.
    /// </summary>
    [Command(
      Name = "info",
      OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
      UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    public class Info : IBaseCmd
    {
    }
}
