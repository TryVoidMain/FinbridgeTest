using FluentValidation;
using FluentValidation.Results;

using CalculationSite.Types.Base;
using static CalculationSite.Controllers.RestApiController;

namespace CalculationSite.Types.Validators
{
    public class ApiCalculateRequestValidator : AbstractValidator<ICalculateValues>
    {
        private int minValue;
        private int maxValue;
        private int maxLength;

        public ApiCalculateRequestValidator(IConfiguration configuration) 
        {
            InitVariables(configuration);

            CascadeMode = CascadeMode.Stop;

            RuleFor(v => v.Values)
                .NotNull()
                .NotEmpty()
                .WithMessage("Input values must be not empty");

            RuleFor(v => v.Values)
                .Custom(CheckRanges);
        }

        public void CheckRanges(int[] inputArray, ValidationContext<ICalculateValues> context)
        {
            if (inputArray.Length > maxLength)
                context.AddFailure(new ValidationFailure(nameof(ApiCalculateRequest.Values), $"Array length should be less than {maxLength}"));

            foreach (var value in inputArray)
            {
                if (value > maxValue)
                {
                    context.AddFailure(new ValidationFailure(nameof(ApiCalculateRequest.Values), $"Value {value} should be less than {maxValue}"));
                    return;
                }

                if (value < minValue)
                {
                    context.AddFailure(new ValidationFailure(nameof(ApiCalculateRequest.Values), $"Value {value} should be greater than {minValue}"));
                    return;
                }
            }
        }

        private void InitVariables(IConfiguration configuration)
        {
            minValue = configuration.GetValue<int>("MinValue");
            maxValue = configuration.GetValue<int>("MaxValue");
            maxLength = configuration.GetValue<int>("MaxLength");

            if (minValue == default)
                minValue = int.MinValue;

            if (maxValue == default)
                maxValue = int.MaxValue;

            if (maxLength == default)
                maxLength = int.MaxValue;
        }
    }
}
