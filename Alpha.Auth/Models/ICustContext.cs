namespace Alpha.Auth.Models
{
    using System;
    using Sunplus.Agent.Model.Entities;

    public interface ICustContext
    {
        bool IsActive { get; set; }

        DateTime StatusCachedExpiredTime { get; }

        string SubAccPermission { get; set; }

        IUserSession UserSession { get; }

        DateTime SubAccPermissionExpiredTime { get; }
    }
}