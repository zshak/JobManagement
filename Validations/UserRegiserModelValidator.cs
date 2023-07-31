using FluentValidation;
using JobManagementApi.Models.DTOS;
using JobManagementApi.Validations.StaticPredicates;
using System.Text.RegularExpressions;

namespace JobManagementApi.Validations  
{
    public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterModelValidator()
        {
            RuleFor(UserRegisterModel => UserRegisterModel.FirstName)
                .Must(x => Predicates.NotNullOrEmpty(x)).WithMessage("First Name Must Not Be Empty");
            RuleFor(UserRegisterModel => UserRegisterModel.LastName)
                .Must(x => Predicates.NotNullOrEmpty(x)).WithMessage("Last Name Must Not Be Empty");
            RuleFor(UserRegisterModel => UserRegisterModel.Password)
                .Must(x => Predicates.NotNullOrEmpty(x)).WithMessage("Password Must Not Be Empty")
                .Must(x => x.Length >= 8).WithMessage("Password must be longer than 8 symbols");
            RuleFor(UserRegisterModel => UserRegisterModel.IsEmployer)
                .NotNull().WithMessage("IsEmployer Field Must Not Be Empty");
            RuleFor(UserRegisterModel => UserRegisterModel)
                .Must(x => { 
                    if (x.IsEmployer && x.Company == null) return false;
                    if(!x.IsEmployer && x.Company != null) return false;
                    return true;
                }).WithMessage("Invalid Relation Between \"isEmployer\" and \"Company\" Field");
            RuleFor(UserRegisterModel => UserRegisterModel.Email)
                .Must(x => Predicates.NotNullOrEmpty(x)).WithMessage("Email Field Must Not Be Empty")
                .Must(x => Predicates.IsCorrectEmailFormat(x)).WithMessage("Incorrect Email Format");
        }

    }
}