using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddValidatorsFromAssemblyContaining<UsuarioCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioUpdateDtoValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var usuarios = app.MapGroup("/usuarios").WithTags("Usuarios");

usuarios.MapGet("/", async (IUsuarioService service, CancellationToken ct) =>
{
    var usuarios = await service.ListarAsync(ct);
    return Results.Ok(usuarios);
})
.WithName("ListarUsuarios")
.Produces<IEnumerable<Application.DTOs.UsuarioReadDto>>(StatusCodes.Status200OK);

usuarios.MapGet("/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    if (usuario == null)
    {
        return Results.NotFound(new { message = "Usuário não encontrado" });
    }
    return Results.Ok(usuario);
})
.WithName("ObterUsuario")
.Produces<Application.DTOs.UsuarioReadDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

usuarios.MapPost("/", async (
    Application.DTOs.UsuarioCreateDto dto,
    IUsuarioService service,
    IValidator<Application.DTOs.UsuarioCreateDto> validator,
    CancellationToken ct) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(new { errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
    }

    try
    {
        var usuario = await service.CriarAsync(dto, ct);
        return Results.Created($"/usuarios/{usuario.Id}", usuario);
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("Email já cadastrado"))
    {
        return Results.Conflict(new { message = "Email já cadastrado" });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
})
.WithName("CriarUsuario")
.Produces<Application.DTOs.UsuarioReadDto>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status409Conflict)
.Produces(StatusCodes.Status500InternalServerError);

usuarios.MapPut("/{id}", async (
    int id,
    Application.DTOs.UsuarioUpdateDto dto,
    IUsuarioService service,
    IValidator<Application.DTOs.UsuarioUpdateDto> validator,
    CancellationToken ct) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(new { errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
    }

    try
    {
        var usuario = await service.AtualizarAsync(id, dto, ct);
        return Results.Ok(usuario);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { message = "Usuário não encontrado" });
    }
    catch (InvalidOperationException ex) when (ex.Message.Contains("Email já cadastrado"))
    {
        return Results.Conflict(new { message = "Email já cadastrado" });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: 500);
    }
})
.WithName("AtualizarUsuario")
.Produces<Application.DTOs.UsuarioReadDto>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status409Conflict)
.Produces(StatusCodes.Status500InternalServerError);

usuarios.MapDelete("/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var removido = await service.RemoverAsync(id, ct);
    if (!removido)
    {
        return Results.NotFound(new { message = "Usuário não encontrado" });
    }
    return Results.NoContent();
})
.WithName("RemoverUsuario")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

app.Run();
