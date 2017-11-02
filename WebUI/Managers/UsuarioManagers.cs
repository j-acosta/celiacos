using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebUI.Entities;
using WebUI.Utilities;

namespace WebUI.Managers
{
    public class UsuarioManagers : BaseManagers
    {

        #region Consultas

        /// <summary>
        /// Este metodo retorna la empresa (clase Entity) que coincide con el email y password. Caso contrario retorna null.
        /// </summary>
        /// <param name="email">email de la empresa</param>
        /// <param name="password">password de la empresa</param>
        /// <returns>Empresa o null</returns>
        public static Usuario Login(string email, string password)
        {
            Usuario usuario = GetByEmail(email);
            string passwordEncriptada = Strings.Encriptar(password);
            if (usuario != null && usuario.Password == passwordEncriptada)
            {
                return usuario;
            }
            return null;

        }

        /// <summary>
        /// Retorna un usuario a partir de un E-mail.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Usuario GetByEmail(string email)
        {
            Usuario usuario = null;

            //creo una conexion con la base de datos.
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                //abro la conexion
                con.Open();

                //Preparo la consulta a realizar.
                var query = new SqlCommand("SELECT * FROM Empresa WHERE Email = @email", con);
                //Seteo los parametros
                query.Parameters.AddWithValue("@email", email);

                //creo un lector
                using (var dr = query.ExecuteReader())
                {
                    //le digo al lector que lea la 1er fila
                    dr.Read();
                    if (dr.HasRows) //Si hay fila..
                    {
                        //Mapeo la fila con la entidad.
                        usuario = MapearUsuario(dr);
                    }
                }
            }

            return usuario;
        }

