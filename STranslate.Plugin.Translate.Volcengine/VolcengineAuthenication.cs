using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace STranslate.Plugin.Translate.Volcengine;

public class VolcengineAuthenication
{
    public static string GenerateToken(string apiKey, int expSeconds)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("API key is required.");

        // Expect UUID-like format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        var uuidPattern = "^[0-9a-fA-F]{8}(-[0-9a-fA-F]{4}){3}-[0-9a-fA-F]{12}$";
        if (!Regex.IsMatch(apiKey, uuidPattern))
            throw new ArgumentException("Invalid API key format. Expected xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx.");

        var id = apiKey;

        // Derive a 32-byte key from the apiKey using SHA-256 (better than zero-padding)
        byte[] keyBytes;
        using (var sha = SHA256.Create())
        {
            keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(apiKey));
        }

        var securityKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var payload = new JwtPayload
        {
            { "api_key", id },
            { "exp", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + expSeconds },
            { "timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds() }
        };

        var header = new JwtHeader(credentials)
        {
            { "sign_type", "SIGN" }
        };

        var token = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}