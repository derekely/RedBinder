using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedBinder.Application.GetRecipe;

namespace RedBinder.Controllers
{
    public class RecipeController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        // GET: RecipeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: RecipeController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // example of calling a mediator
            var response = await _mediator.Send(new GetRecipeQuery(1));
            return View();
        }

        // GET: RecipeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RecipeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RecipeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RecipeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
