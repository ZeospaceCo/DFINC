using System;
using System.Data;
using System.Data.SqlClient;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class OrganizationChart : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 로그인한 회사의 활성 부서와 구성원 소속을 함께 조회한다.
        /// 비밀번호 및 개인 신상정보는 조직도 조회 대상에 포함하지 않는다.
        /// </summary>
        public DataSet GetChart(int companyID)
        {
            if (companyID <= 0) throw new ArgumentOutOfRangeException("companyID");

            const string query = @"
SELECT DeptID, ParentDeptID, DeptOrder,
       NameBase, NameKo, NameEn, NameJa, NameFr, NameZhcn, NameZhtw, Phone
FROM dbo.DP_ACC_Dept
WHERE CompanyID = @CompanyID AND ActiveDept = '1'
ORDER BY DeptOrder, DeptID;

SELECT u.UserID, ISNULL(ud.DeptID, 0) AS DeptID, ud.BaseDept,
       u.NameBase, u.NameKo, u.NameEn, u.NameJa, u.NameFr, u.NameZhcn, u.NameZhtw,
       title.NameBase AS TitleBase, title.NameKo AS TitleKo,
       title.NameEn AS TitleEn, title.NameJa AS TitleJa, title.NameFr AS TitleFr,
       title.NameZhcn AS TitleZhcn, title.NameZhtw AS TitleZhtw,
       duty.NameBase AS DutyBase, duty.NameKo AS DutyKo,
       duty.NameEn AS DutyEn, duty.NameJa AS DutyJa, duty.NameFr AS DutyFr,
       duty.NameZhcn AS DutyZhcn, duty.NameZhtw AS DutyZhtw,
       u.OfficePhone, u.ExtensionOfficePhone, u.MobilePhone, u.PhotoImage,
       CASE WHEN ISNULL(ud.MailID, '') <> '' AND ISNULL(ud.MailDomain, '') <> ''
            THEN ud.MailID + '@' + ud.MailDomain
            WHEN ISNULL(u.MailID, '') <> '' AND ISNULL(u.MailDomain, '') <> ''
            THEN u.MailID + '@' + u.MailDomain ELSE '' END AS Email
FROM dbo.DP_ACC_User AS u
LEFT JOIN dbo.DP_ACC_UserDept AS ud
  ON ud.CompanyID = u.CompanyID AND ud.UserID = u.UserID
 AND EXISTS (SELECT 1 FROM dbo.DP_ACC_Dept AS d
             WHERE d.CompanyID = ud.CompanyID AND d.DeptID = ud.DeptID AND d.ActiveDept = '1')
LEFT JOIN dbo.DP_COM_Code AS title
  ON title.CompanyID = ud.CompanyID AND title.CodeID = ud.CodeTitleID
LEFT JOIN dbo.DP_COM_Code AS duty
  ON duty.CompanyID = ud.CompanyID AND duty.CodeID = ud.CodeDutyID
WHERE u.CompanyID = @CompanyID AND u.ActiveUser = '1'
ORDER BY ISNULL(duty.CodeOrder, 2147483647), ISNULL(title.CodeOrder, 2147483647),
         ud.UserDeptOrder DESC, u.NameBase, u.UserID, ud.DeptID;";

            using (var connection = new SqlConnection(ConnSolution(companyID)))
            using (var command = new SqlCommand(query, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                command.Parameters.Add("@CompanyID", SqlDbType.Int).Value = companyID;
                var data = new DataSet();
                adapter.Fill(data);
                data.Tables[0].TableName = "Departments";
                data.Tables[1].TableName = "Members";
                return data;
            }
        }
    }
}
