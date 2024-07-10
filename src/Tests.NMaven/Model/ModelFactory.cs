﻿using Microsoft.Build.Framework;
using Moq;
using NMaven.Model;

namespace Tests.NMaven.Model
{
    public static class ModelFactory 
    {
        public static MavenRepository CreateMavenRepository(string name, string url)
        {
            var task = new Mock<ITaskItem>();

            task.Setup(i => i.GetMetadata("Identity")).Returns(name);
            task.Setup(i => i.GetMetadata("Url")).Returns(url);

            return new MavenRepository(task.Object);
        }

        public static MavenReference CreateMavenReference(string artifactId, string groupId, string version)
        {
            var task = new Mock<ITaskItem>();

            task.Setup(i => i.GetMetadata("Identity")).Returns(artifactId);
            task.Setup(i => i.GetMetadata("GroupId")).Returns(groupId);
            task.Setup(i => i.GetMetadata("Version")).Returns(version);

            return new MavenReference(task.Object);
        }

        public static NMavenDeployment CreateNMavenDeployment(string name, string artifactId, string files, string destination, string preserve)
        {
            var task = new Mock<ITaskItem>();

            task.Setup(i => i.GetMetadata("Identity")).Returns(name);
            task.Setup(i => i.GetMetadata("ArtifactId")).Returns(artifactId);
            task.Setup(i => i.GetMetadata("Files")).Returns(files);
            task.Setup(i => i.GetMetadata("Destination")).Returns(destination);
            task.Setup(i => i.GetMetadata("PreserveDirectories")).Returns(preserve);
            return new NMavenDeployment(task.Object);
        }
    }
}
