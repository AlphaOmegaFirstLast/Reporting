using System;

namespace Bepos.Reporting.Engine.Models
{
    public class ApiStatus
    {
        public bool Ok => Id == 0;

        public int Id { get; set; }
        public string Info { get; set; } = "Success";
        public string UserInfo { get; set; } = "Success";

        public void SetError(int errorId, string message, Exception e = null)
        {
            Id = errorId;
            Info = message;
            UserInfo = "An error has occured. Error Code: " + errorId;
            if (e != null)
            {
                Info = Info + ":" + e.Message;
            }

            //log Info
        }

    }
}
