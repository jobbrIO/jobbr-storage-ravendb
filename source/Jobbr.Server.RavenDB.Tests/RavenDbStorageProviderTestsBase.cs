using System;
using System.Globalization;
using Jobbr.ComponentModel.JobStorage.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jobbr.Server.RavenDB.Tests
{
    [TestClass]
    public class RavenDbStorageProviderTestsBase : RavenDbIntegrationTestBase
    {
        [TestMethod]
        public void GivenEmptyDatabase_WhenAddingJob_IdIsSet()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job = new Job
            {
                UniqueName = "testjob",
                Type = "Jobs.Test"
            };

            StorageProvider.AddJob(job);

            Assert.AreNotEqual(0, job.Id);
        }

        [TestMethod]
        public void GivenJob_WhenQueryingById_IsReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job = new Job
            {
                UniqueName = "testjob",
                Type = "Jobs.Test"
            };

            StorageProvider.AddJob(job);

            var job2 = StorageProvider.GetJobById(job.Id);

            Assert.AreEqual(job.Id, job2.Id);
            Assert.AreEqual("testjob", job2.UniqueName);
            Assert.AreEqual("Jobs.Test", job2.Type);
        }

        [TestMethod]
        public void GivenJob_WhenQueryingByUniqueName_IsReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job = new Job
            {
                UniqueName = "testjob",
                Type = "Jobs.Test"
            };

            StorageProvider.AddJob(job);

            WaitForIndexing(Store);

            var job2 = StorageProvider.GetJobByUniqueName(job.UniqueName);

            Assert.AreEqual(job.Id, job2.Id);
            Assert.AreEqual("testjob", job2.UniqueName);
            Assert.AreEqual("Jobs.Test", job2.Type);
        }

        [TestMethod]
        public void GivenTwoJobs_WhenQueryingPaged_ResultIsPaged()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            var job2 = new Job { UniqueName = "testjob2", Type = "Jobs.Test2" };
            var job3 = new Job { UniqueName = "testjob3", Type = "Jobs.Test3" };

            StorageProvider.AddJob(job1);
            StorageProvider.AddJob(job2);
            StorageProvider.AddJob(job3);
            
            WaitForIndexing(Store);

            var jobs = StorageProvider.GetJobs(0, 1);

            Assert.AreEqual(1, jobs.Count);
            Assert.AreEqual(job1.Id, jobs[0].Id);
        }

        [TestMethod]
        public void GivenTwoJobs_WhenQueryingPageTwo_PageTwoIsReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            var job2 = new Job { UniqueName = "testjob2", Type = "Jobs.Test2" };
            var job3 = new Job { UniqueName = "testjob3", Type = "Jobs.Test3" };

            StorageProvider.AddJob(job1);
            StorageProvider.AddJob(job2);
            StorageProvider.AddJob(job3);

            WaitForIndexing(Store);

            var jobs = StorageProvider.GetJobs(1, 1);

            Assert.AreEqual(1, jobs.Count);
            Assert.AreEqual(job2.Id, jobs[0].Id);
        }

        [TestMethod]
        public void GivenSomeTriggers_WhenQueryingForActiveTriggers_AllActiveTriggersAreReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            var job2 = new Job { UniqueName = "testjob2", Type = "Jobs.Test2" };

            StorageProvider.AddJob(job1);
            StorageProvider.AddJob(job2);

            var trigger1 = new InstantTrigger { IsActive = false };
            var trigger2 = new InstantTrigger { IsActive = true };
            var trigger3 = new InstantTrigger { IsActive = true };

            StorageProvider.AddTrigger(job1.Id, trigger1);
            StorageProvider.AddTrigger(job1.Id, trigger2);
            StorageProvider.AddTrigger(job2.Id, trigger3);

            WaitForIndexing(Store);

            var activeTriggers = StorageProvider.GetActiveTriggers();

            Assert.AreEqual(2, activeTriggers.Count);
        }

        [TestMethod]
        public void GivenSomeTriggers_WhenQueryingTriggersByJobId_OnlyTriggersOfThatJobAreReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            var job2 = new Job { UniqueName = "testjob2", Type = "Jobs.Test2" };

            StorageProvider.AddJob(job1);
            StorageProvider.AddJob(job2);

            var trigger1 = new InstantTrigger();
            var trigger2 = new InstantTrigger();
            var trigger3 = new InstantTrigger();

            StorageProvider.AddTrigger(job1.Id, trigger1);
            StorageProvider.AddTrigger(job1.Id, trigger2);
            StorageProvider.AddTrigger(job2.Id, trigger3);

            WaitForIndexing(Store);

            var triggersOfJob1 = StorageProvider.GetTriggersByJobId(job1.Id);
            var triggersOfJob2 = StorageProvider.GetTriggersByJobId(job2.Id);

            Assert.AreEqual(2, triggersOfJob1.Count);
            Assert.AreEqual(1, triggersOfJob2.Count);
        }

        [TestMethod]
        public void GivenJobRun_WhenQueryingById_IsReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow };
            StorageProvider.AddJobRun(jobRun1);

            WaitForIndexing(Store);

            var jobRun2 = StorageProvider.GetJobRunById(jobRun1.Id);

            Assert.AreEqual(jobRun1.Id, jobRun2.Id);
        }

        [TestMethod]
        public void GivenTwoJobRuns_WhenQueryingPaged_ResultIsPaged()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRuns(0, 1);

            Assert.AreEqual(1, jobRuns.Count);

            jobRuns = StorageProvider.GetJobRuns(0, 2);

            WaitForUserToContinueTheTest();

            Assert.AreEqual(2, jobRuns.Count);
        }

        [TestMethod]
        public void GivenTwoJobRuns_WhenQueryingForSpecificState_OnlyThoseJobRunsAreReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Failed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByState(JobRunStates.Failed);

            Assert.AreEqual(1, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRuns_WhenQueryingForSpecificStatePaged_ResultIsPaged()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Failed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByState(JobRunStates.Completed, 0, 1);

            Assert.AreEqual(1, jobRuns.Count);

            jobRuns = StorageProvider.GetJobRunsByState(JobRunStates.Completed, 0, 2);

            Assert.AreEqual(2, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRuns_WhenQueryingByTrigger_AllJobRunsOfTriggerAreReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Failed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            WaitForUserToContinueTheTest();

            var jobRuns = StorageProvider.GetJobRunsByTriggerId(job1.Id, trigger1.Id);

            Assert.AreEqual(3, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRuns_WhenQueryingByTriggerPaged_ResultIsPaged()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };

            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Failed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByTriggerId(job1.Id, trigger1.Id, 0, 2);

            Assert.AreEqual(2, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRunsOfChefkoch_WhenQueryingByUserDisplayName_ReturnsOnlyJobRunsOfThatUser()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true, UserDisplayName = "chefkoch" };

            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByUserDisplayName("chefkoch");

            Assert.AreEqual(3, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRunsOfChefkoch_WhenQueryingByUserDisplayNamePaged_ResultIsPaged()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true, UserDisplayName = "chefkoch" };

            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByUserDisplayName("chefkoch", 0, 2);

            Assert.AreEqual(2, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRunsOfozu_WhenQueryingByUserId_ReturnsOnlyJobRunsOfozu()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };

            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true, UserId = "ozu" };

            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByUserId("ozu");

            Assert.AreEqual(3, jobRuns.Count);
        }

        [TestMethod]
        public void GivenThreeJobRunsOfozu_WhenQueryingByUserIdPaged_ResultIsPaged()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true, UserId = "ozu" };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = DateTime.UtcNow, State = JobRunStates.Completed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var jobRuns = StorageProvider.GetJobRunsByUserId("ozu", 0, 2);

            Assert.AreEqual(2, jobRuns.Count);
        }

        [TestMethod]
        public void GivenEnabledTrigger_WhenDisabling_TriggerIsDisabled()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };

            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };

            StorageProvider.AddTrigger(job1.Id, trigger1);

            StorageProvider.DisableTrigger(job1.Id, trigger1.Id);

            var triggerFromDb = StorageProvider.GetTriggerById(job1.Id, trigger1.Id);

            Assert.IsFalse(triggerFromDb.IsActive);
        }

        [TestMethod]
        public void GivenDisabledTrigger_WhenEnabling_TriggerIsEnabled()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };

            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = false };

            StorageProvider.AddTrigger(job1.Id, trigger1);
            StorageProvider.EnableTrigger(job1.Id, trigger1.Id);

            var triggerFromDb = StorageProvider.GetTriggerById(job1.Id, trigger1.Id);

            Assert.IsTrue(triggerFromDb.IsActive);
        }

        [TestMethod]
        public void GivenJobRuns_WhenQueryingForLastJobRunByTrigger_LastJobRunIsReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var now = DateTime.UtcNow;

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now, ActualStartDateTimeUtc = now, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now.AddMinutes(1), ActualStartDateTimeUtc = now.AddMinutes(1), State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now.AddMinutes(2), ActualStartDateTimeUtc = now.AddMinutes(2), State = JobRunStates.Completed };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var lastJobRun = StorageProvider.GetLastJobRunByTriggerId(job1.Id, trigger1.Id, now.AddSeconds(30));

            Assert.AreEqual(jobRun1.Id, lastJobRun.Id);
        }

        [TestMethod]
        public void GivenJobRuns_WhenQueryingForNextJobRunByTrigger_NextJobRunIsReturned()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var now = DateTime.UtcNow;

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now, ActualStartDateTimeUtc = now, State = JobRunStates.Completed };
            var jobRun2 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now.AddMinutes(1), ActualStartDateTimeUtc = now.AddMinutes(1), State = JobRunStates.Completed };
            var jobRun3 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now.AddMinutes(2), State = JobRunStates.Scheduled };

            StorageProvider.AddJobRun(jobRun1);
            StorageProvider.AddJobRun(jobRun2);
            StorageProvider.AddJobRun(jobRun3);

            WaitForIndexing(Store);

            var lastJobRun = StorageProvider.GetNextJobRunByTriggerId(job1.Id, trigger1.Id, now.AddMinutes(1));
            Assert.AreEqual(jobRun3.Id, lastJobRun.Id);
        }

        [TestMethod]
        public void GivenJobRun_WhenUpdatingProgress_ProgressIsUpdated()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger1 = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger1);

            var now = DateTime.UtcNow;

            var jobRun1 = new JobRun { JobId = job1.Id, TriggerId = trigger1.Id, PlannedStartDateTimeUtc = now, ActualStartDateTimeUtc = now, State = JobRunStates.Completed };
            StorageProvider.AddJobRun(jobRun1);

            StorageProvider.UpdateProgress(jobRun1.Id, 50);

            var jobRun2 = StorageProvider.GetJobRunById(jobRun1.Id);

            Assert.AreEqual(50, jobRun2.Progress);
        }

        [TestMethod]
        public void GivenJob_WhenUpdating_JobIsUpdated()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };

            StorageProvider.AddJob(job1);

            job1.UniqueName = "test-uniquename";
            job1.Title = "test-title";
            job1.Type = "test-type";
            job1.Parameters = "test-parameters";

            StorageProvider.Update(job1);

            var job1Reloaded = StorageProvider.GetJobById(job1.Id);

            Assert.AreEqual("test-uniquename", job1Reloaded.UniqueName);
            Assert.AreEqual("test-title", job1Reloaded.Title);
            Assert.AreEqual("test-type", job1Reloaded.Type);
            Assert.AreEqual("test-parameters", job1Reloaded.Parameters);
        }

        [TestMethod]
        public void GivenJobRun_WhenUpdating_JobRunIsUpdated()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger = new InstantTrigger { IsActive = true };
            StorageProvider.AddTrigger(job1.Id, trigger);

            var jobRun = new JobRun { JobId = job1.Id, TriggerId = trigger.Id, PlannedStartDateTimeUtc = DateTime.UtcNow };
            StorageProvider.AddJobRun(jobRun);

            var newPlannedStartDate = DateTime.UtcNow;
            var newActualStartDate = newPlannedStartDate.AddSeconds(1);
            var newEstimatedStartDate = newPlannedStartDate.AddMilliseconds(1);
            var newActualEndDate = newPlannedStartDate.AddMinutes(1);

            jobRun.JobParameters = "test-jobparameters";
            jobRun.InstanceParameters = "test-instanceparameters";
            jobRun.PlannedStartDateTimeUtc = newPlannedStartDate;
            jobRun.ActualStartDateTimeUtc = newActualStartDate;
            jobRun.EstimatedEndDateTimeUtc = newEstimatedStartDate;
            jobRun.ActualEndDateTimeUtc = newActualEndDate;

            StorageProvider.Update(jobRun);

            var job1Reloaded = StorageProvider.GetJobRunById(jobRun.Id);

            Assert.AreEqual("test-jobparameters", job1Reloaded.JobParameters);
            Assert.AreEqual("test-instanceparameters", job1Reloaded.InstanceParameters);
            Assert.AreEqual(newPlannedStartDate.ToString(CultureInfo.InvariantCulture), job1Reloaded.PlannedStartDateTimeUtc.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(newActualStartDate.ToString(CultureInfo.InvariantCulture), job1Reloaded.ActualStartDateTimeUtc.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(newEstimatedStartDate.ToString(CultureInfo.InvariantCulture), job1Reloaded.EstimatedEndDateTimeUtc.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(newActualEndDate.ToString(CultureInfo.InvariantCulture), job1Reloaded.ActualEndDateTimeUtc.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void GivenInstantTrigger_WhenUpdating_TriggerIsUpdated()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger = new InstantTrigger();
            StorageProvider.AddTrigger(job1.Id, trigger);

            var trigger2 = (InstantTrigger)StorageProvider.GetTriggerById(job1.Id, trigger.Id);
            trigger2.Comment = "bla";
            trigger2.IsActive = true;
            trigger2.Parameters = "test-parameters";
            trigger2.UserId = "ozu";
            trigger2.DelayedMinutes = 5;

            StorageProvider.Update(job1.Id, trigger2);

            var trigger2Reloaded = (InstantTrigger)StorageProvider.GetTriggerById(job1.Id, trigger2.Id);

            Assert.AreEqual("bla", trigger2Reloaded.Comment);
            Assert.IsTrue(trigger2Reloaded.IsActive);
            Assert.AreEqual("test-parameters", trigger2Reloaded.Parameters);
            Assert.AreEqual("ozu", trigger2Reloaded.UserId);
            Assert.AreEqual(5, trigger2Reloaded.DelayedMinutes);
        }

        [TestMethod]
        public void GivenScheduledTrigger_WhenUpdating_TriggerIsUpdated()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger = new ScheduledTrigger { StartDateTimeUtc = DateTime.UtcNow };
            StorageProvider.AddTrigger(job1.Id, trigger);

            var trigger2 = (ScheduledTrigger)StorageProvider.GetTriggerById(job1.Id, trigger.Id);

            var startDateTime = DateTime.UtcNow.AddHours(5);

            trigger2.Comment = "bla";
            trigger2.IsActive = true;
            trigger2.Parameters = "test-parameters";
            trigger2.UserId = "ozu";
            trigger2.StartDateTimeUtc = startDateTime;

            StorageProvider.Update(job1.Id, trigger2);

            var trigger2Reloaded = (ScheduledTrigger)StorageProvider.GetTriggerById(job1.Id, trigger2.Id);

            Assert.AreEqual("bla", trigger2Reloaded.Comment);
            Assert.IsTrue(trigger2Reloaded.IsActive);
            Assert.AreEqual("test-parameters", trigger2Reloaded.Parameters);
            Assert.AreEqual("ozu", trigger2Reloaded.UserId);
            Assert.AreEqual(startDateTime.ToString(CultureInfo.InvariantCulture), trigger2Reloaded.StartDateTimeUtc.ToString(CultureInfo.InvariantCulture));
        }

        [TestMethod]
        public void GivenRecurringTrigger_WhenUpdating_TriggerIsUpdated()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job1 = new Job { UniqueName = "testjob1", Type = "Jobs.Test1" };
            StorageProvider.AddJob(job1);

            var trigger = new RecurringTrigger();

            StorageProvider.AddTrigger(job1.Id, trigger);

            var trigger2 = (RecurringTrigger)StorageProvider.GetTriggerById(job1.Id, trigger.Id);

            var startDateTime = DateTime.UtcNow.AddHours(5);
            var endDateTime = DateTime.UtcNow.AddHours(7);

            trigger2.Comment = "bla";
            trigger2.IsActive = true;
            trigger2.Parameters = "test-parameters";
            trigger2.UserId = "ozu";
            trigger2.StartDateTimeUtc = startDateTime;
            trigger2.Definition = "* * * * *";
            trigger2.EndDateTimeUtc = endDateTime;
            trigger2.NoParallelExecution = true;

            StorageProvider.Update(job1.Id, trigger2);

            var trigger2Reloaded = (RecurringTrigger)StorageProvider.GetTriggerById(job1.Id, trigger2.Id);

            Assert.AreEqual("bla", trigger2Reloaded.Comment);
            Assert.IsTrue(trigger2Reloaded.IsActive);
            Assert.AreEqual("test-parameters", trigger2Reloaded.Parameters);
            Assert.AreEqual("ozu", trigger2Reloaded.UserId);
            Assert.AreEqual(startDateTime.ToString(CultureInfo.InvariantCulture), trigger2Reloaded.StartDateTimeUtc.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(endDateTime.ToString(CultureInfo.InvariantCulture), trigger2Reloaded.EndDateTimeUtc.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual("* * * * *", trigger2Reloaded.Definition);
            Assert.IsTrue(trigger2Reloaded.NoParallelExecution);
        }

        [TestMethod]
        public void GivenRunningDatabase_WhenCheckingAvailability_IsAvailable()
        {
            GivenRavenDb();
            GivenStorageProvider();

            Assert.IsTrue(StorageProvider.IsAvailable());
        }

        [TestMethod]
        public void GivenNonRunningDatabase_WhenCheckingAvailability_IsAvailable()
        { 
            GivenRavenDb();
            GivenStorageProvider();

            Store.Dispose();

            Assert.IsFalse(StorageProvider.IsAvailable());
        }

        [TestMethod]
        public void GivenEmptyDatabase_WhenAddingJob_JobCountIsIncreased()
        {
            GivenRavenDb();
            GivenStorageProvider();

            var job = new Job
            {
                UniqueName = "testjob",
                Type = "Jobs.Test"
            };

            StorageProvider.AddJob(job);

            WaitForIndexing(Store);

            var jobCount = StorageProvider.GetJobsCount();

            Assert.AreEqual(1, jobCount);
        }
    }
}
