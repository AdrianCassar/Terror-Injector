/*
    Terror Injector is an easy-to-use tool for seamlessly injecting the free GTA V Terror mod menu.
    Copyright (C) 2021 MoistyMarley <https://github.com/MoistyMarley/Terror-Injector/>.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Terror_Injector
{
    /// <summary>
    /// Access Control
    /// - Lock a specific file
    /// - Unlock a specific file
    /// </summary>
    public class FileAccessControl
    {
        private FileInfo fileInfo;
        private SecurityIdentifier userIdentity = WindowsIdentity.GetCurrent().User;

        /// <summary>
        /// Instance of FileAccessControl
        /// </summary>
        /// <param name="file">The file to lock.</param>
        public FileAccessControl(FileInfo file)
        {
            fileInfo = file;
        }

        /// <summary>
        /// Check whether the file is locked.
        /// </summary>
        /// <returns>True if locked otherwise, false.</returns>
        public bool IsLocked()
        {
            bool result = false;

            try
            {
                if (fileInfo.Exists)
                {
                    FileSecurity fSecurity = fileInfo.GetAccessControl();
                    AuthorizationRuleCollection rules = fSecurity.GetAccessRules(true, false, typeof(NTAccount));

                    foreach (FileSystemAccessRule rule in rules)
                    {
                        if (rule.IdentityReference.Value.Equals(WindowsIdentity.GetCurrent().Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if ((rule.FileSystemRights & FileSystemRights.Read) == FileSystemRights.Read && rule.AccessControlType == AccessControlType.Allow)
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }

        /// <summary>
        /// Toggles Lock
        /// </summary>
        /// <returns>True if file is locked otherwise, false.</returns>
        public bool ToggleLock()
        {
            bool result;

            if (fileInfo.Exists)
            {
                if (IsLocked())
                    result = !Unlock();
                else
                    result = Lock();
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Locks the File
        /// </summary>
        private bool Lock()
        {
            bool result = true;

            try
            {
                //Set the file to ReadOnly.
                File.SetAttributes(fileInfo.FullName, FileAttributes.ReadOnly);

                //Disable Inheritance Access
                RemoveInheritanceAccess(fileInfo, true, true);

                //Remove the current user entry from ACL.
                RemoveFileSecurity(fileInfo, userIdentity, FileSystemRights.FullControl, AccessControlType.Allow);

                //Add an ACL entry to allow reading.
                AddFileSecurity(fileInfo, userIdentity, FileSystemRights.Read, AccessControlType.Allow);

            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Unlocks the File
        /// </summary>
        private bool Unlock()
        {
            bool result = true;

            try
            {
                //Remove all none inherited access rules.
                RemoveExplicitSecurity(fileInfo);

                //Enable inheritance access
                RemoveInheritanceAccess(fileInfo, false, false);

                //Set the file to Normal to remove the ReadOnly attribute.
                File.SetAttributes(fileInfo.FullName, FileAttributes.Normal);
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Adds an ACL entry on the specified file for the specified account.
        /// </summary>
        /// <param name="file">The file to change permissions.</param>
        /// <param name="account">Reference to the user account.</param>
        /// <param name="rights">The type of operation associated with the access rule.</param>
        /// <param name="controlType">Allow or Deny access.</param>
        private void AddFileSecurity(FileInfo file, IdentityReference account, FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = file.GetAccessControl();

            fSecurity.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));

            file.SetAccessControl(fSecurity);
        }

        /// <summary>
        /// Removes an ACL entry on the specified file for the specified account.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="account">Reference to the user account.</param>
        /// <param name="rights">The type of operation associated with the access rule.</param>
        /// <param name="controlType">Allow or Deny access.</param>
        private void RemoveFileSecurity(FileInfo file, IdentityReference account, FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = file.GetAccessControl();

            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));

            file.SetAccessControl(fSecurity);
        }

        /// <summary>
        /// Disable inheritance and preserve inherited access rules
        /// </summary>
        /// <param name="file"></param>
        /// <param name="isProtected">If true disable inheritance otherwise, allow inheritances</param>
        /// <param name="preserveInheritance">If true preserve inherited access rules otherwise, to remove inherited access rules.</param>
        private void RemoveInheritanceAccess(FileInfo file, bool isProtected, bool preserveInheritance)
        {
            FileSecurity fSecurity = file.GetAccessControl();

            fSecurity.SetAccessRuleProtection(isProtected, preserveInheritance);

            file.SetAccessControl(fSecurity);
        }

        /// <summary>
        /// Remove all explicit access rules on the specified file.
        /// </summary>
        /// <param name="file"></param>
        private void RemoveExplicitSecurity(FileInfo file)
        {
            FileSecurity fSecurity = file.GetAccessControl();

            AuthorizationRuleCollection rules = fSecurity.GetAccessRules(true, false, typeof(NTAccount));

            foreach (FileSystemAccessRule rule in rules)
                fSecurity.RemoveAccessRule(rule);

            file.SetAccessControl(fSecurity);
        }
    }
}
