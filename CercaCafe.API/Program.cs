using CercaCafe.API.Middlewares;
using CercaCafe.BLL.Services;
using CercaCafe.DAL;
using CercaCafe.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar política de CORS para permitir peticiones desde Angular (Web) y React Native (Móvil / Expo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebAndMobile",
        policy => policy.SetIsOriginAllowed(_ => true) 
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

// 1. Agregar servicios de controladores 
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// 2. REGISTRAR EL DBCONTEXT 
builder.Services.AddDbContext<CercaCafeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CercaCafeConnection")));

// 3. REGISTRAR SERVICIOS DE LA CAPA DE NEGOCIO (BLL) Y AUDITORÍA
builder.Services.AddScoped<CafeteriaService>();
builder.Services.AddScoped<HorarioService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<ResenaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AuditoriaService>();
builder.Services.AddScoped<FavoritoService>();

// 4. CONFIGURACIÓN DE JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// 5. CONFIGURACIÓN DE SWAGGER CON SOPORTE PARA JWT BEARER 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CercaCafe.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese la palabra 'Bearer' seguida de un espacio y su token JWT.\n\nEjemplo: \"Bearer eyJhbGciOiJIUzI1Ni...\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// 1. MIDDLEWARE GLOBAL DE MANEJO DE ERRORES
app.UseMiddleware<ErrorHandlingMiddleware>();

// Habilitar Swagger siempre para pruebas en servidor
app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Habilitar archivos estáticos (imágenes en wwwroot)
app.UseStaticFiles();

// Middleware de CORS para Web y App Móvil
app.UseCors("AllowWebAndMobile");

app.UseAuthentication(); // Valida la identidad (JWT)
app.UseAuthorization();  // Valida permisos y roles

app.MapControllers();

app.Run();