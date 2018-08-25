using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestGitLabAPI
{
    class Program
    {
        private static string Token = "yHe-Kh9yJ-Uqzy22H3dZ";
        static void Main(string[] args)
        {
            GitLabClient client = new GitLabClient(Token);
            //Console.WriteLine(client.GetProjectDataAsync(6500892).Result.ToString());
            //Console.WriteLine(client.GetProjectIdAsync("Economic Crisis").Result.ToString());
            Console.WriteLine(client.GetRepositoryTreeAsync(6500892, true).Result.ToString());

            Console.WriteLine("Finished!");
            Console.ReadLine();
        }
    }
}
