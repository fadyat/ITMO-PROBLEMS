namespace Roslyn.RoslynGenerated
{
    using System.Web;

    public static class StudentServiceGenerated
    {
        private static readonly HttpClient Client = new HttpClient();
        private const string LocalHost = "http://localhost:8080";
        public static async Task<HttpResponseMessage> getStudents(List<int> ids)
        {
            var url = $"{LocalHost}/api/student";
            if (!ids.Any())
            {
                return await Client.GetAsync(url);
            }

            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            ids.ForEach(id => query.Add("id", $"{id}"));
            uriBuilder.Query = query.ToString();
            url = uriBuilder.Uri.ToString();
            return await Client.GetAsync(url);
        }

        public static async Task<HttpResponseMessage> getStudent(int id)
        {
            return await Client.GetAsync($"{LocalHost}/api/student/{id}");
        }

        public static async Task<HttpResponseMessage> saveStudent(StringContent content)
        {
            return await Client.PostAsync($"{LocalHost}/api/student", content);
        }
    }
}