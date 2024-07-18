using RedBinder.Domain.DTOs;

namespace RedBinder.Web.Models;

public class RecipeModel
{
    public string Name { get; set; } = string.Empty;
    public string Directions { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    internal static RecipeModel FromDto(RecipeOverviewDto dto)
    {
        return new RecipeModel
        {
            Name = dto.Name,
            Directions = dto.Directions,
            Description = dto.Description
        };
    }
}