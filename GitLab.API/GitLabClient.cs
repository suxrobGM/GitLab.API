using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GitLab.API
{
    public class GitLabClient : IDisposable
    {     
        private ProjectsList projects;
        private string accessToken;
        private string hostUrl;
        private string baseUrl;
        internal HttpClient request;
        internal HttpResponseMessage response;
        internal HttpContent content;

        public VersionApi VersionApi { get; private set; }
        public string Token { get => this.accessToken; }
        public string HostUrl { get => this.hostUrl; }
        public ProjectsList Projects { get => projects; }

        public GitLabClient(string accessToken, string hostUrl = "https://gitlab.com", VersionApi versionApi = VersionApi.v4)
        {
            this.VersionApi = versionApi;
            this.accessToken = accessToken;
            this.hostUrl = hostUrl;
            this.baseUrl = $"{hostUrl}/api/{versionApi}/";

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
            project.Repository = new Repository();
            

            foreach (var tag in jObject["tag_list"])
            {
                project.TagList.Add(tag.ToString());
            }
            project.Repository.client = this;

            //TODO for non simple request
            //project.Visibility = ApiTools.GetProjecVisibility(jObject["visibility"].ToString());          
            return project;
        }

        private async Task<ProjectsList> GetProjectsAsync()
        {
            response = await request.GetAsync($"projects?owned=yes");

            content = response.Content;
            var jArray = JArray.Parse(await content.ReadAsStringAsync());
            var idList = new List<long>();
            var projectsList = new ProjectsList();

            foreach (var item in jArray)
            {
                idList.Add(long.Parse(item["id"].ToString()));
            }           

            foreach (var id in idList)
            {
                projectsList.projectsList.Add(await this.GetProjectAsync(id, true));
            }

            return projectsList;
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
