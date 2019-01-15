using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Windows;

namespace VPNManager
{
    [RunInstaller(true)]
    public partial class VPNManagerInstaller : Installer
    {
        public VPNManagerInstaller()
        {
            InitializeComponent();
        }
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            //get Protected Configuration Provider name from custom action parameter
            string sectionName = this.Context.Parameters["sectionName"];

            //get Protected Configuration Provider name from custom action parameter
            string provName = this.Context.Parameters["provName"];

            // get the exe path from the default context parameters
            string exeFilePath = this.Context.Parameters["assemblypath"];

            //encrypt the configuration section
            ProtectSection(sectionName, provName, exeFilePath);

            //move this files during installation
            List<string> filesToMove = new List<string>(new string[] 
            { "VPNManager.exe",
              "VPNManager.exe.config"
            });

            //removing the filename from the path
            int i = exeFilePath.Length - 1;
            while (exeFilePath[i] != 'l') --i;
            string rootFolderPath = exeFilePath.Substring(0, i);
            i = exeFilePath.Length - 1;
            while (exeFilePath[i] != '\\') --i;
            string libFolderPath = exeFilePath.Substring(0, i);
            foreach (string file in filesToMove)
            {
                System.IO.File.Copy(libFolderPath + "\\" + file, rootFolderPath + file, true);
            }
        }


        private void ProtectSection(string sectionName, string provName, string exeFilePath)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(exeFilePath);
            ConfigurationSection section = config.GetSection(sectionName);

            if (!section.SectionInformation.IsProtected)
            {
                //Protecting the specified section with the specified provider
                section.SectionInformation.ProtectSection(provName);
            }
            section.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);

        }
    }
}
