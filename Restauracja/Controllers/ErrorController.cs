using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("/Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        if (statusCode == 404)
        {
            // Return the custom NotFound view
            return View("~/Views/Shared/NotFound.cshtml");
        }

        // Handle other error codes or return a general error view
        return View("~/Views/Shared/Error.cshtml");
    }
}
