using FluentAssertions;

namespace Tests.NMaven.Model
{
    [TestFixture]
    public class MavenReferenceTester
    {
        [Test]
        public void ShouldGetDependencyUrl()
        {
            var repository = ModelFactory.CreateMavenRepository("Repo", "http://monrepo.fr");

            var dependency = ModelFactory.CreateMavenReference("artifact-id", "mon.group", "1.0.0");
            
            dependency.GetRepositoryUrl(repository)
                .Should().Be("http://monrepo.fr/mon/group/artifact-id/1.0.0/artifact-id-1.0.0.jar");
        }

        [Test]
        public void ShouldGetDependencyUrlWithClassifierAndType()
        {
            var repository = ModelFactory.CreateMavenRepository("Repo", "http://monrepo.fr");

            var dependency = ModelFactory.CreateMavenReference("artifact-id", "mon.group", "1.0.0", classifier: "setup", type: "zip");

            dependency.GetRepositoryUrl(repository)
                .Should().Be("http://monrepo.fr/mon/group/artifact-id/1.0.0/artifact-id-1.0.0-setup.zip");
        }
    }
}
