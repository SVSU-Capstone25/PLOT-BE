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
/// env file using EnvironmentSettings to configure the token
/// settings. Tokens are generated using HMAC-SHA256
/// signing algorithm and are signed with a symmetric key based of the 
/// applications secret key.
/// The AuthController uses this service to generate and validate
/// tokens for password reset and application authentication/authorization security.
/// 
///
/// Written by: Michael Polhill
/// </summary>
public class TokenService
{
    // CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS -- CONSTANTS ------

    //Default token expiration time if tokens exp cant be found in the env
    private const double _DEFAULT_EXPIRATION_TIME = 30;

    // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

    // Stores token issuer 
    private readonly string _issuer;

    // Stores token audience 
    private readonly string _audience;

    // Stores secret key for signing auth tokens.
    private readonly string _authSecretKey;

    // Stores secret key for signing password reset tokens.
    private readonly string _passwordResetSecretKey;

    // Expiration time in minuets for auth tokens
    private readonly double _authExpirationTime;

    // Expiration time for password in minuets reset tokens
    private readonly double _passwordResetExpirationTime;

    // Service to parse claims for jwt
    private readonly ClaimParserService _claimParserService;

    //Service to get variables from env
    private readonly EnvironmentSettings _envSettings;


    // Methods -- Methods -- Methods -- Methods -- Methods -- Methods -----

    /// <summary>
    /// Constructor for class sets all relevant variables to create tokens.
    /// </summary>
    /// <param name="claimParserService">Service to parse jwt claims</param>
    /// <param name="envSettings">Service to get vars from env</param>
    public TokenService(ClaimParserService claimParserService, EnvironmentSettings envSettings)
    {
        _envSettings = envSettings;
        _claimParserService = claimParserService;
        _audience = _envSettings.audience;
        _issuer = _envSettings.issuer;
        _authSecretKey = _envSettings.auth_secret_key;
        _passwordResetSecretKey = _envSettings.password_reset_secret_key;
        
        //Try to parse env settings, set default for bad parse
        if (double.TryParse(_envSettings.auth_expiration_time, 
            out double auth_expiration_time))
        {
            _authExpirationTime = auth_expiration_time;
        }
        else
        {
            _authExpirationTime = _DEFAULT_EXPIRATION_TIME;
        }

        if (double.TryParse(_envSettings.password_reset_expiration_time, 
            out double password_reset_expiration_time))
        {
            _passwordResetExpirationTime = password_reset_expiration_time;
        }
        else
        {
            _passwordResetExpirationTime = _DEFAULT_EXPIRATION_TIME;
        }
    }


    /// <summary>
    /// Method to generate a jwt token for authentication/authorization.
    /// </summary>
    /// <param name="user">User model to base claims</param>
    /// <returns>Jwt token as string</returns>
    public string GenerateAuthToken(User user)
    {
        ClaimsIdentity authClaimsIdentity = new ClaimsIdentity(
                [
                    new("Email", user.EMAIL!),
                    new("Role", user.ROLE!),
                    new("UserId", user.TUID.ToString()!)
                ]);

        //Use private method to create the token
        return GenerateToken(authClaimsIdentity, _authSecretKey, _authExpirationTime);
    }   


    /// <summary>
    /// Method to generate a jwt token for a password reset.
    /// </summary>
    /// <param name="user">User model to base claims</param>
    /// <returns>Jwt token as a string</returns>
    public string GeneratePasswordResetToken(User user)
    {
        ClaimsIdentity passwordResetClaimsIdentity = new ClaimsIdentity(
                [
                    new("Email", user.EMAIL!),
                ]);

        //Use private method to create the token
        return GenerateToken(passwordResetClaimsIdentity, _passwordResetSecretKey, _passwordResetExpirationTime);
    }   


    /// <summary>
    /// Method to validate a jwt auth token
    /// </summary>
    /// <param name="token">Auth jwt token as string</param>
    /// <returns>Validated user email</returns>
    public string? ValidateAuthToken(string token)
    {
       return ValidateToken(token, _authSecretKey);
    }


    /// <summary>
    /// Method to validate a jwt passwordReset token
    /// </summary>
    /// <param name="token">PasswordReset jwt token as string</param>
    /// <returns>Validated user email</returns>
    public string? ValidatePasswordResetToken(string token)
    {
       return ValidateToken(token, _passwordResetSecretKey);
    }


    /// <summary>
    /// Internal class method to generate a JWT token.
    /// </summary>
    /// <param name="claimsIdentity">Tokens claims</param>
    /// <param name="secretKey">Secret key for token validation</param>
    /// <param name="expirationTime">Token expiration time</param>
    /// <returns>Jwt token as string</returns>
    private string GenerateToken(ClaimsIdentity claimsIdentity, string secretKey, double expirationTime)
    {
        // Create a handler for token creation
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey!);
    
        //Set token settings
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                 SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddMinutes(expirationTime)
        };
    
        var token = tokenHandler.CreateToken(tokenDescriptor);
    
        // Convert the token a string
        // to be used in the password reset url.
        return tokenHandler.WriteToken(token);
    }
    

    /// <summary>
    /// This method validates a JWT token and returns the user's email if
    /// valid.
    /// </summary>
    /// <param name="token">Jwt token to validate</param>
    /// <param name="secretKey">Key for validation</param>
    /// <returns>Email from claims or null if validation fails</returns>
    private string? ValidateToken(string token, string secretKey)
    {
        // Token handler to validate the token.
        var tokenHandler = new JwtSecurityTokenHandler();

        // Convert applications secret key into a byte array
        // to create a new symmetric security key to match
        // against the incoming token.
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

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