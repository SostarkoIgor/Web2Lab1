namespace Web2Lab1.Server.Dtos
{
    public class TicketInfoDto
    {
        public Guid? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Vatin {  get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
