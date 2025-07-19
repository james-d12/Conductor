using Conductor.Domain.Models;
using Environment = Conductor.Domain.Models.Environment;

namespace Conductor.Domain.Services;

public interface IDeploymentManager
{
    Deployment DeployAppToEnvironment(Application application, Environment environment);
    void UndeployAppFromEnvironment(Deployment deployment, Application application, Environment environment);
}

public sealed class DeploymentManager : IDeploymentManager
{
    public Deployment DeployAppToEnvironment(Application application, Environment environment)
    {
        var deployment = Deployment.Create(application.Id, environment.Id);
        application.Deploy(deployment);
        environment.Deploy(deployment);
        return deployment;
    }

    public void UndeployAppFromEnvironment(Deployment deployment, Application application,
        Environment environment)
    {
        application.Undeploy(deployment);
        environment.Undeploy(deployment);
    }
}