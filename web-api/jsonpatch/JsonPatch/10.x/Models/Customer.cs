using System.Text.Json.Serialization;

namespace App.Models;

public class Customer
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Address? Address { get; set; }
    public string? Email { get; set; }
    public List<PhoneNumber>? PhoneNumbers { get; set; }

    public Customer()
    {
        Id = Guid.NewGuid().ToString();
    }
}

public class PhoneNumber
{
    public PhoneNumberType Type { get; set; }
    public string? Number { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<PhoneNumberType>))]
public enum PhoneNumberType
{
    Home,
    Work,
    Mobile,
    Other
}
