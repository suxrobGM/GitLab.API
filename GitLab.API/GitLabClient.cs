using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GitLab.API
{
    public class GitLabClient : GitLab, IDisposable
    {     
        private Project[] projects;
        private HttpClient request;
        private HttpResponseMessage response;
        private HttpContent content;

        public VersionApi VersionApi { get; private set; }
        public string Token { get => base.accessToken; }
        public string HostUrl { get => base.hostUrl; }
        public Project[] Projects { get => projects; }

        public GitLabClient(string accessToken, string hostUrl = "https://gitlab.com", VersionApi versionApi = VersionApi.v4)
        {
            this.VersionApi = versionApi;
            this.accessToken = accessToken;
            base.hostUrl = hostUrl;                  
            base.baseUrl = $"{hostUrl}/api/{versionApi}/";
            
            this.request = new HttpClient();
            this.request.BaseAddress = new Uri(baseUrl);
            this.request.DefaultRequestHeaders.Add("Private-Token", this.accessToken);
            this.projects = this.GetProjectsAsync().Result;
        }

        private async Task<Project> GetProjectAsync(long projectId, bool simple = true)
        {               
            if(simple)
                response = await request.GetAsync($"projects/{projectId}?simple=yes");
            else
                response = await request.GetAsync($"projects/{projectId}?simple=no");

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
            project.CreatedDate = DateTime.Parse(jObject["created_at"].ToString());
            project.LastActivityDate = DateTime.Parse(jObject["last_activity_at"].ToString());

            foreach (var tag in jObject["tag_list"])
            {
                project.TagList.Add(tag.ToString());
            }

            //TODO for non simple request
            //project.Visibility = ApiTools.GetProjecVisibility(jObject["visibility"].ToString());          
            return project;
        }

        private async Task<Project[]> GetProjectsAsync()
        {
            response = await request.GetAsync($"projects?owned=yes");

            content = response.Content;
            var jArray = JArray.Parse(await content.ReadAsStringAsync());
            var idList = new List<long>();
            var projectsList = new List<Project>();

            foreach (var item in jArray)
            {
                idList.Add(long.Parse(item["id"].ToString()));
            }           

            foreach (var id in idList)
            {
                projectsList.Add(await this.GetProjectAsync(id, true));
            }

            return projectsList.ToArray();
        }

        public static async Task<Project> GetProjectAsync(long projectId, string hostUrl = "https://gitlab.com", VersionApi versionApi = VersionApi.v4)
        {
            string baseUrl = $"{hostUrl}/api/{versionApi}/";                   

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
            project.CreatedDate = DateTime.Parse(jObject["created_at"].ToString());
            project.LastActivityDate = DateTime.Parse(jObject["last_activity_at"].ToString());

            foreach (var tag in jObject["tag_list"])
            {
                    project.TagList.Add(tag.ToString());
            }
       
            return project;
        }

        public void Dispose()
        {
            this.request.Dispose();
            this.response.Dispose();
            this.content.Dispose();
            this.projects = null;
        }
    }
}
