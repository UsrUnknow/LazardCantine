using LazardCantine.Models.Enums;

namespace LazardCantine.Models;

public class Client
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public ClientType ClientType { get; set; }
}