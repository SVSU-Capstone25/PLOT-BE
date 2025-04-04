using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Plot.Data.Models.Users;
using Plot.Data.Models.Env;

namespace Plot.Services;

/// <summary>
/// Filename: TokenService.cs
/// Part of Project: PLOT/PLOT-BE/Plot/Services
///
/// File Purpose:
/// This file defines the TokenService class, responsible for generating
/// and validating JSON Web Tokens (JWT) for authentication and
/// authorization.
///
/// Class Purpose:
/// The TokenService class has methods to generate JWTs with user email
/// claims and validate existing tokens. Variables are set from the 
/// appsettings.json using IOptions<TokenSettings> to configure the token
/// issuer, audience, and lifetime. Tokens are generated using HMAC-SHA256
/// signing algorithm and are signed with a symmetric key based of the 
/// applications secret key.(change this when I put key somewhere secure)
/// The PasswordResetController uses this service to generate and validate
/// tokens for password reset security.
/// 
/// Dependencies:
/// - Microsoft.IdentityModel.Tokens: Used for signing and verifying JWTs.
/// - System.IdentityModel.Tokens.Jwt: Handles JWT encoding and decoding.
/// - System.Security.Claims: Email claim is used for user identification.
/// - System.Text: Encoding the secret key before signing.
/// - IOptions: Provides token configuration from app settings.
/// - TokenSettings: Model for token configuration settings.
///
/// Written by: Michael Polhill
/// </summary>
public class TokenService
{
    private const double _DEFAULT_EXPIRATION_TIME = 30;

    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Stores token issuer from settings.
    private readonly string _issuer;

    // Stores token audience from settings.
    private readonly string _audience;

    // Stores secret key for signing tokens.(change this when I put key somewhere secure)
    private readonly string _secretKey;

    // Stores expiration time (in hours).
    private readonly double _expirationTime;
    private readonly ClaimParserService _claimParserService;
    private readonly EnvironmentSettings _envSettings;

    public TokenService(ClaimParserService claimParserService, EnvironmentSettings envSettings)
    {
        _envSettings = envSettings;
        _claimParserService = claimParserService;
        _audience = _envSettings.audience;
        _issuer = _envSettings.issuer;
        _secretKey = _envSettings.secret_key;

        if (double.TryParse(_envSettings.expiration_time, out double expirationTime))
        {
            _expirationTime = expirationTime;
        }
        else
        {
            _expirationTime = _DEFAULT_EXPIRATION_TIME;
        }
    }

    /// <summary>
    /// Generates a JWT token with the user's email as a claim for 
    /// authentication. Used when creating a password reset link.
    /// </summary>
    /// <param name="userEmail">Email to be used in the token</param>
    /// <returns>JWT token as a string</returns>
    public string GenerateToken(User user)
    {
        // Building the token based on the user input and token settings (appsettings.json)

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey!);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                [
                    new("Email", user.Email!),
                    new("Role", user.Role.ToString()!),
                    new("UserId", user.UserId.ToString()!)
                ]),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTime)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Convert the token into a compact format as a string
        // to be used in the password reset url.
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// This method validates a JWT token and returns the user's email if
    /// valid. Used to validate the token in the password reset url.
    /// </summary>
    /// <param name="token">Jwt token to validate</param>
    /// <returns>Valid Token: Users email. Else: null</returns>
    public string? ValidateToken(string token)
    {
        // Token handler to validate the token.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Convert applications secret key into a byte array
        // to create a new symmetric security key to match
        // against the incoming token.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secretKey));

        // Create validation parameters to match the
        // incoming token.
        var validationParameters = new TokenValidationParameters
        {
            // Ensure the incoming tokens signing key is valid.
            ValidateIssuerSigningKey = true,
            // Match the incoming tokens signing key to the applications
            // secret key.
            IssuerSigningKey = key,
            // Check the tokens issuer and audience.
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            // Make sure the token is not expired.
            ValidateLifetime = true
        };

        try
        {
            // Validate the token based on the validation parameters. out_ 
            // is used to ignore the security token as it is not needed.
            var principal = tokenHandler.ValidateToken(
                token, validationParameters, out _);

            return _claimParserService.GetEmail(principal);
        }
        catch
        {
            // Return null if the token is invalid.
            return null;
        }
    }
}