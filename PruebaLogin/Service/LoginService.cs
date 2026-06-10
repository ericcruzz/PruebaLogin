using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PruebaLogin.Models;

namespace PruebaLogin.Service
{
    public class LoginService
    {
        private readonly StagingContext _context;
        public LoginService(StagingContext context)
        {
            _context = context;
        }
        public async Task<bool> Access(string username, string password)
        {
            var error = await _context.Database
                .SqlQuery<int>($"EXECUTE SP_Operations 'login', NULL, {username}, {password}")
                .ToListAsync();

            var response = error.FirstOrDefault() == 0 ? true : false;

            return response;
        }

        public string EncryptSHA256(string username, string pass)
        {

            var hasher = new PasswordHasher<string>();
            string hashNativo = hasher.HashPassword(username, pass);

            return hashNativo;

        }

        public bool VerifyPassword(string username, string pass, string passwordHash)
        {
            var hasher = new PasswordHasher<string>();
            var result = hasher.VerifyHashedPassword(username, passwordHash, pass);
            return result == PasswordVerificationResult.Success;
        }
    }
}
