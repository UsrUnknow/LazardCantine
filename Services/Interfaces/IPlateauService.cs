using LazardCantine.DTOs;
using LazardCantine.Models;

namespace LazardCantine.Services
{
    public interface IPlateauService
    {
        (bool Succes, string Message, Ticket? Ticket) PayerPlateau(Guid clientId, PlateauDto plateauDto);
    }
}