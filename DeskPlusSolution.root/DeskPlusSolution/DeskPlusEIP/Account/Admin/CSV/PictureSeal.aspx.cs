using System;
using System.Drawing;
using System.IO;

namespace Zeospace.DeskPlus.Account.Web.Admin.CSV
{
    public partial class PictureSeal : Base.BasePageAccount
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 사진 업로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPhoto_Click(object sender, EventArgs e)
        {
            try
            {
                ltlPhoto.Text = "";

                DAL.User dal = new DAL.User();
                var userList = dal.GetUserInfo(CurrentUserInfo.CompanyID, false);

                string photoPath = "/DeskPlusEIP/FileData/Photo/";
                string photo = "";

                foreach (var info in userList)
                {
                    photo = info.LogInID + ".jpg";

                    if (File.Exists(Server.MapPath(photoPath + photo)))
                    {
                        if (!File.Exists(Server.MapPath(photoPath + CurrentUserInfo.CompanyID + "/users/" + info.UserID.ToString())))
                        {
                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.AddUserFolder(CurrentUserInfo.CompanyID, info.UserID);
                        }

                        #region 섬네일 이미지

                        System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(photoPath + photo));
                        //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                        Size thumbnailSize = new Size(94, 124);

                        System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                        thumbnail.Save(Server.MapPath("/DeskPlusEIP/FileData/" + info.CompanyID + "/users/" + info.UserID + "/picture/Photo.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);    //확장자 처리 2016-03-25

                        #endregion 섬네일 이미지

                        //File.Copy(
                        //        Server.MapPath(photoPath + photo),
                        //        Server.MapPath("/DeskPlusEIP/FileData/" + CurrentUserInfo.CompanyID + "/users/" + info.UserID + "/picture/Photo.jpg"), true);

                        dal.ModPhotoSeal(CurrentUserInfo.CompanyID, info.UserID, info.SealImage, "photo.jpg");
                        ltlPhoto.Text += photo + "이 사진 복사 되었습니다.<br>";
                    }
                }
            }
            catch (Exception ex)
            {
                this.ltlPhoto.Text = "btnPhoto_Click : " + ex.Message;
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("btnPhoto_Click : " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 도장 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDojang_Click(object sender, EventArgs e)
        {
            try
            {
                this.ltlDojang.Text = "";

                DAL.User dal = new DAL.User();
                var userList = dal.GetUserInfo(CurrentUserInfo.CompanyID, false);

                string dojangPath = "/DeskPlusEIP/FileData/Dojang/";
                string dojang = "";

                foreach (var info in userList)
                {
                    dojang = info.LogInID + ".jpg";

                    if (File.Exists(Server.MapPath(dojangPath + dojang)))
                    {
                        Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.AddUserFolder(info.CompanyID, info.UserID);

                        #region 섬네일 이미지

                        System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(dojangPath + dojang));
                        //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                        Size thumbnailSize = new Size(94, 124);

                        System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                        thumbnail.Save(Server.MapPath("/DeskPlusEIP/FileData/" + info.CompanyID + "/users/" + info.UserID + "/picture/seal.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                        #endregion 섬네일 이미지

                        //File.Copy(
                        //        Server.MapPath(dojangPath + dojang),
                        //        Server.MapPath("/DeskPlusEIP/FileData/" + info.CompanyID + "/users/" + info.UserID + "/picture/seal.jpg"), true);

                        dal.ModPhotoSeal(info.CompanyID, info.UserID, "seal.jpg", info.PhotoImage);
                        this.ltlDojang.Text += dojang + "이 도장 복사 되었습니다.<br>";
                    }
                }
            }
            catch (Exception ex)
            {
                this.ltlDojang.Text = "btnDojang_Click : " + ex.Message;
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("btnDojang_Click : " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 사용자별 폴더 자동생성하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCreateFolder_Click(object sender, EventArgs e)
        {
            try
            {
                this.ltlCreateFolder.Text = "";

                DAL.User dal = new DAL.User();
                var userList = dal.GetUserInfo(CurrentUserInfo.CompanyID, false);

                foreach (var info in userList)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.AddUserFolder(info.CompanyID, info.UserID);
                }

                this.ltlCreateFolder.Text += "사용자 폴더 자동생성 되었습니다.<br>";
            }
            catch (Exception ex)
            {
                this.ltlCreateFolder.Text = "btnCreateFolder_Click : " + ex.Message;
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("btnCreateFolder_Click : " + ex.Message, ex);
            }
        }
    }
}