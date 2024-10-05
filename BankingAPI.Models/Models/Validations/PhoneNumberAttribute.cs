using System.ComponentModel.DataAnnotations;
using PhoneNumbers;

public class PhoneNumberAttribute : ValidationAttribute
{
    private readonly string _region;

    public PhoneNumberAttribute(string region)
    {
        _region = region;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult("Phone Number is required.");
        }
        // using the library libphonenumber-csharp to validate the phone number with the region country code...
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        try
        {
            var phoneNumber = phoneNumberUtil.Parse(value.ToString(), _region);
            if (phoneNumberUtil.IsValidNumber(phoneNumber))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Invalid phone number format.");
            }
        }
        catch (NumberParseException)
        {
            return new ValidationResult("Invalid phone number format.");
        }
    }
}
