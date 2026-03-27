using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Core.Entities;

namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        
        string GenerateAccessToken(AppUser user);

        
        string GenerateRefreshToken();
    }
}