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

        public ReservationsController(IProcessOrders orderProcessor)
        {
            _orderProcessor = orderProcessor;
        }



        // POST /reservations -- collection (plural)

        [HttpPost("")]
        public async Task<ActionResult> AddReservation([FromBody] ReservationRequest request)
        {
            // You send me something. (what ya got)
            // Validate it (if bad, send them a 400)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // that's a 400.
            }
           // await Task.Delay(3000); // simulating all the code we will write later.

            // If it is good, process it or save it or whatever.
            // return a 201 Created
            //   -- since you created something, tell the name of it (location header)
            //   -- be a sport and return it to them.

            var response = new ReservationResponse
            {
                ReservationId = Guid.NewGuid(),
                For = request.For,
                Books = request.Books,
                Status = "Pending"
            };

            await _orderProcessor.Send(response);
            return Accepted(response);

        }
    }


    public class ReservationRequest : IValidatableObject
    {
        [Required]
        public string For { get; set; }
        public string[] Books { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Books == null || Books?.Length == 0) { yield return new ValidationResult("You need some books"); }
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
