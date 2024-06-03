using Microsoft.AspNetCore.Mvc;
using TripsApp.Data;

namespace TripsApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly ScaffoldContext _context;
    public TripsController(ScaffoldContext context)
    {
        _context = context;
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