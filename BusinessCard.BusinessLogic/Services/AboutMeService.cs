using BusinessCard.BusinessLogicLayer.Interfaces;
using BusinessCard.BusinessLogicLayer.Utils.Extensions;
using BusinessCard.DataAccessLayer.Entities.Content;
using BusinessCard.DataAccessLayer.Interfaces.Content;
using BusinessCard.Entities.DTO.AboutMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessCard.BusinessLogicLayer.Services
{
    /// <inheritdoc cref="IAboutMeService" />
    internal class AboutMeService : IAboutMeService
    {
        /// <summary>
        /// Репозиторий биографии
        /// </summary>
        private readonly IBiographyRepository _biographyRepository;

        /// <summary>
        /// Репозиторий навыков
        /// </summary>
        private readonly ISkillRepository _skillRepository;

        /// <summary>
        /// Репозиторий опыта
        /// </summary>
        private readonly IExperienceRepository _experienceRepository;

        /// <summary>
        /// Репозиторий образования
        /// </summary>
        private readonly IEducationRepository _educationRepository;

        public AboutMeService(IBiographyRepository biographyRepository, ISkillRepository skillRepository, IExperienceRepository experienceRepository, IEducationRepository educationRepository)
        {
            _biographyRepository = biographyRepository;
            _skillRepository = skillRepository;
            _experienceRepository = experienceRepository;
            _educationRepository = educationRepository;
        }

        /// <inheritdoc/>
        public async Task<AboutMeInfo> GetInformationAboutMe()
            => new()
            {
                Biography = await GetBiographyAsync(),
                Skills = await GetSkillsAsync(),
                Experience = await GetExperienceAsync(),
                Education = await GetEducationAsync()
            };

        /// <inheritdoc/>
        public async Task<string> GetBiographyAsync()
            => (await _biographyRepository.GetAsync()).First().Text;

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<SkillDto>> GetSkillsAsync()
            => (await _skillRepository.GetAsync())
            .Select(x => new SkillDto
            {
                Id = x.Id,
                Name = x.Name,
                PercentOfKnowledge = x.PercentOfKnowledge,
                Description = x.Description
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<ExperienceDto>> GetExperienceAsync()
            => (await _experienceRepository.GetAsync())
            .Select(x => new ExperienceDto
            {
                Id = x.Id,
                Company = x.Company,
                Position = x.Position,
                Description = x.Description,
                FromDate = x.StartDate.ConvertToReadableFormat(),
                ToDate = x.EndDate is not null ? x.EndDate?.ConvertToReadableFormat() : "по настоящее время"
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<EducationDto>> GetEducationAsync()
            => (await _educationRepository.GetAsync())
            .OrderBy(x => x.StartDate)
            .Select(x => new EducationDto
            {
                Id = x.Id,
                Organization = x.Organization,
                Description = x.Description,
                FromDate = x.StartDate.ConvertToReadableFormat(),
                ToDate = x.EndDate is not null ? x.EndDate?.ConvertToReadableFormat() : "по настоящее время"
            })
            .ToArray();

        /// <inheritdoc/>
        public async Task<bool> UpdateBiographyAsync(string biographyText)
            => (await _biographyRepository.UpdateBiographyAsync(biographyText)) > 0;

        /// <inheritdoc/>
        public async Task<bool> AddSkillAsync(SkillDto skill)
            => (await _skillRepository.AddAsync(new Skill
            {
                Name = skill.Name,
                Description = skill.Description,
                PercentOfKnowledge = skill.PercentOfKnowledge
            })) == 1;

        /// <inheritdoc/>
        public async Task<bool> UpdateSkillAsync(SkillDto skill)
            => (await _skillRepository.UpdateAsync(new Skill
            {
                Id = skill.Id,
                Name = skill.Name,
                Description = skill.Description,
                PercentOfKnowledge = skill.PercentOfKnowledge
            })) == 1;

        /// <inheritdoc/>
        public async Task<bool> DeleteSkillAsync(int skillId)
            => (await _skillRepository.DeleteAsync(skillId)) == 1;

        /// <inheritdoc/>
        public async Task<bool> AddExperienceAsync(ExperienceDto experience)
            => (await _experienceRepository.AddAsync(new Experience
            {
                Company = experience.Company,
                Position = experience.Position,
                Description = experience.Description,
                StartDate = DateTime.ParseExact(experience.FromDate, "d MMMM yyyy", null),
                EndDate = string.IsNullOrEmpty(experience.ToDate) ? null : DateTime.ParseExact(experience.ToDate, "d MMMM yyyy", null)
            })) == 1;

        /// <inheritdoc/>
        public async Task<bool> UpdateExperienceAsync(ExperienceDto experience)
            => (await _experienceRepository.UpdateAsync(new Experience
            {
                Id = experience.Id,
                Company = experience.Company,
                Position = experience.Position,
                Description = experience.Description,
                StartDate = DateTime.ParseExact(experience.FromDate, "d MMMM yyyy", null),
                EndDate = string.IsNullOrEmpty(experience.ToDate) ? null : DateTime.ParseExact(experience.ToDate, "d MMMM yyyy", null)
            })) == 1;

        /// <inheritdoc/>
        public async Task<bool> DeleteExperienceAsync(int experienceId)
            => (await _experienceRepository.DeleteAsync(experienceId)) == 1;

        /// <inheritdoc/>
        public async Task<bool> AddEducationAsync(EducationDto education)
            => (await _educationRepository.AddAsync(new Education
            {
                Organization = education.Organization,
                Description = education.Description,
                StartDate = DateTime.ParseExact(education.FromDate, "d MMMM yyyy", null),
                EndDate = string.IsNullOrEmpty(education.ToDate) ? null : DateTime.ParseExact(education.ToDate, "d MMMM yyyy", null)
            })) == 1;

        /// <inheritdoc/>
        public async Task<bool> UpdateEducationAsync(EducationDto education)
            => (await _educationRepository.UpdateAsync(new Education
            {
                Id = education.Id,
                Organization = education.Organization,
                Description = education.Description,
                StartDate = DateTime.ParseExact(education.FromDate, "d MMMM yyyy", null),
                EndDate = string.IsNullOrEmpty(education.ToDate) ? null : DateTime.ParseExact(education.ToDate, "d MMMM yyyy", null)
            })) == 1;

        /// <inheritdoc/>
        public async Task<bool> DeleteEducationAsync(int educationId)
            => (await _educationRepository.DeleteAsync(educationId)) == 1;
    }
}