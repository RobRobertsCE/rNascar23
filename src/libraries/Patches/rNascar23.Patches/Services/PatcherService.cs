using rNascar23.Patches.GitHub;
using rNascar23.Patches.Models;
using rNascar23.Patches.AppRegistry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace rNascar23.Patches.Services
{
    public class PatcherService
    {
        #region events

        public event EventHandler<string> InfoMessage;
        protected virtual void OnInfoMessage(string message)
        {
            InfoMessage?.Invoke(this, message);
        }

        #endregion

        #region fields

        private string[] _patchableExtensions = new string[] { "dll", "exe" };
        private IList<CurrentAssemblyInfo> _currentAssemblyInfos = new List<CurrentAssemblyInfo>();
        private IList<PatchSet> _patchSets = new List<PatchSet>();
        private Version _installedVersion = null;
        private string _installDirectory = String.Empty;

        #endregion

        #region public

        public async Task<Version> CheckForAvailablePatchesAsync()
        {
            try
            {
                _installedVersion = GetInstalledVersion();

                if (_installedVersion == null)
                    _installedVersion = new Version();

                _currentAssemblyInfos = GetInstalledAssemblies();

                _patchSets = await GitHubHelper.CheckForAvailablePatchesAsync(_installedVersion);

                if (_patchSets == null || _patchSets.Count == 0)
                {
                    return null;
                }

                var latestPatch = _patchSets.OrderByDescending(p => p.Version).FirstOrDefault();

                return latestPatch.Version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates the currently installed assemblies to the given patch version.
        /// </summary>
        /// <param name="version">The version to patch the application to</param>
        public async Task<bool> ApplyPatchAsync(Version version)
        {
            try
            {
                if (version == null)
                    throw new ArgumentNullException(nameof(version));

                if (_installedVersion == null)
                    await CheckForAvailablePatchesAsync();

                var patchToApply = _patchSets.OrderByDescending(p => p.Version).FirstOrDefault(p => p.Version == version);

                if (patchToApply == null)
                    throw new ArgumentException($"Patch for version {version} not found");

                OnInfoMessage("Patch found");

                var assetFile = patchToApply.Assets.FirstOrDefault();

                if (assetFile == null)
                    throw new ArgumentException($"No asset file found for version {version}");

                OnInfoMessage("Downloading assets for patch");

                var patchFiles = await GitHubHelper.DownloadAvailablePatchesAsync(assetFile);

                if (patchFiles == null || patchFiles.Count == 0)
                    throw new ArgumentException($"No patch assemblies available for download for version {version}");

                OnInfoMessage($"{patchFiles.Count} assets downloaded");

                var changeSet = BuildChangeSet(patchFiles);

                if (changeSet == null || changeSet.Count == 0)
                    throw new ArgumentException($"No changes found for version {version}");

                OnInfoMessage("Patch change set built");

                if (ApplyChangeSet(changeSet))
                {
                    OnInfoMessage("Updating registry");

                    RegistryHelper.SetVersion(version.ToString());

                    OnInfoMessage("Cleaning up temp files");

                    var firstPatch = changeSet.FirstOrDefault(c => c.Patch != null);

                    if (firstPatch != null)
                    {
                        var tempDirectory = Path.GetDirectoryName(firstPatch.Patch.Name);

                        if (Directory.Exists(tempDirectory))
                            Directory.Delete(tempDirectory);
                    }

                    OnInfoMessage($"Patch applied!");

                    return true;
                }
            }
            catch (Exception ex)
            {
                OnInfoMessage($"Error: {ex.Message}");
                throw ex;
            }

            return false;
        }

        public Version GetInstalledVersion()
        {
            var versionString = RegistryHelper.GetVersion();

            if (!Version.TryParse(versionString, out var version))
            {
                return null;
            }

            return version;
        }

        /// <summary>
        /// Gets a list of all currently installed assemblies in the Install folder
        /// </summary>
        /// <returns>List of currently installed assemblies</returns>
        public virtual IList<CurrentAssemblyInfo> GetInstalledAssemblies()
        {
            IList<CurrentAssemblyInfo> currentAssemblies = new List<CurrentAssemblyInfo>();

            var installFolder = RegistryHelper.GetInstallFolder();

            if (!String.IsNullOrEmpty(installFolder))
            {
                currentAssemblies = GetCurrentAssemblies(installFolder);
            }

            return currentAssemblies;
        }

        /// <summary>
        /// Gets a list of all currently installed assemblies
        /// </summary>
        /// <param name="assembliesFolder">The folder where the application is installed</param>
        /// <returns>List of currently installed assemblies in the given folder</returns>
        public virtual IList<CurrentAssemblyInfo> GetCurrentAssemblies(string assembliesFolder)
        {
            var currentAssemblies = new List<CurrentAssemblyInfo>();

            var assembliesDirectoryInfo = new DirectoryInfo(assembliesFolder);

            var assemblyList = assembliesDirectoryInfo.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly)
                .Where(file => _patchableExtensions.Any(x => file.FullName.EndsWith(x, StringComparison.OrdinalIgnoreCase)));

            foreach (FileInfo assembly in assemblyList)
            {
                Version version = null;

                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.FullName);

                if (!String.IsNullOrEmpty(myFileVersionInfo.ProductVersion))
                {
                    version = Version.Parse(myFileVersionInfo.FileVersion);
                }

                var currentAssembly = new CurrentAssemblyInfo()
                {
                    Version = version,
                    AssemblyPath = assembly.FullName,
                    AssemblyName = assembly.Name,
                };

                currentAssemblies.Add(currentAssembly);
            }

            return currentAssemblies;
        }

        #endregion

        #region protected

        private bool ApplyChangeSet(IList<PatchChange> changeSet)
        {
            _installDirectory = RegistryHelper.GetInstallFolder();

            foreach (PatchChange patchChange in changeSet.Where(c => c.Action == PatchAction.Delete))
            {
                OnInfoMessage($"Removing assembly {patchChange.Current.AssemblyName}");

                if (File.Exists(patchChange.Current.AssemblyPath))
                    File.Delete(patchChange.Current.AssemblyPath);
            }

            foreach (PatchChange patchChange in changeSet.Where(c => c.Action == PatchAction.Add))
            {
                OnInfoMessage($"Adding assembly {patchChange.Patch.Name}");

                var newFilePath = Path.Combine(_installDirectory, patchChange.Patch.Name);
                File.Copy(patchChange.Patch.Uri, newFilePath);
            }

            foreach (PatchChange patchChange in changeSet.Where(c => c.Action == PatchAction.Replace))
            {
                OnInfoMessage($"Replacing assembly {patchChange.Patch.Name}");

                var currentAssemblyPath = patchChange.Current.AssemblyPath;

                if (File.Exists(currentAssemblyPath))
                    File.Delete(currentAssemblyPath);

                File.Copy(patchChange.Patch.Uri, currentAssemblyPath);
            }

            return true;
        }

        private IList<PatchChange> BuildChangeSet(IList<PatchFile> patchFiles)
        {
            IList<PatchChange> changes = new List<PatchChange>();

            foreach (CurrentAssemblyInfo current in _currentAssemblyInfos)
            {
                var match = patchFiles.FirstOrDefault(p => p.Name == current.AssemblyName);

                if (match != null)
                {
                    if (match.FileVersion == current.Version)
                    {
                        changes.Add(new PatchChange()
                        {
                            Name = current.AssemblyName,
                            Current = current,
                            Patch = match,
                            Action = PatchAction.None
                        });
                    }
                    else
                    {
                        changes.Add(new PatchChange()
                        {
                            Name = current.AssemblyName,
                            Current = current,
                            Patch = match,
                            Action = PatchAction.Replace
                        });
                    }
                }
                else
                {
                    changes.Add(new PatchChange()
                    {
                        Name = current.AssemblyName,
                        Current = current,
                        Action = PatchAction.Delete
                    });
                }
            }

            foreach (PatchFile patchFile in patchFiles)
            {
                var match = _currentAssemblyInfos.FirstOrDefault(p => p.AssemblyName == patchFile.Name);

                if (match == null)
                {
                    // This is a new file being added
                    changes.Add(new PatchChange()
                    {
                        Name = patchFile.Name,
                        Patch = patchFile,
                        Action = PatchAction.Add
                    });
                }
            }

            return changes;
        }

        #endregion
    }
}
