using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DatesAndStuff.Tests")]

namespace DatesAndStuff;

public class Person
{
    public const double SubscriptionFee = 500;

    public bool CanEatChocolate { get; }

    public bool CanEatEgg { get; }

    public bool CanEatGluten { get; }

    public bool CanEatLactose { get; }

    private bool married;

    public Person(string name, EmploymentInformation employment, IPaymentService paymentService, LocalTaxData taxData,
        FoodPreferenceParams foodPreferenceParams)
    {
        this.Name = name;
        this.married = false;
        this.Employment = employment;
        this.PreferredPayment = paymentService;
        this.TaxData = taxData;
        this.CanEatGluten = foodPreferenceParams.CanEatGluten;
        this.CanEatLactose = foodPreferenceParams.CanEatLactose;
        this.CanEatEgg = foodPreferenceParams.CanEatEgg;
        this.CanEatChocolate = foodPreferenceParams.CanEatChocolate;
    }

    public string Name { get; private set; }

    public double Salary => this.Employment.Salary;

    public EmploymentInformation Employment { get; }

    public IPaymentService PreferredPayment { get; }

    public LocalTaxData TaxData { get; }

    public void GotMarried(string newName)
    {
        if (this.married)
        {
            throw new InvalidOperationException("Poligamy not yet supported.");
        }

        this.married = true;
        this.Name = newName;
    }

    public void IncreaseSalary(double percentage) => this.Employment.IncreaseSalary(percentage);

    public static Person Clone(Person p) =>
        new(p.Name,
            new EmploymentInformation(p.Employment.Salary, p.Employment.Employer.Clone()),
            p.PreferredPayment,
            p.TaxData,
            new FoodPreferenceParams
            {
                CanEatGluten = p.CanEatGluten,
                CanEatEgg = p.CanEatEgg,
                CanEatChocolate = p.CanEatChocolate,
                CanEatLactose = p.CanEatLactose
            }
        );

    public bool PerformSubsriptionPayment()
    {
        this.PreferredPayment.StartPayment();

        var currnetBalance = this.PreferredPayment.Balance;
        if (currnetBalance < SubscriptionFee)
        {
            this.PreferredPayment.CancelPayment();
            return false;
        }

        this.PreferredPayment.SpecifyAmount(SubscriptionFee);
        this.PreferredPayment.ConfirmPayment();
        return true;
    }
}
