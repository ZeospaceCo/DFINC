using System;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.Util;

namespace Zeospace.DeskPlus.Component.Excel
{
    /// <summary>
    /// Serves as the base class that defines the methods, properties and events common 
    /// to all datagrid exporters in the Web.Generic.DataGridTools
    /// </summary>
    public abstract class DataGridExporterBase
    {
        /// <summary>
        /// Holds a reference to the datagrid being exported
        /// </summary>
        protected DataGrid MyDataGrid;

        /// <summary>
        /// Holds a reference to the page where the datagrid locates
        /// </summary>
        protected Page CurrentPage;

        /// <summary>
        /// Overloaded. Initializes a new instance of the DataGridExporterBase class.
        /// </summary>
        /// <param name="dg">The datagrid to be exported</param>
        /// <param name="pg">The page to which the datagrid is to be exported</param>
        public DataGridExporterBase(DataGrid dg, Page pg)
        {
            MyDataGrid = dg;
            CurrentPage = pg;
        }

        /// <summary>
        /// Overloaded. Initializes a new instance of the DataGridExporterBase class.
        /// </summary>
        /// <param name="dg">The datagrid to be exported</param>
        public DataGridExporterBase(DataGrid dg)
            : this(dg, dg.Page)
        {
        }

        /// <summary>
        /// Exports the current datagrid
        /// </summary>
        public abstract void Export();
    }

    /// <summary>
    /// Exports a datagrid to a excel file. 
    /// </summary>
    /// <requirements>Microsoft Excel 97 or above should be installed on the client machine in order to make 
    /// this function work
    /// </requirements>
    public class DataGridExcelExporter : DataGridExporterBase
    {

        /// <summary>
        /// CSS file for decoration, se it if any or dont use it
        /// </summary>
        private const string MY_CSS_FILE = "./css/MDF.css";

        /// <summary>
        /// Overloaded. Initializes a new instance of the DataGridExcelExporter class.
        /// </summary>
        /// <param name="dg">The datagrid to be exported</param>
        /// <param name="pg">The page to which the datagrid is to be exported</param>
        public DataGridExcelExporter(DataGrid dg, Page pg)
            : base(dg, pg)
        {
        }

        /// <summary>
        /// Overloaded. Initializes a new instance of the DataGridExcelExporter class.
        /// </summary>
        /// <param name="dg">The datagrid to be exported</param>
        public DataGridExcelExporter(DataGrid dg)
            : base(dg)
        {
        }

        /// <summary>
        /// Overloaded. Exports a datagrid to an excel file, the title of which is empty
        /// </summary>
        public override void Export()
        {
            Export(String.Empty);
        }

        /// <summary>
        /// Renders the html text before the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        protected virtual void FrontDecorator(HtmlTextWriter writer)
        {
            writer.WriteFullBeginTag("HTML");
            writer.WriteFullBeginTag("Head");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.Write("td{mso-number-format:\"\\@\";}");

            //StreamReader sr = File.OpenText(CurrentPage.MapPath(MY_CSS_FILE));
            //String input;
            //while ((input = sr.ReadLine()) != null)
            //{
            //    writer.WriteLine(input);
            //}
            //sr.Close();
            //writer.Write("-->");
            writer.RenderEndTag();
            writer.WriteEndTag("Head");
            writer.WriteFullBeginTag("Body");
        }

        /// <summary>
        /// Renders the html text after the datagrid.
        /// </summary>
        /// <param name="writer">A HtmlTextWriter to write html to output stream</param>
        protected virtual void RearDecorator(HtmlTextWriter writer)
        {
            writer.WriteEndTag("Body");
            writer.WriteEndTag("HTML");
        }

