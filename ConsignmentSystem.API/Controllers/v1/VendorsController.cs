using ConsignmentSystem.Application.Common.Constants;
using ConsignmentSystem.Application.DTOs.Vehicles;
using ConsignmentSystem.Application.DTOs.Vendors;
using ConsignmentSystem.Application.Features.Vehicles.Commands.AddVehicle;
using ConsignmentSystem.Application.Features.Vendors.Commands.CreateVendor;
using ConsignmentSystem.Application.Features.Vendors.Commands.DeleteVendor;
using ConsignmentSystem.Application.Features.Vendors.Commands.UpdateVendor;
using ConsignmentSystem.Application.Features.Vendors.Queries.GetAllVendors;
using ConsignmentSystem.Application.Features.Vendors.Queries.GetVendorById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsignmentSystem.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class VendorsController : ControllerBase
    {
        private readonly ISender _mediator;
        public VendorsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VendorResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVendors()
        {
            var result = await _mediator.Send(new GetAllVendorsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(VendorResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVendorById(Guid id)
        {
            var result = await _mediator.Send(new GetVendorByIdQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(typeof(VendorResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateVendor([FromBody] CreateVendorCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetVendorById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVendor(Guid id, [FromBody] UpdateVendorCommand command)
        {
            command.Id = id;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = AppRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVendor(Guid id)
        {
            await _mediator.Send(new DeleteVendorCommand(id));
            return NoContent();
        }

        [HttpPost("{vendorId:guid}/vehicles")]
        [Authorize(Roles = AppRoles.Admin + "," + AppRoles.Storekeeper)]
        [ProducesResponseType(typeof(VehicleResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddVehicleToVendor(Guid vendorId, [FromBody] AddVehicleCommand command)
        {
            command.VendorId = vendorId;
            var result = await _mediator.Send(command);
            return Created(string.Empty, result);
        }
    }
}