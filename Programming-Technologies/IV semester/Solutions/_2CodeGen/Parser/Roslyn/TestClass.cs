// namespace Roslyn
// {
//     public static class StudentService
//     {
//         private static readonly HttpClient Client = new ();
//         private const string LocalHost = "http://localhost:8080";
//
//         public static async Task<HttpResponseMessage> getStudents(List<int>? ids = null)
//         {
//             var pickedIds = ids?.ConvertAll(x => x.ToString()) ?? new List<string>();
//             var stringOfIds = string.Join(",", pickedIds);
//             var queryArgs = string.Empty;
//             if (stringOfIds != string.Empty) queryArgs += $"id={stringOfIds}";
//             return await Client.GetAsync($"{LocalHost}/api/student?{queryArgs}");
//         }
//
//         public static async Task<HttpResponseMessage> getStudent(int id)
//         {
//             return await Client.GetAsync($"{LocalHost}/api/student/{id}");
//         }
//
//         public static async Task<HttpResponseMessage> saveStudent(StringContent content)
//         {
//             return await Client.PostAsync($"{LocalHost}/api/student", content);
//         }
//     }
// }


