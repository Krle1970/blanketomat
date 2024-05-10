namespace Blanketomat.API.DTOs;

public class PaginationResponseDTO<T>
{
    public List<T> Podaci { get; set; } = new List<T>();
    public int BrojStranica { get; set; }
    public int TrenutnaStranica { get; set; }
}