using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.StudentDTOs;

public class AzurirajStudentaDTO : StudentDTO
{
    public List<Komentar>? PostavljeniKomentari { get; set; }
    public List<Odgovor>? PostavljeniOdgovori { get; set; }
}