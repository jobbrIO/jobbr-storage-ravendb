using System;

namespace Jobbr.Server.RavenDB.Model
{
    public class InstantTrigger : JobTriggerBase
    {
        public int DelayedMinutes { get; set; }
    }
}