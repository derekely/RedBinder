using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RedBinder.Domain.Entities;

namespace RedBinder.Application.GetRecipe
{
    public class GetRecipeQuery : IRequest<Result<Recipe>>
    {
        public int RecipeId { get; }
    }

    public class GetRecipeHander : IRequestHandler<GetRecipeQuery, Result<Recipe>>
    {
        public async Task<Result<Recipe>> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Result.Success(new Recipe
            {
                Name = "Fruit Salad",
                Directions = "peel banana, eat banana, eat apple, eat orange",
            }));
        }
    }

}