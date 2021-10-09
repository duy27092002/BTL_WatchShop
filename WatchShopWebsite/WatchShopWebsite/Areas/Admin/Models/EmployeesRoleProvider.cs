using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WatchShopWebsite.Models.EF;

namespace WatchShopWebsite.Areas.Admin.Models
{
    public class EmployeesRoleProvider : RoleProvider
    {
        private DB_WatchShopEntities1 db = new DB_WatchShopEntities1();

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        public override string[] GetRolesForUser(string username)
        {
            var employeeRoles = (from role in db.VaiTroes
                                 join roleMapping in db.NhanVien_VaiTro
                                 on role.MaVaiTro equals roleMapping.MaVaiTro
                                 join emp in db.NhanViens
                                 on roleMapping.MaNV equals emp.MaNV
                                 where emp.TenDangNhap == username
                                 select role.TenVaiTro).ToArray();
            return employeeRoles;
        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}