using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitLab.API;

namespace TestGitLab.API
{
    class Program
    {
        private static string Token = "yHe-Kh9yJ-Uqzy22H3dZ";

        static void Main(string[] args)
        {
            //try
            //{
                using (var client = new GitLabClient(Token))
                {
                    //Console.WriteLine(client.GetProjectDataAsync(6500892).Result);
                    //Project project = client.GetProjectAsync(6500892).Result;            
                    //Console.WriteLine(project.CreatedDate);          
                    //Console.WriteLine(client.GetProjectIdAsync("Economic Crisis").Result.ToString());
                    //Console.WriteLine(client.GetRepositoryTreeAsync(6500892, true).Result.ToString());          
                    
                    foreach (var project in client.Projects)
                    {
                        Console.WriteLine(project.Path);
                    }                 
                    Console.WriteLine("Finished!");
                    Console.ReadLine();
                }
            //}
            //catch(Exception)
            //{
                //Console.WriteLine("Network connection error");
                //Console.ReadLine();
            //}
        }
    }
}
