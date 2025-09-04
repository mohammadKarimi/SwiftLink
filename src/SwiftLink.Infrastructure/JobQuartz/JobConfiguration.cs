namespace SwiftLink.Infrastructure.JobQuartz;
public class JobConfiguration
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public TimeSpan Interval { get; set; }
    public TimeSpan StartDelay { get; set; }
}

public class JobConfigurations
{
    public List<JobConfiguration> Configurations { get; set; }
}
