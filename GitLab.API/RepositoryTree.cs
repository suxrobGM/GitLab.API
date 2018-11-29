using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitLab.API
{
    public class RepositoryTree 
    {
        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public string Path { get; internal set; }
        public string Mode { get; internal set; }

        internal RepositoryTree() { }
    }
}
