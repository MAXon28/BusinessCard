using BusinessCard.DataAccessLayer.Interfaces.Content;
using System.Threading.Tasks;
using BusinessCard.BusinessLogicLayer.Interfaces;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal class RuleService : IRuleService
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