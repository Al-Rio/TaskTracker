 using System.ComponentModel.DataAnnotations;
 public class Assignment
    {
        public int Id { get; set; }
        [Required]
        public string AssignmentName { get; set; } = string.Empty;
        [Required]
        public string Project { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateOnly DueDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }