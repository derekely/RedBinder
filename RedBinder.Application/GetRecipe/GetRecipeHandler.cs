﻿using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using RedBinder.Application.ServiceInterface;
using RedBinder.Domain.Entities;

namespace RedBinder.Application.GetRecipe;
public record GetRecipeQuery(int RecipeId) : IRequest<Result<Recipe>>; 

public class GetRecipeHandler(IRepositoryService repositoryService) : IRequestHandler<GetRecipeQuery, Result<Recipe>>
{
    private readonly IRepositoryService _repositoryService = repositoryService;
    public async Task<Result<Recipe>> Handle(GetRecipeQuery request, CancellationToken cancellationToken) => await _repositoryService.GetRecipeAsync(request.RecipeId);
}