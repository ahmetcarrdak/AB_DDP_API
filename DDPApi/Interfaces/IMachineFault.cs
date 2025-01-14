using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IMachineFault
{
    // Yeni bir arıza kaydı ekler.
    Task<MachineFault> AddFaultAsync(MachineFault fault);

    // Arıza kaydını kimliğine göre günceller.
    Task<MachineFault> UpdateFaultAsync(int faultId, MachineFault updatedFault);

    // Arıza kaydını kimliğine göre siler.
    Task<bool> DeleteFaultAsync(int faultId);

    // Belirli bir arıza kaydını kimliğine göre getirir.
    Task<MachineFault> GetFaultByIdAsync(int faultId);

    // Tüm arıza kayıtlarını getirir.
    Task<IEnumerable<MachineFault>> GetAllFaultsAsync();

    // Çözülmemiş (IsResolved = false) arızaları getirir.
    Task<IEnumerable<MachineFault>> GetUnresolvedFaultsAsync();

    // Çözülmüş (IsResolved = true) arızaları getirir.
    Task<IEnumerable<MachineFault>> GetResolvedFaultsAsync();

    // Makine koduna göre arıza kayıtlarını getirir.
    Task<IEnumerable<MachineFault>> GetFaultsByMachineCodeAsync(string machineCode);
}