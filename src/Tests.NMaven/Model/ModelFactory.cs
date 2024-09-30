using Microsoft.Build.Framework;
using Moq;
using NMaven.Model;

namespace Tests.NMaven.Model
{
    public static class ModelFactory 
    {
        public static MavenRepository CreateMavenRepository(string name, string url, string? username = null, string? password = null)
        {
            var task = new Mock<ITaskItem>();

            task.Setup(i => i.GetMetadata("Identity")).Returns(name);
            task.Setup(i => i.GetMetadata("Url")).Returns(url);
            task.Setup(i => i.GetMetadata("Username")).Returns(username ?? string.Empty);
            task.Setup(i => i.GetMetadata("Password")).Returns(password ?? string.Empty);

            return new MavenRepository(task.Object);
        }

        public static MavenReference CreateMavenReference(string artifactId, string groupId, string version, bool overwrite = false, string? classifier = null, string? type = null)
        {
            var task = new Mock<ITaskItem>();

            task.Setup(i => i.GetMetadata("Identity")).Returns(artifactId);
            task.Setup(i => i.GetMetadata("GroupId")).Returns(groupId);
            task.Setup(i => i.GetMetadata("Version")).Returns(version);
            task.Setup(i => i.GetMetadata("Overwrite")).Returns(overwrite.ToString());
            task.Setup(i => i.GetMetadata("Classifier")).Returns(classifier ?? string.Empty);
            task.Setup(i => i.GetMetadata("Type")).Returns(type ?? string.Empty);

            return new MavenReference(task.Object);
        }

        public static NMavenDeployment CreateNMavenDeployment(string name, string artifactId, string files, string destination)
        {
            var task = new Mock<ITaskItem>();

            task.Setup(i => i.GetMetadata("Identity")).Returns(name);
            task.Setup(i => i.GetMetadata("ArtifactId")).Returns(artifactId);
            task.Setup(i => i.GetMetadata("Files")).Returns(files);
            task.Setup(i => i.GetMetadata("Destination")).Returns(destination);

            return new NMavenDeployment(task.Object);
        }
    }
}
