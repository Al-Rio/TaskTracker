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
    public int TotalCount { get; private set; }
    public int InProgressCount { get; private set; } // In Progress total count

    public int DoneCount { get; private set; } // Done status total count
    public int OpenCount { get; private set; } // Open status total count

    public double InProgressPercent { get; private set; } // double
    public double DonePercent { get; private set; }       // double
    public double OpenPercent { get; private set; }     // double

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

        // Total tasks across the entire DB
        TotalCount = await _context.Assignments.CountAsync();

        // Compute total of the In Progress status tasks
        InProgressCount = await _context.Assignments.CountAsync(a => a.Status == "InProgress");
        // Compute total of the Done status tasks
        DoneCount = await _context.Assignments.CountAsync(a => a.Status == "Done");
        // Compute total of the Open status tasks
        OpenCount = await _context.Assignments.CountAsync(a => a.Status == "Open");

        // Direct division to get the percentage and Math.Round to round the fractions
        InProgressPercent = Math.Round((double)InProgressCount / TotalCount * 100.0);
        DonePercent = Math.Round((double)DoneCount / TotalCount * 100.0);
        OpenPercent = Math.Round((double)OpenCount / TotalCount * 100.0);
    }
}