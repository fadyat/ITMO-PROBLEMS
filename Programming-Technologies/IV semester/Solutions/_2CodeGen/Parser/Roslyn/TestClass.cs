namespace TestClass
{
    public static class StudentService
    {
        private static readonly HttpClient _client = new ();
        
        public static async Task<HttpResponseMessage> getStudent(int id)
        {
            return await _client.GetAsync($"/api/student/{id}");
        }
    }
}