using Microsoft.EntityFrameworkCore;
using TripsApp.Data;
using TripsApp.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripsApp.Models;

namespace TripsApp.Repositories;

public class TripRepositorie : ITripRepositorie
{
    private readonly ScaffoldContext _context;
    
    public TripRepositorie(ScaffoldContext context)
    {
        _context = context;
    }

    public async Task<GetTripDTO> GetTrips(int page, int pageSize)
    {
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
        
        var tripsDB = await _context.Trips
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TripDTO
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                clients = t.ClientTrips.Select(e => new ClientDTO()
                {
                    FirstName = e.IdClientNavigation.FirstName,
                    LastName = e.IdClientNavigation.LastName
                }),
                
                countries=t.IdCountries.Select(e => new CountryDTO()
                {
                    Name = e.Name
                })
                
            })
            .ToListAsync();
        var counter = _context.Trips.Count();
        var result = new GetTripDTO()
        {
            pageNum = page,
            pageSize = pageSize,
            allPages = counter,
            trips = tripsDB
        };
        return result;
    }

    public async Task<bool> DoesClinetHasTrip(int id)
    {
        var methodSyntax=  _context.Trips
            .Where(t => t.ClientTrips.Any(c => c.IdClient == id)).Count();
        bool result = false;
        if (methodSyntax == 0) result = false;
        else  result = true;
        return result;
    }

    public async Task DeleteClient(int id)
    {
        var result =  _context.Clients.Where(e => e.IdClient == id).FirstOrDefaultAsync();;
        if (result != null)
        {
           // await _context.Clients.Where(e => e.IdClient == id).ExecuteDeleteAsync();
             _context.Clients.Remove(await result);
             await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DoesClientExist(string pesel)
    {
        var methodSyntax=_context.Clients.Where(c => c.Pesel == pesel).Count();
        bool result = false;
        if (methodSyntax == 0) result = false;
        else  result = true;
        return result;
    }

    public async Task<bool> IsClientAssignetForThisTrip(string pesel, int tripID)
    {
        var id = _context.Clients.Where(c => c.Pesel == pesel).Select(c => c.IdClient);
        var methodSyntax=_context.ClientTrips.Where(c => c.IdClient.Equals(id)  &&  c.IdTrip==tripID).Count();
        bool result = false;
        if (methodSyntax == 0) result = false;
        else  result = true;
        return result;
    }

    public async Task<bool> DoesTripExist(int tripID)
    {
        var methodSyntax=_context.Trips.Where(t=>  t.IdTrip==tripID).Count();
        bool result = false;
        if (methodSyntax == 0) result = false;
        else  result = true;
        return result;
    }

    public async Task<bool> DidTripEnd(int tripId)
    {
        //var time=_context.Trips.Where(t=>  t.IdTrip==tripId).Select(t=> t.DateFrom);
        var time=_context.Trips.FirstOrDefault(t => t.IdTrip == tripId);
        bool result = time.DateFrom <= DateTime.Now;

        return result;
    }

    public async Task<bool> DoesDateMatch(string pesel, int tripId)
    {
        var client = _context.Clients.FirstOrDefault(c => c.Pesel == pesel);
        var id = client.IdClient;
        var time=_context.ClientTrips.FirstOrDefault(t => t.IdTrip == tripId && t.IdClient==id);
        bool result = time.RegisteredAt == DateTime.Now;

        return result;
    }

    public async Task AssignClient(AssignTripDTO dto, int id)
    {
        var newClient = new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Telephone = dto.Telephone,
            Pesel = dto.Pesel
        };

        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();
        
        var newTrip = new ClientTrip
        {
            IdClient = newClient.IdClient,
            IdTrip = id,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
        };

        _context.ClientTrips.Add(newTrip);
        await _context.SaveChangesAsync();
        
    }
}