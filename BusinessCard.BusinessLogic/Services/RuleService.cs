using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleService : IRuleService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IRuleRepository _ruleRepository;

        public RuleService(IRuleRepository ruleRepository)
        {
            _ruleRepository = ruleRepository;
        }

        public async Task<string> GetServiceRuleAsync(int serviceId)
        {
            throw new System.NotImplementedException();
        }
    }
}