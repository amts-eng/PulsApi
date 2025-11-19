using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Auth.Register
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/auth/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            // Check if email already exists
            var existingUser = await Db.Users.FirstOrDefaultAsync(u => u.Email == r.Email, c);
            if (existingUser != null)
            {
                AddError("Email already registered");
                await SendErrorsAsync(409, c);
                return;
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(r.Password);

            // Create user
            var user = new Models.User
            {
                Email = r.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            Db.Users.Add(user);
            await Db.SaveChangesAsync(c);

            // Generate JWT token
            var token = JwtBearer.CreateToken(
                o =>
                {
                    o.SigningKey = Config["Auth:JwtKey"]!;
                    o.ExpireAt = DateTime.UtcNow.AddDays(7);
                    o.User["UserId"] = user.Id.ToString();
                    o.User["Email"] = user.Email;
                });

            await SendAsync(new Response
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token
            }, 201, c);
        }
    }
}
