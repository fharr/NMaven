using System.IO;
using System.Text;
using Microsoft.Build.Framework;

namespace NMaven.Model
{
    public class MavenReference : TaskItemBased
    {
        private const string DEFAULT_TYPE = "jar";

        public MavenReference(ITaskItem item)
            : base(item)
        { }

        public string ArtifactId => this.GetItemMetadata("Identity");
        public string GroupId => this.GetItemMetadata();
        public string Version => this.GetItemMetadata();
        public string Classifier => this.GetItemMetadata();
        public string Type
        {
            get
            {
                var type = this.GetItemMetadata();

                return string.IsNullOrWhiteSpace(type)
                    ? DEFAULT_TYPE
                    : type;
            }
        }

        /// <summary>
        /// When <c>true</c>, the artifact will be redownloaded and overwritten if it already exists on disk.
        /// </summary>
        public bool Overwrite => bool.TryParse(this.GetItemMetadata(), out var b) && b;

        public string ArtifactFileName
        {
            get
            {
                var builder = new StringBuilder($"{this.ArtifactId}-{this.Version}");

                if (!string.IsNullOrWhiteSpace(this.Classifier))
                {
                    builder.Append($"-{this.Classifier}");
                }

                builder.Append($".{this.Type}");

                return builder.ToString();
            }
        }

        public string GetRepositoryUrl(MavenRepository repository)
        {
            var url = string.Join("/",
                repository.Url,
                this.GroupId.Replace('.', '/'),
                this.ArtifactId,
                this.Version,
                this.ArtifactFileName);

            return url;
        }

        public DirectoryInfo GetArtifactDirectory(DirectoryInfo nmvnPackageRoot)
        {
            var directoryInfo = new DirectoryInfo(Path.Combine(nmvnPackageRoot.FullName, this.ArtifactId, this.Version));

            return directoryInfo;
        }

        public FileInfo GetArtifactFilePath(DirectoryInfo nmvnPackageRoot)
        {
            var fileInfo = new FileInfo(Path.Combine(this.GetArtifactDirectory(nmvnPackageRoot).FullName, this.ArtifactFileName));

            return fileInfo;
        }
    }
}
