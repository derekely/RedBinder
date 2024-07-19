namespace Redbinder.Web3.StaticObjects;

public class ShoppingListState
{
    public static List<int> RecipeIds { get; set; } = [];

    public static void AddRecipeId(int recipeId)
    {
        if (!RecipeIds.Contains(recipeId)) RecipeIds.Add(recipeId);
        
    }

    public static void RemoveRecipeId(int recipeId)
    {
        if(RecipeIds.Contains(recipeId)) RecipeIds.Remove(recipeId);
    }

    public static void ClearRecipeIds()
    {
        RecipeIds.Clear();
    }
}