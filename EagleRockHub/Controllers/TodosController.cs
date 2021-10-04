using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EagleRockHub.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Redis.Sample.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodosController : ControllerBase
	{
		List<string> todos = new List<string> { "shopping", "Watch Movie", "Gardening" };

		private readonly ICacheProviderService _cacheProviderService;
		public TodosController(ICacheProviderService cacheProviderService)
		{
			_cacheProviderService = cacheProviderService;
		}

		[HttpGet]
		[Route("all")]
		public async Task<IActionResult> GetAll()
		{
			List<string> myTodos = new List<string>();
			bool IsCached = false;
			string cachedTodosString = string.Empty;
			myTodos = await _cacheProviderService.GetFromCache<List<string>>("_todos");
			if (myTodos != null)
			{
				// loaded data from the redis cache.
				IsCached = true;
			}
			else
			{
				myTodos = todos;
				IsCached = false;
				await _cacheProviderService.SetCache<List<string>>("_todos", todos,  new DistributedCacheEntryOptions());
			}
			return Ok(new { IsCached, myTodos });
		}
	}
}