        /// <summary>
        /// Exports the datagrid to an Excel file with the name of the datasheet provided by the passed in parameter
        /// </summary>
        /// <param name="reportName">Name of the datasheet.
        /// </param>
        public virtual void Export(string reportName)
        {
            ClearChildControls(MyDataGrid);
            MyDataGrid.EnableViewState = false;//Gets rid of the viewstate of the control. The viewstate may make an excel file unreadable.


            CurrentPage.Response.Clear();
            CurrentPage.Response.Buffer = true;

            //This will make the browser interpret the output as an Excel file            
            //CurrentPage.Response.AddHeader("Content-Disposition", "filename=" + reportName);
            CurrentPage.Response.AddHeader("Content-Disposition", "attachment;filename=" + reportName);
            CurrentPage.Response.ContentType = "application/vnd.ms-excel;charset=utf-8";
            CurrentPage.Response.Charset = "";

            

            //Prepares the html and write it into a StringWriter
            StringWriter stringWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                        
            FrontDecorator(htmlWriter);
            MyDataGrid.RenderControl(htmlWriter);
            RearDecorator(htmlWriter);

            String convStr = stringWriter.ToString();

            convStr = System.Text.RegularExpressions.Regex.Replace(convStr, "border=\"0\"", "border=\"1\"");
            convStr = System.Text.RegularExpressions.Regex.Replace(convStr, "&nbsp;", " ");

            //Write the content to the web browser
            CurrentPage.Response.Write(convStr);
            //CurrentPage.Response.End();

	    CurrentPage.Response.Flush();
            CurrentPage.Response.SuppressContent = true; 
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Iterates a control and its children controls, ensuring they are all LiteralControls
        /// <remarks>
        /// Only LiteralControl can call RenderControl(System.Web.UI.HTMLTextWriter htmlWriter) method. Otherwise 
        /// a runtime error will occur. This is the reason why this method exists.
        /// </remarks>
        /// </summary>
        /// <param name="control">The control to be cleared and verified</param>
        private void RecursiveClear(Control control)
        {
            //Clears children controls
            for (int i = control.Controls.Count - 1; i >= 0; i--)
            {
                RecursiveClear(control.Controls[i]);
            }

            //
            //If it is a LinkButton, convert it to a LiteralControl
            //
            if (control is LinkButton)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                literal.Text = ((LinkButton)control).Text;
                control.Parent.Controls.Remove(control);
            }
            //We don't need a button in the excel sheet, so simply delete it
            else if (control is Button)
            {
                control.Parent.Controls.Remove(control);
            }

            //If it is a ListControl, copy the text to a new LiteralControl
            else if (control is ListControl)
            {
                LiteralControl literal = new LiteralControl();
                control.Parent.Controls.Add(literal);
                try
                {
                    literal.Text = ((ListControl)control).SelectedItem.Text;
                }
                catch
                {
                }
                control.Parent.Controls.Remove(control);

            }
            //You may add more conditions when necessary

            return;
        }

        /// <summary>
        /// Clears the child controls of a Datagrid to make sure all controls are LiteralControls
        /// </summary>
        /// <param name="dg">Datagrid to be cleared and verified</param>
        protected void ClearChildControls(DataGrid dg)
        {

            for (int i = dg.Columns.Count - 1; i >= 0; i--)
            {
                DataGridColumn column = dg.Columns[i];
                if (column is ButtonColumn)
                {
                    dg.Columns.Remove(column);
                }
            }

            this.RecursiveClear(dg);

        }

    }

