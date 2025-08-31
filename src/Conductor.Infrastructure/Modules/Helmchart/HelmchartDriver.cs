namespace Conductor.Infrastructure.Modules.Helmchart;

public interface IHelmchartDriver
{
    Task PlanAsync();
}

public sealed class HelmchartDriver : IHelmchartDriver
{
    public Task PlanAsync()
    {
        throw new NotImplementedException();
    }
}