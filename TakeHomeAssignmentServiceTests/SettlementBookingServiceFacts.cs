using NUnit.Framework;
using TakeHomeAssignmentServices;
using static TakeHomeAssignmentServices.SettlementBookingService;

namespace TakeHomeAssignmentServiceTests;

public class SettlementBookingServiceFacts
{
    public List<SettlementBooking> ListReference { get; private set; }

    public SettlementBookingService Fixture;
    [SetUp]
    public void Setup()
    {

        // Note: Doing a bit of dodgy here, you'd realistically mock a db context and poke "it" to see if things have been added, instead im just raw using a reference to a list.
        ListReference = new List<SettlementBooking>();
        Fixture = new SettlementBookingService(ListReference);

    }

    /// <summary>
    /// Naming conventions don't matter what they are, so long as they are consistent :)
    /// </summary>
    [Test]
    public void BookSlot_ShouldAddToList()
    {
        var time = new BookingTimeSlot(1, 2);
        Fixture.BookSlot("Test 1", time);

        Assert.That(ListReference.Count, Is.EqualTo(1));
        Assert.That(ListReference.First().Name, Is.EqualTo("Test 1"));
        Assert.That(ListReference.First().StartTime, Is.EqualTo(time));
    }

    [Test]
    public void GetBookings_ShouldReturnAllEntities()
    {
        ListReference.Add(new SettlementBookingService.SettlementBooking(Guid.NewGuid(), "test1", new BookingTimeSlot(1, 2) ));
        ListReference.Add(new SettlementBookingService.SettlementBooking(Guid.NewGuid(), "test2", new BookingTimeSlot(1, 2)));
        ListReference.Add(new SettlementBookingService.SettlementBooking(Guid.NewGuid(), "test3", new BookingTimeSlot(1, 2)));
        var result = Fixture.GetBookings();

        Assert.That(result.Count, Is.EqualTo(3));

        Assert.That(result[0].Name, Is.EqualTo("test1"));
        Assert.That(result[1].Name, Is.EqualTo("test2"));
        Assert.That(result[2].Name, Is.EqualTo("test3"));
        // Also should test the actual fields/data/perhaps the ordering etc.
    }
    [Test]
    public void GetOverlappingBookings_ShouldReturnOverlappingEntities()
    {
        ListReference.Add(new SettlementBooking(Guid.NewGuid(), "test1", new BookingTimeSlot(1, 0)));
        ListReference.Add(new SettlementBooking(Guid.NewGuid(), "test2", new BookingTimeSlot(2, 0)));
        ListReference.Add(new SettlementBooking(Guid.NewGuid(), "test3", new BookingTimeSlot(3, 0)));

        var newStartTime = new DateTime(1, 1, 1, 2, 30, 0);
        var newEndTime = newStartTime.AddHours(1);

        var overlappingBookings = Fixture.GetOverlappingBookings(newStartTime, newEndTime);

        Assert.That(overlappingBookings.Count, Is.EqualTo(2));
        Assert.That(overlappingBookings[0].Name, Is.EqualTo("test2"));
        Assert.That(overlappingBookings[1].Name, Is.EqualTo("test3"));
    }

    [TestCase("10:30", 10, 30)]
    [TestCase("09:00", 9, 0)]
    [TestCase("16:59", 16, 59)]
    public void IsValidBookingTime_ShouldValidateTimeForValidCases(string inputTime, int expectedHour, int expectedMinute)
    {
        bool result = Fixture.IsValidBookingTime(inputTime, out var timeSlot);

        Assert.That(result, Is.True);
        Assert.That(timeSlot, Is.Not.Null);
        Assert.That(timeSlot.Hour, Is.EqualTo(expectedHour));
        Assert.That(timeSlot.Minute, Is.EqualTo(expectedMinute));
    }

    [TestCase("08:30")]
    [TestCase("10:60")]
    [TestCase("")]
    [TestCase(null)]
    [TestCase("10-30")]
    public void IsValidBookingTime_ShouldValidateTimeForInvalidCases(string inputTime)
    {

        bool result = Fixture.IsValidBookingTime(inputTime, out var timeSlot);

        Assert.That(result, Is.False);
        Assert.That(timeSlot, Is.Null);
    }

}