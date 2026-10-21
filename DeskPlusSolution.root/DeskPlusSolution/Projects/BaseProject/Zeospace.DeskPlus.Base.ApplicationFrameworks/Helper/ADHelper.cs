using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Xml;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// ADHelper에 대한 요약 설명입니다.
    /// </summary>
    public class ADHelper
    {
        private string sourceName = "DeskPlusEP Manager";

        private string currentPath = "";
        private string ouDataPath = "";
        private string userDataPath = "";
        private string adUserName = "";
        private string adPassword = "";
        private string ldapURI = "";
        private string ldapRoot = "";
        private string[] syncOUFileArr;
        private string[] syncUserArr;

        /// <summary>
        /// Creates a new <see cref="ADHelper"/> instance.
        /// </summary>
        /// <param name="_currentPath">_current path.</param>
        public ADHelper(string _currentPath)
        {
            currentPath = _currentPath;
        }

        #region Property

        /// <summary>
        /// Sets the name of the user.
        /// </summary>
        /// <value></value>
        public string UserName { set { adUserName = value; } }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <value></value>
        public string Password { set { adPassword = value; } }

        /// <summary>
        /// Sets the ldap URI.
        /// </summary>
        /// <value></value>
        public string LdapURI { set { ldapURI = value; } }

        /// <summary>
        /// Sets the ldap root.
        /// </summary>
        /// <value></value>
        public string LdapRoot { set { ldapRoot = value; } }

        /// <summary>
        /// Sets the sync OU files.
        /// </summary>
        /// <value></value>
        public string[] SyncOUFiles { set { syncOUFileArr = value; } }

        /// <summary>
        /// Sets the sync user files.
        /// </summary>
        /// <value></value>
        public string[] SyncUserFiles { set { syncUserArr = value; } }

        #endregion Property

        /// <summary>
        /// Sets the XML file.
        /// </summary>
        /// <param name="_ds">_DS.</param>
        /// <param name="_isOU">_is OU.</param>
        public void SetXmlFile(DataSet _ds, bool _isOU)
        {
            XmlTextWriter xtw = new XmlTextWriter((_isOU == true ? ouDataPath : userDataPath), null);
            _ds.WriteXml(xtw, XmlWriteMode.DiffGram);
            xtw.Close();
        }

        /// <summary>
        /// Xml Data 파일을 불러와서 기존의 존재하는 Data와 비교 하여 변동된것만 DataSet으로 생성
        /// </summary>
        /// <param name="_syncData">_sync data.</param>
        /// <returns></returns>
        public DataSet LoadXmlFile(string _syncData)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(_syncData, XmlReadMode.InferSchema);

            return ds;
        }

        /// <summary>
        /// Syncs this instance.
        /// </summary>
        /// <returns></returns>
        public bool Sync()
        {
            try
            {
                OUSync();
                UserSync();
                return true;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                return false;
            }
        }

        /// <summary>
        /// Users the sync.
        /// </summary>
        /// <returns></returns>
        public bool UserSync()
        {
            string nowTime = DateTime.Now.ToLocalTime().ToString().Replace(":", "_");

            for (int i = 0; i < syncUserArr.Length; i++)
            {
                if (!File.Exists(syncUserArr[i]))
                {
                    EventLog.WriteEntry(sourceName, "[" + syncOUFileArr[i] + "] 파일이 존재하지 않습니다", EventLogEntryType.Error);
                    continue;
                }

                DataSet ds = LoadXmlFile(syncUserArr[i]);

                ArrayList userColumnName = new ArrayList();
                foreach (DataColumn dc in ds.Tables[0].Columns)	// 컬럼 이름 가져오기
                {
                    userColumnName.Add(dc.ColumnName);
                }

                try
                {
                    ADUserInfo ADUserInfo = new ADUserInfo();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ADUserInfo.Clear();
                        for (int j = 0; j < userColumnName.Count; j++)
                        {
                            string columnName = userColumnName[j].ToString().ToLower();
                            switch (columnName)
                            {
                                //								case "cn":
                                //									ADUserInfo.cn = columnName;
                                //									break;
                                case "displayname":
                                    ADUserInfo.displayName = dr["displayname"].ToString();
                                    break;

                                case "sn":
                                    ADUserInfo.sn = dr["sn"].ToString();
                                    break;

                                case "givenname":
                                    ADUserInfo.givenName = dr["givenName"].ToString();
                                    break;

                                case "initials":
                                    ADUserInfo.initials = dr["initials"].ToString();
                                    break;

                                case "title":
                                    ADUserInfo.title = dr["title"].ToString();
                                    break;

                                case "userid":
                                    ADUserInfo.userid = dr["userid"].ToString();
                                    break;

                                case "userpassword":
                                    ADUserInfo.userPassword = dr["userPassword"].ToString();
                                    break;

                                case "mail":
                                    ADUserInfo.mail = dr["mail"].ToString();
                                    break;

                                case "telephonenumber":
                                    ADUserInfo.telephoneNumber = dr["telephoneNumber"].ToString();
                                    break;

                                case "facsimiletelephonenumber":
                                    ADUserInfo.facsimileTelephoneNumber = dr["facsimileTelephoneNumber"].ToString();
                                    break;

                                case "mobile":
                                    ADUserInfo.mobile = dr["mobile"].ToString();
                                    break;

                                case "roomnumber":
                                    ADUserInfo.roomNumber = dr["roomNumber"].ToString();
                                    break;

                                case "carlicense":
                                    ADUserInfo.carLicense = dr["carlicense"].ToString();
                                    break;

                                case "o":
                                    ADUserInfo.o = dr["o"].ToString();
                                    break;

                                case "ou":
                                    ADUserInfo.ou = dr["ou"].ToString();
                                    break;

                                case "departmentnumber":
                                    ADUserInfo.departmentNumber = dr["departmentNumber"].ToString();
                                    break;

                                case "employeenumber":
                                    ADUserInfo.employeeNumber = dr["employeeNumber"].ToString();
                                    break;

                                case "employeetype":
                                    ADUserInfo.employeeType = dr["employeeType"].ToString();
                                    break;

                                case "preferredlanguage":
                                    ADUserInfo.preferredLanguage = dr["preferredLanguage"].ToString();
                                    break;

                                case "labeleduri":
                                    ADUserInfo.labeledURI = dr["labeledURI"].ToString();
                                    break;

                                case "description":
                                    ADUserInfo.description = dr["description"].ToString();
                                    break;
                            }
                        }

                        switch (dr["action"].ToString().ToUpper())
                        {
                            case "I":
                                AddUser(ADUserInfo, userColumnName);
                                break;

                            case "E":
                                ModUser(ADUserInfo, userColumnName);
                                break;

                            case "D":
                                DelUser(ADUserInfo);
                                break;
                        }

                        dr["action"] = "C";
                    }

                    userDataPath = syncUserArr[i] + "_" + nowTime + ".fix";
                    BuildFixFile(ds.Clone(), false);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry(sourceName, ex.Message.ToString(), EventLogEntryType.Error);
                    return false;
                }

                File.Move(syncUserArr[i], syncUserArr[i] + "_" + nowTime + ".dat");
            }
            return true;
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="_ADUserInfo">_ AD user info.</param>
        /// <param name="_userColumn">_user column.</param>
        private void AddUser(ADUserInfo _ADUserInfo, ArrayList _userColumn)
        {
            try
            {
                string ldapFullPath = ldapURI + ldapRoot;
                SearchResult result = GetLdapObjectPath(_ADUserInfo.departmentNumber, ldapFullPath, ADObjectType.organizationalUnit, SearchScope.Subtree);
                using (DirectoryEntry entry = new DirectoryEntry(result.Path))
                {
                    string cn = "CN=" + _ADUserInfo.userid;
                    DirectoryEntry usr = entry.Children.Add(cn, "user");

                    for (int i = 0; i < _userColumn.Count; i++)
                    {
                        string columnName = _userColumn[i].ToString().ToLower();
                        switch (columnName)
                        {
                            case "displayname":
                                usr.Properties["displayName"].Add(_ADUserInfo.displayName);
                                break;

                            case "sn":
                                usr.Properties["sn"].Add(_ADUserInfo.sn);
                                break;

                            case "givenname":
                                usr.Properties["givenName"].Add(_ADUserInfo.givenName);
                                break;

                            case "initials":
                                usr.Properties["initials"].Add(_ADUserInfo.initials);
                                break;

                            case "title":
                                usr.Properties["title"].Add(_ADUserInfo.title);
                                break;

                            case "userid":
                                usr.Properties["uid"].Add(_ADUserInfo.userid);
                                break;

                            case "userpassword":
                                usr.Properties["userParameters"].Add(_ADUserInfo.userPassword);
                                break;

                            case "mail":
                                usr.Properties["mail"].Add(_ADUserInfo.mail);
                                break;

                            case "telephonenumber":
                                usr.Properties["telephoneNumber"].Add(_ADUserInfo.telephoneNumber);
                                break;

                            case "facsimiletelephonenumber":
                                usr.Properties["facsimileTelephoneNumber"].Add(_ADUserInfo.facsimileTelephoneNumber);
                                break;

                            case "mobile":
                                usr.Properties["mobile"].Add(_ADUserInfo.mobile);
                                break;

                            case "roomnumber":
                                usr.Properties["roomNumber"].Add(_ADUserInfo.roomNumber);
                                break;

                            case "carlicense":
                                usr.Properties["carLicense"].Add(_ADUserInfo.carLicense);
                                break;

                            case "o":
                                usr.Properties["o"].Add(_ADUserInfo.o);
                                break;

                            case "ou":
                                usr.Properties["ou"].Add(_ADUserInfo.ou);
                                break;

                            case "departmentnumber":
                                usr.Properties["departmentNumber"].Add(_ADUserInfo.departmentNumber);
                                break;

                            case "employeenumber":
                                usr.Properties["employeeNumber"].Add(_ADUserInfo.employeeNumber);
                                break;

                            case "employeetype":
                                usr.Properties["employeeType"].Add(_ADUserInfo.employeeType);
                                break;

                            case "preferredlanguage":
                                usr.Properties["preferredLanguage"].Add(_ADUserInfo.preferredLanguage);
                                break;

                            case "labeleduri":
                                usr.Properties["labeledURI"].Add(_ADUserInfo.labeledURI);
                                break;

                            case "description":
                                usr.Properties["description"].Add(_ADUserInfo.description);
                                break;
                        }
                    }

                    usr.CommitChanges();

                    string userGroup = ldapURI + "CN=Users,CN=Roles," + ldapRoot;		//UserRole
                    using (DirectoryEntry roleEntry = new DirectoryEntry(userGroup))
                    {
                        roleEntry.Properties["member"].Add(usr.Properties["distinguishedName"].Value);
                        roleEntry.CommitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                switch (ex.GetType().ToString())
                {
                    case "InvalidOperationException":
                        {
                            InvalidOperationException InvOpEx = new InvalidOperationException();
                            EventLog.WriteEntry(sourceName, InvOpEx.Message, EventLogEntryType.Error);
                            break;
                        }
                    case "System.Runtime.InteropServices.COMException":
                        {
                            System.Runtime.InteropServices.COMException COMEx = (System.Runtime.InteropServices.COMException)ex;
                            EventLog.WriteEntry(sourceName, COMEx.Message, EventLogEntryType.Error);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Mods the user.
        /// </summary>
        /// <param name="_ADUserInfo">_ AD user info.</param>
        /// <param name="_userColumn">_user column.</param>
        private void ModUser(ADUserInfo _ADUserInfo, ArrayList _userColumn)
        {
            try
            {
                string ldapFullPath = ldapURI + ldapRoot;
                SearchResult result = GetLdapObjectPath(_ADUserInfo.employeeNumber, ldapFullPath, ADObjectType.user, SearchScope.Subtree);
                using (DirectoryEntry usr = new DirectoryEntry(result.Path))
                {
                    if (_ADUserInfo.userid != usr.Properties["uid"].Value.ToString()) // userID가 바뀐경우
                    {
                        DelUser(_ADUserInfo);
                        AddUser(_ADUserInfo, _userColumn);
                        return;
                    }

                    for (int i = 0; i < _userColumn.Count; i++)
                    {
                        string columnName = _userColumn[i].ToString().ToLower();
                        switch (columnName)
                        {
                            case "displayname":
                                if (_ADUserInfo.displayName != (usr.Properties["displayName"].Value == null ? "" : usr.Properties["displayName"].Value.ToString()))
                                {
                                    usr.Properties["displayName"].Clear();
                                    usr.Properties["displayName"].Add(_ADUserInfo.displayName);
                                }
                                break;

                            case "sn":
                                if (_ADUserInfo.sn != (usr.Properties["sn"].Value == null ? "" : usr.Properties["sn"].Value.ToString()))
                                {
                                    usr.Properties["sn"].Clear();
                                    usr.Properties["sn"].Add(_ADUserInfo.sn);
                                }
                                break;

                            case "givenname":
                                if (_ADUserInfo.givenName != (usr.Properties["givenName"].Value == null ? "" : usr.Properties["givenName"].Value.ToString()))
                                {
                                    usr.Properties["givenName"].Clear();
                                    usr.Properties["givenName"].Add(_ADUserInfo.givenName);
                                }
                                break;

                            case "initials":
                                if (_ADUserInfo.initials != (usr.Properties["initials"].Value == null ? "" : usr.Properties["initials"].Value.ToString()))
                                {
                                    usr.Properties["initials"].Clear();
                                    usr.Properties["initials"].Add(_ADUserInfo.initials);
                                }
                                break;

                            case "title":
                                if (_ADUserInfo.title != (usr.Properties["title"].Value == null ? "" : usr.Properties["title"].Value.ToString()))
                                {
                                    usr.Properties["title"].Clear();
                                    usr.Properties["title"].Add(_ADUserInfo.title);
                                }
                                break;

                            case "userid":
                                if (_ADUserInfo.userid != (usr.Properties["uid"].Value == null ? "" : usr.Properties["uid"].Value.ToString()))
                                {
                                    usr.Properties["uid"].Clear();
                                    usr.Properties["uid"].Add(_ADUserInfo.userid);
                                }
                                break;

                            case "userpassword":
                                if (_ADUserInfo.userPassword != (usr.Properties["userParameters"].Value == null ? "" : usr.Properties["userParameters"].Value.ToString()))
                                {
                                    usr.Properties["userParameters"].Clear();
                                    usr.Properties["userParameters"].Add(_ADUserInfo.userPassword);
                                }
                                break;

                            case "mail":
                                if (_ADUserInfo.mail != (usr.Properties["mail"].Value == null ? "" : usr.Properties["mail"].Value.ToString()))
                                {
                                    usr.Properties["mail"].Clear();
                                    usr.Properties["mail"].Add(_ADUserInfo.mail);
                                }
                                break;

                            case "telephonenumber":
                                if (_ADUserInfo.telephoneNumber != (usr.Properties["telephoneNumber"].Value == null ? "" : usr.Properties["telephoneNumber"].Value.ToString()))
                                {
                                    usr.Properties["telephoneNumber"].Clear();
                                    usr.Properties["telephoneNumber"].Add(_ADUserInfo.telephoneNumber);
                                }
                                break;

                            case "facsimiletelephonenumber":
                                if (_ADUserInfo.facsimileTelephoneNumber != (usr.Properties["facsimileTelephoneNumber"].Value == null ? "" : usr.Properties["facsimileTelephoneNumber"].Value.ToString()))
                                {
                                    usr.Properties["facsimileTelephoneNumber"].Clear();
                                    usr.Properties["facsimileTelephoneNumber"].Add(_ADUserInfo.facsimileTelephoneNumber);
                                }
                                break;

                            case "mobile":
                                if (_ADUserInfo.mobile != (usr.Properties["mobile"].Value == null ? "" : usr.Properties["mobile"].Value.ToString()))
                                {
                                    usr.Properties["mobile"].Clear();
                                    usr.Properties["mobile"].Add(_ADUserInfo.mobile);
                                }
                                break;

                            case "roomnumber":
                                if (_ADUserInfo.roomNumber != (usr.Properties["roomNumber"].Value == null ? "" : usr.Properties["roomNumber"].Value.ToString()))
                                {
                                    usr.Properties["roomNumber"].Clear();
                                    usr.Properties["roomNumber"].Add(_ADUserInfo.roomNumber);
                                }
                                break;

                            case "carlicense":
                                if (_ADUserInfo.carLicense != (usr.Properties["carLicense"].Value == null ? "" : usr.Properties["carLicense"].Value.ToString()))
                                {
                                    usr.Properties["carLicense"].Clear();
                                    usr.Properties["carLicense"].Add(_ADUserInfo.carLicense);
                                }
                                break;

                            case "o":
                                if (_ADUserInfo.o != (usr.Properties["o"].Value == null ? "" : usr.Properties["o"].Value.ToString()))
                                {
                                    usr.Properties["o"].Clear();
                                    usr.Properties["o"].Add(_ADUserInfo.o);
                                }
                                break;

                            case "ou":
                                if (_ADUserInfo.ou != (usr.Properties["ou"].Value == null ? "" : usr.Properties["ou"].Value.ToString()))
                                {
                                    usr.Properties["ou"].Clear();
                                    usr.Properties["ou"].Add(_ADUserInfo.ou);
                                }
                                break;

                            case "departmentnumber":
                                if (_ADUserInfo.departmentNumber != (usr.Properties["departmentNumber"].Value == null ? "" : usr.Properties["departmentNumber"].Value.ToString())) // 부서 정보가 같지 않을시
                                {
                                    SearchResult departResult = GetLdapObjectPath(_ADUserInfo.departmentNumber, ldapFullPath, ADObjectType.organizationalUnit, SearchScope.Subtree);

                                    usr.MoveTo(new DirectoryEntry(departResult.Path));
                                    usr.Properties["departmentNumber"].Clear();
                                    usr.Properties["departmentNumber"].Add(_ADUserInfo.departmentNumber);
                                }
                                break;

                            case "employeenumber":
                                if (_ADUserInfo.employeeNumber != (usr.Properties["employeeNumber"].Value == null ? "" : usr.Properties["employeeNumber"].Value.ToString()))
                                {
                                    usr.Properties["employeeNumber"].Clear();
                                    usr.Properties["employeeNumber"].Add(_ADUserInfo.employeeNumber);
                                }
                                break;

                            case "employeetype":
                                if (_ADUserInfo.employeeType != (usr.Properties["employeeType"].Value == null ? "" : usr.Properties["employeeType"].Value.ToString()))
                                {
                                    usr.Properties["employeeType"].Clear();
                                    usr.Properties["employeeType"].Add(_ADUserInfo.employeeType);
                                }
                                break;

                            case "preferredlanguage":
                                if (_ADUserInfo.preferredLanguage != (usr.Properties["preferredLanguage"].Value == null ? "" : usr.Properties["preferredLanguage"].Value.ToString()))
                                {
                                    usr.Properties["preferredLanguage"].Clear();
                                    usr.Properties["preferredLanguage"].Add(_ADUserInfo.preferredLanguage);
                                }
                                break;

                            case "labeleduri":
                                if (_ADUserInfo.labeledURI != (usr.Properties["labeledURI"].Value == null ? "" : usr.Properties["labeledURI"].Value.ToString()))
                                {
                                    usr.Properties["labeledURI"].Clear();
                                    usr.Properties["labeledURI"].Add(_ADUserInfo.labeledURI);
                                }
                                break;

                            case "description":
                                if (_ADUserInfo.description != (usr.Properties["description"].Value == null ? "" : usr.Properties["description"].Value.ToString()))
                                {
                                    usr.Properties["description"].Clear();
                                    usr.Properties["description"].Add(_ADUserInfo.description);
                                }
                                break;
                        }
                    }

                    usr.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                switch (ex.GetType().ToString())
                {
                    case "InvalidOperationException":
                        {
                            InvalidOperationException InvOpEx = new InvalidOperationException();
                            EventLog.WriteEntry(sourceName, InvOpEx.Message, EventLogEntryType.Error);
                            break;
                        }
                    case "System.Runtime.InteropServices.COMException":
                        {
                            System.Runtime.InteropServices.COMException COMEx = (System.Runtime.InteropServices.COMException)ex;
                            EventLog.WriteEntry(sourceName, COMEx.Message, EventLogEntryType.Error);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Dels the user.
        /// </summary>
        /// <param name="_ADUserInfo">_ AD user info.</param>
        private void DelUser(ADUserInfo _ADUserInfo)
        {
            try
            {
                string ldapFullPath = ldapURI + ldapRoot;
                SearchResult result = GetLdapObjectPath(_ADUserInfo.employeeNumber, ldapFullPath, ADObjectType.user, SearchScope.Subtree);
                using (DirectoryEntry entry = new DirectoryEntry(result.Path))
                {
                    entry.DeleteTree();
                }
            }
            catch (Exception ex)
            {
                switch (ex.GetType().ToString())
                {
                    case "InvalidOperationException":
                        {
                            InvalidOperationException InvOpEx = new InvalidOperationException();
                            EventLog.WriteEntry(sourceName, InvOpEx.Message, EventLogEntryType.Error);
                            break;
                        }
                    case "System.Runtime.InteropServices.COMException":
                        {
                            System.Runtime.InteropServices.COMException COMEx = (System.Runtime.InteropServices.COMException)ex;
                            EventLog.WriteEntry(sourceName, COMEx.Message, EventLogEntryType.Error);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// OUs the sync.
        /// </summary>
        /// <returns></returns>
        public bool OUSync()
        {
            string nowTime = DateTime.Now.ToLocalTime().ToString().Replace(":", "_");

            for (int i = 0; i < syncOUFileArr.Length; i++)
            {
                if (!File.Exists(syncOUFileArr[i]))
                {
                    EventLog.WriteEntry(sourceName, "[" + syncOUFileArr[i] + "] 파일이 존재하지 않습니다", EventLogEntryType.Error);
                    continue;
                }

                DataSet ds = LoadXmlFile(syncOUFileArr[i]);
                try		// Root 부터 존재
                {
                    ds.Relations.Add("parentChild", ds.Tables[0].Columns["id"], ds.Tables[0].Columns["parentid"]);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr.IsNull("parentid"))		// 처음 생성일 경우
                        {
                            string parentLdapPath = "";
                            string ldapFullPath = ldapURI + ldapRoot;
                            switch (dr["action"].ToString().ToUpper())
                            {
                                case "I":
                                    parentLdapPath = AddOU(dr["name"].ToString(), dr["id"].ToString(), ldapFullPath);
                                    break;

                                case "E":
                                    parentLdapPath = ModOU(dr["name"].ToString(), dr["id"].ToString(), dr["parentid"].ToString(), ldapFullPath);
                                    break;

                                case "D":
                                    DelOU(dr["id"].ToString(), ldapFullPath);
                                    break;
                            }
                            dr["action"] = "C";

                            if (parentLdapPath != "")
                            {
                                SubOUTree(dr, parentLdapPath);
                            }
                        }
                    }

                    ouDataPath = syncOUFileArr[i] + "_" + nowTime + ".fix";
                    BuildFixFile(ds.Clone(), true);
                }
                catch  // Root가 존재 하지 않음
                {
                    try
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            string parentLdapPath = "";
                            string ldapFullPath = ldapURI + ldapRoot;
                            switch (dr["action"].ToString().ToUpper())
                            {
                                case "I":
                                    parentLdapPath = AddOU(dr["name"].ToString(), dr["id"].ToString(), ldapFullPath);
                                    break;

                                case "E":
                                    parentLdapPath = ModOU(dr["name"].ToString(), dr["id"].ToString(), dr["parentid"].ToString(), ldapFullPath);
                                    break;

                                case "D":
                                    DelOU(dr["id"].ToString(), ldapFullPath);
                                    break;
                            }

                            dr["action"] = "C";
                        }
                        ouDataPath = syncOUFileArr[i] + "_" + nowTime + ".fix";
                        BuildFixFile(ds.Clone(), true);
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry(sourceName, ex.Message.ToString(), EventLogEntryType.Error);
                        return false;
                    }
                }

                File.Move(syncOUFileArr[i], syncOUFileArr[i] + "_" + nowTime + ".dat");
            }

            return true;
        }

        /// <summary>
        /// Builds the fix file.
        /// </summary>
        /// <param name="_ds">_DS.</param>
        /// <param name="_dsObjType">_DS obj type.</param>
        private void BuildFixFile(DataSet _ds, bool _dsObjType)
        {
            DataRow[] fixRows = _ds.Tables[0].Select("action <> 'C'");
            DataSet fixDS = new DataSet();
            DataTable fixDT = new DataTable();

            foreach (DataRow row in fixRows)
            {
                fixDT.Rows.Add(row);
            }
            fixDS.Merge(fixDT);
            SetXmlFile(fixDS, _dsObjType);
        }

        /// <summary>
        /// Subs the OU tree.
        /// </summary>
        /// <param name="_dr">_DR.</param>
        /// <param name="_parentLdapPath">_parent ldap path.</param>
        private void SubOUTree(DataRow _dr, string _parentLdapPath)
        {
            foreach (DataRow childRow in _dr.GetChildRows("parentChild"))
            {
                string currentLdapPath = "";
                switch (childRow["action"].ToString().ToUpper())
                {
                    case "I":
                        currentLdapPath = AddOU(childRow["name"].ToString(), childRow["id"].ToString(), _parentLdapPath);
                        break;

                    case "E":
                        currentLdapPath = ModOU(childRow["name"].ToString(), childRow["id"].ToString(), childRow["parentid"].ToString(), _parentLdapPath);
                        break;

                    case "D":
                        DelOU(childRow["id"].ToString(), _parentLdapPath);
                        break;
                }

                childRow["action"] = "C";
                if (currentLdapPath != "")
                {
                    SubOUTree(childRow, currentLdapPath);
                }
            }
        }

        /// <summary>
        /// Adds the OU.
        /// </summary>
        /// <param name="_ouName">Name of the _ou.</param>
        /// <param name="_ouID">_ou ID.</param>
        /// <param name="_parentLdapPath">_parent ldap path.</param>
        /// <returns></returns>
        private string AddOU(string _ouName, string _ouID, string _parentLdapPath)
        {
            SearchResult result = GetLdapObjectPath(_ouID, _parentLdapPath, ADObjectType.organizationalUnit, SearchScope.OneLevel);

            if (result != null)
            {
                return result.Path;
            }

            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(_parentLdapPath))
                {
                    string name = "OU=" + _ouName;
                    DirectoryEntry newOU = entry.Children.Add(name, "organizationalUnit");
                    newOU.Properties["cn"].Value = _ouID;
                    newOU.CommitChanges();
                    return newOU.Path;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(sourceName, ex.Message.ToString(), EventLogEntryType.Error);
                return "";
            }
        }

        /// <summary>
        /// Mods the OU.
        /// </summary>
        /// <param name="_ouName">Name of the _ou.</param>
        /// <param name="_ouID">_ou ID.</param>
        /// <param name="_parentID">_parent ID.</param>
        /// <param name="_parentLdapPath">_parent ldap path.</param>
        /// <returns></returns>
        private string ModOU(string _ouName, string _ouID, string _parentID, string _parentLdapPath)
        {
            try
            {
                string ldapFullPath = ldapURI + ldapRoot;
                SearchResult curEntry = GetLdapObjectPath(_ouID, ldapFullPath, ADObjectType.organizationalUnit, SearchScope.Subtree);

                string parentID = "";

                DirectoryEntry entry = new DirectoryEntry(curEntry.Path);

                DirectoryEntry parentEntry = entry.Parent;
                parentID = parentEntry.Properties["cn"].Value.ToString();

                if (_parentID != parentID)		// 부모가 같지 않다
                {
                    entry.MoveTo(new DirectoryEntry(_parentLdapPath));
                }

                string curOuName = entry.Properties["cn"].Value.ToString();

                if (curOuName != _ouName)
                {
                    entry.Rename("OU=" + _ouName);
                }

                return entry.Path;
            }
            catch (Exception ex)
            {
                switch (ex.GetType().ToString())
                {
                    case "InvalidOperationException":
                        {
                            InvalidOperationException InvOpEx = new InvalidOperationException();
                            EventLog.WriteEntry(sourceName, InvOpEx.Message, EventLogEntryType.Error);
                            break;
                        }
                    case "System.Runtime.InteropServices.COMException":
                        {
                            System.Runtime.InteropServices.COMException COMEx = (System.Runtime.InteropServices.COMException)ex;
                            EventLog.WriteEntry(sourceName, COMEx.Message, EventLogEntryType.Error);
                            break;
                        }
                }
                return "";
            }
        }

        /// <summary>
        /// Dels the OU.
        /// </summary>
        /// <param name="_ouID">_ou ID.</param>
        /// <param name="_parentLdapPath">_parent ldap path.</param>
        private void DelOU(string _ouID, string _parentLdapPath)
        {
            try
            {
                SearchResult resultEntry = GetLdapObjectPath(_ouID, _parentLdapPath, ADObjectType.organizationalUnit, SearchScope.OneLevel);
                DirectoryEntry curEntry = new DirectoryEntry(resultEntry.Path);

                DirectorySearcher srchObject = new DirectorySearcher(curEntry);
                srchObject.Filter = "(objectClass=user)";
                srchObject.SearchScope = SearchScope.Subtree;

                //========= CN=Users 존재 유무 체크
                string ldapFullPath = ldapURI + ldapRoot;
                DirectorySearcher srchUserContainer = new DirectorySearcher(new DirectoryEntry(ldapFullPath));
                srchUserContainer.Filter = "(&(objectClass=container)(cn=Users))";
                srchUserContainer.SearchScope = SearchScope.OneLevel;
                SearchResult result = srchUserContainer.FindOne();

                DirectoryEntry usersContainer;
                if (result == null)
                {
                    using (DirectoryEntry rootEntry = new DirectoryEntry(ldapFullPath))
                    {
                        string name = "CN=Users";
                        usersContainer = rootEntry.Children.Add(name, "container");
                        usersContainer.CommitChanges();
                    }
                }
                else
                {
                    usersContainer = new DirectoryEntry(result.Path);
                }
                //========= CN=Users 존재 유무 체크

                DirectoryEntry userEntry;
                foreach (SearchResult res in srchObject.FindAll())
                {
                    userEntry = new DirectoryEntry(res.Path);
                    userEntry.MoveTo(usersContainer);
                }
                curEntry.DeleteTree();
            }
            catch (Exception ex)
            {
                switch (ex.GetType().ToString())
                {
                    case "InvalidOperationException":
                        {
                            InvalidOperationException InvOpEx = new InvalidOperationException();
                            EventLog.WriteEntry(sourceName, InvOpEx.Message, EventLogEntryType.Error);
                            break;
                        }
                    case "System.Runtime.InteropServices.COMException":
                        {
                            System.Runtime.InteropServices.COMException COMEx = (System.Runtime.InteropServices.COMException)ex;
                            EventLog.WriteEntry(sourceName, COMEx.Message, EventLogEntryType.Error);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets the ldap object path.
        /// </summary>
        /// <param name="_objID">_obj ID.</param>
        /// <param name="_basePath">_base path.</param>
        /// <param name="_ldapType">_ldap type.</param>
        /// <param name="_ldapScope">_ldap scope.</param>
        /// <returns></returns>
        private SearchResult GetLdapObjectPath(string _objID, string _basePath, ADObjectType _ldapType, SearchScope _ldapScope)
        {
            using (DirectoryEntry entry = new DirectoryEntry(_basePath))   //Full Search
            {
                DirectorySearcher srchObject = new DirectorySearcher(entry);

                string ldapQuery = "";
                SearchResult result = null;
                switch (_ldapType)
                {
                    case ADObjectType.organizationalUnit:
                        ldapQuery = "(&(objectClass=organizationalUnit)(cn=" + _objID + "))";
                        break;

                    case ADObjectType.user:
                        ldapQuery = "(&(objectClass=user)(employeeNumber=" + _objID + "))";
                        break;

                    case ADObjectType.group:
                        ldapQuery = "(&(objectClass=group)(displayName=" + _objID + "))";
                        break;
                }

                if (ldapQuery != "")
                {
                    srchObject.Filter = ldapQuery;
                    srchObject.SearchScope = _ldapScope;

                    result = srchObject.FindOne();
                }

                return result;
            }
        }
    }

    /// <summary>
    /// ADObjectType
    /// </summary>
    public enum ADObjectType
    {
        /// <summary>
        ///
        /// </summary>
        organizationalUnit = 0,

        /// <summary>
        ///
        /// </summary>
        user = 1,

        /// <summary>
        ///
        /// </summary>
        group = 2
    };

    /// <summary>
    /// ADUserInfo
    /// </summary>
    public class ADUserInfo			// 메뉴 리스트
    {
        /// <summary>
        /// displayName
        /// </summary>
        public string displayName;

        /// <summary>
        /// sn
        /// </summary>
        public string sn;

        /// <summary>
        /// givenName
        /// </summary>
        public string givenName;

        /// <summary>
        /// initials
        /// </summary>
        public string initials;

        /// <summary>
        /// title
        /// </summary>
        public string title;

        /// <summary>
        ///
        /// </summary>
        public string userid;

        /// <summary>
        /// userPassword
        /// </summary>
        public string userPassword;

        /// <summary>
        /// mail
        /// </summary>
        public string mail;

        /// <summary>
        /// telephoneNumber
        /// </summary>
        public string telephoneNumber;

        /// <summary>
        /// facsimileTelephoneNumber
        /// </summary>
        public string facsimileTelephoneNumber;

        /// <summary>
        /// mobile
        /// </summary>
        public string mobile;

        /// <summary>
        /// roomNumber
        /// </summary>
        public string roomNumber;

        /// <summary>
        /// carLicense
        /// </summary>
        public string carLicense;

        /// <summary>
        /// o
        /// </summary>
        public string o;

        /// <summary>
        /// ou
        /// </summary>
        public string ou;

        /// <summary>
        /// departmentNumber
        /// </summary>
        public string departmentNumber;

        /// <summary>
        /// employeeNumber
        /// </summary>
        public string employeeNumber;

        /// <summary>
        /// employeeType
        /// </summary>
        public string employeeType;

        /// <summary>
        /// preferredLanguage
        /// </summary>
        public string preferredLanguage;

        /// <summary>
        /// labeledURI
        /// </summary>
        public string labeledURI;

        /// <summary>
        /// description
        /// </summary>
        public string description;

        /// <summary>
        /// ADUserInfo
        /// </summary>
        public ADUserInfo()
        {
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            //cn = "";
            displayName = "";
            sn = "";
            givenName = "";
            initials = "";
            title = "";
            userid = "";
            userPassword = "";
            mail = "";
            telephoneNumber = "";
            facsimileTelephoneNumber = "";
            mobile = "";
            roomNumber = "";
            carLicense = "";
            o = "";
            ou = "";
            departmentNumber = "";
            employeeNumber = "";
            employeeType = "";
            preferredLanguage = "";
            labeledURI = "";
            description = "";
        }
    }
}