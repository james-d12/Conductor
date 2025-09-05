using Conductor.Core.Modules.Application.Domain;
using Conductor.Core.Modules.Deployment.Domain;
using Domain_Environment = Conductor.Core.Modules.Environment.Domain.Environment;

namespace Conductor.Core.Common.Services;

public interface IDeploymentManager
{
    Deployment DeployAppToEnvironment(Application application, Domain_Environment environment, Commit commit);
    void UndeployAppFromEnvironment(Deployment deployment, Application application, Domain_Environment environment);
}

public sealed class DeploymentManager : IDeploymentManager
{
    public Deployment DeployAppToEnvironment(Application application, Domain_Environment environment, Commit commit)
    {
        var deployment = Deployment.Create(application.Id, environment.Id, commit.Id);

        application.Deploy(deployment);
        environment.Deploy(deployment);
        return deployment;
    }

    public void UndeployAppFromEnvironment(Deployment deployment, Application application,
        Domain_Environment environment)
    {
        application.Undeploy(deployment);
        environment.Undeploy(deployment);
    }
}