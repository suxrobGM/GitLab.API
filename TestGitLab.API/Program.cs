using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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
                var repositoryArchive = client.Projects.ElementAt(0).Repository.GetArchiveAsync(client.Projects.ElementAt(0).Id).Result;
                File.WriteAllBytes("Test.zip", repositoryArchive);
            }
                     
            Console.WriteLine("Finished!");
            Console.ReadLine();
        }
    }
}
