using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Pages
{
    public class AssignmentModel : PageModel
    {
        private readonly TaskDbContext _context;

        /// Constructor injects the application's DbContext.
        public AssignmentModel(TaskDbContext context)
        {
            _context = context;
        }

        // Public list of Assignments so the view can access Model.Assignments.
        public IList<Assignment> Assignments { get; set; } = new List<Assignment>();

        public class AssignmentViewModel
        {
            public int Id { get; set; }

            [Required(ErrorMessage = "Assignment Name is required.")]
            [StringLength(100, ErrorMessage = "Assignment Name cannot exceed 100 characters.")]
            public string AssignmentName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Project Name is required.")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
            public string Project { get; set; } = string.Empty;

            [Required(ErrorMessage = "Priority level must be Low, Medium, or High.")]
            [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
            public string Priority { get; set; } = string.Empty;

            [Required(ErrorMessage = "A Valid date is required.")]
            public DateOnly DueDate { get; set; }

            [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
            public string Description { get; set; } = string.Empty;

            [StringLength(12, ErrorMessage = "Status must be (Open, In Progress, Done) & cannot exceed 12 characters.")]
            public string Status { get; set; } = string.Empty;
        }

        // BindProperty to ensures Input is populated from (form POST) values.
        [BindProperty]
        public AssignmentViewModel Input { get; set; } = new AssignmentViewModel();

        public async Task OnGetAsync()
        {
            // Load Assignment entities into the DbContext sorted by due date (The earliest date first).
            Assignments = await _context.Assignments.OrderBy(a => a.DueDate).ToListAsync();
        }

        // Handler for server-side Edit that loads the entity into (Input) and returns the page
        public async Task<IActionResult> OnGetEditAsync(int id)
        {
            var entity = await _context.Assignments.FindAsync(id);
            if (entity == null)
            {
                // Reload the list and return page
                Assignments = await _context.Assignments.OrderBy(a => a.DueDate).ToListAsync();
                return Page();
            }

            Input = new AssignmentViewModel
            {
                Id = entity.Id,
                AssignmentName = entity.AssignmentName,
                Project = entity.Project,
                Priority = entity.Priority,
                DueDate = entity.DueDate,
                Description = entity.Description,
                Status = entity.Status
            };

            Assignments = await _context.Assignments.OrderBy(a => a.DueDate).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Server-side validation using Data Annotations
            if (!ModelState.IsValid)
            {
                // Reload assignments so the page can display the list and the validation errors at the same time
                Assignments = await _context.Assignments.OrderBy(a => a.DueDate).ToListAsync();
                return Page();
            }
            if (Input.Id > 0)
            {
                // load the entity, modify properties and save
                var existing = await _context.Assignments.FindAsync(Input.Id);
                if (existing == null)
                {
                    Assignments = await _context.Assignments.OrderBy(a => a.DueDate).ToListAsync();
                    return Page();
                }

                existing.AssignmentName = Input.AssignmentName;
                existing.Project = Input.Project;
                existing.Priority = Input.Priority;
                existing.DueDate = Input.DueDate;
                existing.Description = Input.Description;
                existing.Status = Input.Status;

                // SaveChangesAsync will persist modifications
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "The Assignment was Updated successfully.";
            }
            else
            {
                // Create an Assignment
                var entity = new Assignment
                {
                    AssignmentName = Input.AssignmentName,
                    Project = Input.Project,
                    Priority = Input.Priority,
                    DueDate = Input.DueDate,
                    Description = Input.Description,
                    Status = Input.Status
                };

                _context.Assignments.Add(entity);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "The Assignment was Added Successfully.";
            }

            return RedirectToPage();
        }

        // Delete Assignment
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var entity = await _context.Assignments.FindAsync(id);
            if (entity != null)
            {
                _context.Assignments.Remove(entity);
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "The Assignment was deleted successfully.";
            }
            return RedirectToPage();
        }
    }
}