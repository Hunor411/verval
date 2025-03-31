namespace DatesAndStuff;

public class LocalTaxData
{
    public LocalTaxData(string UAT) => this.UAT = UAT;

    /// <summary>
    ///     Administrative territorial unit identifier.
    /// </summary>
    public string UAT { get; private set; }

    public List<TaxItem> TaxItems { get; set; }

    public double DiscountPercentage { get; set; }

    public static double YearlyTax => 0;
}
