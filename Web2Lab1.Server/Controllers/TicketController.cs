using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web2Lab1.Server.Dtos;
using Web2Lab1.Server.Interfaces;
using QRCoder;

namespace Web2Lab1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService ticketService;

        public TicketController(ITicketService ticketService)
        {
            this.ticketService = ticketService;
        }

        [Authorize]
        [HttpPost("createTicket")]
        public async Task<IActionResult> CreateTicket([FromBody] GenerateTicketDto ticketDto)
        {
            if (ticketDto == null || ticketDto.Vatin == null || ticketDto.LastName == null || ticketDto.FirstName == null
                || ! (await ticketService.CheckIfCanMakeNewTicketForVatinAsync(ticketDto.Vatin)))
            {
                return BadRequest();
            }
            var id = await ticketService.GenerateTicketAsync(ticketDto.Vatin, ticketDto.FirstName, ticketDto.LastName);

            string url = $"https://dev-sx10l5srw3t5xwff.us.auth0.com/ticket/{id}";
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using (PngByteQRCode qrCode = new (qrCodeData))
            {
                var qrCodeImage = qrCode.GetGraphic(20);
                return File(qrCodeImage, "image/png");
            }
            
        }

        [HttpGet("numberOfTickets")]
        public async Task<ActionResult<int>> GetNumberOfGeneratedTickets()
        {
            return await ticketService.GetNumberOfGeneratedTicketsAsync();
        }
    }
}
