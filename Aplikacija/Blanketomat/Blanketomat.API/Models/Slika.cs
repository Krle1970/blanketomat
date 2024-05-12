namespace Blanketomat.API.Models;

public class Slika
{
    public int Id { get; set; }
    public required string Putanja { get; set; }
    public List<Blanket>? Blanketi { get; set; }
    public Komentar? Komentar { get; set; }
}