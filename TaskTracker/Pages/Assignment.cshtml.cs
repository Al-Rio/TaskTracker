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
        }

        // BindProperty to ensures Input is populated from (form POST) values.
        [BindProperty]
        public AssignmentViewModel Input { get; set; } = new AssignmentViewModel();

        public async Task OnGetAsync()
        {
            // Load Assignment entities into the DbContext.
            Assignments = await _context.Assignments.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Server-side validation using Data Annotations
            if (!ModelState.IsValid)
            {
                // Reload assignments so the page can display the list and the validation errors at the same time
                Assignments = await _context.Assignments.ToListAsync();
                return Page();
            }

            // Map the validated input from the user to the Assignment entity
            var assignment = new Assignment
            {
                AssignmentName = Input.AssignmentName,
                Project = Input.Project,
                Priority = Input.Priority,
                DueDate = Input.DueDate,
                Description = Input.Description
            };

            // Add the new assignment to the DbContext and save changes
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            // Redirect to avoid duplicate form submissions.
            return RedirectToPage();
        }
    }
}