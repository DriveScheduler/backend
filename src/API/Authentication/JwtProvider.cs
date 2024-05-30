﻿using Domain.Enums;
using Domain.Models;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Authentication
{
    public class JwtProvider(IConfiguration configuration)
    {
        public static string CLAIM_ID = "ID";
        public static string CLAIM_FIRSTNAME = "FirstName";
        public static string CLAIM_ROLE = "Role";

        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(CLAIM_ID, user.Id.ToString()),
                new Claim(CLAIM_FIRSTNAME, user.FirstName),
                new Claim(CLAIM_ROLE, ((int)user.Type).ToString()),
            };
            var jwtToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,                
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public ClaimsPrincipal ConvertToken(string token)
        {            
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            try
            {                
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,                        
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);
                
                return claimsPrincipal;
            }
            catch (SecurityTokenExpiredException)
            {                
                throw new ApplicationException("Token has expired.");
            }
        }

    }
}
