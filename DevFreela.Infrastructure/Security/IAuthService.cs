using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Security
{
    public interface IAuthService
    {
        string ComputeHash(string password);
        string GenerateToken(string email, string role);
    }
}
