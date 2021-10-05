using EagleRockHub.Enums;
using EagleRockHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Attributes
{
    public class ValidateParamAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var response = new ApiResponse();
            var validationErrors = context.ModelState.Keys.SelectMany(key => context.ModelState[key].Errors.Select(x => x.ErrorMessage)).ToList();
            validationErrors.ForEach(errorMessage => response.AddMessage(errorMessage, ApiMessageType.Error));
            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
