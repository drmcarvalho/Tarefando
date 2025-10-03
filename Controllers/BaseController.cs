using Microsoft.AspNetCore.Mvc;

namespace Tarefando.Api.Controllers
{
    public class BaseController: ControllerBase
    {
        internal static IEnumerable<dynamic> FormatErrors(IReadOnlyList<FluentResults.IError> errors)
        {
            return errors.Select(e => new { e.Message, e.Metadata, Reasons = e.Reasons.Select(r => e.Message) });
        }
    }
}
