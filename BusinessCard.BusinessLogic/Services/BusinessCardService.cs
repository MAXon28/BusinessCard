using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    public class BusinessCardService : IBusinessCardService
    {
        private readonly IFactOnBusinessCardRepository _factOnBusinessCardRepository;

        public BusinessCardService(IFactOnBusinessCardRepository factOnBusinessCardRepository)
        {
            _factOnBusinessCardRepository = factOnBusinessCardRepository;
        }

        public async Task<List<string>> GetFactsAsync()
        {
            var data = await _factOnBusinessCardRepository.GetAsync();

            data = from element in data
                   orderby element.Priority
                   select element;

            var facts = new List<string>();

            foreach (var element in data)
                facts.Add(element.Data);

            return facts;
        }
    }
}