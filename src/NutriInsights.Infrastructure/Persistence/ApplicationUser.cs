using Microsoft.AspNetCore.Identity;
using NutriInsights.Domain.Usuario;

namespace NutriInsights.Infrastructure.Persistence;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser<Guid>
{
    public decimal? Peso { get; set; }
    public decimal? Estatura { get; set; }
    public Sexo? Sexo { get; set; }
    public DateOnly? FechaNacimiento { get; set; }
    public NivelActividad? NivelActividad { get; set; }
}