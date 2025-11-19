using Microsoft.EntityFrameworkCore;
using PulsApi.Data;

namespace PulsApi.Auth.Login
{
    sealed class Endpoint : Endpoint<Request, Response>
    {
        public ApplicationDbContext Db { get; set; } = null!;

        public override void Configure()
        {
            Post("/api/auth/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            // Find user by email
            var user = await Db.Users.FirstOrDefaultAsync(u => u.Email == r.Email, c);
            if (user == null)
            {
                AddError("Invalid email or password");
                await SendErrorsAsync(401, c);
                return;
            }

            // Verify password
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(r.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                AddError("Invalid email or password");
                await SendErrorsAsync(401, c);
                return;
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
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
            }, cancellation: c);
        }
    }
}
