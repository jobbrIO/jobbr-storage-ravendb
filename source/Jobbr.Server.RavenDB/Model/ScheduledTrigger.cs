using System;

namespace Jobbr.Server.RavenDB.Model
{
    public class ScheduledTrigger : JobTriggerBase
    {
        public DateTime StartDateTimeUtc { get; set; }
    }
}