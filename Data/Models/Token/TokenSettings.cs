namespace Plot.Data.Models.Token
{
    /// <summary>
    /// Filename: TokenSettings.cs
    /// 
    /// File Purpose:
    /// This file defines the TokenSettings model class, 
    /// which holds configuration for TokenService.
    /// 
    /// Class Purpose:
    /// This class serves as a model for data to configure the TokenService
    /// class. The properties correspond to token configuration values needed
    /// by the TokenService class.
    /// 
    /// Written by: Michael Polhill
    /// </summary>
    public class TokenSettings
    {
        // VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES -- VARIABLES ------

        // Issuer of the token (Http://Plot.com)!!!!!!!!Change when info is available!!!!!!!!
        public string? Issuer { get; set; }

        // Audience of the token (Http://Plot.com)!!!!!!!!Change when info is available!!!!!!!!
        public string? Audience { get; set; }

        //Token lifetime in minutes.
        public int? Lifetime { get; set; }
    }
}