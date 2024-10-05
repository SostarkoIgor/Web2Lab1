using Microsoft.EntityFrameworkCore;
using Web2Lab1.Server.Data;
using Web2Lab1.Server.Interfaces;
using Web2Lab1.Server.Models;

namespace Web2Lab1.Server.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _appDbContext;

        public TicketService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> CheckIfCanMakeNewTicketForVatinAsync(string vatin)
        {
            return (await _appDbContext.Tickets.Where(a=>a.Vatin==vatin).CountAsync())<3;
        }

        public async Task<Guid> GenerateTicketAsync(string vatin, string firstName, string lastName)
        {
            var ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Vatin = vatin,
                CreatedAt = DateTime.UtcNow
            };
            _appDbContext.Tickets.Add(ticket);
            await _appDbContext.SaveChangesAsync();
            return ticket.Id;
        }

        public async Task<int> GetNumberOfGeneratedTicketsAsync()
        {
            return await _appDbContext.Tickets.CountAsync();
        }
    }
}
