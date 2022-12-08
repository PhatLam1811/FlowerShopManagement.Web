namespace FlowerShopManagement.Core.Enums;

public class Gender
{
    public string Value { get; private set; }

    private Gender(string value) => Value = value;

    public static Gender Male { get => new Gender("Male"); }
    public static Gender Female { get => new Gender("Female"); }
    public static Gender Other { get => new Gender("Other"); }
}
