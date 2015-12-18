namespace Fanex.Razorbill.Security.Entities
{
    using System;

    /// <summary>
    /// Result Entity.
    /// </summary>
    [Serializable]
    public class Result
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}