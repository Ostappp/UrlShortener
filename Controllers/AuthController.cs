using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Interfaces;
using UrlShortener.Models.Data;
using UrlShortener.Models.RequestModels;
using UrlShortener.Models.ResponseModels;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UrlShortenerContext _context;

        private readonly IAuthService _authenticationService;

        private readonly ITokenService _tokenService;


        private readonly IMapper _mapper;
        public AuthController(UrlShortenerContext context, IAuthService authenticationService, ITokenService tokenService, IMapper mapper)
        {
            _context = context;

            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            User user = await _authenticationService.SignInAsync(request);
            TokenResponseModel response = _mapper.Map<TokenResponseModel>(await _tokenService.CreateTokenAsync(user));

            return Ok(response);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] SignUpRequest request)
        {
            User user = await _authenticationService.SignUpAsync(request);
            TokenResponseModel response = _mapper.Map<TokenResponseModel>(await _tokenService.CreateTokenAsync(user));

            return Ok(response);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string token)
        {
            TokenResponseModel response = _mapper.Map<TokenResponseModel>(await _tokenService.RefreshTokenAsync(token));

            return Ok(response);
        }
    }
}
