using CalculationSite.Services;
using CalculationSite.Types.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CalculationSite.Controllers
{
    [ApiController]
    public partial class RestApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICalculationService _calculationService;
        public RestApiController(IConfiguration configuration, ICalculationService calculation)
        {
            _configuration = configuration;
            _calculationService = calculation;
        }
        
        [HttpPost("/api/calculate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public object Calculate(ApiCalculateRequest request)
        {
            try
            {
                var validator = new ApiCalculateRequestValidator(_configuration);
                var validationResult = validator.Validate(request);
                ValidationException.ThrowOnFailure(validationResult);
                var result = _calculationService.Calculate(request.Values);

                return result;
            }
            catch (Exception ex)
            {
                return BuildError(ex);
            }
        }

        public class ValidationException : Exception 
        {
            public ValidationFailure Failure { get; set; }

            public ValidationException(ValidationFailure failure)
            {
                Failure = failure;
            }

            public ValidationException(string errorMessage) 
            {
                Failure = new ValidationFailure(null, errorMessage);
            }

            public ValidationException(string propertyName, string errorMessage) 
            {
                Failure = new ValidationFailure(propertyName, errorMessage);
            }

            public static void ThrowOnFailure(ValidationResult validationResult)
            {
                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors.FirstOrDefault());
            }
        }

        private ApiError BuildError(Exception  ex)
        {
            return new ApiError() { ErrorMessage = ex is ValidationException vex ? vex.Failure.ErrorMessage : ex.Message };
        }
    }
}
