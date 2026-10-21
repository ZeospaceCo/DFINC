using System.Configuration;
using System.DirectoryServices;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// ADAM(LDAP)에 연결하는 DataAccess Helper
    /// </summary>
    public abstract class LdapHelper
    {
        /// <summary>
        /// F : Ldap Root 경로를 web.config에서 가져온다
        /// </summary>
        public static readonly string LDAP_ROOT_URI = ConfigurationManager.AppSettings["LdapURI"] + ConfigurationManager.AppSettings["LdapRoot"];

        /// <summary>
        /// M : Ldap Root 객체 가져오기
        /// </summary>
        /// <param name="_ldapRootUri">DistinguishedName</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetRootDirectoryEntry(string _ldapRootUri)
        {
            return new DirectoryEntry(_ldapRootUri);
        }

        /// <summary>
        /// M : Ldap Tree에서 Roles Entry를 가져오기
        /// </summary>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetBaseRols()
        {
            string roleBase = ConfigurationManager.AppSettings["LdapURI"];
            roleBase += "CN=Roles,";
            roleBase += ConfigurationManager.AppSettings["LdapRoot"];
            return new DirectoryEntry(roleBase);
        }

        /// <summary>
        /// M : Ldap Tree에서 Users Entry를 가져오기
        /// </summary>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetBaseUsers()
        {
            string usersBase = ConfigurationManager.AppSettings["LdapURI"];
            usersBase += "CN=Users,";
            usersBase += ConfigurationManager.AppSettings["LdapRoot"];
            return new DirectoryEntry(usersBase);
        }

        /// <summary>
        /// M : 사용자 정보 가져오기
        /// </summary>
        /// <param name="_distinguishedName">User DistinguishedName</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindUser(string _distinguishedName)
        {
            string ldapFullPath = ConfigurationManager.AppSettings["LdapURI"] + _distinguishedName;

            return new DirectoryEntry(ldapFullPath);

            /*
            string query = "(&(objectClass=user)(distinguishedName=" + _distinguishedName + "))";

            SearchResult rtnResult = GetSearchResult(LDAP_ROOT_URI, query, SearchScope.Subtree);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
            */
        }

        /// <summary>
        /// M : 사용자 정보 찾기
        /// </summary>
        /// <param name="_searchBaseRoot">찾기 시작할 위치</param>
        /// <param name="_searchText">찾을 조건</param>
        /// <param name="_elementName">필드명</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindUser(string _searchBaseRoot, string _searchText, LdapElementName _elementName)
        {
            string conNm = "(" + GetElementName(_elementName) + "=" + _searchText + ")";
            string op = "&";

            string query = BuildQueryFilter("user", conNm, op);

            SearchResult rtnResult = GetSearchResult(_searchBaseRoot, query, SearchScope.Subtree);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// M : 사용자 찾기
        /// </summary>
        /// <param name="_searchBaseRoot">찾기 시작할 기본 위치</param>
        /// <param name="_searchFilter">찾을 조건</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindUser(string _searchBaseRoot, string _searchFilter)
        {
            string query = _searchFilter;

            SearchResult rtnResult = GetSearchResult(_searchBaseRoot, query, SearchScope.Subtree);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// M : 사용자 정보 찾기
        /// </summary>
        /// <param name="_searchBaseRoot">찾기 시작할 기본 위치</param>
        /// <param name="_searchText">찾을 조건</param>
        /// <param name="_elementName">필드명</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static SearchResultCollection GetFindUsers(string _searchBaseRoot, string _searchText, LdapElementName _elementName)
        {
            string conNm = "(" + GetElementName(_elementName) + "=" + _searchText + ")";
            string op = "&";

            string query = BuildQueryFilter("user", conNm, op);

            SearchResultCollection rtnResult = GetSearchResults(_searchBaseRoot, query, SearchScope.Subtree);

            if (rtnResult != null)
            {
                return rtnResult;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// M : OrganizationUnit 찾기
        /// </summary>
        /// <param name="_searchBaseRoot">찾을 기본위치</param>
        /// <param name="_departmentNumber">부서 ID</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindOU(string _searchBaseRoot, string _departmentNumber)
        {
            string query = "(&(objectClass=organizationalUnit)(cn=" + _departmentNumber + "))";

            SearchResult rtnResult = GetSearchResult(_searchBaseRoot, query, SearchScope.Subtree);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// M : Role 찾기
        /// </summary>
        /// <param name="_roleName">role 명</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindRole(string _roleName)
        {
            string query = "(&(objectClass=group)(cn=" + _roleName + "))";
            string searchBaseRole = ConfigurationManager.AppSettings["LdapURI"] + "CN=Roles," + ConfigurationManager.AppSettings["LdapRoot"];
            SearchResult rtnResult = GetSearchResult(searchBaseRole, query, SearchScope.OneLevel);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// M : Role 찾기
        /// </summary>
        /// <param name="_distinguishedName">role의 distinguishedName</param>
        /// <param name="_tmp">blank값</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindRole(string _distinguishedName, bool _tmp)
        {
            string ldapFullPath = ConfigurationManager.AppSettings["LdapURI"] + _distinguishedName;

            return new DirectoryEntry(ldapFullPath);

            /*
            string query = "(&(objectClass=group)(distinguishedName=" + _distinguishedName + "))";
            string searchBaseRole = ConfigurationManager.AppSettings["LdapURI"] + "CN=Roles," + ConfigurationManager.AppSettings["LdapRoot"];
            SearchResult rtnResult = GetSearchResult(searchBaseRole, query, SearchScope.OneLevel);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
            */
        }

        /// <summary>
        /// M : Container 찾기
        /// </summary>
        /// <param name="_searchBaseRoot">찾기 시작할 기본위치</param>
        /// <param name="_containerName">Container 이름</param>
        /// <param name="_scope">SearchScope</param>
        /// <returns>DirectoryEntry 객체</returns>
        public static DirectoryEntry GetFindContainer(string _searchBaseRoot, string _containerName, SearchScope _scope)
        {
            string query = "(&(objectClass=container)(cn=" + _containerName + "))";
            SearchResult rtnResult = GetSearchResult(_searchBaseRoot, query, _scope);

            if (rtnResult != null)
            {
                return new DirectoryEntry(rtnResult.Path);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// M : ClassName 가져오기
        /// </summary>
        /// <param name="_classType">LdapClassType</param>
        /// <returns>ClassType(organizationalUnit/user/group)</returns>
        private static string GetClassName(LdapClassType _classType)
        {
            string className = "";
            switch (_classType)
            {
                case LdapClassType.organizationalUnit:
                    className = "organizationalUnit";
                    break;

                case LdapClassType.user:
                    className = "user";
                    break;

                case LdapClassType.group:
                    className = "group";
                    break;
            }
            return className;
        }

        /// <summary>
        /// M : 필드명 가져오기
        /// </summary>
        /// <param name="_eleName">LdapElementName</param>
        /// <returns>필드명</returns>
        private static string GetElementName(LdapElementName _eleName)
        {
            string tmpName = "";
            switch (_eleName)
            {
                case LdapElementName.userid:
                    tmpName = "userid";
                    break;

                case LdapElementName.displayName:
                    tmpName = "displayName";
                    break;

                case LdapElementName.employeeNumber:
                    tmpName = "employeeNumber";
                    break;

                case LdapElementName.departmentNumber:
                    tmpName = "departmentNumber";
                    break;

                case LdapElementName.mail:
                    tmpName = "mail";
                    break;

                case LdapElementName.title:
                    tmpName = "title";
                    break;

                case LdapElementName.ou:
                    tmpName = "ou";
                    break;
            }
            return tmpName;
        }

        /// <summary>
        /// M : 검색결과 가져오기
        /// </summary>
        /// <param name="_basePath">기본 검색위치</param>
        /// <param name="_query">조건</param>
        /// <param name="_srchScope">SearchScope</param>
        /// <returns>SearchResult</returns>
        private static SearchResult GetSearchResult(string _basePath, string _query, SearchScope _srchScope)
        {
            using (DirectoryEntry entry = new DirectoryEntry(_basePath))   //Full Search
            {
                DirectorySearcher srch = new DirectorySearcher(entry);

                SearchResult result = null;

                if (_query != "")
                {
                    srch.Filter = _query;
                    srch.SearchScope = _srchScope;
                    result = srch.FindOne();
                }
                return result;
            }
        }

        /// <summary>
        /// 검색 결과들 가져오기
        /// </summary>
        /// <param name="_basePath">검색할 기본위치</param>
        /// <param name="_query">조건</param>
        /// <param name="_srchScope">SearchScope</param>
        /// <returns>SearchResultCollection</returns>
        private static SearchResultCollection GetSearchResults(string _basePath, string _query, SearchScope _srchScope)
        {
            using (DirectoryEntry entry = new DirectoryEntry(_basePath))   //Full Search
            {
                DirectorySearcher srch = new DirectorySearcher(entry);

                SearchResultCollection result = null;

                if (_query != "")
                {
                    srch.Filter = _query;
                    srch.SearchScope = _srchScope;

                    result = srch.FindAll();
                }
                return result;
            }
        }

        /// <summary>
        /// M : 조건 만들기
        /// </summary>
        /// <param name="_className">className</param>
        /// <param name="_condition">조건</param>
        /// <param name="_operator">연산자</param>
        /// <returns>검색결과</returns>
        public static string BuildQueryFilter(string _className, string _condition, string _operator)
        {
            string[] _tmpCondition = { _condition };
            string[] _tmpOperator = { _operator };
            return BuildQueryFilter(_className, _tmpCondition, _tmpOperator);
        }

        /// <summary>
        /// M : 조건 만들기
        /// </summary>
        /// <param name="_className">className</param>
        /// <param name="_condition">조건</param>
        /// <param name="_operator">연산자</param>
        /// <returns></returns>
        private static string BuildQueryFilter(string _className, string[] _condition, string[] _operator)
        {
            string query = "(objectClass=" + _className + ")";

            for (int i = 0; i < _condition.Length; i++)
            {
                string tmpCondition = _condition[i];
                query = "(" + _operator[i] + query + tmpCondition + ")";
            }

            return query;
        }
    }

    /// <summary>
    /// E : LDAP 클래스 종류
    /// </summary>
    public enum LdapClassType
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
    /// E : LDAP 필드명
    /// </summary>
    public enum LdapElementName
    {
        /// <summary>
        ///
        /// </summary>
        userid = 0,

        /// <summary>
        ///
        /// </summary>
        displayName = 1,

        /// <summary>
        ///
        /// </summary>
        employeeNumber = 2,

        /// <summary>
        ///
        /// </summary>
        departmentNumber = 3,

        /// <summary>
        ///
        /// </summary>
        mail = 4,

        /// <summary>
        ///
        /// </summary>
        title = 5,

        /// <summary>
        ///
        /// </summary>
        ou = 6
    }

    /// <summary>
    /// E : LDAP 결과조건
    /// </summary>
    public enum LdapFindResult
    {
        /// <summary>
        ///
        /// </summary>
        one = 0,

        /// <summary>
        ///
        /// </summary>
        all = 1
    }
}