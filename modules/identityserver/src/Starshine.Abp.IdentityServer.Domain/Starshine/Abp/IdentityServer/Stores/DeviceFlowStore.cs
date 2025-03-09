using System;
using System.Threading.Tasks;
using IdentityModel;
using Starshine.IdentityServer.Models;
using Starshine.IdentityServer.Stores;
using Starshine.IdentityServer.Stores.Serialization;
using JetBrains.Annotations;
using Volo.Abp.Guids;
using Volo.Abp;
using Starshine.Abp.IdentityServer.Repositories;
using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer.Stores;

/// <summary>
/// �豸���洢
/// </summary>
public class DeviceFlowStore : IDeviceFlowStore
{
    /// <summary>
    /// �豸���洢
    /// </summary>
    protected IDeviceFlowCodesRepository DeviceFlowCodesRepository { get; }
    /// <summary>
    /// GUID������
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }
    /// <summary>
    /// �־û���Ȩ���л���
    /// </summary>
    protected IPersistentGrantSerializer PersistentGrantSerializer { get; }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="deviceFlowCodesRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="persistentGrantSerializer"></param>
    public DeviceFlowStore(
        IDeviceFlowCodesRepository deviceFlowCodesRepository,
        IGuidGenerator guidGenerator,
        IPersistentGrantSerializer persistentGrantSerializer)
    {
        DeviceFlowCodesRepository = deviceFlowCodesRepository;
        GuidGenerator = guidGenerator;
        PersistentGrantSerializer = persistentGrantSerializer;
    }

    /// <summary>
    /// �洢�豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <param name="userCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public virtual async Task StoreDeviceAuthorizationAsync(string deviceCode, string userCode, DeviceCode data)
    {
        Check.NotNull(deviceCode, nameof(deviceCode));
        Check.NotNull(userCode, nameof(userCode));
        Check.NotNull(data, nameof(data));

        await DeviceFlowCodesRepository
            .InsertAsync(
                new DeviceFlowCodes(GuidGenerator.Create())
                {
                    DeviceCode = deviceCode,
                    UserCode = userCode,
                    ClientId = data.ClientId,
                    SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject)?.Value,
                    CreationTime = data.CreationTime.DateTime,
                    Expiration = data.CreationTime.AddSeconds(data.Lifetime),
                    Data = Serialize(data)
                }
            );
    }

    /// <summary>
    /// ͨ���û�������豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public virtual async Task<DeviceCode?> FindByUserCodeAsync(string userCode)
    {
        Check.NotNull(userCode, nameof(userCode));
        var deviceCodes = await DeviceFlowCodesRepository.FindByUserCodeAsync(userCode);
        if (deviceCodes == null)
        {
            return null;
        }
        return DeserializeToDeviceCode(deviceCodes.Data);
    }

    /// <summary>
    /// ͨ���豸������豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <returns></returns>
    public virtual async Task<DeviceCode?> FindByDeviceCodeAsync(string deviceCode)
    {
        Check.NotNull(deviceCode, nameof(deviceCode));
        var deviceCodes = await DeviceFlowCodesRepository.FindByDeviceCodeAsync(deviceCode);
        if (deviceCodes == null)
        {
            return null;
        }
        return DeserializeToDeviceCode(deviceCodes.Data);
    }
    /// <summary>
    ///   �����豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual async Task UpdateByUserCodeAsync(string userCode, DeviceCode data)
    {
        Check.NotNull(userCode, nameof(userCode));
        Check.NotNull(data, nameof(data));
        var deviceCodes = await DeviceFlowCodesRepository.FindByUserCodeAsync(userCode);
        if (deviceCodes == null)
        {
            throw new InvalidOperationException($"Could not update device code by the given userCode: {userCode}");
        }
        deviceCodes.SubjectId = data.Subject?.FindFirst(JwtClaimTypes.Subject)?.Value;
        deviceCodes.Data = Serialize(data);

        await DeviceFlowCodesRepository.UpdateAsync(deviceCodes, autoSave: true);
    }

    /// <summary>
    /// ͨ���豸��ɾ���豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <returns></returns>
    public virtual async Task RemoveByDeviceCodeAsync(string deviceCode)
    {
        Check.NotNull(deviceCode, nameof(deviceCode));
        var deviceCodes = await DeviceFlowCodesRepository.FindByDeviceCodeAsync(deviceCode);
        if (deviceCodes == null)
        {
            return;
        }
        await DeviceFlowCodesRepository.DeleteAsync(deviceCodes, autoSave: true);
    }

    /// <summary>
    /// ���л��豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="deviceCode"></param>
    /// <returns></returns>
    protected virtual string? Serialize(DeviceCode? deviceCode)
    {
        if (deviceCode == null)
        {
            return null;
        }
        return PersistentGrantSerializer.Serialize(deviceCode);
    }

    /// <summary>
    /// �����л��豸��Ȩ��Ϣ
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    protected virtual DeviceCode? DeserializeToDeviceCode(string? data)
    {
        if (data == null)
        {
            return null;
        }

        return PersistentGrantSerializer.Deserialize<DeviceCode>(data);
    }
}
