using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Linnworks.Api;

using LinnworksTest.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinnworksTest.Controllers
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CategoryController : Controller
	{
		private Client GetClient()
		{
			return new Client(User.Identity.Name);
		}

		[HttpGet("[action]")]
		public async Task<IEnumerable<CategoryWithStock>> Index()
		{
			var client = GetClient();

			var stockQuery = @"SELECT p.CategoryId, Count(*) stock
FROM StockItem AS S
JOIN ProductCategories AS P ON P.CategoryId = S.CategoryId
GROUP BY P.CategoryId";

			var queryResultTask = client.ExecuteCustomScriptQuery(stockQuery);
			var categoriesTask = client.GetCategories();

			var queryResult = await queryResultTask;
			var categories = await categoriesTask;

			var stock = queryResult.Results.ToDictionary(
				x => new Guid(x["CategoryId"]),
				x => int.Parse(x["stock"])
			);

			return categories.Select(x => new CategoryWithStock
			{
				CategoryId = x.CategoryId,
				CategoryName = x.CategoryName,
				Stock = stock.ContainsKey(x.CategoryId)
					? stock[x.CategoryId]
					: 0
			});
		}

		[HttpGet]
		[Route("[action]/{id}")]
		public async Task<ActionResult> Details(string id)
		{
			if (String.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid categoryId))
				return new BadRequestResult();

			var client = GetClient();
			var categories = await client.GetCategories();
			var category = categories.FirstOrDefault(c => c.CategoryId == categoryId);
			if (category == null)
				return new NotFoundResult();

			return new OkObjectResult(category);
		}

		[HttpPost("[action]")]
		public async Task<Category> Create([FromBody]Category category)
		{
			return await GetClient().CreateCategory(category.CategoryName);
		}

		[HttpPut("[action]")]
		public async Task Edit([FromBody]Category category)
		{
			await GetClient().UpdateCategory(category);
		}

		[HttpDelete("[action]/{id}")]
		public async Task<ActionResult> Delete(string id)
		{
			if (!Guid.TryParse(id, out Guid categoryId))
				return new BadRequestResult();

			await GetClient().DeleteCategoryById(categoryId);

			return new OkResult();
		}
	}
}
