using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;

using PlatformRestore.Attributes;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore.Commands.Storage.Data
{
    /// <summary>
    /// List command handler. Lists data elements based on given parameters.
    /// </summary>
    [Command(
      Name = "list",
      OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
      UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    public class List : IBaseCmd
    {
        /// <summary>
        /// Instance Id.
        /// </summary>
        [Option(
            CommandOptionType.SingleValue,
            ShortName = "iid",
            LongName = "instanceId",
            ShowInHelpText = true,
            Description = "InstanceId [instanceOwner.partyId/instanceGuid] for the instance the dataElement is connected to.")]
        [InstanceId]
        public string InstanceId { get; set; }

        /// <summary>
        /// Instance guid
        /// </summary>
        [Option(
            CommandOptionType.SingleValue,
            ShortName = "ig",
            LongName = "instanceGuid",
            ShowInHelpText = true,
            Description = "InstanceGuid for the instance the dataElement is connected to.")]
        [Guid]
        public string InstanceGuid { get; set; }

        private readonly ICosmosService _comsosService;

        /// <summary>
        /// Initializes a new instance of the <see cref="List"/> class.
        /// </summary>
        public List(ICosmosService cosmosService)
        {
            _comsosService = cosmosService;
        }

        /// <inheritdoc/>    
        protected override async Task OnExecuteAsync(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(Program.Environment))
            {
                Console.WriteLine("Please set the environment context before using this command.");
                Console.WriteLine("Update environment using cmd: settings update -e [environment] \n ");
                return;
            }

            if (string.IsNullOrEmpty(InstanceId) && string.IsNullOrEmpty(InstanceGuid))
            {
                Console.WriteLine("Please provide an instanceId or instanceGuid");
                return;
            }

            string instanceGuid = InstanceGuid ?? InstanceId.Split('/')[1];

            await ListActiveDataElements(instanceGuid);
        }

        private async Task ListActiveDataElements(string instanceGuid)
        {
            try
            {
                List<string> dataGuids = await _comsosService.ListDataElements(instanceGuid);

                Console.WriteLine($"\n \n Active data elements for instanceGuid {instanceGuid}:");
                foreach (string id in dataGuids)
                {
                    Console.WriteLine("\t" + id);
                }

                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
