using FluentValidation;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Validations.StaticPredicates;

namespace JobManagementApi.Validations
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator() 
        {
            RuleFor(UserLoginModel => UserLoginModel.Email)
                .Must(x => Predicates.NotNullOrEmpty(x)).WithMessage("Email Field Must Not Be Empty");
            RuleFor(UserLoginModel => UserLoginModel.Password)
                .Must(x => Predicates.NotNullOrEmpty(x)).WithMessage("Password Field Must Not Be Empty");
        }
    }
}
