using Conductor.Domain.Models.Application;
using Conductor.Domain.Models.Deployment;
using Environment = Conductor.Domain.Models.Environment.Environment;

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