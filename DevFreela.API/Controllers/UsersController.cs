using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DevFreelaDbContext _context;
        private readonly IAuthService _authService;
        public UsersController(DevFreelaDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Include(u => u.Skills).ThenInclude(s => s.Skill).SingleOrDefault(u => u.Id == id);

            if (user is null)
                return NotFound();

            var model = UserViewModel.FromEntity(user);

            return Ok(model);
        }

        // POST api/users
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post(CreateUserInputModel model)
        {
            var hashPassword = _authService.ComputeHash(model.Password);
            var user = new User(model.FullName, model.Email, model.BirthDate, hashPassword, model.Role);

            _context.Users.Add(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("{id}/skills")]
        public IActionResult PostSkills(int id, UserSkillsInputModel model)
        {
            var userSkills = model.SkillIds.Select(s => new UserSkill(id, s)).ToList();

            _context.UserSkills.AddRange(userSkills);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}/profile-picture")]
        public IActionResult PostProfilePicture(int id, IFormFile file)
        {
            var description = $"File: {file.FileName}, Size: {file.Length}";

            //processar a imagem

            return Ok(description);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginInputModel model)
        {
            var hashPassword = _authService.ComputeHash(model.Password);

            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email && u.Password == hashPassword);

            if (user is null)
            {
                var error = ResultViewModel<LoginViewModel?>.Error("Erro de login");
                return BadRequest(error);
            }

            var token = _authService.GenerateToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            var result = ResultViewModel<LoginViewModel>.Success(viewModel);

            return Ok(result);
        }
    }
}
