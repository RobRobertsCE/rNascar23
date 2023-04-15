using Octokit;
using RestSharp;
using rNascar23.Patches.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace rNascar23.Patches.GitHub
{
    public static class GitHubHelper
    {
        // ableBaker4382

        private const string TokenFile = "C:\\Users\\Rob\\Documents\\GitHub\\RRoberts4382.txt";
        private const string GitHubUserName = "RRoberts4382";
        private const string GitHubRepositoryName = "rNascar23-Releases";
        private const string DefaultBranchName = "heads/main";

        private static readonly string GitHubIdentity = Assembly
            .GetEntryAssembly()
            .GetCustomAttribute<AssemblyProductAttribute>()
            .Product;

        private static GitHubClient GetClient()
        {
            var productInformation = new ProductHeaderValue(GitHubIdentity);

            var client = new GitHubClient(productInformation);

            if (File.Exists(TokenFile))
            {
                var token = File.ReadAllText(TokenFile);
                client.Credentials = new Credentials(token);
            }

            return client;
        }

        public static async Task<bool> CreateBranchAsync(string branchName)
        {
            var client = GetClient();

            return await CreateBranchAsync(client, branchName);
        }
        public static async Task<bool> CreateBranchAsync(GitHubClient client, string branchName)
        {
            var featureBranchName = $"refs/heads/{branchName}";

            var repo = await client.Repository.Get(GitHubUserName, GitHubRepositoryName);

            var defaultBranch = await client.Git.Reference.Get(GitHubUserName, GitHubRepositoryName, DefaultBranchName);

            var featureBranch = await client.Git.Reference.Create(GitHubUserName,
                GitHubRepositoryName,
                new NewReference(featureBranchName, defaultBranch.Object.Sha));

            return true;
        }

        public static async Task<bool> CreatePrAsync(string branchName, string prTitle)
        {
            var client = GetClient();

            return await CreatePrAsync(client, branchName, prTitle);
        }
        public static async Task<bool> CreatePrAsync(GitHubClient client, string branchName, string prTitle)
        {
            var repo = await client.Repository.Get(GitHubUserName, GitHubRepositoryName);

            var defaultBranch = await client.Git.Reference.Get(GitHubUserName, GitHubRepositoryName, DefaultBranchName);

            var featureBranchName = $"refs/heads/{branchName}";

            var featureBranch = await client.Git.Reference.Get(GitHubUserName, GitHubRepositoryName, featureBranchName);

            var pr = await client.PullRequest.Create(repo.Id,
                new NewPullRequest(prTitle, featureBranch.Ref, defaultBranch.Ref));

            //var newMerge = new NewMerge(defaultBranch.Ref, featureBranch.Ref);

            //await client.Repository.Merging.Create(repo.Id, newMerge);

            return true;
        }

        /// <summary>
        /// Checks for available patches, based on the currently installed version number.
        /// </summary>
        /// <param name="currentVersion">The currently installed version number</param>
        /// <returns>A list of available patches for the currently installed version</returns>
        public static async Task<IList<PatchSet>> CheckForAvailablePatchesAsync(Version currentVersion)
        {
            IList<PatchSet> patches = new List<PatchSet>();

            var client = GetClient();

            IReadOnlyList<Release> releases = await client.Repository.Release.GetAll(GitHubUserName, GitHubRepositoryName);

            foreach (var release in releases)
            {
                // beta-0.2.1.0
                // patch-0.2.1.1
                // release-1.0.1.2

                Version version = new Version();

                var releaseNameSections = release.Name.Split('-');

                var versionSections = releaseNameSections[1].Split('.');

                if (versionSections.Length == 4)
                {
                    var major = int.Parse(versionSections[0]);
                    var minor = int.Parse(versionSections[1]);
                    var build = int.Parse(versionSections[2]);
                    var revision = int.Parse(versionSections[3]);

                    version = new Version(major, minor, build, revision);
                }

                if (release.Assets.Count > 0 && release.Assets[0].Name.EndsWith("zip") && version > currentVersion)
                {
                    var patch = new PatchSet()
                    {
                        Name = release.Name,
                        Author = release.Author.Login,
                        Stage = release.Name.ToLower().Contains("beta") ? "Beta" :
                            release.Name.ToLower().Contains("alpha") ? "Alpha" :
                            release.Name.ToLower().Contains("patch") ? "Patch" : "Production",
                        IsPreRelease = release.Prerelease,
                        Version = version,
                        Created = release.CreatedAt,
                        Published = release.PublishedAt,
                        ZipUrl = release.ZipballUrl,
                    };

                    foreach (ReleaseAsset asset in release.Assets)
                    {
                        var assetFile = new AssetFile()
                        {
                            Name = asset.Name,
                            Url = asset.BrowserDownloadUrl,
                            Created = asset.CreatedAt,
                            Size = asset.Size,
                            DownloadCount = asset.DownloadCount
                        };

                        patch.Assets.Add(assetFile);
                    }

                    patches.Add(patch);
                }
            }

            return patches;
        }

        /// <summary>
        /// Downloads all available patches, based on the currently installed version number.
        /// </summary>
        /// <param name="currentVersion">The currently installed version number</param>
        /// <returns>All patch assemblies for the currently installed version</returns>
        public static async Task<IList<PatchFile>> DownloadAvailablePatchesAsync(AssetFile assetFile)
        {
            var patches = new List<PatchFile>();

            var tempDirectory = Path.GetTempPath();

            var tempAssetDirectory = Path.Combine(tempDirectory, $"Assets_{assetFile.Name}");

            if (!Directory.Exists(tempAssetDirectory))
            {
                Directory.CreateDirectory(tempAssetDirectory);
            }

            var tempAssetFile = Path.Combine(tempDirectory, assetFile.Name);

            if (!File.Exists(tempAssetFile))
            {
                var assetBytes = await DownloadAssetAsync(assetFile.Url, tempAssetFile);

                System.IO.Compression.ZipFile.ExtractToDirectory(tempAssetFile, tempAssetDirectory);
            }

            patches = BuildPatchFileList(tempAssetDirectory);

            return patches;
        }

        public static List<PatchFile> BuildPatchFileList(string patchFileDirectory, string rootDirectory = "")
        {
            if (String.IsNullOrEmpty(rootDirectory)) rootDirectory = patchFileDirectory;

            var patches = new List<PatchFile>();

            var assetDirectoryInfo = new DirectoryInfo(patchFileDirectory);

            foreach (FileSystemInfo assemblyFileSystemInfo in assetDirectoryInfo.GetFileSystemInfos())
            {
                Console.WriteLine($"Zipped FIle: {assemblyFileSystemInfo.FullName}");

                if (!assemblyFileSystemInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(assemblyFileSystemInfo.FullName);
                    FileInfo assemblyFileInfo = new FileInfo(assemblyFileSystemInfo.FullName);

                    try
                    {
                        var patchFile = new PatchFile()
                        {
                            Name = assemblyFileSystemInfo.Name,
                            Size = assemblyFileInfo.Length,
                            Created = assemblyFileSystemInfo.CreationTime,
                            Uri = assemblyFileSystemInfo.FullName,
                            RelativeUri = GetRelativePath(assemblyFileSystemInfo.FullName, rootDirectory),
                            FileVersion = String.IsNullOrEmpty(myFileVersionInfo.FileVersion) ? new Version() : Version.Parse(myFileVersionInfo.FileVersion)
                        };

                        patches.Add(patchFile);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            foreach (DirectoryInfo assemblyDirectoryInfo in assetDirectoryInfo.GetDirectories())
            {
                var directoryPatches = BuildPatchFileList(assemblyDirectoryInfo.FullName, rootDirectory);

                patches.AddRange(directoryPatches);
            }

            return patches;
        }

        private static string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private static async Task<bool> DownloadAssetAsync(string assetUrl, string zipFilePath)
        {
            var client = new RestClient(assetUrl);

            var request = new RestRequest(String.Empty, Method.Get);

            using (var zip = File.OpenWrite(zipFilePath))
            {
                using (var stream = await client.DownloadStreamAsync(request))
                {
                    stream.CopyTo(zip);
                }
            }

            return true;
        }
    }
}
