
namespace BucketCompareAlgorithm;

/// <summary>
/// Represents event data with a timestamp.
/// </summary>
public class EventData
{
    private readonly DateTime _timeStamp;

    /// <summary>
    /// Initializes a new instance of the EventData class.
    /// </summary>
    /// <param name="timeStamp">The timestamp of the event.</param>
    public EventData(DateTime timeStamp)
    {
        _timeStamp = timeStamp;
    }

    /// <summary>
    /// Gets the timestamp of the event.
    /// </summary>
    public DateTime TimeStamp => _timeStamp;
}