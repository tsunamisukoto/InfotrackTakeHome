
public record BookingTimeSlot(int Hour, int Minute)
{
    public DateTime GetDateTime()
    {
        return new DateTime(1, 1, 1, Hour, Minute, 0);
    }
}