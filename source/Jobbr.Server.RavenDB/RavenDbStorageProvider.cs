using System;
using System.Collections.Generic;
using Jobbr.ComponentModel.JobStorage;
using Jobbr.ComponentModel.JobStorage.Model;

namespace Jobbr.Server.RavenDB
{
    public class RavenDbStorageProvider : IJobStorageProvider
    {
        public List<Job> GetJobs()
        {
            throw new NotImplementedException();
        }

        public long AddJob(Job job)
        {
            throw new NotImplementedException();
        }

        public List<JobTriggerBase> GetTriggersByJobId(long jobId)
        {
            throw new NotImplementedException();
        }

        public long AddTrigger(RecurringTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public long AddTrigger(InstantTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public long AddTrigger(ScheduledTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public bool DisableTrigger(long triggerId)
        {
            throw new NotImplementedException();
        }

        public bool EnableTrigger(long triggerId)
        {
            throw new NotImplementedException();
        }

        public List<JobTriggerBase> GetActiveTriggers()
        {
            throw new NotImplementedException();
        }

        public JobTriggerBase GetTriggerById(long triggerId)
        {
            throw new NotImplementedException();
        }

        public JobRun GetLastJobRunByTriggerId(long triggerId)
        {
            throw new NotImplementedException();
        }

        public JobRun GetFutureJobRunsByTriggerId(long triggerId)
        {
            throw new NotImplementedException();
        }

        public int AddJobRun(JobRun jobRun)
        {
            throw new NotImplementedException();
        }

        public List<JobRun> GetJobRuns()
        {
            throw new NotImplementedException();
        }

        public bool UpdateProgress(long jobRunId, double? progress)
        {
            throw new NotImplementedException();
        }

        public bool Update(JobRun jobRun)
        {
            throw new NotImplementedException();
        }

        public Job GetJobById(long id)
        {
            throw new NotImplementedException();
        }

        public Job GetJobByUniqueName(string identifier)
        {
            throw new NotImplementedException();
        }

        public JobRun GetJobRunById(long id)
        {
            throw new NotImplementedException();
        }

        public List<JobRun> GetJobRunsForUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public List<JobRun> GetJobRunsForUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public bool Update(Job job)
        {
            throw new NotImplementedException();
        }

        public bool Update(InstantTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public bool Update(ScheduledTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public bool Update(RecurringTrigger trigger)
        {
            throw new NotImplementedException();
        }

        public List<JobRun> GetJobRunsByTriggerId(long triggerId)
        {
            throw new NotImplementedException();
        }

        public List<JobRun> GetJobRunsByState(JobRunStates state)
        {
            throw new NotImplementedException();
        }

        public bool CheckParallelExecution(long triggerId)
        {
            throw new NotImplementedException();
        }
    }
}
