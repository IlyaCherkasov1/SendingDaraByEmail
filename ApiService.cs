using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SendingDataByEmail
{

    public interface IApisService
    {
        Task<string> GetAPIEndpoint(int id);
    }
    public class ApisService : IApisService
    {
        private readonly HttpClient _httpClient;

        public ApisService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetAPIEndpoint(int id)
        {
            API api = GetApiById(id);
            string url = api.Url.Replace("%3CREQUIRED%3E", api.City);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),

                Headers =
                {
                    { "x-rapidapi-host", "weatherapi-com.p.rapidapi.com" },
                    { "x-rapidapi-key", "54e6c09eb3msha00f529fda2d38fp12a3b0jsn60963a951deb" },
                },

            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }

        public API GetApiById(int id)
        {
            API api = null;
            string sqlExpression = $"SELECT * FROM Api where ApiId={id}";
            using (var connection = new SqliteConnection("Data Source=API.db"))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(1);
                            string url = reader.GetString(2);
                            string city = reader.GetString(3);

                            api = new API(id, name, url, city);
                        }
                    }
                }
            }
            return api;
        }
    }
}
