using dtos;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using System.Text.Json;
using System.Text.Json.Nodes;
using static Play.Catalog.Contract.Contracts;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        private static List<ItemDto> itemDtos = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restore HP", 5, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Antidote", "Restore status", 7, DateTimeOffset.Now),
            new ItemDto(Guid.NewGuid(), "Bronce Sword", "Hit damage", 5, DateTimeOffset.Now),

        };

        public ItemController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("nugget")]
        public string getNugget()
        {
            Common.PlayCommon nugget = new Common.PlayCommon();
            return nugget.MetodoNugget() + PlayCommon.MetodoStatico();
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> get()
        {
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _publishEndpoint.Publish(new LogActions("GetItemDto", "", "", "Get", JsonSerializer.Serialize(itemDtos), remoteIpAddress, DateTime.Now));
            return itemDtos;
        }

        [HttpGet("{id}")]
        public ItemDto getById(Guid id)
        {
            return itemDtos.Where(a => a.Id.Equals(id)).FirstOrDefault();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemDto(CreateItemDto c)
        {
            var item = new ItemDto(Guid.NewGuid(), c.Name, c.Description, c.Price, DateTimeOffset.Now);
            itemDtos.Add(item);

            await _publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description));

            return CreatedAtAction(nameof(CreateItemDto), new { id = item.Id }, item);

        }
    }
}
