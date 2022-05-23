using BusinessCard.BusinessLogicLayer.DTOs.AboutMeDtos;
using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IAboutMeService" />
    public class AboutMeService : IAboutMeService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IBiographyRepository _biographyRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ISkillRepository _skillRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IExperienceRepository _experienceRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IEducationRepository _educationRepository;

        public AboutMeService(IBiographyRepository biographyRepository, ISkillRepository skillRepository, IExperienceRepository experienceRepository, IEducationRepository educationRepository)
        {
            _biographyRepository = biographyRepository;
            _skillRepository = skillRepository;
            _experienceRepository = experienceRepository;
            _educationRepository = educationRepository;
        }

        public async Task<Dictionary<string, List<object>>> GetInformationAboutMe()
        {
            var aboutMeDictionary = new Dictionary<string, List<object>>();
            aboutMeDictionary.Add("Biography", await GetBiographyAsync());
            aboutMeDictionary.Add("Skills", await GetSkillsAsync());
            aboutMeDictionary.Add("Experience", await GetExperienceAsync());
            aboutMeDictionary.Add("Education", await GetEducationAsync());
            return aboutMeDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        private async Task<List<object>> GetBiographyAsync()
        {
            var data = await _biographyRepository.GetSortedBiographyByPriorityAsync();

            var biographyData = new List<object>();

            foreach (var element in data)
                biographyData.Add(new BiographyDto { Data = element.Data });

            return biographyData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        private async Task<List<object>> GetSkillsAsync()
        {
            var data = await _skillRepository.GetAsync();

            var skills = new List<object>();

            foreach (var element in data)
                skills.Add(new SkillDto
                {
                    Name = element.Name,
                    PercentOfKnowledge = element.PercentOfKnowledge,
                    Description = element.Description
                });

            return skills;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        private async Task<List<object>> GetExperienceAsync()
        {
            var data = await _experienceRepository.GetAsync();

            var experience = new List<object>();

            foreach (var element in data)
                experience.Add(new ExperienceDto
                {
                    Company = element.Company,
                    Position = element.Position,
                    Description = element.Description,
                    StartDate = element.StartDate.ToString("Y"),
                    EndDate = element.EndDate != null ? element.EndDate?.ToString("Y") : "по настоящее время"
                });

            return experience;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>  </returns>
        private async Task<List<object>> GetEducationAsync()
        {
            var data = await _educationRepository.GetAsync();

            var education = new List<object>();

            foreach (var element in data)
                education.Add(new EducationDto
                {
                    Organization = element.Organization,
                    Description = element.Description,
                    StartDate = element.StartDate.ToString("Y"),
                    EndDate = element.EndDate != null ? element.EndDate?.ToString("Y") : "по настоящее время"
                });

            return education;
        }
    }
}