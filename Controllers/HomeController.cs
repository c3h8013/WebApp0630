using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp0630.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;
        public HomeController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44310"); // 設定API的基礎位址
        }

        public async Task<ActionResult> Index()
        {
            //ViewBag.Title = "Home Page";

            //return View();

            HttpResponseMessage response = await httpClient.GetAsync("/api/Weather/GetData"); // 發送GET請求到API的特定端點
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                return View("Index", responseData); // 將資料傳遞給檢視
            }
            else
            {
                // 處理錯誤情況
                return View("Error");
            }
        }
    }
}
