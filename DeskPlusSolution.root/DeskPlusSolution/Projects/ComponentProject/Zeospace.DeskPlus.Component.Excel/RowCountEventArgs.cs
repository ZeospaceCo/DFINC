using System;
using System.Text;

namespace Zeospace.DeskPlus.Component.Excel
{
    /// <summary>
    /// RowCountEventArgs V1.0
    /// </summary>
    public class RowCountEventArgs : System.EventArgs
    {
        private int rowCount = 0;
        private string tableName = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="RowCountEventArgs"/> class.
        /// </summary>
        /// <param name="rowCount">The row count.</param>
        public RowCountEventArgs(int rowCount)
        {
            this.rowCount = rowCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RowCountEventArgs"/> class.
        /// </summary>
        /// <param name="rowCount">The row count.</param>
        /// <param name="tableName">Name of the table.</param>
        public RowCountEventArgs(int rowCount, string tableName)
        {
            this.rowCount = rowCount;
            this.tableName = tableName;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TableName
        {
            get
            {
                return this.tableName;
            }
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        /// <value>The row count.</value>
        public int RowCount
        {
            get
            {
                return this.rowCount;
            }
        }
    }
}
