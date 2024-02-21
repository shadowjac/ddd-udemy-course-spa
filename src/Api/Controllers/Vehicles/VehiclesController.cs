using Application.Vehicles.SearchVehicles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Vehicles;

public class VehiclesController : BaseController
{
    public VehiclesController(ISender sender) : base(sender)
    {
        
    }

    [HttpGet]
    public async Task<IActionResult> SearchVehiclesAsync(
        DateOnly startDate,
        DateOnly endDate,
        string? test,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchVehiclesQuery(startDate, endDate, test);
        var result = await _sender.Send(query, cancellationToken);
        
        return Ok(result.Value);
    }
    
}