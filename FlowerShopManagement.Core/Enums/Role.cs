namespace FlowerShopManagement.Core.Enums;

public class Role
{
    public string Value { get; private set; }

    private Role(string value) => Value = value;

    public static Role Admin { get => new Role("Admin"); }
    public static Role Staff { get => new Role("Staff"); }
    public static Role Customer { get => new Role("Customer"); }
}
