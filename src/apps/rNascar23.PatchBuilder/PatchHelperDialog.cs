using rNascar23.PatchBuilder.Properties;
using rNascar23.Patches.AppRegistry;
using rNascar23.Patches.GitHub;
using rNascar23.Patches.Models;
using rNascar23.Patches.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rNascar23.PatchBuilder
{
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
            cboReleaseType.SelectedIndex = 1;

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
                lvi.SubItems.Add(item.RelativePath);
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

                lblPatchZip.Text = "";
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
                lvi.SubItems.Add(item.RelativeUri);
                lvi.SubItems.Add(item.Uri);

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

            IEnumerable<CurrentAssemblyInfo> currentAssemblyInfos = new List<CurrentAssemblyInfo>();
            IEnumerable<PatchFile> patchFiles = new List<PatchFile>();

            if (chkIncludeLauncher.Checked)
            {
                currentAssemblyInfos = _currentAssemblyInfos;
                patchFiles = _patchFiles;
            }
            else
            {
                currentAssemblyInfos = _currentAssemblyInfos.Where(a => !a.RelativePath.StartsWith("Launcher"));
                patchFiles = _patchFiles.Where(a => !a.RelativeUri.StartsWith("Launcher"));
            }

            foreach (CurrentAssemblyInfo current in currentAssemblyInfos)
            {
                var match = patchFiles.FirstOrDefault(p => p.RelativeUri == current.RelativePath);

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
                var match = currentAssemblyInfos.FirstOrDefault(p => p.RelativePath == patchFile.RelativeUri);

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

            IEnumerable<PatchChange> changeSet = new List<PatchChange>();

            if (chkDisplayChangesOnly.Checked)
            {
                changeSet = changes.Where(c => c.Action != PatchAction.None);
            }
            else
            {
                changeSet = changes;
            }

            foreach (var item in changeSet.OrderBy(c => c.Name))
            {
                var lvi = new ListViewItem(item.Name);

                lvi.SubItems.Add(item.Action.ToString());
                lvi.SubItems.Add(item.Current?.RelativePath);
                lvi.SubItems.Add(item.Current?.Version?.ToString());
                lvi.SubItems.Add(item.Patch?.RelativeUri);
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
                string buildDirectory = String.Empty;

                if (chkUseReleaseBuild.Checked)
                {
                    buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23\\bin\\Release";
                }
                else
                {
                    buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23\\bin\\Debug";
                }

                IList<CurrentAssemblyInfo> assemblies = service.GetCurrentAssemblies(buildDirectory);

                var assemblyBytes = new Dictionary<string, byte[]>();

                foreach (CurrentAssemblyInfo assemblyFile in assemblies)
                {
                    var key = assemblyFile.RelativePath;
                    if (!assemblyBytes.ContainsKey(key))
                    {
                        var value = System.IO.File.ReadAllBytes(assemblyFile.AssemblyPath);
                        assemblyBytes.Add(key, value);
                    }
                }

                if (chkIncludeLauncher.Checked)
                {
                    ///*** rNascar23.Launcher - Can't patch launcher ***/
                    buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23.Launcher\\bin\\Debug";

                    assemblies = service.GetCurrentAssemblies(buildDirectory);

                    foreach (CurrentAssemblyInfo assemblyFile in assemblies)
                    {
                        var key = $"Launcher\\{assemblyFile.AssemblyName}";
                        var value = System.IO.File.ReadAllBytes(assemblyFile.AssemblyPath);

                        if (!assemblyBytes.ContainsKey(key))
                            assemblyBytes.Add(key, value);
                    }

                    /*** rNascar23.Patcher ***/
                    buildDirectory = "C:\\Users\\Rob\\Source\\repos\\rNascar23\\src\\apps\\rNascar23.Patcher\\bin\\Debug";

                    assemblies = service.GetCurrentAssemblies(buildDirectory);

                    foreach (CurrentAssemblyInfo assemblyFile in assemblies)
                    {
                        var key = $"Launcher\\{assemblyFile.AssemblyName}";
                        var value = System.IO.File.ReadAllBytes(assemblyFile.AssemblyPath);

                        if (!assemblyBytes.ContainsKey(key))
                            assemblyBytes.Add(key, value);
                    }
                }

                /*** zip file ***/
                var zipFileDirectory = "C:\\Users\\Rob\\source\\repos\\rNascar23\\patches";

                var zipFileName = $"{releaseName}.zip";

                var zipFilePath = Path.Combine(zipFileDirectory, zipFileName);

                CompressToZip(zipFilePath, assemblyBytes);

                MessageBox.Show($"Patch saved to {zipFilePath}");
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

        private void btnOpenPatchZip_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Filter = "Zip FIle *.zip|*.zip|All Files *.*|*.*";
            dialog.FilterIndex = 0;
            dialog.InitialDirectory = "C:\\Users\\Rob\\source\\repos\\rNascar23\\patches";

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var zipFileDirectory = Path.GetDirectoryName(dialog.FileName);

                var zipFileName = Path.GetFileNameWithoutExtension(dialog.FileName);

                var extractedZipFileDirectory = Path.Combine(zipFileDirectory, zipFileName);

                if (!Directory.Exists(extractedZipFileDirectory))
                {
                    Directory.CreateDirectory(extractedZipFileDirectory);

                    System.IO.Compression.ZipFile.ExtractToDirectory(dialog.FileName, extractedZipFileDirectory);
                }

                _patchFiles = GitHubHelper.BuildPatchFileList(extractedZipFileDirectory);

                DisplayPatchFiles(_patchFiles);

                lblPatchZip.Text = zipFileName;
            }
        }

        private void btnApplyPatch_Click(object sender, EventArgs e)
        {
            try
            {
                _service.ApplyChangeSet(_changeSet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void chkDisplayChangesOnly_CheckStateChanged(object sender, EventArgs e)
        {
            DisplayChangeSet(_changeSet);
        }
    }
}
