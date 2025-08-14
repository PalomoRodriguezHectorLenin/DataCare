using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataCare.Pages
{
    public class InicioDeSesion : PageModel
    {
        public UsuarioInfo usuarioInfo = new UsuarioInfo();
        public String errorMessage = "";
        public String MessageDB = "";
        public String TemMessage = "";
        public String pass = "";
        int categoria;
        public String rol = "";
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
                    
                    String sql = "SELECT u.id_categoria, u.password, e.rol FROM Empleados e right join Usuario u ON e.id_usuario = u.id_usuario WHERE u.correo=@correo;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@correo", usuarioInfo.correo);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoria = reader.GetInt32(0);
                                pass = reader.GetString(1);
                                rol = reader.IsDBNull(2) ? null : reader.GetString(2);

                               
                            }
                        }
                    }

                    if(rol == null)
                    {
                        rol = "a";
                    }

                }
            }
            catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return;
            }


            if (pass == usuarioInfo.password && rol == "Doctor" && categoria == 2)
            {
                MessageDB = "Contraseña correcta, acceso garantizado para empleado";
                Response.Redirect("/PrincipalDoctor");
            }

            else if (pass == usuarioInfo.password && rol == "Recepcionista" && categoria == 2)
            {
                MessageDB = "Contraseña correcta, acceso garantizado para Recepcionista";
                Response.Redirect("/PrincipalRecepcionista");

            }

            else if (pass == usuarioInfo.password && categoria == 1)
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
