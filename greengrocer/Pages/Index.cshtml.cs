using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace greengrocer.Pages
{
    
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                
                return RedirectToPage("/Order");
            }

            
            return RedirectToPage("/Login");
        }
    }
}
