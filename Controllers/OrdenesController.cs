using Microsoft.AspNetCore.Mvc;

namespace WebAPI_07_10_2026.Controllers
{
    [ApiController]
    [Route("api/ordenes")]
    public class OrdenesController : ControllerBase
    {
        // Lista estática en memoria para guardar las órdenes
        private static List<Orden> ordenes = new List<Orden>
        {
            // Inicializamos la orden con ID 5 y una lista de 2 productos
            new Orden
            {
                Id = 5,
                ClienteId = 101,
                Fecha = DateTime.Now,
                Total = 17.00m,
                Productos = new List<Producto>
                {
                    new Producto { Nombre = "Hamburguesa clásica", Cantidad = 2, Precio = 6.50m },
                    new Producto { Nombre = "Gaseosa cola", Cantidad = 2, Precio = 2.00m }
                }
            }
        };

        // Consultar todas las órdenes registradas en el sistema
        // Ruta: GET /api/ordenes
        [HttpGet]
        public ActionResult<List<Orden>> ObtenerTodas()
        {
            return Ok(ordenes);
        }

        // Crear una nueva orden de compra
        // Ruta: POST /api/ordenes
        [HttpPost]
        public ActionResult<Orden> CrearOrden([FromBody] Orden nuevaOrden)
        {
            ordenes.Add(nuevaOrden);
            return Ok(nuevaOrden);
        }

        // Cancelar/Eliminar la orden con el ID 5 (o cualquier ID)
        // Ruta: DELETE /api/ordenes/5
        [HttpDelete("{id}")]
        public ActionResult EliminarOrden(int id)
        {
            // Buscamos la orden en la lista por su Id
            Orden? ordenEncontrada = ordenes.FirstOrDefault(o => o.Id == id);

            if (ordenEncontrada == null)
            {
                return NotFound("No se encontró la orden.");
            }

            ordenes.Remove(ordenEncontrada);
            return Ok($"La orden con ID {id} fue eliminada.");
        }

        // Demostración de error de cliente (400 Bad Request)
        // Ruta: POST /api/ordenes/error-cliente
        [HttpPost("error-cliente")]
        public ActionResult ErrorCliente([FromBody] Orden? orden)
        {
            // Validación: si no enviamos datos o falta ClienteId
            if (orden == null || orden.ClienteId <= 0)
            {
                return BadRequest(new
                {
                    error = "Error de cliente (400)",
                    mensaje = "El ClienteId es requerido y debe ser mayor a 0.",
                    detalles = "Datos inválidos enviados desde el cliente."
                });
            }

            if (orden.Productos == null || orden.Productos.Count == 0)
            {
                return BadRequest(new
                {
                    error = "Error de cliente (400)",
                    mensaje = "La orden debe contener al menos 1 producto.",
                    detalles = "Lista de productos vacía o nula."
                });
            }

            return Ok(orden);
        }

        // Demostración de error de servidor (500 Internal Server Error)
        // Ruta: POST /api/ordenes/error-servidor
        [HttpPost("error-servidor")]
        public ActionResult ErrorServidor([FromBody] Orden orden)
        {
            try
            {
                // Simulamos un error del servidor
                throw new InvalidOperationException("Se ha producido un error crítico en el servidor al procesar la orden.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "Error de servidor (500)",
                    mensaje = ex.Message,
                    detalles = "El servidor encontró una condición inesperada que le impidió cumplir la solicitud.",
                    timestamp = DateTime.UtcNow
                });
            }
        }
    }

    // Clases para modelar el JSON 
    public class Orden
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<Producto> Productos { get; set; } = new List<Producto>();
    }

    public class Producto
    {
        public string Nombre { get; set; } = "";
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}