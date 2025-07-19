using Conductor.Domain.Models;
using Models_Environment = Conductor.Domain.Models.Environment;

namespace Conductor.Domain.Services;

public interface IDeploymentManager
{
    Deployment DeployAppToEnvironment(Application application, Models_Environment environment);
    void UndeployAppFromEnvironment(Deployment deployment, Application application, Models_Environment environment);
}

public sealed class DeploymentManager : IDeploymentManager
{
    public Deployment DeployAppToEnvironment(Application application, Models_Environment environment)
    {
        var deployment = new Deployment
        {
            Id = Guid.NewGuid(),
            ApplicationId = application.Id,
            EnvironmentId = environment.Id
        };

        application.Deployments.Add(deployment);
        environment.Deployments.Add(deployment);

        return deployment;
    }

    public void UndeployAppFromEnvironment(Deployment deployment, Application application, Models_Environment environment)
    {
        application.Deployments.Remove(deployment);
        environment.Deployments.Remove(deployment);
    }
}