using System;
using System.Collections.Generic;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class HomeList : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        /// <summary>
        /// new 표기여부
        /// </summary>
        /// <param name="_al">The _al.</param>
        /// <returns></returns>
        private List<BE.ListViewModel> SetShowNew(List<BE.ListViewModel> _al)
        {
            int i = 0;

            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

            foreach (var info in _al)
            {                
                TimeSpan gap = todayDate.Subtract(info.WriteDateDT);
                _al[i].ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";

                i += 1;
            }

            return _al;
        }

        #region 공용 템플릿

        /// <summary>
        /// 최신글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetRecentWrite(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetRecentWrite(_companyID, _userID, _pageSize));
        }

        /// <summary>
        /// 최다 댓글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMaxShort(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMaxShort(_companyID, _userID, _pageSize));
        }

        /// <summary>
        /// 최다 조회
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMaxRead(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMaxRead(_companyID, _userID, _pageSize));
        }

        /// <summary>
        /// 평가우수
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMaxRate(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMaxRate(_companyID, _userID, _pageSize));
        }

        #endregion 공용 템플릿

        #region 나의 템플릿

        /// <summary>
        /// 등록한 게시글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyWrite(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMyWrite(_companyID, _userID, _pageSize));
        }

        /// <summary>
        /// 댓글 단 게시글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyListByShort(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMyListByShort(_companyID, _userID, _pageSize));
        }

        /// <summary>
        /// 등록한 댓글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyShort(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMyShort(_companyID, _userID, _pageSize));
        }

        /// <summary>
        /// 중요표시
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyImpo(int _companyID, int _userID, int _pageSize)
        {
            DAL.HomeList dal = new DAL.HomeList();
            return SetShowNew(dal.GetMyImpo(_companyID, _userID, _pageSize));
        }

        #endregion 나의 템플릿

        #region 최다 작성자,최다 댓글자

        /// <summary>
        /// 금년도 최다 작성자 가져오기
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.MaxWriterInfo> GetMaxWriter(int _companyID, int _pageSize)
        {
            try
            {
                DAL.HomeList dal = new DAL.HomeList();
                return dal.GetMaxWriter(_companyID, _pageSize);
            }
            catch (Exception ex)
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 금년도 최다 댓글자 가져오기
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.MaxShortReplyerInfo> GetMaxShorter(int _companyID, int _pageSize)
        {
            try
            {
                DAL.HomeList dal = new DAL.HomeList();
                return dal.GetMaxShorter(_companyID, _pageSize);
            }
            catch (Exception ex)
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 최다 작성자,최다 댓글자


        /// <summary>
        /// Gets the new tab.
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <returns></returns>
        public List<int> GetNewTab(int _companyID, int _userID)
        {
            try
            {
                DAL.HomeList dal = new DAL.HomeList();
                return dal.GetNewTab(_companyID, _userID);
            }
            catch (Exception ex)
            {
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}