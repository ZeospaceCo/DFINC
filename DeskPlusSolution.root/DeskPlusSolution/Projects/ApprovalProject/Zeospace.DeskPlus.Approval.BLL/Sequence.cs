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

namespace Zeospace.DeskPlus.Approval.BLL
{
    public class Sequence : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET

        /// <summary>
        /// 해당 결재 결재페이지 접근가능여부 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_sequence"></param>
        /// <param name="_approvalTag"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public BE.SequenceInfo GetViewing(int _companyID, int _documentID, int _sequence, string _approvalTag, int _userID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0) || _approvalTag.Equals("")) return null;

            DAL.Sequence seqDal = new DAL.Sequence();
            BE.SequenceInfo info = seqDal.GetViewing(_companyID, _documentID, _sequence, _approvalTag);

            info.IsViewing = "N";
            if (info != null)
            {
                if (!info.UserID.Equals(_userID))
                {
                    if (info.DocViewing.Equals("Y") && info.Viewing.Equals("Y") && info.ViewingTime.AddMinutes(30) > DateTime.Now)
                    {
                        info.IsViewing = "Y";   //rtnval = info.DeptBase + "/" + info.TitleBase + "/" + info.NameBase;
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 해당 결재 결재페이지 접근가능여부 확인
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public BE.SequenceInfo GetViewing(int _companyID, int _documentID, int _userID)
        {
            if (_companyID.Equals(0) || _documentID.Equals(0)) return null;

            DAL.Sequence seqDal = new DAL.Sequence();
            BE.SequenceInfo info = seqDal.GetViewing(_companyID, _documentID);

            info.IsViewing = "N";
            if (info != null)
            {
                if (info.DocViewing.Equals("Y") && !info.UserID.Equals(_userID))
                {
                    if (info.DocViewing.Equals("Y") && info.Viewing.Equals("Y") && info.ViewingTime.AddMinutes(30) > DateTime.Now)
                    {
                        info.IsViewing = "Y";
                        //rtnval = info.DeptBase + "/" + info.TitleBase + "/" + info.NameBase;
                    }
                }
            }

            return info;
        }
        #endregion

        #region ADD
        #endregion

        #region MOD
        #endregion

        #region DEL
        #endregion
    }
}
