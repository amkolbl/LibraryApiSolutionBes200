using LibraryApi.Filters;
using LibraryApi.Models.Reservations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    public class ReservationsController: ControllerBase
    {
        private readonly ILookupReservations _reservationLookups;
        private readonly IReservationCommands _reservationCommands;

        public ReservationsController(ILookupReservations reservationLookups, IReservationCommands reservationCommands)
        {
            _reservationLookups = reservationLookups;
            _reservationCommands = reservationCommands;
        }

        // POST a reservation
        [HttpPost("/reservations")]
        [ValidateModel]
        public async Task<ActionResult> AddAReservation([FromBody] PostReservationRequest request)
        {
            // Decide if this thing is WORTHY of being part of my collection
            GetReservationSummaryResponseItem response = await _reservationCommands.AddReservationAsync(request);
            return CreatedAtRoute("reservations#getbyid", new { Id = response.Id }, response);
        }
        // GET /{id}
        [HttpGet("/reservations/{id:int}", Name = "reservations#getbyid")]
        public async Task<ActionResult> GetReservationById(int id)
        {
            GetReservationSummaryResponseItem response = await _reservationLookups.GetByIdAsync(id);
            return this.Maybe(response);
        }


        // GET /
        [HttpGet("/reservations")]
        public async Task<ActionResult> GetAlLReservations()
        {
            GetReservationSummaryResponse response = await _reservationLookups.GetAllReservationsAsync();
            return Ok(response);
        }
        // GET /pending
        // GET /ready
        // POST /ready (worker)
        [HttpPost("/reservations/ready")]
        [ValidateModel]
        public async Task<ActionResult> PutResrvationInReadyBucket([FromBody] GetReservationSummaryResponseItem item)
        {
            if(await _reservationCommands.MarkReady(item))
            {
                return Accepted();
            }
            else
            {
                return BadRequest("No Such Reservation");
            }
        }
        // GET /denied
        // POST /denied (worker)
        [HttpPost("/reservations/denied")]
        [ValidateModel]
        public async Task<ActionResult> PutReservationInDeniedBucket([FromBody] GetReservationSummaryResponseItem item)
        {
            if (await _reservationCommands.MarkDenied(item))
            {
                return Accepted();
            }
            else
            {
                return BadRequest("No Such Reservation");
            }
        }
    }
}
