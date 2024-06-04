using TripsApp.DTO_s;

namespace TripsApp.Repositories;

public interface ITripRepositorie
{
    Task<GetTripDTO> GetTrips(int page, int pageSize );
    Task<bool> DoesClinetHasTrip(int id);
    Task DeleteClient(int id);
    Task<bool> DoesClientExist(string pesel);
    Task<bool> IsClientAssignetForThisTrip(string pesel, int TripID);
    Task<bool> DoesTripExist(int tripID);
    Task<bool> DidTripEnd(int tripId);
    Task<bool> DoesDateMatch(string pesel, int tripId);
    Task AssignClient(AssignTripDTO dto, int id);

}