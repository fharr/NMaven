using System;
using Microsoft.Build.Framework;

namespace NMaven.Model
{
    public class MavenRepository : TaskItemBased
    {
        public MavenRepository(ITaskItem item)
            : base(item)
        { }

        public string Name => this.GetItemMetadata("Identity");
        public string Url => this.GetItemMetadata();
        public string Username => this.GetItemMetadata();
        public string Password => this.GetItemMetadata();

        public string GetBasicAuthorizationHeader()
        {
            if (string.IsNullOrWhiteSpace(this.Username) || string.IsNullOrWhiteSpace(this.Password))
            {
                return null;
            }

            var auth = $"{Username}:{Password}";
            var auth64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));

            return auth64;
        }
    }
}
