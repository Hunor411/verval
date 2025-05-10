namespace DatesAndStuff.Mobile;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

public class PersonViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    private readonly SalaryIncreaseData salaryIncreaseData = new();

    public PersonViewModel() =>
        this.ValidateProperty(nameof(this.SalaryIncreasePercentage), nameof(this.SalaryIncreasePercentage));

    public Person Person { get; set; } = new("Test Kálmán",
        new EmploymentInformation(5000, new Employer("RO12312312", "Valami utca sok", "Gipsz Jakab", null)),
        new UselessPaymentService(),
        new LocalTaxData("UAT Gazdag varos"),
        new FoodPreferenceParams { CanEatChocolate = true, CanEatGluten = false });

    public string SalaryIncreasePercentage
    {
        get => this.salaryIncreaseData.SalaryIncreasePercentage?.ToString(CultureInfo.InvariantCulture);
        set
        {
            if (double.TryParse(value, out var parsed))
            {
                this.salaryIncreaseData.SalaryIncreasePercentage = parsed;
            }
            else
            {
                this.salaryIncreaseData.SalaryIncreasePercentage = null;
            }

            this.ValidateProperty(nameof(this.SalaryIncreasePercentage));
            this.OnPropertyChanged();
        }
    }

    public string SalaryIncreasePercentageValidationMessage =>
        this.errors.TryGetValue(nameof(this.SalaryIncreasePercentage), out var errors_)
            ? string.Join(Environment.NewLine, errors_)
            : string.Empty;

    public ICommand SubmitCommand => new Command(this.OnSubmit);

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnSubmit()
    {
        if (!this.HasValidationErrors)
        {
            this.Person.IncreaseSalary(this.salaryIncreaseData.SalaryIncreasePercentage.Value);
            this.OnPropertyChanged(nameof(this.Person));
        }
    }

    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public class SalaryIncreaseData
    {
        [Required(ErrorMessage = "Percentage should be specified")]
        [Range(-10d, double.MaxValue, ErrorMessage = "The specified percentag should be between -10 and infinity.")]
        public double? SalaryIncreasePercentage { get; set; }
    }

    #region Validation

    private readonly Dictionary<string, List<string>> errors = new();

    public bool HasValidationErrors => this.errors.Count != 0;

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (propertyName != null && this.errors.TryGetValue(propertyName, out var errors))
        {
            return errors;
        }

        return null;
    }

    public bool HasErrors => this.HasValidationErrors;

    private void ValidateProperty(object value, [CallerMemberName] string propertyName = "")
    {
        this.errors.Remove(propertyName);

        var context = new ValidationContext(this.salaryIncreaseData) { MemberName = propertyName };
        var propertyInfo = typeof(SalaryIncreaseData).GetProperty(propertyName);
        var valueToValidate = propertyInfo?.GetValue(this.salaryIncreaseData);

        var results = new List<ValidationResult>();
        if (!Validator.TryValidateProperty(valueToValidate, context, results))
        {
            this.errors[propertyName] = results.Select(r => r.ErrorMessage).ToList();
        }

        // the validation message might have changed and the state of errors
        this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        this.OnPropertyChanged(propertyName + "ValidationMessage");
        this.OnPropertyChanged(nameof(this.HasValidationErrors));
    }

    #endregion
}
