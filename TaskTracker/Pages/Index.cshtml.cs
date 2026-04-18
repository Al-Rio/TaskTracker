using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Pages;

public class IndexModel : PageModel
{
     
    private readonly TaskDbContext _context;
    // Constructor injects the application's DbContext.
    public IndexModel(TaskDbContext context)
    {
        _context = context;
    }

    // Public property the Razor view can access via Model.Products
    public IList<Assignment> Assignments { get; set; } = new List<Assignment>();

    // Load the entities from the database into the Assignments property.
    public async Task OnGetAsync()
    {
        Assignments = await _context.Assignments.ToListAsync();
    }
}