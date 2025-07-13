using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class InstructorsController : ControllerBase
{
    private readonly AppDbContext _context;

    public InstructorsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructors()
    {
        return await _context.Instructors.Include(i => i.CourseInstructors).ThenInclude(ci => ci.Course).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Instructor>> GetInstructor(int id)
    {
        var instructor = await _context.Instructors.Include(i => i.CourseInstructors).ThenInclude(ci => ci.Course).FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
            return NotFound();

        return instructor;
    }

    [HttpPost]
    public async Task<ActionResult<Instructor>> PostInstructor(Instructor instructor)
    {
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInstructor), new { id = instructor.Id }, instructor);
    }

}