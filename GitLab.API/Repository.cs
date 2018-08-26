using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GitLab.API
{
    public class Repository : GitLab
    {
        public RepositoryTree RepositoryTree { get; internal set; }
        internal Repository() { }

        /*public Stream GetArchive(long projectId)
        {

        }*/
    }
}
