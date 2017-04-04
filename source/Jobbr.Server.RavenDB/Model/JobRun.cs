using System;

namespace Jobbr.Server.RavenDB.Model
{
    public class JobRun
    {
        public static string CollectionPrefix = "jobruns";

        public string Id { get; set; }
        public string JobId { get; set; }
        public long TriggerId { get; set; }
        public JobRunStates State { get; set; }
        public double? Progress { get; set; }
        public DateTime PlannedStartDateTimeUtc { get; set; }
        public DateTime? ActualStartDateTimeUtc { get; set; }
        public DateTime? ActualEndDateTimeUtc { get; set; }
        public DateTime? EstimatedEndDateTimeUtc { get; set; }
        public string JobParameters { get; set; }
        public string InstanceParameters { get; set; }
        public int? Pid { get; set; }
        
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}