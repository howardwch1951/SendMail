using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeerPark_StatusCheck
{
    class api_call
    {
        public static string CallPostController(string controller_name, string jsonString)
        {
            string api_url = ConfigurationManager.ConnectionStrings["api_url"].ConnectionString;

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(api_url + controller_name);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = jsonString;

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    //var result = streamReader.ReadToEnd()
                    string json = streamReader.ReadToEnd().ToString();

                    return json;
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
