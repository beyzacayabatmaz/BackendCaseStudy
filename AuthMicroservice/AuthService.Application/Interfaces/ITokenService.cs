using System;
using System.Collections.Generic;
using System.Text;
using AuthService.Core.Entities;

namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        // 1. Kısa ömürlü asıl JWT token'ı üretir
        string GenerateAccessToken(AppUser user);

        // 2. Uzun ömürlü Refresh Token'ı üretir (Rastgele şifreli bir metin)
        string GenerateRefreshToken();
    }
}