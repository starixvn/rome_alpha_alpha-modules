namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.Entities.Base;

    // SP return subaccid, username, firstname, lastname, createdate, isnull(closed, 0) closed, PageAccess
     [Obsolete("Should rename to Sunplus.Agent.Model.Base")]
    public class SubAccount : IEntity
    {
        public SubAccount(DataRow dataRow)
            : this(dataRow, true)
        {
        }

        public SubAccount(DataRow dataRow, bool isGetCreatedDate)
        {
            this.Id = (int)dataRow["subaccid"];
            this.UserName = dataRow["username"] as string;
            this.FirstName = dataRow["firstname"] as string;
            this.LastName = dataRow["lastname"] as string;
            if (isGetCreatedDate)
            {
                this.CreateDate = (DateTime)dataRow["createdate"];
            }

            this.IsClosed = (bool)dataRow["closed"];
            this.PageAccess = dataRow["PageAccess"] as string;
        }

        public SubAccount(IDataReader reader)
            : this(reader, true)
        {
        }

        public SubAccount(IDataReader reader, bool isGetCreatedDate)
        {
            this.Id = (int)reader["subaccid"];
            this.UserName = reader["username"] as string;
            this.FirstName = reader["firstname"] as string;
            this.LastName = reader["lastname"] as string;
            if (isGetCreatedDate)
            {
                this.CreateDate = (DateTime)reader["createdate"];
            }

            this.IsClosed = (bool)reader["closed"];
            this.PageAccess = reader["PageAccess"] as string;
        }

        public SubAccount()
        {
            this.PageAccess = new PageAccess();
        }

        public int Id { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsClosed { get; set; }

        public PageAccess PageAccess { get; set; }

        public static implicit operator SubAccount(DataRow dataRow)
        {
            return new SubAccount(dataRow);
        }                
    }
}