using notif_be.Models;

namespace notif_be_tests;

public class EventTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t\r\n")]
    public void Event_rejects_a_blank_body(string? body)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Event(17, "breaking-news", "Important news", body!));

        Assert.Equal("body", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" \t\r\n")]
    public void Event_rejects_a_blank_title(string? title)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Event(17, "breaking-news", title!, "Full report"));

        Assert.Equal("title", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("unknown-event")]
    [InlineData("Breaking-News")]
    public void Event_rejects_an_unknown_event_type(string? eventType)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Event(17, eventType!, "Important news", "Full report"));

        Assert.Equal("eventType", exception.ParamName);
    }

    [Theory]
    [InlineData("breaking-news")]
    [InlineData("market-movement")]
    [InlineData("natural-disaster")]
    public void Event_retains_supplied_fields(string eventType)
    {
        var worldEvent = new Event(17, eventType, "  Important news  ", "  Full report\n ");

        IModel model = worldEvent;
        Assert.Equal(17, model.Id);
        Assert.Equal(eventType, worldEvent.EventType);
        Assert.Equal("  Important news  ", worldEvent.Title);
        Assert.Equal("  Full report\n ", worldEvent.Body);
    }
}
