namespace Alpha.Auth.Models
{
    using System;
    using Sunplus.Agent.Model.Entities;

    public interface ICustContext
    {
        bool IsStatusActive { get; set; }

        DateTime StatusExpiredTime { get; }

        string Permission { get; set; }

        IUserSession UserSession { get; }

        DateTime PermissionExpiredTime { get; }
    }
}