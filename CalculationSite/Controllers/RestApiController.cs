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
        private readonly ILogger<RestApiController> _logger;
        public RestApiController(IConfiguration configuration, ICalculationService calculation, ILogger<RestApiController> logger)
        {
            _configuration = configuration;
            _calculationService = calculation;
            _logger = logger;
        }
        
        [HttpPost("/api/calculate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<object> Calculate(ApiCalculateRequest request)
        {
            try
            {
                _logger.LogInformation($"Calculation started with values: {string.Join(" ", request.Values)}");

                var validator = new ApiCalculateRequestValidator(_configuration);
                var validationResult = validator.Validate(request);
                ValidationException.ThrowOnFailure(validationResult);
                var result = await _calculationService.Calculate(request.Values);

                _logger.LogInformation($"Result of calculation: {result}");

                return result;
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning($"Validation failure on calculation: {vex.Failure.ErrorMessage}");
                return BuildError(vex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error on calculation: {ex.Message}");
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
