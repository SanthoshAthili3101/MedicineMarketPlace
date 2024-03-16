using MedicineMarketPlace.Admin.Application.Models;

namespace MedicineMarketPlace.Admin.Application.Services
{
    public interface ITaxStatusService
    {
        Task<List<TaxStatusDto>> FindAsync();

        Task<TaxStatusDto> FindByIdAsync(int id);

        Task<TaxStatusDto> CreateAsync(CreateOrUpdateTaxStatusDto dto);
    }
}
