namespace Sunplus.Agent.Utilities
{
    using System;
    using System.Data;
    using System.Collections.Generic;
    using System.Text;

    public class HtmlGenerator
    {
        #region Combobox

        public static string ComboBox(string id, object selectedValue, IEnumerable<object> dataSource, string onchange)
        {
            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"");

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            foreach (object value in dataSource)
            {
                builder.Append("<option value=\"");
                builder.Append(value);
                builder.Append("\"");

                if (value.Equals(selectedValue))
                {
                    builder.Append(" selected");
                }

                builder.Append(">");
                builder.Append(value);
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        public static string ComboBox(string id, IEnumerable<object> dataSource)
        {
            return ComboBox(id, null, dataSource, null);
        }

        /// <summary>
        /// Generate combo box for position taking / commission.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static string ComboBox(string id, double min, double max, double step)
        {
            return ComboBox(id, min, max, step, null);
        }

        /// <summary>
        /// Generate combo box for position taking / commission.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="step"></param>
        /// <param name="onchange"></param>
        /// <returns></returns>
        public static string ComboBox(string id, double min, double max, double step, string onchange)
        {
            return ComboBox(id, double.MinValue, min, max, step, onchange);
        }

        /// <summary>
        /// Generate combo box for position taking / commission.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectedValue"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="step"></param>
        /// <param name="onchange"></param>
        /// <returns></returns>
        public static string ComboBox(string id, double selectedValue, double min, double max, double step, string onchange)
        {
            if (0 == step) throw new DivideByZeroException();

            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"");

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            int c = (int)(Math.Round((max - min) / step));
            for (int i = c; i >= 0; i--)
            {
                double f = min + (i * step);

                builder.Append("<option value=\"");
                builder.Append(f);
                builder.Append("\"");

                if (Math.Abs(selectedValue - f) < 0.000000001)
                {
                    builder.Append(" selected");
                }

                builder.Append(">");
                builder.Append(f);
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }
        public static string ComboBox(string id, double selectedValue, double min, double max, double step, bool withButton, string onchange)
        {
            return ComboBox(id, selectedValue, min, max, step, withButton, onchange); 
        }
        public static string ComboBox(string id, double selectedValue, bool disabled, double min, double max, double step, string onchange)
        {
            if (0 == step) throw new DivideByZeroException();

            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"" + (disabled == true ? "disabled='true'" : string.Empty));

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            var hasSelected = false;
            var maxEqualMin = false;

            if (min <= max)
            {

                if (max == min)
                    maxEqualMin = true;

                var tmp = max == min ? min + step : max;

                int c = (int)(Math.Round((tmp - min) / step));

                for (int i = c; i >= 0; i--)
                {
                    double f = min + (i * step);

                    if (maxEqualMin)
                    {
                        f = max + (i * step) - step;
                    }

                    if (f < 0 || f < min)
                        break;

                    builder.Append("<option value=\"");
                    builder.Append(f);
                    builder.Append("\"");

                    if (Math.Abs(selectedValue - f) < 0.000000001)
                    {
                        builder.Append(" selected");
                        hasSelected = true;
                    }

                    builder.Append(">");
                    builder.Append(f);
                    builder.Append("</option>");
                }
            }

            if (selectedValue > max || !hasSelected)
            {
                builder.Append("<option value=\"");
                builder.Append(selectedValue);
                builder.Append("\"");
                builder.Append(" selected");
                builder.Append(">");
                builder.Append(selectedValue);
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        /// <summary>
        /// Generate combo box for position taking / commission.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectedValue"></param>
        /// <param name="data">First column/ Second column indicate value/text</param>
        /// <param name="onchange"></param>
        /// <returns></returns>
        public static string ComboBox(string id, object selectedValue, DataTable data, string valueColumnName, string textColumnName, string onchange)
        {
            if (null == data) return null;
            DataRowCollection rows = data.Rows;

            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"");

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            int c = rows.Count;

            for (int i = 0; i < c; i++)
            {
                builder.Append("<option value=\"");
                builder.Append(rows[i][valueColumnName]);
                builder.Append("\"");

                if (rows[i][valueColumnName].Equals(selectedValue))
                {
                    builder.Append(" selected");
                }

                builder.Append(">");
                builder.Append(rows[i][textColumnName]);
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        /// <summary>
        /// Generate combo box for position taking / commission.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectedValue"></param>
        /// <param name="data">First column/ Second column indicate value/text</param>
        /// <param name="onchange"></param>
        /// <returns></returns>
        public static string ComboBox(string id, object selectedValue, DataTable data, string valueColumnName, string textColumnName1, string textColumnName2, string lblResource, string onchange)
        {
            if (null == data) return null;
            DataRowCollection rows = data.Rows;

            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"");

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            int c = rows.Count;

            for (int i = 0; i < c; i++)
            {
                builder.Append("<option value=\"");
                builder.Append(rows[i][valueColumnName]);
                builder.Append("\"");

                if (rows[i][valueColumnName].Equals(selectedValue))
                {
                    builder.Append(" selected");
                }

                builder.Append(">");
                builder.Append(rows[i][textColumnName1] + "(" + rows[i]["betcount"] + " " + lblResource + ")");
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }


        /// <summary>
        /// Generate combo box for position taking / commission.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="selectedValue"></param>
        /// <param name="data">First column/ Second column indicate value/text</param>
        /// <param name="onchange"></param>
        /// <returns></returns>
        public static string ComboBox(string id, string textDefault, string valueDefault, DataTable data, string valueColumnName, string textColumnName1, string textColumnName2, string lblResource, string onchange)
        {
            if (null == data) return null;
            DataRowCollection rows = data.Rows;

            StringBuilder builder = new StringBuilder(200);

            builder.Append("<select id=\"" + id + "\" name=\"" + id + "\"");

            if (null != onchange)
            {
                builder.Append(" onchange=\"");
                builder.Append(onchange);
            }

            builder.Append("\">");

            int c = rows.Count;

            builder.Append("<option value=\"");
            builder.Append(valueDefault);
            builder.Append("\"");
            builder.Append(" selected ");
            builder.Append(">");
            builder.Append(textDefault);
            builder.Append("</option>");

            for (int i = 0; i < c; i++)
            {
                builder.Append("<option value=\"");
                builder.Append(rows[i][valueColumnName]);
                builder.Append("\"");
                builder.Append(">");
                builder.Append(rows[i][textColumnName1] + "(" + rows[i]["betcount"] + " " + lblResource + ")");
                builder.Append("</option>");
            }

            builder.Append("</select>");

            return builder.ToString();
        }

        public static DataTable ModifyDataTable(DataTable data, string textAdd,string valueColumnName, string textColumnName1, string textColumnName2,string lblResource)
        {
            if (null == data) return null;

            DataTable newDT = new DataTable();
            DataColumn col1 = new DataColumn("custid", typeof(int));
            DataColumn col2 = new DataColumn("text", typeof(string));
            newDT.Columns.Add(col1);
            newDT.Columns.Add(col2);
            
            if (!string.IsNullOrEmpty(textAdd))
            {
                DataRow rowAdd = newDT.NewRow();
                rowAdd["custid"] = 0;
                rowAdd["text"] = textAdd;
                newDT.Rows.Add(rowAdd);
            }

            DataRowCollection rows = data.Rows;
            int c = rows.Count;
            for (int i = 0; i < c; i++)
            {
                DataRow newRow = newDT.NewRow();
                newRow["custid"] = rows[i][valueColumnName];
                newRow["text"] = (rows[i][textColumnName1] + "(" + rows[i]["betcount"] + " " + lblResource + ")");
                newDT.Rows.Add(newRow);
            }

            return newDT;
        }

        #endregion
    }
}
