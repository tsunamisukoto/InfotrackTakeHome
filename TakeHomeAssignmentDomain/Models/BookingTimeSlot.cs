
public class BookingTimeSlot
{
    public int Hour { get; set; }
    public int Minute { get; set; }

    public DateTime GetDateTime()
    {
        return new DateTime(1, 1, 1, Hour, Minute, 0);
    }
}