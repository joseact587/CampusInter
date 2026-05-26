using CampusInter.Application.Interfaces.Security;
using CampusInter.Domain.Entidades;
using CampusInter.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CampusInter.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        await SeedProfesoresAsync(context);
        await SeedMateriasAsync(context);
        await SeedUsuariosAsync(context, passwordHasher);
        await SeedEstudiantesAsync(context);
        await SeedInscripcionesAsync(context);
    }

    private static async Task SeedProfesoresAsync(ApplicationDbContext context)
    {
        if (await context.Profesores.AnyAsync())
            return;

        var profesores = new List<Profesor>
        {
            new("Carlos", "Andres", "Gomez", "Ruiz"),
            new("Laura", "Marcela", "Torres", "Diaz"),
            new("Andres", "Felipe", "Moreno", "Castro"),
            new("Diana", "Carolina", "Perez", "Rojas"),
            new("Miguel", "Angel", "Ramirez", "Soto")
        };

        await context.Profesores.AddRangeAsync(profesores);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMateriasAsync(ApplicationDbContext context)
    {
        if (await context.Materias.AnyAsync())
            return;

        var profesores = await context.Profesores
            .OrderBy(profesor => profesor.ProfesorId)
            .ToListAsync();

        if (profesores.Count < 5)
            return;

        var materias = new List<Materia>
        {
            new("Matematicas", profesores[0].ProfesorId, Materia.CreditosPorDefecto),
            new("Fisica", profesores[0].ProfesorId, Materia.CreditosPorDefecto),

            new("Quimica", profesores[1].ProfesorId, Materia.CreditosPorDefecto),
            new("Biologia", profesores[1].ProfesorId, Materia.CreditosPorDefecto),

            new("Historia", profesores[2].ProfesorId, Materia.CreditosPorDefecto),
            new("Geografia", profesores[2].ProfesorId, Materia.CreditosPorDefecto),

            new("Ingles", profesores[3].ProfesorId, Materia.CreditosPorDefecto),
            new("Literatura", profesores[3].ProfesorId, Materia.CreditosPorDefecto),

            new("Programacion", profesores[4].ProfesorId, Materia.CreditosPorDefecto),
            new("Bases de Datos", profesores[4].ProfesorId, Materia.CreditosPorDefecto)
        };

        await context.Materias.AddRangeAsync(materias);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsuariosAsync(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher)
    {
        if (await context.Usuarios.AnyAsync())
            return;

        var usuarios = DatosEstudiantes()
            .Select(dato =>
            {
                var passwordHash = passwordHasher.Hash("Prueba123");

                return new Usuario(
                    dato.Correo,
                    passwordHash,
                    RolUsuario.Estudiante);
            })
            .ToList();

        await context.Usuarios.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();
    }

    private static async Task SeedEstudiantesAsync(ApplicationDbContext context)
    {
        if (await context.Estudiantes.AnyAsync())
            return;

        var usuariosPorCorreo = await context.Usuarios
            .ToDictionaryAsync(usuario => usuario.Correo, StringComparer.OrdinalIgnoreCase);

        var estudiantes = DatosEstudiantes()
            .Where(dato => usuariosPorCorreo.ContainsKey(dato.Correo))
            .Select(dato =>
            {
                var usuario = usuariosPorCorreo[dato.Correo];

                return new Estudiante(
                    usuario.UsuarioId,
                    dato.PrimerNombre,
                    dato.SegundoNombre,
                    dato.PrimerApellido,
                    dato.SegundoApellido,
                    dato.Documento);
            })
            .ToList();

        await context.Estudiantes.AddRangeAsync(estudiantes);
        await context.SaveChangesAsync();
    }

    private static async Task SeedInscripcionesAsync(ApplicationDbContext context)
    {
        if (await context.Inscripciones.AnyAsync())
            return;

        var estudiantes = await context.Estudiantes
            .OrderBy(estudiante => estudiante.EstudianteId)
            .ToListAsync();

        if (estudiantes.Count < 15)
            return;

        var materias = await context.Materias
            .Include(materia => materia.Profesor)
            .ToListAsync();

        var materiasPorNombre = materias.ToDictionary(
            materia => materia.Nombre,
            StringComparer.OrdinalIgnoreCase);

        var combinaciones = new[]
        {
            new[] { "Matematicas", "Quimica", "Historia" },
            new[] { "Matematicas", "Biologia", "Ingles" },
            new[] { "Matematicas", "Quimica", "Programacion" },
            new[] { "Fisica", "Historia", "Ingles" },
            new[] { "Quimica", "Geografia", "Bases de Datos" },
            new[] { "Matematicas", "Literatura", "Programacion" },
            new[] { "Biologia", "Historia", "Ingles" },
            new[] { "Fisica", "Quimica", "Programacion" },
            new[] { "Matematicas", "Geografia", "Literatura" },
            new[] { "Biologia", "Historia", "Bases de Datos" },
            new[] { "Fisica", "Quimica", "Ingles" },
            new[] { "Matematicas", "Historia", "Programacion" },
            new[] { "Quimica", "Literatura", "Bases de Datos" },
            new[] { "Fisica", "Biologia", "Geografia" },
            new[] { "Matematicas", "Ingles", "Bases de Datos" }
        };

        var inscripciones = estudiantes
            .Take(15)
            .Select((estudiante, index) =>
            {
                var materiasSeleccionadas = combinaciones[index]
                    .Select(nombre => materiasPorNombre[nombre])
                    .ToList();

                return new Inscripcion(estudiante.EstudianteId, materiasSeleccionadas);
            })
            .ToList();

        await context.Inscripciones.AddRangeAsync(inscripciones);
        await context.SaveChangesAsync();
    }

    private static IReadOnlyList<EstudianteSeedData> DatosEstudiantes()
    {
        return
        [
            new("juan.perez@test.com", "Juan", "David", "Perez", "Gomez", "1001001001"),
            new("ana.rodriguez@test.com", "Ana", "Maria", "Rodriguez", "Lopez", "1001001002"),
            new("luis.garcia@test.com", "Luis", "Fernando", "Garcia", "Torres", "1001001003"),
            new("maria.gomez@test.com", "Maria", "Camila", "Gomez", "Rojas", "1001001004"),
            new("carlos.martinez@test.com", "Carlos", "Andres", "Martinez", "Diaz", "1001001005"),
            new("laura.torres@test.com", "Laura", "Marcela", "Torres", "Castro", "1001001006"),
            new("andres.moreno@test.com", "Andres", "Felipe", "Moreno", "Ruiz", "1001001007"),
            new("diana.perez@test.com", "Diana", "Carolina", "Perez", "Soto", "1001001008"),
            new("miguel.ramirez@test.com", "Miguel", "Angel", "Ramirez", "Vargas", "1001001009"),
            new("sofia.castro@test.com", "Sofia", "Alejandra", "Castro", "Moreno", "1001001010"),
            new("camila.rojas@test.com", "Camila", "Andrea", "Rojas", "Perez", "1001001011"),
            new("felipe.diaz@test.com", "Felipe", "Esteban", "Diaz", "Gomez", "1001001012"),
            new("valentina.soto@test.com", "Valentina", "Isabel", "Soto", "Ramirez", "1001001013"),
            new("sebastian.lopez@test.com", "Sebastian", "Nicolas", "Lopez", "Martinez", "1001001014"),
            new("natalia.vargas@test.com", "Natalia", "Paola", "Vargas", "Torres", "1001001015")
        ];
    }

    private sealed record EstudianteSeedData(
        string Correo,
        string PrimerNombre,
        string? SegundoNombre,
        string PrimerApellido,
        string? SegundoApellido,
        string Documento);
}
