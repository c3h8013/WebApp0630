using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using WebApp0630.Models;




namespace WebApp0630.Controllers
{
    public class WeatherController : ApiController
    {
        SqlConnection con = new SqlConnection("server=.; database=DB_Weather; Integrated Security=true;");
        private readonly HttpClient _httpClient;

        public WeatherController()
        {
            _httpClient = new HttpClient();
        }


        // GET: api/Weather/GetData
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
                        ParameterUnit = t.Parameter.ParameterUnit,
                        AddTime = datetimeNow
                    })
                )
            );

            // 物件轉table
            DataTable dt = ConvertToDataTable(flattenedData);


            // 資料儲存 連線本機DB、本機登入不用帳號密碼 、 資料庫名稱 DB_Weather
            using (SqlConnection conn = new SqlConnection("Data Source=.;Initial Catalog=DB_Weather;Integrated Security=True;"))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                //SqlBulkCopy批次處理新增
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, trans))
                {
                    bulkCopy.DestinationTableName = "[dbo].[Weather36Hour]";
                    bulkCopy.ColumnMappings.Add("DatasetDescription", "DatasetDescription");
                    bulkCopy.ColumnMappings.Add("LocationName", "LocationName");
                    bulkCopy.ColumnMappings.Add("ElementName", "ElementName");
                    bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                    bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
                    bulkCopy.ColumnMappings.Add("ParameterName", "ParameterName");
                    bulkCopy.ColumnMappings.Add("ParameterValue", "ParameterValue");
                    bulkCopy.ColumnMappings.Add("ParameterUnit", "ParameterUnit");
                    bulkCopy.ColumnMappings.Add("AddTime", "AddTime");
                    bulkCopy.WriteToServer(dt);
                }

                trans.Commit();
            }

            // 回傳資料
            return flattenedData;
        }

        public static DataTable ConvertToDataTable<T>(IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}