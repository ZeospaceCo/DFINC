using System;
using System.Collections.Generic;

namespace Zeospace.DeskPlus.Account.BE
{
    public class OrganizationSheetDocument
    {
        public int schemaVersion { get; set; }
        public string title { get; set; }
        public string sourceFileName { get; set; }
        public string sourceSheetName { get; set; }
        public List<double> columns { get; set; }
        public List<double> rows { get; set; }
        public List<OrganizationSheetStyle> styles { get; set; }
        public List<OrganizationSheetCell> cells { get; set; }
        public List<OrganizationSheetImage> images { get; set; }
        public List<OrganizationSheetLine> lines { get; set; }
    }

    public class OrganizationSheetCell
    {
        public int r { get; set; }
        public int c { get; set; }
        public int rs { get; set; }
        public int cs { get; set; }
        public string text { get; set; }
        public string formula { get; set; }
        public string prefix { get; set; }
        public string suffix { get; set; }
        public int style { get; set; }
    }

    public class OrganizationSheetBorder
    {
        public double width { get; set; }
        public string color { get; set; }
    }

    public class OrganizationSheetStyle
    {
        public double fontSize { get; set; }
        public bool bold { get; set; }
        public string color { get; set; }
        public string fill { get; set; }
        public string align { get; set; }
        public string vertical { get; set; }
        public OrganizationSheetBorder top { get; set; }
        public OrganizationSheetBorder right { get; set; }
        public OrganizationSheetBorder bottom { get; set; }
        public OrganizationSheetBorder left { get; set; }
    }

    public class OrganizationSheetAnchor
    {
        public int r { get; set; }
        public int c { get; set; }
        public double dx { get; set; }
        public double dy { get; set; }
    }

    public class OrganizationSheetImage
    {
        public string id { get; set; }
        public OrganizationSheetAnchor anchor { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string src { get; set; }
    }

    public class OrganizationSheetLine
    {
        public string id { get; set; }
        public OrganizationSheetAnchor start { get; set; }
        public OrganizationSheetAnchor end { get; set; }
        public double width { get; set; }
        public string color { get; set; }
    }

    public class OrganizationSheetRecord
    {
        public bool StorageReady { get; set; }
        public string LayoutJson { get; set; }
        public int Revision { get; set; }
        public int ModifyUserID { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
