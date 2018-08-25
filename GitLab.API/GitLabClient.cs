using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace GitLab.API
{
    enum VersionApi
    {
        v1 = 1,
        v2,
        v3,
        v4
    }

    enum AccessLevel
    {
        Guest = 10,
        Reporter = 20,
        Developer = 30,
        Maintainer = 40,
        Owner = 50 // Only valid for groups
    }

    class GitLabClient
    {
        private string accessToken;
        private string baseUrl;
        private HttpClient request;
        private HttpResponseMessage response;
        private HttpContent content;

        public VersionApi VersionApi { get; set; }
        public string Token { get => accessToken; }

        public GitLabClient(string accessToken, string hostName = null, VersionApi versionApi = VersionApi.v4)
        {
            this.VersionApi = versionApi;
            this.accessToken = accessToken;

            if (hostName == null)
                baseUrl = $"https://gitlab.com/api/{VersionApi}/";
            else
                baseUrl = $"{hostName}/api/{VersionApi}/";

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
