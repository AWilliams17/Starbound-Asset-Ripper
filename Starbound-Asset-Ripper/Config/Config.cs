using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starbound_Asset_Ripper.Config
{
    public static class Config
    {
        static Validators validators = new Validators();
        static Registrar.RegSettings settings = new Registrar.RegSettings(Registrar.BaseKeys.HKEY_CURRENT_USER, "TestOptionsRootKey");
        static Registrar.RegOption optionOne = new Registrar.RegOption("option_one", validators.DirectoryValidator, 1, typeof(string));
    }
}
