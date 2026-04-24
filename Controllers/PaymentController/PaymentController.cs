using ecomServer.DTO.PaymentDTO;
using ecomServer.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ecomServer.Controllers.PaymentController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServices _paymentService;

        public PaymentController(IPaymentServices paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto request)
        {
            // Mock Validation Logic
            if (string.IsNullOrEmpty(request.CardNumber) || request.CardNumber.Length < 13)
            {
                return BadRequest("Invalid Card Number.");
            }

            // Mock Failure Condition: If card ends in 2, fail the payment
            if (request.CardNumber.EndsWith("2"))
            {
                return BadRequest("Payment Failed: Insufficient Funds.");
            }

            // Mock Success Condition: If card ends in anything else, succeed
            var payment = await _paymentService.ProcessPaymentAsync(request.OrderId, request.Amount, request.PaymentMethod);
            return Ok(payment);
        }
    }
}
