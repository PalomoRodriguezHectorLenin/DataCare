using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataCare.Pages
{
    public class InicioDeSesion : PageModel
    {
        public UsuarioInfo usuarioInfo = new UsuarioInfo();
        public String errorMessage = "";
        public String MessageDB = "";
        public String pass = "";
        public void OnGet()
        {
            
        }
        public void OnPost()
        {
            usuarioInfo.correo = Request.Form["Correo"];
            usuarioInfo.password = Request.Form["Password"];

            if(usuarioInfo.correo.Length == 0 || usuarioInfo.password.Length == 0)
            {
                errorMessage = "Se necesitan todos los campos";
                return;
            }

            //Inicio de sesion
            try
            {
                String connectionString = "Data Source=LENIN;Initial Catalog=DATACARE;User ID=sa;Password=n0m3l0;Encrypt=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT u.password FROM Usuario u WHERE u.correo = @correo;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@correo", usuarioInfo.correo);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pass = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return;
            }

            if(pass == usuarioInfo.password)
            {
                MessageDB = "Contraseña correcta, acceso garantizado, bienvenido señor stark";
                Response.Redirect("/PrincipalPaciente");
            }
            else
            {
                MessageDB = "Contraseña incorrecta, vuelva a intentarlo";
                usuarioInfo.password = "";

            }
           
        }
    }
}
