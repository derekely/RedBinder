using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedBinder.Application.GetRecipe
{
    public class GetRecipeRequest : IRequest<GetRecipeHander>
    {
        public int RecipeId { get; set; }

    }

    public class GetRecipeHander : IRequestHandler<GetRecipeRequest, Task<Unit>>
    {
        public Task<Unit> Handle(GetRecipeRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}