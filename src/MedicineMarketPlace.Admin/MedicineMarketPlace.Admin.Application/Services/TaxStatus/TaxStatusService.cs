using AutoMapper;
using MedicineMarketPlace.Admin.Application.Models;
using MedicineMarketPlace.BuildingBlocks.EntityFramework.UnitOfWork;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.Shared.Context;
using MedicineMarketPlace.Shared.Entities;

namespace MedicineMarketPlace.Admin.Application.Services
{
    public class TaxStatusService : ITaxStatusService
    {
        private readonly IMapper _mapper;
        private readonly MedicineMarketDbContext _dbcontext;
        private readonly IUnitOfWork _unitOfWork;

        public TaxStatusService(
            IMapper mapper,
            MedicineMarketDbContext dbcontext)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;
            _unitOfWork = new UnitOfWork<MedicineMarketDbContext>(_dbcontext);
        }

        public async Task<List<TaxStatusDto>> FindAsync()
        {
            var taxStatus = await _unitOfWork.Repository<TaxStatus, int>().FindAsync();
            return _mapper.Map<List<TaxStatusDto>>(taxStatus);
        }

        public async Task<TaxStatusDto> FindByIdAsync(int id)
        {
            var taxStatus = await _unitOfWork.Repository<TaxStatus, int>().FindByIdAsync(id);
            return _mapper.Map<TaxStatusDto>(taxStatus);
        }

        public async Task<TaxStatusDto> CreateAsync(CreateOrUpdateTaxStatusDto dto)
        {
            try
            {
                var taxStatus = _mapper.Map<TaxStatus>(dto);

                await _unitOfWork.Repository<TaxStatus, int>().CreateAsync(taxStatus);
                await _unitOfWork.CommitAsync();

                return _mapper.Map<TaxStatusDto>(taxStatus);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return null;
            }
        }
    }
}
