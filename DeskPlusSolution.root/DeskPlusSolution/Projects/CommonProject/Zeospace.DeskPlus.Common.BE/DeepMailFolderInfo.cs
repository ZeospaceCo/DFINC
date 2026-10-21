using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class DeepMailFolderInfo
    {
        /// <summary>
        /// 메일함 UID
        /// </summary>
        public string uid { get; set; }

        /// <summary>
        /// 상위 메일함 UID
        /// </summary>
        public string puid { get; set; }

        /// <summary>
        /// 메일함 이름
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 총 사이즈
        /// </summary>
        public float totalSize { get; set; }

        /// <summary>
        /// 총 메일건수
        /// </summary>
        public int totalCount { get; set; }

        /// <summary>
        /// 읽지 않은 메일건
        /// </summary>
        public int unseenCount { get; set; }
    }
}