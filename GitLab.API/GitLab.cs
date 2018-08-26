using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitLab.API
{
    public abstract class GitLab
    {
        protected string accessToken;
        protected string hostUrl;
        protected string baseUrl;
    }
}