using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Bcpg.Sig;
using Plot.Data.Models.Users;
using Plot.DataAccess.Interfaces;
using Plot.Services;



public class RoleHandler : AuthorizationHandler<RoleRequirement>, IAuthorizationHandler
{
    //private readonly AuthHttpClient _authHttpClient;

    //private readonly Cookie _cookie;

    private readonly TokenService _tokenService;

    private readonly IAuthContext _authContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimParserService _claimParserService;

    //private readonly ClaimParserService _claimParserService;

    public RoleHandler(IAuthContext authContext, IHttpContextAccessor httpContextAccessor, TokenService tokenService, ClaimParserService claimParserService)
    {
        _authContext = authContext;
        _httpContextAccessor = httpContextAccessor;
        _tokenService=tokenService;
        _claimParserService=claimParserService;
        //_jwtService=jwtService;
        //_claimParserService=claimParserService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        //string? authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext != null)
        {
            var bearerToken = httpContext.Request.Headers["Authorization"].ToString();

            string token = bearerToken.Substring("Bearer ".Length).Trim();

            var requestUserClaims=_tokenService.ValidateAuthToken(token);

            if(requestUserClaims!=null)
            {
                var requestUserEmail = _claimParserService.GetEmail(requestUserClaims);
                var requestUserRole = _claimParserService.GetRole(requestUserClaims);


                if(String.IsNullOrEmpty(requestUserEmail) || String.IsNullOrEmpty(requestUserRole) )
                {
                    context.Fail();
                }else
                {
                    var currentUser= await _authContext.GetUserByEmail(requestUserEmail);

                    if(currentUser!=null)
                    {
                        if(requestUserRole != currentUser.ROLE)
                        {
                            //KeyExpirationTime = httpContext.Request.
                            //httpContext.Response.Cookies.Append
                        }

                    if(requirement.AllowedRoles.Contains(currentUser.ROLE))
                    {
                        
                        context.Succeed(requirement);
                    }

                    }

                    
                }
            }
        }


        context.Fail();
        

        

        

        


        // if (string.IsNullOrEmpty(authHeader))
        // {
        //     return BadRequest();
        // }
        
        // string token = authHeader.Substring("Bearer ".Length).Trim();

        // var userEmail = _tokenService.ValidateAuthToken(token);

        
        




        // UserDTO? currentUser = await _authHttpClient.GetCurrentUser();

        // if(currentUser!=null && requirement.AllowedRoles.Contains(currentUser.ROLE))
        // {
        //     Console.WriteLine("ROLE GOOD");
        //     context.Succeed(requirement);
        // }else
        // {
        //     Console.WriteLine("ROLE BAD");
        //     context.Fail();
        // }


        //var token = await _cookie.GetValue("Auth");

        // if(String.IsNullOrEmpty(token))
        // {
        //     var userPrincipal = _jwtService.ValidateAuthToken(token);

        //     if(userPrincipal!=null)
        //     {
        //         var currentUserTUID = _claimParserService.GetUserId(userPrincipal);

        //         var userInDb = _usersHttpClient.GetUserById(currentUserTUID.Value);

        //         if(userInDb!=null && requirement.AllowedRoles.Contains(userInDb.ROLE))


        //     }

        // }

        // var email = context.User.Identity?.Name; // or ClaimTypes.Email

        // if (string.IsNullOrEmpty(email))
        //     return;

        // var user = await _usersHttpClient.GetUserByEmail(email); // Call DB every time

        // if (user != null && requirement.AllowedRoles.Contains(user.ROLE))
        // {
        //     context.Succeed(requirement);
        // }
    }
}
