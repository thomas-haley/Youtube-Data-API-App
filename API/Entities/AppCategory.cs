using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Category")]
public class AppCategory
{
    public int Id { get; set; }
    public required string API_Id { get; set; } //categoryId from API
    public string? Category {get; set;}//Must get from category|list
}