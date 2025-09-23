namespace Conductor.Core.Provisioning;

public interface IProvisionFactory
{
    Task ProvisionAsync(List<ProvisionInput> provisionInputs, string folderName);
}
