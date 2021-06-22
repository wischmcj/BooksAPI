using Microsoft.AspNetCore.Mvc;
using ReservationsApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationsApi.Controllers
{
    public class ReservationsController : ControllerBase
    {
        private readonly IProcessOrders _orderProcessor;

        public ReservationsController(IProcessOrders orderProcessor){
            orderProcessor = _orderProcessor;       
        }   

        [HttpPost("")]
        public async Task<ActionResult> AddReservation([FromBody] ReservationRequest    request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await Task.Delay(3000); //Simulating processing time when there is actual code here
        

            var response = new ReservationResponse
            {
                ReservationId = Guid.NewGuid(),
                For = request.For,
                Books = request.Books,
                Status = "Approved"
            };
            return Accepted(response);

            await _orderProcessor.Send(response);        
        }
    }

    public class ReservationRequest : IValidatableObject
    {
        public string For { get; set; }
        public string[] Books { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Books == null || Books?.Length == 0) { yield return new ValidationResult("You need some books"); }
        }
    }

    public class ReservationResponse
    {
        public Guid ReservationId { get; set; }
        public string For { get; set; }
        public string[] Books { get; set; }

        public string Status { get; set; }
        
    }


}
