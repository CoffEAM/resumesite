using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        static async Task<string> Main()
        {
            var prompt = new
            {
                modelUri = "gpt://b1g0mb6l3sph3i31ncvk/yandexgpt-lite",
                completionOptions = new
                {
                    stream = false,
                    temperature = 0.6,
                    maxTokens = "2000"
                },
                messages = new List<object>
        {
            new
            {
                role = "system",
                text = "Ты ассистент сайта, будешь генерировать правдоподобное описание человека для его резюме на работу. Генерировать будешь на сайте для создания резюме" +
                "Одним текстом, без отступов и выделений, предложения сплошником"
            },
            new
            {
                role = "user",
                text = "Привет! Мне нужна твоя помощь. Помоги сгенерировать мне описание для резюме про меня, правдоподобное, чтобы приняли на работу. Пиши" +
                "пример описания, так чтобы не было понятно что это написал ты, от лица человека про которого описание. Пиши текст без отступов и без упоминания о себе." +
                "Также не говори про то где родился человек, про места в принципе. В тексте не должно быть предложений по типу 'Вот вариант текста, который мог бы написать Азиз'"
            },
            new
            {
                role = "assistant",
                text = "Привет! Чтобы я смог тебе сгенерировать описание, передай мне немного данных о себе, чтобы я смог их использовать."
            },
            new
            {
                role = "user",
                text = "Хорошо, мое имя Азиз, мне 16 лет, люблю программировать и изучать инностранные языки(Анлийский). До этого нигде не работал."
            }
        }
            };

            var url = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";
            var apiKey = "AQVN3UdrPeRF6iO12fZryA9yoY9ZHOKJPFl9iSSl";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Api-Key", apiKey);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(prompt), System.Text.Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
        }

        static double Parse(string expression)
        {
            return CSharpScript.EvaluateAsync<double>(expression).Result;
        }

        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult Create_Resume(string username, string usersurname, string user_birthday, string user_city)
        //{
        //    return Content($"Hello {username} {usersurname}");
        //}

        public IActionResult Create_Resume()
        {
            ViewBag.Message = Main().Result;
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult Feedback()
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