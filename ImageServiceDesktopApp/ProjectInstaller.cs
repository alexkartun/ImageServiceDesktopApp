using System.Collections;
using System.ComponentModel;

namespace ImageServiceDesktopApp
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            string parameter = "ImageServiceSource\" \"ImageServiceLog";
            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\" \"" + parameter + "\"";
            base.OnBeforeInstall(savedState);
        }
    }
}
