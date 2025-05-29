// Ignore Spelling: Ack

namespace ServerWatchTower.Agent.Model
{
    using ServerWatchTower.Agent.Model.Properties;
    using System;

    /// <summary>
    /// Represents an alert that is displayed on the alert view.
    /// </summary>
    public class Alert
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? AcknowledgedOn { get; set; }
        public AlertType Type { get; set; }
        public string[] AccessRights { get; set; }

        public string AckStatus
        {
            get
            {
                if (this.AcknowledgedOn.HasValue)
                {
                    return string.Format(Res.LblAcknowledgedByUser, this.AcknowledgedOn.GetValueOrDefault());
                }

                if (this.ExpirationDate.HasValue)
                {
                    return string.Format(Res.LblAlertExpired, this.ExpirationDate.GetValueOrDefault());
                }

                return null;
            }
        }

        public override string ToString()
        {
            string formattedDate = this.Date.ToString("yyyy-MM-dd");
            return $"{formattedDate} / {this.Title} - {this.Message}";
        }
    }
}
