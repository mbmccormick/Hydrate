using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hydrate.API.Common;
using Hydrate.API.Models;
using Newtonsoft.Json;

namespace Hydrate.API
{
    public class ServiceClient
    {
        private string serverAddress = "node-hnapi.azurewebsites.net";

        public List<string> PostHistory;
        public int MaxPostHistory = 250;

        public string NextTopPosts = null;

        public ServiceClient(bool debug)
        {
            PostHistory = IsolatedStorageHelper.GetObject<List<string>>("PostHistory");

            if (PostHistory == null)
                PostHistory = new List<string>();

            if (debug == true)
                serverAddress = "node--hnapi-azurewebsites-net-86lzpdm4xgow.runscope.net";
        }

        public async Task GetWaterGoal(Action<GoalResponse> callback)
        {
            HttpWebRequest request = HttpWebRequest.Create("http://" + serverAddress + "/1/user/-/foods/log/water/goal.json") as HttpWebRequest;
            request.Accept = "application/json";

            var response = await request.GetResponseAsync().ConfigureAwait(false);

            Stream stream = response.GetResponseStream();
            UTF8Encoding encoding = new UTF8Encoding();
            StreamReader sr = new StreamReader(stream, encoding);

            JsonTextReader tr = new JsonTextReader(sr);
            GoalResponse data = new JsonSerializer().Deserialize<GoalResponse>(tr);

            tr.Close();
            sr.Dispose();

            stream.Dispose();

            callback(data);
        }

        public async Task GetWaterHistory(Action<HistoryResponse> callback, DateTime date)
        {
            HttpWebRequest request = HttpWebRequest.Create("http://" + serverAddress + "/1/user/-/foods/log/water/" + date.ToString("YYYY-mm-dd") + ".json") as HttpWebRequest;
            request.Accept = "application/json";

            var response = await request.GetResponseAsync().ConfigureAwait(false);

            Stream stream = response.GetResponseStream();
            UTF8Encoding encoding = new UTF8Encoding();
            StreamReader sr = new StreamReader(stream, encoding);

            JsonTextReader tr = new JsonTextReader(sr);
            HistoryResponse data = new JsonSerializer().Deserialize<HistoryResponse>(tr);

            tr.Close();
            sr.Dispose();

            stream.Dispose();

            callback(data);
        }
    }
}
