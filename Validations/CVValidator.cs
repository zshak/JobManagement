using FluentValidation;
using JobManagementApi.Models.DTOS;

namespace JobManagementApi.Validations
{
    public class CVValidator : AbstractValidator<CV>
    {
        public CVValidator() 
        {
            RuleFor(cv => cv)
                .Must(x => ValidProfession(x)).WithMessage("Invalid Education Field");
            RuleFor(cv => cv)
                .Must(x => ValidSkills(x)).WithMessage("Invalid Skills Field");
        }

        private static bool ValidSkills(CV x)
        {
            if (x.Skills == null) return true;
            foreach(var skill in x.Skills) 
            {
                if (skill.Experience == null) return false;
            }
            return true;
        }

        private static bool ValidProfession(CV cv)
        {
            if(cv.Education != null)
            {
                return cv.StartDate.HasValue && cv.EndDate.HasValue && cv.EndDate > cv.StartDate;
            }
            return !cv.StartDate.HasValue && !cv.EndDate.HasValue;
        }
    }
}
