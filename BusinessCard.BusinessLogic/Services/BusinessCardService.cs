using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using BusinessCard.Entities.DTO.AboutMe;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IBusinessCardService"/>
    internal class BusinessCardService : IBusinessCardService
    {
        /// <summary>
        /// Репозиторий фактов
        /// </summary>
        private readonly IFactOnBusinessCardRepository _factOnBusinessCardRepository;

        public BusinessCardService(IFactOnBusinessCardRepository factOnBusinessCardRepository) => _factOnBusinessCardRepository = factOnBusinessCardRepository;

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Fact>> GetFactsAsync()
            => (await _factOnBusinessCardRepository.GetAsync())
            .Select(x => new Fact
            {
                Id = x.Id,
                Text = x.Data
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<bool> UpdateFactAsync(Fact fact)
            => (await _factOnBusinessCardRepository.UpdateAsync(new FactOnBusinessCard
            {
                Id = fact.Id,
                Data = fact.Text
            })) == 1;
    }
}