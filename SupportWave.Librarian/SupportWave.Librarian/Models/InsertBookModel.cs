using System.ComponentModel.DataAnnotations;
using SupportWave.Librarian.Attributes;

namespace SupportWave.Librarian.Api.Models;

public class InsertBookModel
{
    [Required]
    public string? Title { get; set; }
    [Required]
    public string? Author { get; set; }
    [PastDate(ErrorMessage = "Published Date must be in the past.")]
    public DateTime? PublishedDate { get; set; }
    public string? Isbn { get; set; }

    public static implicit operator BookModel(InsertBookModel? model)
    {
        return model == null ? new BookModel() : new BookModel(model);
    }
}