using ComWthRestApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using RestSharp;

namespace ComWthRestApi.Controllers
{
    
    public class HomeController : Controller
    {

        String UrlInfo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            using (var httpClient = new HttpClient())
            {
                

                var response = await httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon/snom");

                if (!response.IsSuccessStatusCode)
                {
                    return View(new PokeData() { name = "undefinned" });

                }
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<PokeData>(json);

                return View(data);

            }
        }

        [Route("/{UrlInfo}")]
        public async Task<IActionResult> IndexWthData(String? UrlInfo)
        {
            using (var httpClient = new HttpClient())
            {
                if(UrlInfo == null) {
                    return View("/Views/Home/Index.cshtml",new PokeData() { name = "undefinned" });
                }

                var response = await httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon/" +UrlInfo);

                if (!response.IsSuccessStatusCode)
                {
                    return View("/Views/Home/Index.cshtml",new PokeData() { name = "undefinned" });

                }
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<PokeData>(json);

                return View("/Views/Home/Index.cshtml",data);

            }
        }


        [Route("/restsharp/{UrlInfo}")]
        public async Task<IActionResult> IndexWthRestSharp(String? UrlInfo)
        {
            var client = new RestClient("https://pokeapi.co/api/v2/");
            var request = new RestRequest("/pokemon/" + UrlInfo, Method.Get);
            

            
                if (UrlInfo == null)
                {
                    return View("/Views/Home/Index.cshtml", new PokeData() { name = "undefinned" });
                }

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return View("/Views/Home/Index.cshtml", new PokeData() { name = "undefinned" });

                }


                var json = response.Content;
                var data = JsonSerializer.Deserialize<PokeData>(json);

                return View("/Views/Home/Index.cshtml", data);

            
        }


        [Route("/raw/{UrlInfo}")]
        public async Task<String> raw(String? UrlInfo)
        {
            using (var httpClient = new HttpClient())
            {
                if (UrlInfo == null)
                {
                    return "empty";
                }

                var response = await httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon/" + UrlInfo);

                if (!response.IsSuccessStatusCode)
                {
                    return "empty";

                }
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<PokeData>(json);

                return json;

            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}