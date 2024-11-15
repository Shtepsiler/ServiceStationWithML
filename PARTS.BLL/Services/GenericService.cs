using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.BLL.Services.Interaces;
using PARTS.DAL.Entities;
using PARTS.DAL.Interfaces;

namespace PARTS.BLL.Services
{

    public class GenericService<TEntity, TRequest, TResponse> : IGenericService<TEntity, TRequest, TResponse>
            where TEntity : Base
            where TRequest : class
            where TResponse : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;
        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<IEnumerable<TResponse?>?> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAsync();
                return _mapper.Map<IEnumerable<TEntity?>?, IEnumerable<TResponse?>?>(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<TResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                return _mapper.Map<TEntity, TResponse>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<TResponse?> PostAsync(TRequest request)
        {
            try
            {
                var entity = _mapper.Map<TRequest, TEntity>(request);
                await _repository.InsertAsync(entity);
                return _mapper.Map<TEntity, TResponse>(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task<TResponse?> UpdateAsync(TRequest request)
        {
            try
            {
                var entity = _mapper.Map<TRequest, TEntity>(request);
                await _repository.UpdateAsync(entity);
                return _mapper.Map<TEntity, TResponse>(entity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual async Task DeleteByIdAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
