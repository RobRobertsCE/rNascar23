using Octokit;
using rNascar23.Patches.AppRegistry;
using rNascar23.Patches.GitHub;
using rNascar23.Patches.Models;
using rNascar23.PatchBuilder.Properties;
using rNascar23.Patches.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.PatchBuilder
{
    // 1. Identify install folder.
    //      Registry Entry?
    // 2. Identify current version that is installed.
    //      Check version # of main exe
    // 3. Identify most current patch available.
    //      Check GitHub
    // 4. Compare with most current patch(es) available.
    // 5. If up to date, notify user and exit.
    // 6. If a newer version is available, prompt user to update.
    // 7. Shut down any running apps.
    // 8. Compare current assemblies to patch assemblies, making an update list.
    // 9. Make a restore point.
    // 10. Make a backup of current version's assemblies.
    // 11. Overwrite current assemblies with new ones.
    // 12. Relaunch applications, if any were running.


    // TODO: Include launcher and patcher and patches in zip

    public partial class PatchHelperDialog : Form
    {
        private PatcherService _service = new PatcherService();

        private IList<CurrentAssemblyInfo> _currentAssemblyInfos = new List<CurrentAssemblyInfo>();
        private IList<PatchSet> _patchSets = new List<PatchSet>();
        private IList<PatchFile> _patchFiles = new List<PatchFile>();
        private IList<PatchChange> _changeSet = new List<PatchChange>();

        public PatchHelperDialog()
        {
            InitializeComponent();
        }

        private async void PatcherDialog_Load(object sender, EventArgs e)
        {
            cboReleaseType.SelectedIndex = 0;

            lblRegistryPath.Text = RegistryHelper.GetInstallFolder();

            lblRegistryVersion.Text = RegistryHelper.GetVersion();

            txtVersion.Text = GetAssemblyInfoVersion();

            _patchSets = await GitHubHelper.CheckForAvailablePatchesAsync(new Version());

            DisplayAvailablePatches(_patchSets);

            GetInstalledAssemblies();
        }

        private void btnGetCurrentAssemblies_Click(object sender, EventArgs e)
        {
            GetInstalledAssemblies();
        }
        private void GetInstalledAssemblies()
        {
            var service = new PatcherService();

            _currentAssemblyInfos = service.GetInstalledAssemblies();

            DisplayInstalledAssemblies(_currentAssemblyInfos);
        }
        private void DisplayInstalledAssemblies(IList<CurrentAssemblyInfo> assemblies)
        {
            lvCurrent.Items.Clear();

            foreach (var item in assemblies)
            {
                var lvi = new ListViewItem(item.AssemblyName);

                lvi.SubItems.Add(item.Version?.ToString());
                lvi.SubItems.Add(item.AssemblyPath);

                lvi.Tag = item;

                lvCurrent.Items.Add(lvi);
            }
        }

        private async void btnGetAvailablePatches_Click(object sender, EventArgs e)
        {
            await CheckForAvailablePatchesAsync();
        }
        private async Task CheckForAvailablePatchesAsync()
        {
            _patchSets = await GitHubHelper.CheckForAvailablePatchesAsync(new Version());

            DisplayAvailablePatches(_patchSets);
        }
        private void DisplayAvailablePatches(IList<PatchSet> patchSets)
        {
            lvPatchSets.Items.Clear();

            foreach (var item in patchSets)
            {
                var lvi = new ListViewItem(item.Name);

                lvi.SubItems.Add(item.Version?.ToString());
                lvi.SubItems.Add(item.Stage);
                lvi.SubItems.Add(item.Files.Count().ToString());
                lvi.SubItems.Add(item.Created.ToString());
                lvi.SubItems.Add(item.Published?.ToString());
                lvi.SubItems.Add(item.Author);

                lvi.Tag = item;

                lvPatchSets.Items.Add(lvi);
            }
        }

        private void lvPatchSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPatchSets.SelectedItems.Count == 0)
                return;

            var patchSet = (PatchSet)lvPatchSets.SelectedItems[0].Tag;

            DisplayAssetFiles(patchSet.Assets);
        }
        private void DisplayAssetFiles(IList<AssetFile> assetFiles)
        {
            lvAssets.Items.Clear();

            foreach (var assetFile in assetFiles.OrderBy(a => a.Name))
            {
                var lvi = new ListViewItem(assetFile.Name);

                lvi.SubItems.Add(assetFile.Size.ToString());
                lvi.SubItems.Add(assetFile.DownloadCount.ToString());
                lvi.SubItems.Add(assetFile.Created.ToString());
                lvi.SubItems.Add(assetFile.Url);

                lvi.Tag = assetFile;

                lvAssets.Items.Add(lvi);
            }
        }
        private void lvAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGetPatchFiles.Enabled = lvAssets.SelectedItems.Count > 0;
        }

        private async void btnGetPatchFiles_Click(object sender, EventArgs e)
        {
            try
            {
                _patchFiles = await DownloadAvailablePatchesAsync();

                DisplayPatchFiles(_patchFiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
        private async Task<IList<PatchFile>> DownloadAvailablePatchesAsync()
        {
            if (lvAssets.SelectedItems.Count == 0)
                return new List<PatchFile>();

            var assetFile = (AssetFile)lvAssets.SelectedItems[0].Tag;

            return await GitHubHelper.DownloadAvailablePatchesAsync(assetFile);
        }
        private void DisplayPatchFiles(IList<PatchFile> patchFiles)
        {
            lvPatchFiles.Items.Clear();

            foreach (var item in patchFiles.OrderBy(p => p.Name))
            {
                var lvi = new ListViewItem(item.Name);

                lvi.SubItems.Add(item.FileVersion?.ToString());
                lvi.SubItems.Add(item.Size.ToString());
                lvi.SubItems.Add(item.Created.ToString());

                lvi.Tag = item;

                lvPatchFiles.Items.Add(lvi);
            }
        }

        private void btnBuildChangeSet_Click(object sender, EventArgs e)
        {
            _changeSet = BuildChangeSet();

            DisplayChangeSet(_changeSet);
        }
        private IList<PatchChange> BuildChangeSet()
        {
            IList<PatchChange> changes = new List<PatchChange>();

            foreach (CurrentAssemblyInfo current in _currentAssemblyInfos)
            {
                var match = _patchFiles.FirstOrDefault(p => p.Name == current.AssemblyName);

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

            foreach (PatchFile patchFile in _patchFiles)
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
        private void DisplayChangeSet(IList<PatchChange> changes)
        {
            lvChangeSet.Items.Clear();

            foreach (var item in changes.OrderBy(c => c.Name))
            {
                var lvi = new ListViewItem(item.Name);

                lvi.SubItems.Add(item.Action.ToString());
                lvi.SubItems.Add(item.Current?.Version.ToString());
                lvi.SubItems.Add(item.Patch?.FileVersion.ToString());

                lvi.Tag = item;

                lvChangeSet.Items.Add(lvi);
            }
        }

        private void btnBuildRelease_Click(object sender, EventArgs e)
        {
            try
            {
                // set version number
                // set release name (beta, patch, release, etc...)
                // create new branch
                // zip all files in debug folder
                // copy to releases repo
                // create commit
                // create tag?
                // push branch
                // create PR

                if (cboReleaseType.SelectedIndex == -1) return;

                if (!Version.TryParse(txtVersion.Text, out var newVersion))
                    return;

                string releaseName = $"{cboReleaseType.SelectedItem.ToString().ToLower()}-{newVersion.ToString()}";

                //if (!UpdateGlobalAssemblyInfo(newVersion.ToString()))
                //    return;

                //if (!await CreateBranchAsync(releaseName))
                //    return;

                var service = new PatcherService();

                /*** rNascar23 ***/
                string buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23\\bin\\Debug";

                IList<CurrentAssemblyInfo> assemblies = service.GetCurrentAssemblies(buildDirectory);

                var assemblyBytes = new Dictionary<string, byte[]>();

                foreach (CurrentAssemblyInfo assemblyFile in assemblies)
                {
                    var key = assemblyFile.AssemblyName;
                    var value = System.IO.File.ReadAllBytes(assemblyFile.AssemblyPath);

                    assemblyBytes.Add(key, value);
                }

                /*** rNascar23.Launcher ***/
                buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23.Launcher\\bin\\Debug";

                assemblies = service.GetCurrentAssemblies(buildDirectory);

                foreach (CurrentAssemblyInfo assemblyFile in assemblies)
                {
                    var key = assemblyFile.AssemblyName;
                    var value = System.IO.File.ReadAllBytes(assemblyFile.AssemblyPath);

                    if (!assemblyBytes.ContainsKey(key))
                        assemblyBytes.Add(key, value);
                }

                /*** rNascar23.Patcher ***/
                buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23.Patcher\\bin\\Debug";

                assemblies = service.GetCurrentAssemblies(buildDirectory);

                foreach (CurrentAssemblyInfo assemblyFile in assemblies)
                {
                    var key = assemblyFile.AssemblyName;
                    var value = System.IO.File.ReadAllBytes(assemblyFile.AssemblyPath);

                    if (!assemblyBytes.ContainsKey(key))
                        assemblyBytes.Add(key, value);
                }

                /*** zip file ***/
                var zipFileDirectory = "C:\\Users\\Rob\\source\\repos\\rNascar23\\patches";

                var zipFileName = $"{releaseName}.zip";

                var zipFilePath = Path.Combine(zipFileDirectory, zipFileName);

                CompressToZip(zipFilePath, assemblyBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private string GetAssemblyInfoVersion()
        {
            var assemblyFilePath = "C:\\Users\\Rob\\source\\repos\\rNascar23\\src\\GlobalAssemblyInfo.cs";

            var lines = File.ReadAllLines(assemblyFilePath);

            foreach (var line in lines)
            {
                if (line.Contains("[assembly: AssemblyFileVersion("))
                {
                    var lineSections = line.Split('"');
                    var versionSection = lineSections[1];

                    return versionSection;
                }
            }

            return String.Empty;
            // [assembly: AssemblyFileVersion("0.2.1.4")]
        }
        private void CompressToZip(string fileName, Dictionary<string, byte[]> fileList)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in fileList)
                    {
                        var demoFile = archive.CreateEntry(file.Key);

                        using (var entryStream = demoFile.Open())
                        using (var b = new BinaryWriter(entryStream))
                        {
                            b.Write(file.Value);
                        }
                    }
                }

                using (var fileStream = new FileStream(fileName, System.IO.FileMode.Create))
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    memoryStream.CopyTo(fileStream);
                }
            }
        }
        private bool UpdateGlobalAssemblyInfo(string versionNumber)
        {
            try
            {
                var globalAssemblyInfoFilePath = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\GlobalAssemblyInfo.cs";

                var template = Resources.AssemblyInfoTemplate;

                var updatedAssemblyInfoText = template.Replace("<VERSION_TAG>", versionNumber);

                File.WriteAllText(globalAssemblyInfoFilePath, updatedAssemblyInfoText);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnSetVersion_Click(object sender, EventArgs e)
        {
            RegistryHelper.SetVersion(txtVersionNumber.Text.Trim());
            lblRegistryVersion.Text = RegistryHelper.GetVersion();
        }

        private void btnGetPath_Click(object sender, EventArgs e)
        {
            lblRegistryPath.Text = RegistryHelper.GetInstallFolder();
        }

        private void btnGetVersion_Click(object sender, EventArgs e)
        {
            lblRegistryVersion.Text = RegistryHelper.GetVersion();
        }
    }
}
