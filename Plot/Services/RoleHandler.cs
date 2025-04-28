using Microsoft.AspNetCore.Authorization;
using Plot.Data.Models.Env;
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

    private readonly EnvironmentSettings _envSettings;

    //private readonly ClaimParserService _claimParserService;

    public RoleHandler(IAuthContext authContext, IHttpContextAccessor httpContextAccessor, TokenService tokenService, ClaimParserService claimParserService, EnvironmentSettings environmentSettings)
    {
        _authContext = authContext;
        _httpContextAccessor = httpContextAccessor;
        _tokenService=tokenService;
        _claimParserService=claimParserService;
        _envSettings = environmentSettings;
        //_jwtService=jwtService;
        //_claimParserService=claimParserService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        //string? authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        var httpContext = _httpContextAccessor.HttpContext;

        //Console.WriteLine(httpContext.Response.HasStarted);

        Console.WriteLine(httpContext.Request.Path);

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
                }
                else
                {
                    var currentUser= await _authContext.GetUserByEmail(requestUserEmail);

                    if(currentUser!=null)
                    {
                        if(requestUserRole != currentUser.ROLE)
                        {

                            httpContext.Response.Headers.Add("X-Role-Mismatch", "true");
                            // var updatedToken = _tokenService.GenerateAuthToken(currentUser);
                            // var expTime = Convert.ToDouble(_envSettings.auth_expiration_time);

                            // Console.WriteLine("Adding new token");
                            // Console.WriteLine(httpContext.Response.HasStarted);
                            

                            // httpContext.Response.Headers.Append("NEW-HEADER","TESET-TETET");
                            
                            //KeyExpirationTime = httpContext.Request.
                            //httpContext.Response.Cookies.Append
                        }

                        if(requirement.AllowedRoles.Contains(currentUser.ROLE))
                        {
                            httpContext.Response.Headers.Add("X-Role-Mismatch", "true");
                            Console.WriteLine("GOOD Req");
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
            }
        }

        Console.WriteLine("Bad Req");
        context.Fail();
        return;
    }
}
