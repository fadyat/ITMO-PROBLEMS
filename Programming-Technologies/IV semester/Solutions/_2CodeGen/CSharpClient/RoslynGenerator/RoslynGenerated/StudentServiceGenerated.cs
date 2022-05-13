namespace RoslynGenerator.RoslynGenerated
{
    using System.Web;
    using Newtonsoft.Json;
    using System.Text;

    public static class StudentServiceGenerated
    {
        private static readonly HttpClient Client = new HttpClient();
        private const string LocalHost = "http://localhost:8080";
        public static List<Student>? getStudents(List<int>? ids)
        {
            var url = $"{LocalHost}/api/student";
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            ids?.ForEach(id => query.Add("id", $"{id}"));
            uriBuilder.Query = query.ToString();
            url = uriBuilder.Uri.ToString();
            var response = Client.GetAsync(url);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<Student>>(responseString);
        }

        public static Student? getStudent(int? id)
        {
            var url = $"{LocalHost}/api/student/{id}";
            var response = Client.GetAsync(url);
            var responseString = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Student>(responseString);
        }

        public static void saveStudent(Student? student)
        {
            var content = new StringContent(JsonConvert.SerializeObject(student), Encoding.UTF8, "application/json");
            Client.PostAsync($"{LocalHost}/api/student", content);
        }
    }
}