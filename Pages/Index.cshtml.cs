using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataCare.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
    public class PersonaInfo
    {
        public String id;
        public String nombre;
        public String apellidoP;
        public String apellidoM;
        public String telefono;
        public String fecha_nacimiento;
        public String curp;
        public String genero;
    }

    public class UsuarioInfo 
    {
        public String correo;
        public String password;
    }

}


