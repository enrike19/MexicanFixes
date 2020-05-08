using DbfDataReader;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ConnectorAdminPAQ
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime inicio = DateTime.Now;

            string posPath = ConfigurationManager.AppSettings["posFile"].ToString();
            string prodPath = ConfigurationManager.AppSettings["prodFile"].ToString();            

            List<Venta> ventas = new List<Venta>();
            List<Producto> productos = new List<Producto>();

            Console.WriteLine(inicio.ToString("HH:mm:ss"));
            int i = 0;
            DbfDataReader.DbfDataReader dbfData = new DbfDataReader.DbfDataReader(posPath);
            // Leer todas las ventas.
            while (dbfData.Read())
            {
                i++;
                string codigoProd = dbfData.GetValue(4).ToString();
                double precio = (double)dbfData.GetValue(10);
                double importe = (double)dbfData.GetValue(25);
                DateTime fecha = DateTime.Parse(dbfData.GetValue(30).ToString());

                ventas.Add(new Venta(){
                        codigoProd = codigoProd, 
                        precio = precio,  
                        importe = importe,
                        fecha = fecha
                });
                Console.WriteLine(" ");
        
                // Mostrar avances cada 10,000..
                if (i % 10000 == 0) Console.WriteLine(i);
               
            }
            // leer todos los productos.
            dbfData = new DbfDataReader.DbfDataReader("C:\\workspace\\conectorAdminPAQ\\LATIENDITA\\MGW10005.dbf");
            while (dbfData.Read())
            {
                string id = dbfData.GetValue(0).ToString();
                string codProd = dbfData.GetValue(1).ToString();
                string nombre = dbfData.GetValue(2).ToString();
                string precio = dbfData.GetValue(43).ToString();

                productos.Add(new Producto()
                {
                    id = id,
                    codigo = codProd,
                    nombre = nombre,
                    precio = precio
                }) ;

            }

           Console.WriteLine(inicio.ToString("HH:mm:ss"));
           
           List<Venta> result= (from X in ventas where X.fecha > DateTime.Now.AddDays(-50000) && X.importe > 0 select X).ToList();

            DateTime filtro = DateTime.Now;

            List<Venta> ordenada = (from X in result 
                                    group X by X.codigoProd into newGroup orderby newGroup.Sum(s => s.importe) descending 
                                    select new Venta { codigoProd = newGroup.Key, importe = newGroup.Sum(s => s.importe)  }   ).Take(200).ToList();


            List<Producto> joinada = (from X in ordenada join p in productos on X.codigoProd equals p.id select new Producto { codigo = p.codigo, nombre = p.nombre, precio = p.precio } ).ToList();

            int pos = 1;
            foreach (Producto producto in joinada)
            {                
                Console.WriteLine(producto.codigo+" - "+producto.nombre);                
                pos++;
            }

            DateTime finalizacion = DateTime.Now;
            Console.WriteLine($"Proceso terminado. Duracion total {(finalizacion - inicio).TotalSeconds} segundos....");
            Thread.Sleep(160000);            
        }
    }

    public class Producto 
    {
        public string id { get; set; } = "";
        public string codigo { get; set; } = "";
        public string nombre { get; set; } = "";
        public string precio { get; set; } = "";
    }

    public class Venta
    {
        public string codigoProd { get; set; } = ""; // 4
        public string nomProducto { get; set; } = "";
        public double precio { get; set; } = 0;  // 11
        public double importe { get; set; } = 0; // 15  /  36
        public DateTime fecha { get; set; }  // 43


    }


}
