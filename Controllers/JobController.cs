using Hangfire;
using Hangfire.Storage.Monitoring;
using HangfireMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HangfireMVC.Controllers;

public class JobController : Controller
{
    private readonly IJobTestService _jobTestService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;

    private readonly JobStorage _storage;
    //  private readonly IMonitoringApi _storage;

    public JobController(IJobTestService jobTestService,
        IBackgroundJobClient backgroundJobClient,
        IRecurringJobManager recurringJobManager, JobStorage storage
        /*IMonitoringApi storage*/)
    {
        _jobTestService = jobTestService;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
        _storage = storage;
    }
    [HttpGet("/FireAndForgetJob")]
    public ActionResult CreateFireAndForgetJob()
    {
        _backgroundJobClient.Enqueue( () => _jobTestService.FireAndForgetJob());
        return RedirectToAction("Index","Home");
    }
    
    [HttpGet("/DelayedJob")]
    public ActionResult CreateDelayedJob()
    {
        _backgroundJobClient.Schedule(() => _jobTestService.DelayedJob(), TimeSpan.FromSeconds(60));
        return RedirectToAction("Index","Home");

    }
    [HttpGet("/ReccuringJob")]
    public ActionResult CreateReccuringJob()
    {
        RecurringJob.RemoveIfExists("jobId");
        RecurringJob.AddOrUpdate("powerfuljob", () => _jobTestService.ReccuringJob(), "*/5 * * * *");
        return RedirectToAction("Index","Home");
    }

    [HttpGet("/ContinuationJob")]
    public ActionResult CreateContinuationJob()
    {
        var parentJobId = _backgroundJobClient.Enqueue(() => _jobTestService.FireAndForgetJob());
        _backgroundJobClient.ContinueJobWith(parentJobId, () => _jobTestService.ContinuationJob());
        return RedirectToAction("Index","Home");

    }

    [HttpGet("/GetStatistic")]
    public StatisticsDto GetJobs()
    {
        var test = _storage.GetMonitoringApi().GetStatistics();
        return test;
    }
}