using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace ApplicationUtils
{
    public static class MiscUtils
    {
        public static void WarnIfNotAdmin()
        {
            if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
            {
                MessageBox.Show("Warning - You are not running as an administrator. It is highly recommended you do so.", "Not Administrator Warning");
            }
        }
    }
}
