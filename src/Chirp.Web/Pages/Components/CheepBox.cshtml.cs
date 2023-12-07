using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages.Components
{
    public class CheepBoxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CheepablePageModel model)
        {
            return View(model);
        }
    }
}