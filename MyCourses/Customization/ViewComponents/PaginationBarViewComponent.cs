using Microsoft.AspNetCore.Mvc;
using MyCourses.Models.ViewModels;

namespace MyCourses.Customization.ViewComponents
{
    public class PaginationBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPaginationInfo model)
        {
            //Mi servono:
            //Il numero della pagina corrente
            //Il numero di risultati totali
            //Il numero di risultati per pagina

            return View(model);
        }
    }
}
