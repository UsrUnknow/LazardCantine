using System.Text.Json.Serialization;

namespace LazardCantine.DTOs;

using Models.Enums;

public class ClientDto
{
    public string Name { get; set; } = string.Empty;
    public ClientType Type { get; set; }
    public decimal Balance { get; set; }
}