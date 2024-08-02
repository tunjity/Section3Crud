using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Section3Crud.Model
{
    public class CustomerFormModel
    {
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
    public class ProductFormModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
    public class CustomerFormModelValidator : AbstractValidator<CustomerFormModel>
    {
        public CustomerFormModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(250);
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).MaximumLength(200);
        }
    }
    public class ProductFormModelValidator : AbstractValidator<ProductFormModel>
    {
        public ProductFormModelValidator()
        {
            RuleFor(x => x.ProductName)
                .Empty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .LessThanOrEqualTo(0).WithMessage("Enter valid amount.")
                .ScalePrecision(2, 18).WithMessage("Price must not have more than 2 decimal places and must be within valid range.");
        }
    }
}
