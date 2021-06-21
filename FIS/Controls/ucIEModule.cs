using DevExpress.XtraEditors.Controls;
using FIS.Controllers;
using FIS.Entities;
using FIS.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace FIS.AppClient.Controls
{
    public partial class ucIEModule : ucModule
    {
        private List<ModuleInfo> m_Modules;

        public ucIEModule()
        {
            InitializeComponent();
#if !DEBUG
            btnExport.Enabled = false;
#endif
        }

        private void ucIEModule_Load(object sender, EventArgs e)
        {
            lstModule.ImageList = ThemeUtils.Image24;
            RefreshModules();
        }

        private void RefreshModules()
        {
            using (var ctrlSA = new SAController())
            {
                lstModule.BeginUpdate();
                lstModule.Items.Clear();

                ctrlSA.ListModuleInfo(out m_Modules);
                var groupModules = (from module in m_Modules
                                    group module by module.ModuleID into groupModule
                                    select groupModule);
                foreach (var modtype in CodeUtils.GetCodes("DEFMOD", "MODTYPE"))
                {
                    if (modtype.CodeValueName.Contains(txtFilter.Text.ToUpper()) || (txtFilter.Text == "?TYPE"))
                        lstModule.Items.Add(new ImageListBoxItem(modtype, Language.ModuleTypeImageIndex));
                }

                foreach (var groupModule in groupModules)
                {
                    if (groupModule.First().ModuleName.Contains(txtFilter.Text.ToUpper()))
                    {
                        lstModule.Items.Add(new ImageListBoxItem(groupModule.First(), Language.ModuleImageIndex));
                    }
                }

                lstModule.EndUpdate();
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            RefreshModules();
        }

        private void ExportModules(string fileName)
        {
            try
            {
                var generatedModules = new List<string>();
                foreach (ImageListBoxItem item in lstModule.SelectedItems)
                {
                    using (var ctrlSA = new SAController())
                    {
                        string generatedModule;
                        if (item.Value is CodeInfo)
                        {
                            var code = item.Value as CodeInfo;
                            ctrlSA.ExecuteGenerateModulePackage(code.CodeValue, out generatedModule);
                            generatedModules.Add(generatedModule);
                        }
                        if (item.Value is ModuleInfo)
                        {
                            var module = item.Value as ModuleInfo;
                            ctrlSA.ExecuteGenerateModulePackage(module.ModuleID, out generatedModule);
                            generatedModules.Add(generatedModule);
                        }
                    }
                }

                var serilizer = new XmlSerializer(typeof(List<string>));
                var stream = File.Open(fileName, FileMode.Create, FileAccess.Write);
                serilizer.Serialize(stream, generatedModules);
                stream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ImportModule(string fileName)
        {
            try
            {
                var serilizer = new XmlSerializer(typeof(List<string>));
                var stream = File.Open(fileName, FileMode.Open, FileAccess.Read);
                var generatedModules = (List<string>)serilizer.Deserialize(stream);
                stream.Close();

                foreach (var generatedModule in generatedModules)
                {
                    using (var ctrlSA = new SAController())
                    {
                        ctrlSA.ExecuteInstallModule(generatedModule);
                    }
                }
                RefreshModules();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var folderDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\App Modules"
            };

            Directory.CreateDirectory(folderDialog.InitialDirectory);
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                ExportModules(folderDialog.FileName);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                UninstallModules();
                RefreshModules();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UninstallModules()
        {
            foreach (ImageListBoxItem module in lstModule.SelectedItems)
            {
                using (var ctrlSA = new SAController())
                {
                    if (module.Value is CodeInfo)
                    {
                        ctrlSA.ExecuteUninstallModule((module.Value as CodeInfo).CodeValue);
                    }
                    if (module.Value is ModuleInfo)
                    {
                        ctrlSA.ExecuteUninstallModule((module.Value as ModuleInfo).ModuleID);
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\App Modules",
                Filter = "Module Package (*.mpkg)|*.mpkg"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                ImportModule(openDialog.FileName);
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                lstModule.SelectAll();
            }
            else
            {
                lstModule.UnSelectAll();
            }
        }
    }
}
