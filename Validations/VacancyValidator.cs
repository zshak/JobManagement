using FluentValidation;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Validations.StaticPredicates;

namespace JobManagementApi.Validations
{
    public class VacancyValidator : AbstractValidator<Vacancy>
    {
        public VacancyValidator() 
        {
            RuleFor(vacancy => vacancy.JobTitle)
                .Must(x => Predicates.NotNullOrEmpty(x))
                .WithMessage("Job Title Field Must Not Be Empty");
            RuleFor(Vacancy => Vacancy.ExpirationDate)
                .NotNull()
                .WithMessage("Job Expiration Date Field Must Not Be Empty");
            RuleFor(Vacancy => Vacancy.Skills)
                .Must(x => ValidSkills(x))
                .WithMessage("More Than 5 Unique Not-Null Skills Requiered");
        }

        private static bool ValidSkills(List<VacancySkillModel>? x)
        {
            if (x.Count < 5) return false;
            foreach (var skill in x)
            {
                if (skill == null || skill.SkillId == null || skill.Weight == null)
                    return false;
            }
            return true;
        }
    }
}
