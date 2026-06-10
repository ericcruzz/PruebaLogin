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
            bool response = false;

            //if (ExistsPasswordHash(username, password))
            //{

            //    var passwordHash = GetPasswordHash(username);
            //    var passHashCreated = EncryptPBKDF2(username, password);
            //    response = VerifyPassword(username, passHashCreated, passwordHash);
            //}
            //else
            //{
               
                //CreatePasswordHash(username, password);
                //var passwordHash = EncryptPBKDF2(username, password);
                var error = await _context.Database
                    .SqlQuery<int>($"EXECUTE SP_Operations 'login', NULL, {username}, {password}")
                    .ToListAsync();

                 response = error.FirstOrDefault() == 0 ? true : false;
            //}

            return response;
        }

        public string EncryptPBKDF2(string username, string pass)
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

        public string GetPasswordHash(string username)
        {
            var passwordHash = from p in _context.Passwords
                               join u in _context.Users on p.UserId equals u.UserId
                               where u.Username == username
                               select p.PasswordHash;


            return passwordHash.FirstOrDefault();
        }

        public bool ExistsPasswordHash(string username, string password) 
        {
            var passHash = GetPasswordHash(username);
            
            if (passHash.Length < 84)
                return false;

            return true;
        }

        public void CreatePasswordHash(string username, string password)
        {
            var passwordHash = EncryptPBKDF2(username, password);

            _context.Users.Where(u => u.Username == username).ToList().ForEach(u =>
            {
                var userId = u.UserId;
                var passwordEntry = _context.Passwords.FirstOrDefault(p => p.UserId == userId);
                if (passwordEntry != null)
                {
                    passwordEntry.PasswordHash = passwordHash;
                    _context.SaveChanges();
                }
            });
        }
    }
}
