namespace DatesAndStuff;

using System.Diagnostics;
using System.Globalization;

/// <summary>
///     Represents the time in the simulation.
/// </summary>
[Serializable]
[DebuggerDisplay("{ToString()}")]
public readonly struct SimulationTime : IEquatable<SimulationTime>, IComparable<SimulationTime>
{
    public static readonly SimulationTime MaxValue = new(DateTime.MaxValue);
    public static readonly SimulationTime MinValue = new(0);
    public static readonly TimeSpan OneMillisecond = TimeSpan.FromMilliseconds(1);

    /// <summary>
    ///     Used to convert logical counter value into physical time.
    /// </summary>
    private const long LogicalDigitMask = 10000;

    [DebuggerStepThrough]
    public SimulationTime(DateTime value) => this.LogicalTicks = value.Ticks / LogicalDigitMask * LogicalDigitMask;

    public SimulationTime(int year, int month, int day)
        : this(new DateTime(year, month, day))
    {
    }

    public SimulationTime(int year, int month, int day, int hour, int minute, int second)
        : this(new DateTime(year, month, day, hour, minute, second))
    {
    }

    public SimulationTime(string logicalTickStr) =>
        this.LogicalTicks = Convert.ToInt64(logicalTickStr, CultureInfo.InvariantCulture);

    /// <summary>
    ///     Strictly for internal use only.
    /// </summary>
    private SimulationTime(long rawLogicalTicks) => this.LogicalTicks = rawLogicalTicks;

    public static SimulationTime Now => new(DateTime.Now);

    /// <summary>
    ///     Ticks are related to the actual physical time in the simulation, it has an accuracy of 1 ms.
    /// </summary>
    public long TotalMilliseconds => this.LogicalTicks / LogicalDigitMask;

    public SimulationTime NextMillisec => new((this.TotalMilliseconds + 1) * LogicalDigitMask);

    public SimulationTime PreviousMillisec => new((this.TotalMilliseconds - 1) * LogicalDigitMask);

    public SimulationTime PreviousLogicalTick => new(this.LogicalTicks - 1);

    public SimulationTime NextLogicalTick => new(this.LogicalTicks + 1);

    /// <summary>
    ///     The actual logical time in the simulation.
    ///     The value is a logical counter, but the first digits correspond to an absolute real-world time.
    ///     Last 4 digits are used as a logical counter the remaining, and
    ///     the remaining digits provide an accuracy of 1 ms of the physical clock.
    ///     Logical time is not accessible directly, but can be queried with special functions used by the simulation engine
    ///     and the loggers.
    /// </summary>
    public long LogicalTicks { get; }

    #region operators

    [DebuggerStepThrough]
    public static TimeSpan operator -(SimulationTime a, SimulationTime b) =>
        // using TimeSpan.FromMilliseconds(a.TotalMilliseconds - b.TotalMilliseconds) would be more logical but it does not work consistently,
        // as TimeSpan.FromMilliseconds (315537897599999).Ticks returns 3155378975999989760 instead of 3155378975999990000.
        TimeSpan.FromTicks((a.TotalMilliseconds - b.TotalMilliseconds) * LogicalDigitMask);

    [DebuggerStepThrough]
    public static SimulationTime operator -(SimulationTime a, TimeSpan tspan)
    {
        if (tspan > TimeSpan.Zero && tspan < OneMillisecond)
        {
            return a.PreviousLogicalTick;
        }

        // timespan is stripped to have ms accuracy (same as SimulationTime)
        return new SimulationTime((a.TotalMilliseconds - (tspan.Ticks / LogicalDigitMask)) * LogicalDigitMask);
    }

    [DebuggerStepThrough]
    public static SimulationTime operator +(SimulationTime a, TimeSpan tspan)
    {
        if (tspan == TimeSpan.MaxValue)
        {
            return MaxValue;
        }

        if (tspan > TimeSpan.Zero && tspan < OneMillisecond)
        {
            return a.NextLogicalTick;
        }

        checked
        {
            try
            {
                // timespan is stripped to have ms accuracy (same as SimulationTime)
                return new SimulationTime((a.TotalMilliseconds + (tspan.Ticks / LogicalDigitMask)) * LogicalDigitMask);
            }
            catch (OverflowException)
            {
                return MaxValue;
            }
        }
    }

    [DebuggerStepThrough]
    public static bool operator >(SimulationTime a, SimulationTime b) => a.TotalMilliseconds > b.TotalMilliseconds;

    [DebuggerStepThrough]
    public static bool operator <(SimulationTime a, SimulationTime b) => a.TotalMilliseconds < b.TotalMilliseconds;

    [DebuggerStepThrough]
    public static bool operator >=(SimulationTime a, SimulationTime b) => a.TotalMilliseconds >= b.TotalMilliseconds;

    [DebuggerStepThrough]
    public static bool operator <=(SimulationTime a, SimulationTime b) => a.TotalMilliseconds <= b.TotalMilliseconds;

    [DebuggerStepThrough]
    public static bool operator ==(SimulationTime a, SimulationTime b) => a.TotalMilliseconds == b.TotalMilliseconds;

    [DebuggerStepThrough]
    public static bool operator !=(SimulationTime a, SimulationTime b) => a.TotalMilliseconds != b.TotalMilliseconds;

    [DebuggerStepThrough]
    public static SimulationTime Min(params SimulationTime[] p) => Min(p.AsSpan());

    [DebuggerStepThrough]
    public static SimulationTime Min(Span<SimulationTime> p)
    {
        if (p.Length == 0)
        {
            throw new ArgumentException("At least one element is needed for minimum calculation.");
        }

        var min = p[0];
        for (var k = 1; k < p.Length; k++)
        {
            if (p[k].LogicalTicks < min.LogicalTicks)
            {
                min = p[k];
            }
        }

        return min;
    }

    [DebuggerStepThrough]
    public static SimulationTime Max(SimulationTime a, SimulationTime b) => a > b ? a : b;

    #endregion

    public static long MilliSecondsFromLogicalTicks(long tick) => tick / LogicalDigitMask;

    public static SimulationTime FromLogicalTicks(long rawLogicalTicks) => new(rawLogicalTicks);

    #region IEquatable, IComparable

    [DebuggerStepThrough]
    public override int GetHashCode() => this.TotalMilliseconds.GetHashCode();

    [DebuggerStepThrough]
    public override bool Equals(object obj)
    {
        if (!(obj is SimulationTime))
        {
            return false;
        }

        var other = (SimulationTime)obj;
        return this.TotalMilliseconds == other.TotalMilliseconds;
    }

    [DebuggerStepThrough]
    public bool Equals(SimulationTime other) => this.TotalMilliseconds == other.TotalMilliseconds;

    [DebuggerStepThrough]
    public int CompareTo(SimulationTime other) => this.TotalMilliseconds.CompareTo(other.TotalMilliseconds);

    #endregion

    [DebuggerStepThrough]
    public TimeSpan ToTimeSpan()
    {
        var physicalTicks = this.TotalMilliseconds * LogicalDigitMask;
        return TimeSpan.FromTicks(physicalTicks);
    }

    [DebuggerStepThrough]
    public DateTime ToAbsoluteDateTime()
    {
        var physicalTicks = this.TotalMilliseconds * LogicalDigitMask;
        return new DateTime(physicalTicks);
    }

    [DebuggerStepThrough]
    public DateOnly ToAbsoluteDateOnly()
    {
        var physicalTicks = this.TotalMilliseconds * LogicalDigitMask;
        return DateOnly.FromDateTime(new DateTime(physicalTicks));
    }

    [DebuggerStepThrough]
    public SimulationTime LastMidnight()
    {
        var date = this.ToAbsoluteDateTime();
        date -= date.TimeOfDay;

        return new SimulationTime(date);
    }

    [DebuggerStepThrough]
    public override string ToString() => this.ToAbsoluteDateTime().ToIsoStringFast();

    [DebuggerStepThrough]
    public SimulationTime AddMilliseconds(double milliSeconds) => this + TimeSpan.FromMilliseconds(milliSeconds);

    [DebuggerStepThrough]
    public SimulationTime AddSeconds(double seconds) => this + TimeSpan.FromSeconds(seconds);

    [DebuggerStepThrough]
    public SimulationTime AddMinutes(double minutes) => this + TimeSpan.FromMinutes(minutes);

    [DebuggerStepThrough]
    public SimulationTime AddHours(double hours) => this + TimeSpan.FromHours(hours);

    [DebuggerStepThrough]
    public SimulationTime AddDays(double days) => this + TimeSpan.FromDays(days);

    [DebuggerStepThrough]
    public SimulationTime AddTimeSpan(TimeSpan timeSpan) => this + timeSpan;
}
