using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Proposal.BE
{
    [Serializable]
    public class ExamInfo
    {
        public ExamInfo() { }

        public int CompanyID { get; set; }
        public int ProposalNo { get; set; }
        public int Degree { get; set; }
        public int UserID { get; set; }
        public int ItemNo { get; set; }
        public double Score1 { get; set; }
        public double Score2 { get; set; }
        public double Score3 { get; set; }
        public double Score4 { get; set; }
        public double Score5 { get; set; }
        public double Score6 { get; set; }
        public double Score7 { get; set; }
        public double Score8 { get; set; }
        public double Score9 { get; set; }
        public double Score10 { get; set; }
        public double TotalScore { get; set; }
        public string JudgeView { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
