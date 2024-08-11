using System.ComponentModel.DataAnnotations;
using Librarian.UI.Attributes;

namespace Librarian.UI.Models;

public class BookModel
{
    public Guid Id { get; set; }
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Author { get; set; }
    [PastDate]
    public DateTime? PublishedDate { get; set; }
    [Required]
    public string Isbn { get; set; }
}