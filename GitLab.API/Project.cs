using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GitLab.API
{
    public class Project : GitLab
    {
        public long Id { get; internal set; }
        public string Name { get; internal set; }
        public string NameWithNamespace { get; internal set; }
        public string Description { get; internal set; }
        public string Path { get; internal set; }
        public string PathWithNamespace { get; internal set; }
        public string DefaultBranch { get; internal set; }
        public long ForksCount { get; internal set; }
        public long StarCount { get; internal set; }
        public List<string> TagList { get; internal set; }
        public DateTime CreatedDate { get; internal set; }
        public DateTime LastActivityDate { get; internal set; }
        public string SshUrl { get; internal set; }
        public string HttpUrl { get; internal set; }
        public string WebUrl { get; internal set; }
        public string ReadmeUrl { get; internal set; }
        public string AvatarUrl { get; internal set; }
        public Repository Repository { get; internal set; }
        public ProjecVisibility Visibility { get; internal set; }


        internal Project() { }
    }
}
