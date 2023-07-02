using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp0630.Models
{
    public class Weather36HourViewModel
    {

        /// <summary>
        /// 名稱
        /// </summary>
        public string DatasetDescription { get; set; }
        /// <summary>
        /// 臺灣各縣市
        /// </summary>
        public string LocationName { get; set; }
        /// <summary>
        /// 天氣因子
        /// </summary>
        public string ElementName { get; set; }
        /// <summary>
        /// 時間因子
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 時間因子
        /// </summary>
        public DateTime EndTime { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterUnit { get; set; }
        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime AddTime { get; set; }
    }

    public class RootObject
    {
        public bool Success { get; set; }
        public ResultObject Result { get; set; }
        public RecordsObject Records { get; set; }
    }

    public class ResultObject
    {
        public string ResourceId { get; set; }
        public List<FieldObject> Fields { get; set; }
    }

    public class FieldObject
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public class RecordsObject
    {
        public string DatasetDescription { get; set; }
        public List<LocationObject> Location { get; set; }
    }

    public class LocationObject
    {
        public string LocationName { get; set; }
        public List<WeatherElementObject> WeatherElement { get; set; }
    }

    public class WeatherElementObject
    {
        public string ElementName { get; set; }
        public List<TimeObject> Time { get; set; }
    }

    public class TimeObject
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ParameterObject Parameter { get; set; }
    }

    public class ParameterObject
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterUnit { get; set; }
    }
}