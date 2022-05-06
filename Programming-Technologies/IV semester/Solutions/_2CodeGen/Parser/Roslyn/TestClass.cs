using System.Web;

namespace Roslyn                                                                             
{                                                                                            
    public static class StudentService                                                       
    {                                                                                        
        private static readonly HttpClient Client = new ();                                  
        private const string LocalHost = "http://localhost:8080";
        
        public static async Task<HttpResponseMessage> getStudents(List<int>? ids = null)
        {
            const string url = $"{LocalHost}/api/student";
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (ids == null) return await Client.GetAsync(url);
            
            ids.ForEach(id => query.Add("id",  $"{id}"));
            uriBuilder.Query = query.ToString();
            return await Client.GetAsync(uriBuilder.ToString());            
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