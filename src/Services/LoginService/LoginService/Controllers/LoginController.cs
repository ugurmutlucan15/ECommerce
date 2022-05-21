using AutoMapper;

using LoginService.Models;
using LoginService.Repositories.Interfaces;
using LoginService.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Net;
using System.Threading.Tasks;

namespace LoginService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _repository;
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _authenticationService;
        private readonly IMapper _mapper;

        public LoginController(IUserRepository repository, ILogger<LoginController> logger,
            IUserService authenticationService, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _logger.LogError("test");
        }

        [HttpGet("{username},{password}", Name = "Login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Login(string username, string password)
        {
            try
            {
                var user = await _repository.Get(m => m.UserName == username && m.Password == password);
                if (user == null) return Ok();

                var userviewModel = _mapper.Map<UserViewModel>(user);
                var token = _authenticationService.CreateAccessToken(userviewModel);
                return Ok(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(new { message = e.Message });
            }
        }
    }
}