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
    [BindProperty(SupportsGet = true)]
    public string? Status { get; set; }   // "Open", "InProgress", "Done"
    // Load the entities from the database into the Assignments property.
    public async Task OnGetAsync()
    {
        var query = _context.Assignments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Status))
        {
            query = query.Where(a => a.Status == Status);
        }
        // Display the filtered query 
        Assignments = await query.OrderBy(a => a.DueDate).ToListAsync();
    }
}