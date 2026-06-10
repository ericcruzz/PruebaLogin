using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontlogin.Pages.Login
{
    public class LoginModel : PageModel
    {
        private IHttpClientFactory _clientFactory;

        public LoginModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _clientFactory.CreateClient("api");

            var respuesta = await client.PostAsJsonAsync("Auth/login", Input);

            if (respuesta.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Credenciales inválidas. Por favor, inténtalo de nuevo.";
                return Page();

            }

        }

        public class LoginViewModel
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
