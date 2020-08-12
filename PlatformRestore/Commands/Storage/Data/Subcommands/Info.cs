using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altinn.Platform.Storage.Interface.Models;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using PlatformRestore.Attributes;
using PlatformRestore.Services.Interfaces;

namespace PlatformRestore.Commands.Storage.Data
{
    /// <summary>
    /// Info command handler. Returns metadata about a data element.
    /// </summary>
    [Command(
      Name = "info",
      OptionsComparison = StringComparison.InvariantCultureIgnoreCase,
      UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.CollectAndContinue)]
    public class Info : IBaseCmd
    {
        /// <summary>
        /// Instance guid
        /// </summary>
        [Option(
            CommandOptionType.SingleValue,
            ShortName = "dg",
            LongName = "dataGuid",
            ShowInHelpText = true,
            Description = "DataGuid for the data element.")]
        [Guid]
        [Required]
        public string DataGuid { get; set; }

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

        private readonly ICosmosService _cosmosService;

        /// <summary>
        /// Initializes a new instance of the <see cref="Info"/> class.
        /// </summary>
        public Info(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }

        /// <summary>
        /// Retrieves metadata about the dataElement
        /// </summary>
        protected override async Task OnExecuteAsync(CommandLineApplication app)
        {
            string instanceGuid = InstanceGuid ?? InstanceId.Split('/')[1];

            try
            {
                DataElement element = await _cosmosService.GetDataElement(DataGuid, instanceGuid);
                Console.WriteLine(JsonConvert.SerializeObject(element, Formatting.Indented) + "\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
