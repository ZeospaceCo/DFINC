using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Approval.DAL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Xml;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class EnforceStamp : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD
        #endregion

        #region MOD
        #endregion

        #region DEL
        /// <summary>
        /// 시행문인장삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_enforceStampID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _enforceStampID, int _deptID)
        {
            try
            {
                if (_companyID.Equals(0) || _enforceStampID.Equals(0) || _deptID.Equals(0)) return false;

                bool rtnval = false;

                DAL.EnforceStamp dal = new DAL.EnforceStamp();
                rtnval = dal.Del(_companyID, _enforceStampID, _deptID);
                if (rtnval)
                {
                    // 첨부파일이 있다면, 해당 부분 삭제
                    string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/
                    string targetFolder = baseFileDirectory + _companyID.ToString() + "/Approval/Enforce/Stamp/" + _deptID.ToString();

                    //  ex) /DeskPlusEIP/FileData/1/Approval/Enforce/Stamp/deptID/Stamp_deptID.png
                    if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                }
                
                return rtnval;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.BLL.EnforceStamp[Del] : " + ex);
                return false;
            }
        }
        #endregion
    }
}
