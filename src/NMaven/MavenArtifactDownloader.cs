﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NMaven.Logging;
using NMaven.Model;

namespace NMaven
{
    public class MavenArtifactDownloader : IDisposable
    {
        private readonly ITaskLogger _logger;
        private readonly HttpClient _httpClient;
        private readonly DirectoryInfo _nmvnPackageRoot;
        private readonly MavenRepository[] _repositories;

        public MavenArtifactDownloader(ITaskLogger logger, DirectoryInfo nmvnPackageRoot, params MavenRepository[] repositories)
        {
            _logger = logger;
            _repositories = repositories;
            _nmvnPackageRoot = nmvnPackageRoot;

            _httpClient = new HttpClient();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<bool> DownloadArtifactAsync(MavenReference reference)
        {
            var artifactFileInfo = reference.GetArtifactFilePath(_nmvnPackageRoot);

            if (artifactFileInfo.Exists)
            {
                _logger.LogMessage($"Artifact {reference.ArtifactId} already downloaded on disk. Skipping download.");
                return true;
            }

            foreach (var repository in _repositories)
            {
                try
                {
                    var artifact = await this.DownloadArtifactAsync(reference, repository);

                    if (!artifactFileInfo.Directory.Exists)
                    {
                        artifactFileInfo.Directory.Create();
                    }

                    File.WriteAllBytes(artifactFileInfo.FullName, artifact);

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"{reference.ArtifactId} not found in {repository.Name} ({repository.Url}). Error: {ex.Message}.");
                }
            }

            _logger.LogError($"Artifact {reference.ArtifactId} not found.");

            return false;
        }

        private async Task<byte[]> DownloadArtifactAsync(MavenReference reference, MavenRepository repository)
        {
            var url = reference.GetRepositoryUrl(repository);

            _logger.LogMessage($"Downloading reference {reference.ArtifactId} ({reference.GroupId}) in version {reference.Version}");

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Error while downloading the artifact ({(int)response.StatusCode} - {response.ReasonPhrase})");
            }

            var content = await response.Content.ReadAsByteArrayAsync();

            return content;
        }
    }
}
