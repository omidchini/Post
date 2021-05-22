using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Post.Application.Zones.Commands.CreateZone;
using Post.Application.Zones.Commands.DeleteZone;
using Post.Application.Zones.Commands.UpdateZone;
using Post.Application.Zones.Queries.ExportDeliveries;
using Post.Application.Zones.Queries.GetDeliveries;

namespace Post.WebUI.Controllers {
    [Authorize]
    public class ZoneController : ApiControllerBase {
        [HttpGet]
        public async Task<ActionResult<DeliveriesVm>> Get() {
            return await Mediator.Send(new GetDeliveriesQuery());
        }

        [HttpGet("{id}")]
        public async Task<FileResult> Export(int id) {
            var vm = await Mediator.Send(new ExportDeliveriesQuery { ZoneId = id });

            return File(vm.Content, vm.ContentType, vm.FileName);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateZoneCommand command) {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UpdateZoneCommand command) {
            if (id != command.Id) {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            await Mediator.Send(new DeleteZoneCommand { Id = id });

            return NoContent();
        }
    }
}