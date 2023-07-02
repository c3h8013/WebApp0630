//using Newtonsoft.Json;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApp0630.Models;




namespace WebApp0630.Controllers
{
    public class WeatherController : ApiController
    {

        private readonly HttpClient _httpClient;

        public WeatherController()
        {
            _httpClient = new HttpClient();
        }


        // GET: Weather
        public async Task<IEnumerable<Weather36HourViewModel>> GetData()
        {
            // 先取得最新的資料
            string url = "https://opendata.cwb.gov.tw/api/v1/rest/datastore/F-C0032-001?Authorization=CWB-0F81C956-3210-42C0-9F8A-5033C96EBA53";

            string result = string.Empty;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Handle error response
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            // 資料轉物件
            RootObject root = JsonConvert.DeserializeObject<RootObject>(result);

            // 物件處理 資料攤平
            var datetimeNow = DateTime.Now;
            var flattenedData = root.Records.Location
            .SelectMany(loc => loc.WeatherElement
                .SelectMany(we => we.Time
                    .Select(t => new Weather36HourViewModel()
                    {
                        DatasetDescription = root.Records.DatasetDescription,
                        LocationName = loc.LocationName,
                        ElementName = we.ElementName,
                        StartTime = t.StartTime,
                        EndTime = t.EndTime,
                        ParameterName = t.Parameter.ParameterName,
                        ParameterValue = t.Parameter.ParameterValue,
                        ParameterUnit = t.Parameter.ParameterUnit
                    })
                )
            );

            // 資料儲存

            // 回傳資料
            return flattenedData;
        }
    }
}