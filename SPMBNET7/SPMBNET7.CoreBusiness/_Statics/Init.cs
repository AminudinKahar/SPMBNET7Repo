using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMBNET7.CoreBusiness._Statics
{
    public static class Init
    {
        public const string superAdminName = "SuperAdmin";
        public const string superAdminEmail = "superadmin@idwal.com.my";
        public const string superAdminPassword = "SuperIdwalsys57#";
        
        public const string superAdminAuthRole = "SuperAdmin";

        public const string adminRole = "Admin";
        public const string superVisorRole = "Supervisor";
        public const string userRole = "User";

        public const string superAdminAdminAuthRole = superAdminAuthRole + "," + adminRole;
        public const string superAdminSuperVisorAuthRole = superAdminAuthRole + "," + superVisorRole;
        public const string allExceptAdminRole = superAdminAuthRole + "," + superVisorRole + "," + userRole;
        public const string allExceptSuperadminRole = adminRole + "," + superVisorRole + "," + userRole;
        public const string allRole = superAdminAuthRole + "," + superVisorRole + "," + userRole + "," + adminRole;

        // initial password
        public const string commonPassword = "Spmb1234#";
    }
}
