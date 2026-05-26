using CampusInter.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (!await context.Profesores.AnyAsync())
        {
            var profesores = new List<Profesor>
            {
                new("Carlos", "Andrés", "Gómez", "Ruiz"),
                new("Laura", "Marcela", "Torres", "Díaz"),
                new("Andrés", "Felipe", "Moreno", "Castro"),
                new("Diana", "Carolina", "Pérez", "Rojas"),
                new("Miguel", "Ángel", "Ramírez", "Soto")
            };

            await context.Profesores.AddRangeAsync(profesores);
            await context.SaveChangesAsync();
        }

        if (!await context.Materias.AnyAsync())
        {
            var profesores = await context.Profesores
                .OrderBy(profesor => profesor.ProfesorId)
                .ToListAsync();

            if (profesores.Count < 5)
                return;

            var materias = new List<Materia>
            {
                new("Matemáticas", profesores[0].ProfesorId, Materia.CreditosPorDefecto),
                new("Física", profesores[0].ProfesorId, Materia.CreditosPorDefecto),

                new("Química", profesores[1].ProfesorId, Materia.CreditosPorDefecto),
                new("Biología", profesores[1].ProfesorId, Materia.CreditosPorDefecto),

                new("Historia", profesores[2].ProfesorId, Materia.CreditosPorDefecto),
                new("Geografía", profesores[2].ProfesorId, Materia.CreditosPorDefecto),

                new("Inglés", profesores[3].ProfesorId, Materia.CreditosPorDefecto),
                new("Literatura", profesores[3].ProfesorId, Materia.CreditosPorDefecto),

                new("Programación", profesores[4].ProfesorId, Materia.CreditosPorDefecto),
                new("Bases de Datos", profesores[4].ProfesorId, Materia.CreditosPorDefecto)
            };

            await context.Materias.AddRangeAsync(materias);
            await context.SaveChangesAsync();
        }
    }
}
