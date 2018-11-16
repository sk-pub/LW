using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Linnworks.Api
{
	public class Client: IDisposable
	{
		private readonly string _token;
		private readonly HttpClient _httpClient;
		private readonly HttpMessageHandler _requestHandler;
		private const string _baseUri = "https://eu.linnworks.net";

		public static async Task<bool> CheckToken(string token)
		{
			try
			{
				using (var api = new Client(token))
				{
					await api.GetCategories();
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public Client(string token)
		{
			_token = token;

			_requestHandler = new HttpClientHandler
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			};
			_httpClient = new HttpClient(_requestHandler);
			_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
			_httpClient.DefaultRequestHeaders.Add("Authorization", _token);
		}

		private static Uri GetUri(string path)
		{
			return new UriBuilder(_baseUri) { Path = path }.Uri;
		}

		private async Task<string> Get(string path)
		{
			var response = await _httpClient.GetAsync(GetUri(path));
			return await response.Content.ReadAsStringAsync();
		}

		private async Task<string> Post(string path, IEnumerable<KeyValuePair<string, string>> data)
		{
			var response = await _httpClient.PostAsync(GetUri(path), new FormUrlEncodedContent(data));
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<ReadOnlyCollection<Category>> GetCategories()
		{
			var response = await Get("/api/Inventory/GetCategories");

			return JsonConvert.DeserializeObject<ReadOnlyCollection<Category>>(response);
		}

		public async Task<QueryResult> ExecuteCustomScriptQuery(string script)
		{
			var response = await Post("/api/Dashboards/ExecuteCustomScriptQuery",
				new Dictionary<string, string> { { "script", script } }
			);

			return JsonConvert.DeserializeObject<QueryResult>(response);
		}

		public async Task UpdateCategory(Category category)
		{
			await Post("/api/Inventory/UpdateCategory",
				new Dictionary<string, string> { { "category", JsonConvert.SerializeObject(category) } }
			);
		}

		public async Task<Category> CreateCategory(string categoryName)
		{
			var response = await Post("/api/Inventory/CreateCategory",
				new Dictionary<string, string> { { "categoryName", categoryName } }
			);

			return JsonConvert.DeserializeObject<Category>(response);
		}

		public async Task DeleteCategoryById(Guid categoryId)
		{
			await Post("/api/Inventory/DeleteCategoryById",
				new Dictionary<string, string> { { "categoryId", categoryId.ToString("D") } }
			);
		}

		public void Dispose()
		{
			_httpClient.Dispose();
			_requestHandler.Dispose();
		}
	}
}
