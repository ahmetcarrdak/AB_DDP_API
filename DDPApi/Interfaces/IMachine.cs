using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces
{
    public interface IMachine
    {
        // Yeni bir makine ekler.
        Task<Machine> AddMachineAsync(Machine machine);

        // Belirtilen ID'ye sahip bir makineyi günceller.
        Task<Machine> UpdateMachineAsync(int machineId, Machine updatedMachine);

        // Belirtilen ID'ye sahip bir makineyi siler.
        Task<bool> DeleteMachineAsync(int machineId);

        // Belirtilen ID'ye sahip bir makineyi getirir.
        Task<Machine> GetMachineByIdAsync(int machineId);

        // Tüm makinelerin listesini getirir.
        Task<IEnumerable<Machine>> GetAllMachinesAsync();

        // Çalışır durumda olan makinelerin listesini getirir.
        Task<IEnumerable<Machine>> GetOperationalMachinesAsync();

        // Çalışmayan makinelerin listesini getirir.
        Task<IEnumerable<Machine>> GetNonOperationalMachinesAsync();

        // Belirtilen konumdaki makinelerin listesini getirir.
        Task<IEnumerable<Machine>> GetMachinesByLocationAsync(string location);
    }
}