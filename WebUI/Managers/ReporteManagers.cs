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
    public class ReporteManagers : BaseManagers
    {
        
        #region Consultas

        /// <summary>
        /// Retorna todos los reportes de los usuarios anonimos.
        /// </summary>
        /// <returns></returns>
        public static List<Reporte> GetReportes()
        {
            var Reportes = new List<Reporte>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"select r.*, p.Nombre as NombreProducto
                                            from Reportado r inner
                                            join Producto p on (r.idProducto = p.Id)
                                            order by p.Nombre", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Reporte = MapearReporte(dr);
                        Reportes.Add(Reporte);
                    }
                }
            }

            return Reportes;
        }

        /// <summary>
        /// Retorna los ultimos 5 reportes que se hayan efectuado de los usuarios anonimos.
        /// </summary>
        /// <returns></returns>
        public static List<Reporte> GetUltimosReportes()
        {
            var Reportes = new List<Reporte>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"select top 5  r.*, p.Nombre as NombreProducto
                                            from Reportado r inner
                                            join Producto p on (r.idProducto = p.Id)
                                            order by r.id desc", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Reporte = MapearReporte(dr);
                        Reportes.Add(Reporte);
                    }
                }
            }

            return Reportes;
        }

        /// <summary>
        /// Retorna un reporte a partir de un id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Reporte GetById(int id)
        {
            Reporte Reporte = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"SELECT r.Id,r.Email,r.Asunto,r.Mensaje,r.idProducto,p.Nombre as NombreProducto FROM Reportado r inner join Producto p on r.idProducto = p.Id WHERE r.Id = @id", con);
                query.Parameters.AddWithValue("@id", id);

                using (var dr = query.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Reporte = MapearReporte(dr);
                    }
                }
            }

            return Reporte;
        }

        #endregion


        #region ABs

        /// <summary>
        /// Elimina un Reporte a partir de un id.
        /// </summary>
        /// <param name="id"></param>
        public static void ElimnarById(int id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();
                var query = new SqlCommand("DELETE FROM Reportado WHERE Id = @id", con);
                query.Parameters.AddWithValue("@id", id);
                query.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// Crea un nuevo reporte en la BD.
        /// </summary>
        /// <param name="Reporte"></param>
        public static void Nuevo(Reporte reporte)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("INSERT INTO Reportado (Email, Asunto, Mensaje, idProducto) VALUES (@email, @asunto, @mensaje, @idProducto)", con);

                query.Parameters.AddWithValue("@email", reporte.Email);
                query.Parameters.AddWithValue("@asunto", reporte.Asunto);
                query.Parameters.AddWithValue("@mensaje", reporte.Mensaje);
                query.Parameters.AddWithValue("@idProducto", reporte.Producto.Id);

                query.ExecuteNonQuery();
            }
        }

        #endregion


        #region MetodosPrivados
        private static Reporte MapearReporte(SqlDataReader dr)
        {
            var Reporte = new Reporte
            {
                Id = Convert.ToInt32(dr["Id"]),
                Email = dr["Email"].ToString(),
                Asunto = dr["Asunto"].ToString(),
                Mensaje = dr["Mensaje"].ToString(),
                Producto = new Producto
                {
                    Id = Convert.ToInt32(dr["idProducto"]),
                    Nombre = dr["NombreProducto"].ToString()
                }
            };
            return Reporte;
        }

        #endregion

    }
}