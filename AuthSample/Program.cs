using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuthSample.Models;
using AuthSample.Repositories;
using AuthSample.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Config.Secret)),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapPost("login", ([FromBody] User model) =>
{
	var user = UserRepository.Get(model.UserName, model.Password);

	if (user == null) return Results.NotFound(new { message = "invalid username or password" });

	var token = TokenService.GenerateToken(user);

	user.Password = string.Empty;

	return Results.Ok(new { access_token = token });
});

app.MapGet("anonymous", () => "anonymous user");

app.MapGet("authenticated", (HttpContext httpContext) =>
	$"authenticated user: {httpContext.User.Identity?.Name}")
	.RequireAuthorization();

app.MapGet("employee", [Authorize(Roles = "employee,manager")] (HttpContext httpContext) =>
	$"authorized user - employee: {httpContext.User.Identity?.Name}");

app.MapGet("manager", [Authorize(Roles = "manager")] (HttpContext httpContext) =>
	$"authorized user - manager: {httpContext.User.Identity?.Name}");

app.Run();