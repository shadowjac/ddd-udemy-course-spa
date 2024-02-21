using Application.Rentals.BookRental;
using Application.Rentals.GetRentals;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Rentals;

public class RentalsController : BaseController
{
    public RentalsController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRental(Guid id, CancellationToken cancellationToken = default)
    {
        var query = new GetRentalsQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRental(Guid id,
        RentalRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new BookRentalCommand(
            request.VehicleId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await _sender.Send(command, cancellationToken);
        
        if( result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(GetRental), new { id = result.Value }, result.Value);
    }
}