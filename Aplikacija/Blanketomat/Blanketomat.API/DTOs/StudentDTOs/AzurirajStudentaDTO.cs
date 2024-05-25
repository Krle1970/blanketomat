using Blanketomat.API.Models;

namespace Blanketomat.API.DTOs.StudentDTOs;

public class AzurirajStudentaDTO : StudentDTO
{
    public int[]? PostavljeniKomentariIds { get; set; }
    public int[]? PostavljeniOdgovoriIds { get; set; }
}