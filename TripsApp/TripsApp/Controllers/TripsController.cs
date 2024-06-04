using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TripsApp.Data;
using TripsApp.DTO_s;
using TripsApp.Repositories;

namespace TripsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ScaffoldContext _context;
    private readonly ITripRepositorie _tripRepositorie;
    public TripsController(ScaffoldContext context, ITripRepositorie tripRepositorie)
    {
        _context = context;
        _tripRepositorie = tripRepositorie;
    }
    [HttpGet("/api/trips")]
    //public IActionResult GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    public async Task<IActionResult> GetTrips( int page = 1,int pageSize = 10)
    {
        var result = await _tripRepositorie.GetTrips(page, pageSize);
        return Ok(result);
    }
    [HttpDelete ("/api/clients/{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        if (!await _tripRepositorie.DoesClinetHasTrip(idClient))
            return Conflict("This client has trips assigend and cannot be deleted!");
        _tripRepositorie.DeleteClient(idClient);
        return Ok("client deleted");
    }

    [HttpPost("/api/trips/{idTrip}/clients")]
    public async Task<IActionResult> AssignClient(int idTrip,AssignTripDTO dto )
    {
        if (!await _tripRepositorie.DoesClientExist(dto.Pesel))
            return Conflict("No such client with this PESEL!");
        if (!await _tripRepositorie.IsClientAssignetForThisTrip(dto.Pesel, idTrip))
            return Conflict("Client is already assigend for this trip");
        if (!await _tripRepositorie.DoesTripExist(idTrip))
            return Conflict("No such trip!");
        if (!await _tripRepositorie.DidTripEnd(idTrip))
            return Conflict("This trip has already happend");
        if (!await _tripRepositorie.DoesDateMatch(dto.Pesel, idTrip))
            return Conflict("Registered date dose not match request date");
        return Ok();
    }
    /* dodawanie wycieczek
     var trips = await _context.Trips.Select(e => new TripDTO()
       {
       Name = e.Name,
       DateFrom = e.DateFrom,
       MaxPeople = e.MaxPeople,
       Clients = e.ClientTrips.Select(e => new ClientDTO()
       {
       FirstName = e.IdClientNavigation.FirstName,
       LastName = e.IdClientNavigation.LastName
       })
       })
       .OrderBy(e => e.DateFrom)
       .ToListAsync();
       .Skip((page-1) * pageSize)
       .Take(pageSize)
     */
}