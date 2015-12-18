namespace Sunplus.Agent.Model.DL
{
    using System;
    using System.Collections;
    using System.Text;

    [Obsolete("Please rename to Sunplus.Agent.Model.DataAccessLibrary")]
    public class AgentDataAccessException : System.Exception
    {
        private string message = null;

        public AgentDataAccessException(string stpName, Hashtable parameters)
        {
            StringBuilder error = new StringBuilder(100);

            error.Append("Unknown error when execute ");

            error.Append(stpName);

            if (null == parameters)
            {
                this.message = error.ToString();
                return;
            }

            int c = parameters.Count;

            object[] keys = new object[c];
            parameters.Keys.CopyTo(keys, 0);

            object[] values = new object[c];
            parameters.Values.CopyTo(values, 0);

            for (int i = 0; i < c; i++)
            {
                error.Append(" @" + keys[i] + "=" + values[i]);
            }

            this.message = error.ToString();
        }

        public override string Message
        {
            get
            {
                if (null != this.message)
                {
                    return this.message;
                }

                return base.Message;
            }
        }
    }
}
