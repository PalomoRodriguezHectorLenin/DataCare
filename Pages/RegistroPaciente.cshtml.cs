using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Security;
using System.Data;


namespace DataCare.Pages
{
    public class RegistroPaciente : PageModel
    {
        public PersonaInfo personaInfo = new PersonaInfo();
        public UsuarioInfo usuarioInfo = new UsuarioInfo();
        public String errorMessage = "";
        public String MessageDB = "";
        long numTel;
        public void OnGet()
        {
        }
        public void OnPost()
        {
            personaInfo.nombre = Request.Form["Nombre"];
            personaInfo.apellidoP = Request.Form["ApellidoPaterno"];
            personaInfo.apellidoM = Request.Form["ApellidoMaterno"];
            personaInfo.telefono = Request.Form["Telefono"];
            personaInfo.fecha_nacimiento = Request.Form["FechaNacimiento"];
            personaInfo.curp = Request.Form["Curp"];
            personaInfo.genero = Request.Form["Genero"];
            usuarioInfo.correo = Request.Form["Correo"];
            usuarioInfo.password = Request.Form["Password"];
            
            

            if (personaInfo.nombre.Length == 0 || personaInfo.apellidoP.Length == 0 ||
                personaInfo.telefono.Length == 0 ||
                personaInfo.fecha_nacimiento.Length == 0 || personaInfo.curp.Length == 0 ||
                personaInfo.genero.Length == 0 || usuarioInfo.correo.Length == 0 ||
                usuarioInfo.password.Length == 0)
            {
                errorMessage = "Se necesitan todos los campos";
                return;
            }

            //Guardar informacion en la base de datos
            try
            {
                String connectionString = "Data Source=LENIN;Initial Catalog=DATACARE;User ID=sa;Password=n0m3l0;Encrypt=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Persona" +
                                "(nombre, apellidoP,apellidoM,telefono,fecha_nacimiento,curp,genero)" +
                                "VALUES (@nombre, @apellidoP, @apellidoM,@telefono,@fecha_nacimiento,@curp,@genero);";
                    String sql2 = "SELECT p.id_persona FROM Persona p WHERE p.curp = @curp;";
                    String sql3 = "INSERT INTO Usuario" +
                                 "(id_persona, id_categoria, password, correo) VALUES" +
                                 "(@id_persona,1,@password,@correo);";


                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", personaInfo.nombre);
                        command.Parameters.AddWithValue("@apellidoP", personaInfo.apellidoP);
                        command.Parameters.AddWithValue("@apellidoM", personaInfo.apellidoM);
                        command.Parameters.AddWithValue("@telefono", personaInfo.telefono);
                        command.Parameters.AddWithValue("@fecha_nacimiento", personaInfo.fecha_nacimiento);
                        command.Parameters.AddWithValue("@curp", personaInfo.curp);
                        command.Parameters.AddWithValue("@genero", personaInfo.genero);

                        command.ExecuteNonQuery();
                    }
                    using(SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@curp", personaInfo.curp);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                    personaInfo.id = "" + reader.GetInt32(0);
                                    
                            }
                        }
                    }
                    using(SqlCommand command = new SqlCommand(sql3, connection))
                    {
                        command.Parameters.AddWithValue("@id_persona", personaInfo.id);

                        command.Parameters.AddWithValue("@password", usuarioInfo.password);
                        command.Parameters.AddWithValue("@correo", usuarioInfo.correo);

                        command.ExecuteNonQuery();
                    }
                                
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            personaInfo.nombre = "";personaInfo.curp = "";personaInfo.fecha_nacimiento = "";personaInfo.telefono = "";
            personaInfo.genero = "";personaInfo.apellidoM = "";personaInfo.apellidoP = ""; usuarioInfo.correo = "";
            usuarioInfo.password = "";

            MessageDB = "Se registro correctamente";
            Response.Redirect("/Index");

        }

    }
}

