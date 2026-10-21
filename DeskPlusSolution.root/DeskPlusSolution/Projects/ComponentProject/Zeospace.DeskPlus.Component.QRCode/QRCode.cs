using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;

namespace Zeospace.DeskPlus.Component.QRCode
{
    public class QRCode
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Create(int _size, int _version, string _correction, string _path, string _content, string _fileName)
        {
            try
            {
                // QRCodeEncoder 인스턴스 생성
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

                // 인코딩모드는 바이트로 설정.
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                // QRCode 사이즈 지정.
                qrCodeEncoder.QRCodeScale = _size;
                // 버전을 지정.(주의 : MECOARD로 만들려면 9이상으로 지정한다.)
                qrCodeEncoder.QRCodeVersion = _version;

                // 에러 보정 레벨을 지정.
                switch (_correction)
                {
                    case "H":
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                        break;
                    case "Q":
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
                        break;
                    case "L":
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
                        break;
                    case "M":
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        break;
                    default:
                        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                        break;
                }   

                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }

                System.Drawing.Image qrImage;

                // QRCode 이미지를 생성해 줌.
                qrImage = qrCodeEncoder.Encode(_content);
                qrImage.Save(_path + _fileName);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("QRCode", ex);
                return false;
            }
        }
    }
}
