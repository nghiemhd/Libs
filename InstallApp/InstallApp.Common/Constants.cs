using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallApp.Common
{
    public static class Constants
    {
        public static readonly string VDUId = "VDUId";
        public static readonly string ApiEndpoint = "ApiEndpoint";
        public static readonly string Webconfig = "Web.config";

        /// <summary>
        /// <para>378389</para>
        /// <para>.NET Framework 4.5</para>
        /// </summary>
        public const int Fx4_5_DWORD = 378389;

        /// <summary>
        /// .NET Framework 4.5.1 installed with Windows 8.1
        /// </summary>
        public const int Fx4_5_1_DWORD = 378675;

        /// <summary>
        /// .NET Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2
        /// </summary>
        public const int Fx4_5_1b_DWORD = 378758;

        /// <summary>
        /// .NET Framework 4.5.2
        /// </summary>
        public const int Fx4_5_2_DWORD = 379893;

        /// <summary>
        /// .NET Framework 4.6 installed with Windows 10
        /// </summary>
        public const int Fx4_6_DWORD = 393295;

        /// <summary>
        /// .NET Framework 4.6 installed on all other Windows OS versions
        /// </summary>
        public const int Fx4_6b_DWORD = 393297;
    }
}
