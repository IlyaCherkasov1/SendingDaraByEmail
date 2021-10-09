using ChoETL;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace SendingDataByEmail
{
    public class ApiHelper
    {
        private readonly IApisService _apisService;

        public ApiHelper(IApisService apisService)
        {
            _apisService = apisService;
        }

        public async Task GetApiData(Job job)
        {
            string json = await _apisService.GetAPIEndpoint(job.ApiId);
            string path = job.Name + ".json";
            CreateFile(path);

            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            jsonStringToCSV(job.Name);

        }

        private void jsonStringToCSV(string fileName)
        {
            using (var r = new ChoJSONReader(fileName + ".json"))
            {
                using (var w = new ChoCSVWriter(fileName + ".csv").WithFirstLineHeader())
                {
                    w.Write(r);
                }
            }
        }

        private static void CreateFile(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                var myfile = System.IO.File.Create(path);
                myfile.Close();
            }
        }

    }
}
