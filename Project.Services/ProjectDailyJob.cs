using Project.Api;
using Quartz;
using System.Threading.Tasks;

public class ProjectDailyJobOptions
{
    public string Url { get; set; }
}

public class ProjectDailyJob : IJob
{
    public readonly ProjectCommonService _projectCommonService;

    public ProjectDailyJob(ProjectCommonService projectCommonService)
    {
        _projectCommonService = projectCommonService;
    }

    public async Task Execute(IJobExecutionContext context)
    {

    }
}