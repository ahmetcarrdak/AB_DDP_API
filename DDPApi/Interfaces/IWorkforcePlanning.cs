using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IWorkforcePlanning
{
    Task<WorkforcePlanning>
        AddWorkforcePlanningAsync(WorkforcePlanning workforcePlanning); // Yeni iş gücü planlaması ekleme

    Task<WorkforcePlanning>
        UpdateWorkforcePlanningAsync(int id, WorkforcePlanning workforcePlanning); // İş gücü planlamasını güncelleme

    Task<bool> DeleteWorkforcePlanningAsync(int id); // İş gücü planlamasını silme
    Task<WorkforcePlanning> GetWorkforcePlanningByIdAsync(int id); // İş gücü planlamasını ID ile sorgulama
    Task<IEnumerable<WorkforcePlanning>> GetAllWorkforcePlanningsAsync(); // Tüm iş gücü planlamalarını listeleme
}