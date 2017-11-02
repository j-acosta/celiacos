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
    public class ProductoManagers : BaseManagers
    {

        #region Consultas

        /// <summary>
        /// Retorna todos los Productos ordenados por el nombre.
        /// </summary>
        /// <returns></returns>
        public static List<Producto> GetProductos()
        {
            var Productos = new List<Producto>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"select p.*, c.Nombre as CategoriaNombre, e.nombre as nombreEmpresa, e.descripcion as descripcionEmpresa, e.ubicacion, e.horaAtencion, e.CertificadoDeSINTACC
                                            from Producto p inner
                                            join Categoria c on (p.idCategoria = c.Id) 
											join Empresa e on (p.idEmpresa = e.id)
                                            order by p.Nombre", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Producto = MapearProducto(dr);
                        Productos.Add(Producto);
                    }
                }
            }

            return Productos;
        }

        /// <summary>
        /// Retorna los productos de la empresa dada.
        /// </summary>
        /// <param name="empresaId"></param>
        /// <returns></returns>
        public static List<Producto> GetPorEmpresa(int empresaId)
        {
            var Productos = new List<Producto>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"select p.*, c.Nombre as CategoriaNombre
                                            from Producto p inner
                                            join Categoria c on (p.idCategoria = c.Id) 
                                            where p.idEmpresa = @id order by p.Nombre", con);

                query.Parameters.AddWithValue("@id", empresaId);

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Producto = MapearProducto(dr);
                        Productos.Add(Producto);
                    }
                }
            }

            return Productos;
        }

        /// <summary>
        /// Retorna los productos de la categoria dada perteneciente a la empresa pasada por parametro
        /// </summary>
        /// <param name="empresaId"></param>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public static List<Producto> GetPorEmpresaCategoria(int empresaId , int categoriaId)
        {
            var Productos = new List<Producto>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"select p.*, c.Nombre as CategoriaNombre
                                            from Producto p inner join Categoria c on (p.idCategoria = c.Id)
                                            where p.idCategoria = @categoria and p.idEmpresa = @empresa
                                            order by p.Nombre", con);
                query.Parameters.AddWithValue("@categoria", categoriaId);
                query.Parameters.AddWithValue("@empresa", empresaId);

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Producto = MapearProducto(dr);
                        Productos.Add(Producto);
                    }
                }
            }

            return Productos;
        }

        /// <summary>
        /// Retorna un producto a partir de un id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Producto GetById(int id)
        {
            Producto Producto = null;

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"SELECT p.* ,c.Nombre as CategoriaNombre, e.nombre as nombreEmpresa, e.descripcion as descripcionEmpresa, e.ubicacion, e.horaAtencion, e.CertificadoDeSINTACC  FROM Producto p inner join Categoria c on p.idCategoria = c.Id join Empresa e on p.idEmpresa=e.id  WHERE p.Id = @id", con);
                query.Parameters.AddWithValue("@id", id);

                using (var dr = query.ExecuteReader())
                {
                    dr.Read();
                    if (dr.HasRows)
                    {
                        Producto = MapearProducto(dr);
                    }
                }

            }

            return Producto;
        }        

        /// <summary>
        /// Retorna todas las categorias ordenados por el nombre.
        /// </summary>
        /// <returns></returns>
        public static List<CategoriaProducto> GetCategorias()
        {
            var categorias = new List<CategoriaProducto>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("SELECT * FROM Categoria order by Nombre", con);
                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var categoria = new CategoriaProducto
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = dr["Nombre"].ToString()
                        };
                        categorias.Add(categoria);
                    }
                }
            }

            return categorias;
        }

        /// <summary>
        /// Retorna los productos de la categoría dada.
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public static List<Producto> GetPorCategoria(int categoriaId)
        {
            var Productos = new List<Producto>();

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand(@"select p.*, c.Nombre as CategoriaNombre
                                            from Producto p inner join Categoria c on (p.idCategoria = c.Id)
                                            where p.idCategoria = @categoria
                                            order by p.Nombre", con);
                query.Parameters.AddWithValue("@categoria", categoriaId);

                using (var dr = query.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var Producto = MapearProducto(dr);
                        Productos.Add(Producto);
                    }
                }
            }

            return Productos;
        }

        #endregion


        #region ABMs

        /// <summary>
        /// Crea un nuevo plato en la BD.
        /// </summary>
        /// <param name="Producto"></param>
        public static void Nuevo(Producto Producto)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("INSERT INTO Producto (Nombre, Descripcion, Foto, LogoSINTACC, idCategoria, idEmpresa) VALUES (@nombre, @descripcion, @foto, @LogoSINTACC, @idCategoria, @idEmpresa)", con);

                query.Parameters.AddWithValue("@nombre", Producto.Nombre);
                query.Parameters.AddWithValue("@descripcion", Producto.Descripcion);
                query.Parameters.AddWithValue("@foto", Producto.Foto);
                query.Parameters.AddWithValue("@LogoSINTACC", Producto.LogoSINTACC);
                query.Parameters.AddWithValue("@idCategoria", Producto.Categoria.Id);
                query.Parameters.AddWithValue("@idEmpresa", Producto.Empresa.Id);

                query.ExecuteNonQuery();
            }
        }

        public static void ProductoModificado(Producto Producto)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();

                var query = new SqlCommand("UPDATE Producto set Nombre = @nombre, Descripcion = @descripcion, Foto = @foto, LogoSINTACC = @LogoSINTACC, idCategoria = @idCategoria, idEmpresa = @idEmpresa WHERE Id = @id", con);

                query.Parameters.AddWithValue("@id", Producto.Id);
                query.Parameters.AddWithValue("@nombre", Producto.Nombre);
                query.Parameters.AddWithValue("@descripcion", Producto.Descripcion);
                query.Parameters.AddWithValue("@foto", Producto.Foto);
                query.Parameters.AddWithValue("@LogoSINTACC", Producto.LogoSINTACC);
                query.Parameters.AddWithValue("@idCategoria", Producto.Categoria.Id);
                query.Parameters.AddWithValue("@idEmpresa", Producto.Empresa.Id);

                query.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Elimina un producto a partir de un id.
        /// </summary>
        /// <param name="id"></param>
        public static void ElimnarById(int id)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings[Strings.KeyConnectionStringCeliacos].ToString()))
            {
                con.Open();
                var query = new SqlCommand("DELETE FROM Producto WHERE Id = @id", con);
                query.Parameters.AddWithValue("@id", id);
                query.ExecuteNonQuery();

            }
        }

        #endregion


        #region MetodosPrivados
        private static Producto MapearProducto(SqlDataReader dr)
        {
            var Producto = new Producto
            {
                Id = Convert.ToInt32(dr["Id"]),
                Nombre = dr["Nombre"].ToString(),
                Descripcion = dr["Descripcion"].ToString(),
                Foto = dr["Foto"].ToString(),
                LogoSINTACC = dr["LogoSINTACC"].ToString(),
                Categoria = new CategoriaProducto
                {
                    Id = Convert.ToInt32(dr["idCategoria"]),
                    Nombre = dr["CategoriaNombre"].ToString()
                },
                Empresa = new Usuario
                {
                    Id = Convert.ToInt32(dr["idEmpresa"]),
                    Nombre = ColumnExists(dr, "nombreEmpresa") ? dr["nombreEmpresa"].ToString() : string.Empty,
                    Descripcion = ColumnExists(dr, "descripcionEmpresa") ? dr["descripcionEmpresa"].ToString() : string.Empty,
                    Ubicacion = ColumnExists(dr, "ubicacion") ? dr["ubicacion"].ToString() : string.Empty,
                    HoraAtencion = ColumnExists(dr, "horaAtencion") ? dr["horaAtencion"].ToString() : string.Empty,
                    CertificadoDeSINTACC = ColumnExists(dr, "CertificadoDeSINTACC") ? dr["CertificadoDeSINTACC"].ToString() : string.Empty,
                }
            };
            return Producto;
        }

        #endregion
    }
}