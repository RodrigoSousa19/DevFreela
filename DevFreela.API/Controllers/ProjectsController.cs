using DevFreela.Application.Models;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.API.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly DevFreelaDbContext _context;
        public ProjectsController(DevFreelaDbContext context)
        {
            _context = context;
        }

        // GET api/projects?search=1234
        [HttpGet]
        public IActionResult Get(string search = "", int page = 1, int size = 3)
        {
            var projects = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Where(p => !p.IsDeleted && (search == "" || p.Title.Contains(search) || p.Description.Contains(search)))
                .Skip(page * size)
                .Take(size)
                .ToList();

            var model = projects.Select(ProjectItemViewModel.FromEntity).ToList();
            return Ok(model);
        }

        // GET api/projects/1234
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = _context.Projects
                .Include(p => p.Client)
                .Include(p => p.Freelancer)
                .Include(p => p.Comments)
                .SingleOrDefault(p => p.Id == id && !p.IsDeleted);

            var model = ProjectViewModel.FromEntity(project);

            return Ok(model);
        }

        // POST api/projects
        [HttpPost]
        public IActionResult Post([FromBody] CreateProjectInputModel model)
        {
            var project = model.ToEntity();

            _context.Projects.Add(project);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, model);
        }

        // PUT api/projects/1234
        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateProjectInputModel model)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == model.IdProject);

            if (project is null)
                return NotFound();

            project.Update(model.Title, model.Description, model.TotalCost);

            _context.Projects.Update(project);
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/projects/1234
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
                return NotFound();

            project.SetAsDeleted();

            _context.Projects.Update(project);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT api/projects/1234/start
        [HttpPut("{id}/start")]
        public IActionResult Start(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
                return NotFound();

            project.Start();

            _context.Projects.Update(project);
            _context.SaveChanges();

            return NoContent();
        }

        // PUT api/projects/1234/complete
        [HttpPut("{id}/complete")]
        public IActionResult Complete(int id)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == id);

            if (project is null)
                return NotFound();

            project.Complete();

            _context.Projects.Update(project);
            _context.SaveChanges();

            return NoContent();
        }

        // POST api/projects/1234/comments
        [HttpPost("{id}/comments")]
        public IActionResult Comments(int id, CreateProjectCommentInputModel model)
        {
            var project = _context.Projects.SingleOrDefault(p => p.Id == model.IdProject);

            if (project is null)
                return NotFound();

            var projectComment = new ProjectComment(model.Content, model.IdProject, model.IdUser);

            _context.ProjectComments.Add(projectComment);
            _context.SaveChanges();

            return Ok();
        }
    }
}
