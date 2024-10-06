using Web2Lab1.Server.Models;

namespace Web2Lab1.Server.Interfaces
{
    public interface ITicketService
    {
        public Task<bool> CheckIfCanMakeNewTicketForVatinAsync(string vatin);

        public Task<Guid> GenerateTicketAsync(string vatin, string firstName, string lastName);
        public Task<int> GetNumberOfGeneratedTicketsAsync();
        public Task<Ticket?> GetTicketByTicketIdAsync(Guid ticketId);
    }
}
