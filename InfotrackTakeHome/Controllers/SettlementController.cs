using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using TakeHomeAssignmentDomain.Models;
using System.Linq;
using TakeHomeAssignmentServices;

namespace InfotrackTakeHomeAssignment.Controllers;
[ApiController]
[Route("[controller]")]
public class SettlementController : ControllerBase
{
    private const int MAX_SIMILTANEOUS_BOOKINGS = 4;
    private readonly ILogger<SettlementController> _logger;
    private readonly ISettlementBookingService settlementBookingService;

    public SettlementController(ILogger<SettlementController> logger, ISettlementBookingService _settlementBookingService)
    {
        _logger = logger;
        settlementBookingService = _settlementBookingService;
    }


    [HttpPost(Name = "BookSettlement")]
    public ActionResult<BookingSuccessModel> BookSettlement([FromBody] BookingRequestModel request)
    {

        // Note: Could do this with fluent validation or some simlar prescriptive library. But Given time constraints I settled on this
        // Other advantages of fluent validation or some equivalent is that we could return more detailed errors to say "why" its wrong/failed.
        if (request == null || string.IsNullOrWhiteSpace(request.Name) || !this.settlementBookingService.IsValidBookingTime(request.BookingTime, out var timeSlot))
        {
            _logger.LogError("Invalid booking request");
            return BadRequest();
        }

        var newStartTime = timeSlot.GetDateTime();
        var newEndTime = timeSlot.GetDateTime().AddHours(1);
        var overlappingBookings = this.settlementBookingService.GetOverlappingBookings(newStartTime, newEndTime);
        if (overlappingBookings.Count >= MAX_SIMILTANEOUS_BOOKINGS)
        {
            _logger.LogError("Conflicting booking request");
            return Conflict("There are 4 or more time slots that already overlap");
        }

        var newBooking = settlementBookingService.BookSlot(request.Name, timeSlot);
        return Ok(new BookingSuccessModel()
        {
            BookingId = newBooking.Id
        });
    }


    /// <summary>
    /// Mostly for testing, but just so you can see what time slots are currently active
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetBookSettlements")]
    public List<SettlementBookingModel> GetBookings()
    {
        var settlements = this.settlementBookingService.GetBookings();
        // Could use automapper or something here, and realistically the view model would differ more from whats stored, but you get the idea. Return a "subset of fields/mapped" model rather than the "db entity itself"
        return settlements.Select(x => new SettlementBookingModel(x.Id, x.Name, x.StartTime)).ToList();
    }

}
