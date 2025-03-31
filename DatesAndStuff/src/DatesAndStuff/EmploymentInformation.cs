namespace DatesAndStuff;

public class EmploymentInformation
{
    public EmploymentInformation(double salary, Employer e)
    {
        this.Salary = salary;
        this.Employer = e;
    }

    public double Salary { get; private set; }

    public Employer Employer { get; private set; }

    public void IncreaseSalary(double percentage)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(percentage, -10);

        this.Salary = this.Salary * (1 + (percentage / 100));
    }
}
