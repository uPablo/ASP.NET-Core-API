using Microsoft.EntityFrameworkCore;
using asp.Data;
using asp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Configura a chave de segurança de 256 bits (32 caracteres)
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("BDsRJ3ypxC10vRaGn8/2Zbj11k89zJDU5UqtJl5BprA="));

// Configura o banco de dados SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=users.db"));

// Configura autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourdomain.com",
            ValidAudience = "yourdomain.com",
            IssuerSigningKey = securityKey,
        };
    });

// Adiciona o serviço de autorização
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.MapGet("/", () => "ASP.NET Core API");

// Endpoint para criar um novo usuário
app.MapPost("/api/users", async (AppDbContext db, User user) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", user);
}).RequireAuthorization();

// Endpoint para listar todos os usuários, com autorização
app.MapGet("/api/users", async (AppDbContext db) =>
    await db.Users.ToListAsync()
).RequireAuthorization();

// Endpoint para obter um usuário por ID
app.MapGet("/api/users/{id}", async (AppDbContext db, int id) =>
{
    var user = await db.Users.FindAsync(id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
}).RequireAuthorization();

// Endpoint para atualizar um usuário
app.MapPut("/api/users/{id}", async (AppDbContext db, int id, User updatedUser) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.Name = updatedUser.Name;
    user.Age = updatedUser.Age;

    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

// Endpoint para remover um usuário
app.MapDelete("/api/users/{id}", async (AppDbContext db, int id) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Ok(user);
}).RequireAuthorization();

// Endpoint de login para gerar um token JWT
app.MapPost("/api/login", (UserLogin login) =>
{
    if (login.Username == "admin" && login.Password == "password")
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Username)
        };

        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourdomain.com",
            audience: "yourdomain.com",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    return Results.Unauthorized();
});

app.Run();