using System.ComponentModel.DataAnnotations;
using BankingAPI.Models;

public class MinAccountCount : ValidationAttribute
{
    private readonly int _minCount;

    public MinAccountCount(int minCount)
    {
        this._minCount = minCount;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var collection = value as ICollection<Account>;

        if (collection == null || collection.Count < _minCount)
        {
            return new ValidationResult($"At least {_minCount} account(s) are required.");
        }

        return ValidationResult.Success;
    }
}
