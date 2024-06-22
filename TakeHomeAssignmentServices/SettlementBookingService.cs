namespace TakeHomeAssignmentServices;
public class SettlementBookingService : ISettlementBookingService
{

    /// <summary>
    /// Storing it in static memory object. This is obviously not how you'd do it in proper solution.
    /// You'd have an EF Db context set up with your entities being saved to a permanent store.
    /// </summary>
    private static List<SettlementBooking> _timeSlots = new List<SettlementBooking>();

    public SettlementBookingService()
    {
    }

    /// <summary>
    /// For unit testing purposes. In an ideal world you'd use/mock a db context
    /// </summary>
    /// <param name="bookings"></param>
    public SettlementBookingService(List<SettlementBooking> bookings)
    {
        _timeSlots = bookings;
    }
    /// <summary>
    /// Similar to above, I probably wouldn't use a record for the entity in prod, but (last I checked EF doesn't play nice with them) but it keeps me honest and makes sure I define all usages for now.
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="StartTime"></param>
    public record SettlementBooking(Guid Id, string Name, BookingTimeSlot StartTime);
    /// <summary>
    /// Could also do this with a input model, depends how many params/fields
    /// </summary>
    /// <param name="name"></param>
    /// <param name="timeSlot"></param>
    /// <returns></returns>
    public SettlementBooking BookSlot(string name, BookingTimeSlot timeSlot)
    {
        var newBooking = new SettlementBooking(Guid.NewGuid(), name, timeSlot);
        _timeSlots.Add(newBooking);
        return newBooking;
    }

    public List<SettlementBooking> GetOverlappingBookings(DateTime newStartTime, DateTime newEndTime)
    {
        return (from booking in _timeSlots
                let existingStartTime = booking.StartTime.GetDateTime()
                let existingEndTime = existingStartTime.AddHours(1)
                where newStartTime < existingEndTime && newEndTime > existingStartTime
                select booking).ToList();
    }
    /// <summary>
    /// You'd probably add paging and filtering to this guy but... shrug :P
    /// </summary>
    /// <returns></returns>
    public List<SettlementBooking> GetBookings()
    {
        return _timeSlots;
    }

    public bool IsValidBookingTime(string bookingTime, out BookingTimeSlot timeSlot)
    {
        timeSlot = null;
        if (string.IsNullOrWhiteSpace(bookingTime))
        {
            return false;
        }

        // Use TryParse to validate and parse the time components
        string[] timeParts = bookingTime.Split(':');
        if (timeParts.Length == 2 &&
            int.TryParse(timeParts[0], out int hour) &&
            int.TryParse(timeParts[1], out int minute) &&
            hour >= 9 && hour <= 16 &&
            minute >= 0 && minute <= 59)
        {
            timeSlot = new BookingTimeSlot(hour, minute);
            return true;
        }

        return false;
    }
}
