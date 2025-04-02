using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.DTO;
using DDPApi.Models;

namespace DDPApi.Interfaces
{
    public interface IMachine
    {
        // Yeni bir makine ekler.
        Task<Machine> AddMachineAsync(MachineCreateDto machineDto);

        // Belirtilen ID'ye sahip bir makineyi günceller.
        Task<Machine> UpdateMachineAsync(MachineUpdateDto updatedMachineDto);

        // Belirtilen ID'ye sahip bir makineyi siler.
        Task<bool> DeleteMachineAsync(int machineId);

        // Belirtilen ID'ye sahip bir makineyi getirir.
        Task<Machine> GetMachineByIdAsync(int machineId);

        // Tüm makinelerin listesini getirir.
        Task<IEnumerable<Machine>> GetAllMachinesAsync();
    }
}