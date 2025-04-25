using Microsoft.AspNetCore.Authorization;
using Plot.DataAccess.Interfaces;



public class RoleHandler : AuthorizationHandler<RoleRequirement>, IAuthorizationHandler
{
    //private readonly AuthHttpClient _authHttpClient;

    //private readonly Cookie _cookie;

    //private readonly JwtService _jwtService;

    private readonly IAuthContext _authContext;
    private readonly HttpContextAccessor _httpContextAccessor;

    //private readonly ClaimParserService _claimParserService;

    public RoleHandler(IAuthContext authContext, HttpContextAccessor httpContextAccessor)
    {
        _authContext = authContext;
        _httpContextAccessor = httpContextAccessor;
        //_cookie=cookie;
        //_jwtService=jwtService;
        //_claimParserService=claimParserService;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        //string? authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext != null)
        {
            var token = httpContext.Request.Headers["Authorization"].ToString();

            Console.WriteLine(token);

            context.Succeed(requirement);
        }else
        {
            context.Fail();
        }
        

        

        


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