    /// <summary>
    /// HTML Encodes an entire DataGrid. 
    /// It iterates through each cell in the TableRow, ensuring that all 
    /// the text being displayed is HTML Encoded, irrespective of whether 
    /// they are just plain text, buttons, hyperlinks, multiple controls etc..
    /// </summary>
    public class CellFormater
    {
        /// <summary>
        /// Constructs an instance of the CellFormater class.
        /// </summary>
        public CellFormater()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        /// <summary>
        /// Method that HTML Encodes an entire DataGrid. 
        /// It iterates through each cell in the TableRow, ensuring that all 
        /// the text being displayed is HTML Encoded, irrespective of whether 
        /// they are just plain text, buttons, hyperlinks, multiple controls etc..
        /// <seealso cref="System.Web.UI.WebControls.DataGrid.ItemDataBound">DataGrid.ItemDataBound Event</seealso>
        /// </summary>
        /// <param name="item">
        /// The DataGridItem that is currently being bound in the calling Web 
        /// Page's DataGrid.ItemDataBound Event.
        /// </param>
        /// <remarks>
        /// This method should be called from the 
        /// <c>DataGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)</c> 
        /// event in the respective Web View Codebehind.
        /// </remarks>
        /// <example>
        ///          We want to HTMLEncode a complete DataGrid (all columns and all 
        ///          rows that may/do contain characters that will require encoding 
        ///          for display in HTML) called dgIssues.
        ///          Use the following code for the ItemDataBound Event:
        ///          <code>
        ///               private void dgIssues_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        ///               {
        ///                    WebMethod wm = new WebMethod();
        ///                    wm.DataGrid_ItemDataBound_HTMLEncode((DataGridItem) e.Item);
        ///               }//dgIssues_ItemDataBound
        ///          </code>
        /// </example>
        public void AdHocHTMLEncode(System.Web.UI.WebControls.DataGridItem item)
        {
            bool doHTMLEncode = false;
            switch (item.ItemType)
            {
                #region DataBound
                //The following case statements are in ascending TableItemStyle order.
                //See ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfsystemwebuiwebcontrolsdatagridclassitemstyletopic.htm for details.
                case System.Web.UI.WebControls.ListItemType.Item:
                    {
                        doHTMLEncode = true;
                        break;
                    }//ListItemType.Item
                case System.Web.UI.WebControls.ListItemType.AlternatingItem:
                    {
                        doHTMLEncode = true;
                        break;
                    }//ListItemType.AlternatingItem
                case System.Web.UI.WebControls.ListItemType.SelectedItem:
                    {
                        doHTMLEncode = true;
                        break;
                    }//ListItemType.SelectedItem					
                case System.Web.UI.WebControls.ListItemType.EditItem:
                    {
                        //These should not be prone to this as TextBoxes aren't.
                        doHTMLEncode = false;
                        break;
                    }//ListItemType.EditItem
                #endregion DataBound
                #region Non-DataBound
                //The remainder are the other ListItemTypes that are non-Data-bound.
                case System.Web.UI.WebControls.ListItemType.Header:
                    {
                        //We might have specified Headers like "<ID>".
                        doHTMLEncode = true;
                        break;
                    }//ListItemType.Header
                case System.Web.UI.WebControls.ListItemType.Footer:
                    {
                        //Similarly for the Footer as with the Header.
                        doHTMLEncode = true;

                        break;
                    }//ListItemType.Footer
                case System.Web.UI.WebControls.ListItemType.Pager:
                    {
                        //With just numbers or buttons, none is required.
                        //However, for buttons, this is not strictly true as you 
                        //need to specify the text on the buttons. But the Property 
                        //Builder for the DataGrid hints in its defaults that these 
                        //need to be HTMLencoded anyway.
                        doHTMLEncode = false;
                        break;
                    }//ListItemType.Pager
                case System.Web.UI.WebControls.ListItemType.Separator:
                    {
                        doHTMLEncode = false;
                        break;
                    }//ListItemType.Separator
                #endregion Non-DataBound
                default:
                    {
                        //This will never be executed as all ItemTypes are listed above.
                        break;
                    }//default
            }//switch

            if (doHTMLEncode)
            {
                //Encode the cells dependent on the type of content 
                //within (e.g. BoundColumn, Hyperlink), taking into account 
                //that there may be more than one (or even zero) control in 
                //each cell.
                System.Web.UI.WebControls.TableCellCollection cells = (System.Web.UI.WebControls.TableCellCollection)item.Cells;
                foreach (System.Web.UI.WebControls.TableCell cell in cells)
                {
                    if (cell.Controls.Count != 0)
                    {
                        foreach (System.Web.UI.Control ctrl in cell.Controls)
                        {
                            if (ctrl is Button)
                            {
                                Button btn = (Button)ctrl;
                                btn.Text = HttpUtility.HtmlEncode(btn.Text);
                            }//if
                            else if (ctrl is HyperLink)
                            {
                                HyperLink hyp = (HyperLink)ctrl;
                                hyp.Text = HttpUtility.HtmlEncode(hyp.Text);
                                //hyp.NavigateUrl = HttpUtility.UrlEncode(hyp.NavigateUrl);
                            }//else if
                            else if (ctrl is LinkButton)
                            {
                                LinkButton lb = (LinkButton)ctrl;
                                lb.Text = HttpUtility.HtmlEncode(lb.Text);
                            }//else if
                            // this check is for to change the forecolor of REJECTED activities to red
                            else if (ctrl is Label)
                            {
                                Label objL = (Label)ctrl;
                                if (objL.Text == "REJECTED")
                                    objL.ForeColor = System.Drawing.Color.Red;
                            }//else if
                        }//foreach
                    }//if
                    else
                    {
                        //The cell is a BoundColumn.
                        if (cell.Text.ToLower().Trim() != "&nbsp;")
                            cell.Text = HttpUtility.HtmlEncode(cell.Text);

                    }//else
                }//foreach
            }//if
        }//DataGrid_ItemDataBound_HTMLEncode
    }
}
