namespace Web2Lab1.Server.Interfaces
{
    public interface ITicketService
    {
        public Task<bool> CheckIfCanMakeNewTicketForVatinAsync(string vatin);

        public Task<Guid> GenerateTicketAsync(string vatin, string firstName, string lastName);
        public Task<int> GetNumberOfGeneratedTicketsAsync();
    }
}
