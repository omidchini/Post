using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Post.Application.Common.Models;
using Post.Application.Deliveries.Commands.CreateDelivery;
using Post.Application.Deliveries.Commands.DeleteDelivery;
using Post.Application.Deliveries.Commands.UpdateDelivery;
using Post.Application.Deliveries.Commands.UpdateDeliveryDetail;
using Post.Application.Deliveries.Queries.GetDeliveriesWithPagination;
using Post.Application.Zones.Queries.GetDeliveries;

namespace Post.WebUI.Controllers {
    [Authorize]
    public class DeliveryController : ApiControllerBase {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<DeliveryDto>>> GetDeliveriesWithPagination([FromQuery] GetDeliveriesWithPaginationQuery query) {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateDeliveryCommand command) {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateDeliveryCommand command) {
            if (id != command.Id) {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpPut("[action]")]
        public async Task<ActionResult> UpdateItemDetails(int id, UpdateDeliveryDetailCommand command) {
            if (id != command.Id) {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            await Mediator.Send(new DeleteDeliveryCommand { Id = id });

            return NoContent();
        }
    }
}