        /// <summary>
        /// busca el usuario con ese email y le cambia la password por la pasada por parametro.
        /// <param name="email"></param>
        /// <returns></returns>
        public static void ResetearPassword(string email, string NuevaPassword)
        {
            
            //creo una conexion con la base de datos.
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                //abro la conexion
                con.Open();
                //Preparo la consulta a realizar.
                var query = new SqlCommand("UPDATE Empresa set Password = @password WHERE email = @email", con);
                //Seteo los parametros
                query.Parameters.AddWithValue("@email", email);
                query.Parameters.AddWithValue("@password", Strings.Encriptar(NuevaPassword));
                
                query.ExecuteNonQuery();
            }

        }

        /// <summary>
        /// Retorna todos los clientes ordenados por el nombre.
        /// </summary>
        /// <returns></returns>
        public static List<Usuario> GetEmpresas()
        {
            var usuarios = new List<Usuario>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("SELECT * FROM Empresa WHERE TipoDeUsuario = 1 order by nombre", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var usuario = MapearUsuario(dr);
                        // Agregamos el usuario a la lista
                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        /// <summary>
        /// Retorna los ultimos clientes registrados.
        /// </summary>
        /// <returns></returns>
        public static List<Usuario> GetUltimasEmpresas()
        {
            var usuarios = new List<Usuario>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("select top 7 * from Empresa where TipoDeUsuario = 1 order by id desc ", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var usuario = MapearUsuario(dr);
                        // Agregamos el usuario a la lista
                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        
        /// <summary>
        /// Retorna las empresas con su latitud, longitud y nombre para mostrar en el mapa.
        /// </summary>
        /// <returns></returns>
        public static List<EmpresaUbicacion> GetEmpresasUbicaciones()
        {
            var Ubicaciones = new List<EmpresaUbicacion>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("select nombre, foto, ubicacion, horaAtencion, latitud, longitud from empresa where TipoDeUsuario = 1 ", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Ubicacion = new EmpresaUbicacion
                        {
                            Nombre = dr["Nombre"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            Ubicacion = dr["Ubicacion"].ToString(),
                            horaAtencion = dr["horaAtencion"].ToString(),
                            Latitud = dr["Latitud"].ToString(),
                            Longitud = dr["Longitud"].ToString()
                        };
                        // Agregamos el usuario a la lista
                        Ubicaciones.Add(Ubicacion);
                    }
                }
            }

            return Ubicaciones;
        }



        /// <summary>
        /// Retorna un usuario a partir de un id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Usuario GetById(int id)
        {
            Usuario usuario = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("SELECT * FROM Empresa WHERE Id = @id", con);
                query.Parameters.AddWithValue("@id", id);

                using (var dr = query.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        usuario = MapearUsuario(dr);
                    }
                }
            }

            return usuario;
        }

        #endregion


        #region ABMs

        ///// <summary>
        ///// Crea un nuevo usuario(Empresa) en la BD.
        ///// </summary>
        ///// <param name="Empresa"></param>
        public static void Nuevo(Usuario usuario)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("INSERT INTO Empresa (Nombre, Descripcion, Ubicacion, Foto, Email, Password, HoraAtencion, CertificadoDeSINTACC, Latitud, Longitud, TipoDeUsuario) VALUES (@nombre, @descripcion, @ubicacion, @foto, @email, @password, @horaAtencion, @CertificadoDeSINTACC, @Latitud, @Longitud, @TipoDeUsuario)", con);

                query.Parameters.AddWithValue("@nombre", usuario.Nombre);
                query.Parameters.AddWithValue("@descripcion", usuario.Descripcion);
                query.Parameters.AddWithValue("@ubicacion", usuario.Ubicacion);
                query.Parameters.AddWithValue("@foto", usuario.Foto);
                query.Parameters.AddWithValue("@email", usuario.Email);
                query.Parameters.AddWithValue("@password", Strings.Encriptar(usuario.Password) );
                query.Parameters.AddWithValue("@horaAtencion", usuario.HoraAtencion);
                query.Parameters.AddWithValue("@CertificadoDeSINTACC", usuario.CertificadoDeSINTACC);
                query.Parameters.AddWithValue("@Latitud", usuario.Latitud);
                query.Parameters.AddWithValue("@Longitud", usuario.Longitud);
                query.Parameters.AddWithValue("@TipoDeUsuario", usuario.TipoDeUsuario);

                query.ExecuteNonQuery();
            }
        }

        public static void UsuarioModificado(Usuario usuario)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("UPDATE Empresa set Nombre = @nombre, Descripcion = @descripcion, Foto = @foto, Email = @email, ubicacion = @ubicacion, horaAtencion = @horaAtencion, latitud = @latitud, longitud = @longitud, CertificadoDeSINTACC = @CertificadoDeSINTACC WHERE Id = @id", con);

                query.Parameters.AddWithValue("@id", usuario.Id);
                query.Parameters.AddWithValue("@nombre", usuario.Nombre);
                query.Parameters.AddWithValue("@descripcion", usuario.Descripcion);
                query.Parameters.AddWithValue("@foto", usuario.Foto);
                query.Parameters.AddWithValue("@email", usuario.Email);
                query.Parameters.AddWithValue("@ubicacion", usuario.Ubicacion);
                query.Parameters.AddWithValue("@horaAtencion", usuario.HoraAtencion);
                query.Parameters.AddWithValue("@CertificadoDeSINTACC", usuario.CertificadoDeSINTACC);
                query.Parameters.AddWithValue("@latitud", usuario.Latitud);
                query.Parameters.AddWithValue("@longitud", usuario.Longitud);
                query.Parameters.AddWithValue("@TipoDeUsuario", 1);

                query.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Modifica la contraseña del usuario.
        /// </summary>
        /// <param name="usuario"></param>
        public static void ModificarContraseñaUsuario(Usuario usuario)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("UPDATE Empresa set Password = @password WHERE Id = @id", con);

                query.Parameters.AddWithValue("@id", usuario.Id);
                query.Parameters.AddWithValue("@password", usuario.Password);

                query.ExecuteNonQuery();
            }

        }

        ///// <summary>
        ///// Elimina un usuario a partir de un id.
        ///// </summary>
        ///// <param name="id"></param>
        public static void ElimnarById(int id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("DELETE FROM Empresa WHERE Id = @id ", con);
                query.Parameters.AddWithValue("@id", id);
                query.ExecuteNonQuery();

            }
        }

        #endregion


        #region MetodosPrivados

        private static Usuario MapearUsuario(SqlDataReader dr)
        {
            var usuario = new Usuario
            {
                Id = Convert.ToInt32(dr["Id"]),
                Nombre = dr["Nombre"].ToString(),
                Descripcion = dr["Descripcion"].ToString(),
                Ubicacion = dr["Ubicacion"].ToString(),
                Foto = dr["Foto"].ToString(),
                Email = dr["Email"].ToString(),
                Password = dr["Password"].ToString(),
                HoraAtencion = dr["HoraAtencion"].ToString(),
                CertificadoDeSINTACC = dr["CertificadoDeSINTACC"].ToString(),
                Latitud = dr["Latitud"].ToString(),
                Longitud = dr["Longitud"].ToString(),
                TipoDeUsuario = (EUsuarioTipo)Convert.ToInt32(dr["TipoDeUsuario"])
            };
            return usuario;
        }

        #endregion

    }
}