namespace DatesAndStuff;

public class Employer
{
    private readonly List<int> activityDomains;
    private readonly string address;
    private readonly string ownername;

    private readonly string taxId;

    public Employer(
        string taxId,
        string address,
        string ownername,
        List<int> activityDomains)
    {
        this.taxId = taxId;
        this.address = address;
        this.ownername = ownername;
        this.activityDomains = activityDomains;
    }

    internal Employer Clone() => new(this.taxId, this.address, this.ownername, new List<int>(this.activityDomains));
}
