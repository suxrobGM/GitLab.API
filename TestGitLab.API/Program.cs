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
            using (var client = new GitLabClient(Token))
            {                                    
                foreach (var project in client.Projects)
                {
                    Console.WriteLine(project.Path);
                }                 
            }                     

            var myProject = GitLabClient.GetProjectAsync(8088678, "https://gitlab.com").Result;
            
            Console.WriteLine(myProject.Name);
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }
    }
}
