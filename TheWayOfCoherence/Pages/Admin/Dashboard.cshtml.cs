using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AdminDashboardModel : PageModel
{
    [BindProperty]
    public NewPostInput NewPost { get; set; } = new();

    public IActionResult OnGet()
    {
        // Redirect til den nye Owner.razor side
        return Redirect("/owner");
    }

    public IActionResult OnPost()
    {
        // TODO: Persist NewPost to DB; for now we just succeed.
        TempData["Message"] = "Post gemt! (prototype)";
        return Page();
    }

    public class NewPostInput
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
    }
}
