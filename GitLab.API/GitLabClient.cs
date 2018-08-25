using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GitLab.API
{
    public enum VersionApi
    {
        v1 = 1,
        v2,
        v3,
        v4
    }

    public enum AccessLevel
    {
        Guest = 10,
        Reporter = 20,
        Developer = 30,
        Maintainer = 40,
        Owner = 50 // Only valid for groups
    }

    public class GitLabClient 
    {
        private string accessToken;
        private string baseUrl;
        private HttpClient request;
        private HttpResponseMessage response;
        private HttpContent content;

        public VersionApi VersionApi { get; set; }
        public string Token { get => accessToken; }

        public GitLabClient(string accessToken, string hostUrl = null, VersionApi versionApi = VersionApi.v4)
        {
            this.VersionApi = versionApi;
            this.accessToken = accessToken;

            if (hostUrl == null)
                baseUrl = $"https://gitlab.com/api/{VersionApi}/";
            else
                baseUrl = $"{hostUrl}/api/{VersionApi}/";

            this.request = new HttpClient();
            this.request.BaseAddress = new Uri(baseUrl);
            this.request.DefaultRequestHeaders.Add("Private-Token", this.accessToken);

        }

        public async Task<JObject> GetProjectDataAsync(long projectId)
        {
            response = await request.GetAsync($"projects/{projectId}");
            content = response.Content;
            var jObject = JObject.Parse(await content.ReadAsStringAsync());
            return jObject;
        }

        public async Task<Project> GetProjectAsync(long projectId)
        {
            response = await request.GetAsync($"projects/{projectId}");
            content = response.Content;
            var project = new Project();
            var jObject = JObject.Parse(await content.ReadAsStringAsync());

            project.Id = Convert.ToInt64(jObject["id"]);
            project.Name = Convert.ToString(jObject["name"]);
            project.NameWithNamespace = Convert.ToString(jObject["name_with_namespace"]);
            project.Path = Convert.ToString(jObject["path"]);
            project.PathWithNamespace = Convert.ToString(jObject["path_with_namespace"]);
            project.DefaultBranch = Convert.ToString(jObject["default_branch"]);
            project.Description = Convert.ToString(jObject["description"]);
            project.SshUrl = Convert.ToString(jObject["ssh_url_to_repo"]);
            project.AvatarUrl = Convert.ToString(jObject["avatar_url"]);
            project.HttpUrl = Convert.ToString(jObject["http_url_to_repo"]);
            project.WebUrl = Convert.ToString(jObject["web_url"]);
            project.ReadmeUrl = Convert.ToString(jObject["readme_url"]);
            project.ForksCount = Convert.ToInt64(jObject["forks_count"]);
            project.StarCount = Convert.ToInt64(jObject["star_count"]);

            var strArrays = new List<string>();
            foreach (var item in jObject["tag_list"].ToArray())
            {
                strArrays.Add(item.ToString());
            }       
            project.TagList = strArrays.ToArray();
            project.Visibility = Convert.ToString(jObject["visibility"]);
            project.CreatedDate = DateTime.Parse(jObject["created_at"].ToString());
            project.LastActivityDate = DateTime.Parse(jObject["last_activity_at"].ToString());

            return project;
        }

        public static async Task<Project> GetProjectAsync(long projectId, string hostUrl = null, VersionApi versionApi = VersionApi.v4)
        {
            string baseUrl;
            if (hostUrl == null)
                baseUrl = $"https://gitlab.com/api/{versionApi}/";
            else
                baseUrl = $"{hostUrl}/api/{versionApi}/";

            var request = new HttpClient();
            request.BaseAddress = new Uri(baseUrl);

            var response = await request.GetAsync($"projects/{projectId}");
            var content = response.Content;
            var project = new Project();
            var jObject = JObject.Parse(await content.ReadAsStringAsync());

            project.Id = Convert.ToInt64(jObject["id"]);
            project.Name = Convert.ToString(jObject["name"]);
            project.NameWithNamespace = Convert.ToString(jObject["name_with_namespace"]);
            project.Path = Convert.ToString(jObject["path"]);
            project.PathWithNamespace = Convert.ToString(jObject["path_with_namespace"]);
            project.DefaultBranch = Convert.ToString(jObject["default_branch"]);
            project.Description = Convert.ToString(jObject["description"]);
            project.SshUrl = Convert.ToString(jObject["ssh_url_to_repo"]);
            project.AvatarUrl = Convert.ToString(jObject["avatar_url"]);
            project.HttpUrl = Convert.ToString(jObject["http_url_to_repo"]);
            project.WebUrl = Convert.ToString(jObject["web_url"]);
            project.ReadmeUrl = Convert.ToString(jObject["readme_url"]);
            project.ForksCount = Convert.ToInt64(jObject["forks_count"]);
            project.StarCount = Convert.ToInt64(jObject["star_count"]);

            var strArrays = new List<string>();
            foreach (var item in jObject["tag_list"].ToArray())
            {
                strArrays.Add(item.ToString());
            }
            project.TagList = strArrays.ToArray();
            project.Visibility = Convert.ToString(jObject["visibility"]);
            project.CreatedDate = DateTime.Parse(jObject["created_at"].ToString());
            project.LastActivityDate = DateTime.Parse(jObject["last_activity_at"].ToString());

            return project;
        }

        public async Task<Project[]> GetProjectsAsync(bool ownedProjects = true)
        {
            if(ownedProjects)
                response = await request.GetAsync($"projects?owned=yes");
            else
                response = await request.GetAsync($"projects?owned=no");
            
            content = response.Content;
            var jArray = JArray.Parse(await content.ReadAsStringAsync());
            var idList = new List<long>();

            foreach (var item in jArray)
            {
                idList.Add(long.Parse(item["id"].ToString()));
            }

            var projectsList = new List<Project>();

            foreach (var id in idList)
            {
                projectsList.Add(await this.GetProjectAsync(id));
            }

            return projectsList.ToArray();
        }

        public async Task<long> GetProjectIdAsync(string projectName)
        {
            response = await request.GetAsync($"projects?owned=yes");
            content = response.Content;
            var jArray = JArray.Parse(await content.ReadAsStringAsync());

            foreach (var item in jArray)
            {
                if (item["name"].ToString() == projectName)
                {
                    return Convert.ToInt64(item["id"].ToString());
                }
            }

            return -1;
        }

        public async Task<JArray> GetRepositoryTreeAsync(long projectID, bool recursive = false)
        {
            if (recursive)
                response = await request.GetAsync($"projects/{projectID}/repository/tree?recursive=yes");
            else
                response = await request.GetAsync($"projects/{projectID}/repository/tree");

            content = response.Content;
            var jArray = JArray.Parse(await content.ReadAsStringAsync());
            return jArray;
        }
    }
}
