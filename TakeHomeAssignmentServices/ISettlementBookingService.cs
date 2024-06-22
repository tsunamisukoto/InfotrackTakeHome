namespace TakeHomeAssignmentServices;

public interface ISettlementBookingService
{
    SettlementBookingService.SettlementBooking BookSlot(string name, BookingTimeSlot timeSlot);
    List<SettlementBookingService.SettlementBooking> GetBookings();
    List<SettlementBookingService.SettlementBooking> GetOverlappingBookings(DateTime newStartTime, DateTime newEndTime);
    bool IsValidBookingTime(string bookingTime, out BookingTimeSlot timeSlot);
}