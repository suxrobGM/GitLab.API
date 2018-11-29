using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GitLab.API
{
    public class Repository 
    {
        private RepositoryFile[] repositoryFiles;

        public RepositoryTree RepositoryTree { get; internal set; }
        public RepositoryFile[] RepositoryFiles { get => repositoryFiles; }

        internal GitLabClient client;

        internal Repository() { }

        public async Task<byte[]> GetArchiveAsync(long projectId, ArchiveFormat archiveFormat = ArchiveFormat.Zip, string Sha = null)
        {


            if (Sha != null)
                client.response = await client.request.GetAsync($"projects/{projectId}/repository/archive.{ApiTools.GetArchiveFormat(archiveFormat)}?sha={Sha}");
            else
                client.response = await client.request.GetAsync($"projects/{projectId}/repository/archive.{ApiTools.GetArchiveFormat(archiveFormat)}");

            client.content = client.response.Content;
            byte[] archive = await client.content.ReadAsByteArrayAsync();
            return archive;
        }
    }
}
