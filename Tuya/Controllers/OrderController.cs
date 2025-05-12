using Domain.Entidades;
using Domain.Interfases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Tuya.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las órdenes.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderRepository repositorioOrdene) : ControllerBase
    {
        private readonly IOrderRepository _repositorioOrdenes= repositorioOrdene;

        /// <summary>
        /// Obtiene todas las órdenes registradas en el sistema.
        /// </summary>
        /// <param name="customerId">ID del cliente (opcional).</param>
        /// <param name="fechaDesde">Fecha de inicio del rango (opcional).</param>
        /// <param name="fechaHasta">Fecha final del rango (opcional).</param>
        /// <param name="estado">Estado de la orden (opcional).</param>
        /// <returns>Un resultado con la lista de órdenes filtradas.</returns>
        [HttpGet]
        public async Task<IActionResult> ConsultarTodos(int? customerId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, string? estado = null)
        {
            var resultado = await _repositorioOrdenes.ConsultarTodos(customerId, fechaDesde, fechaHasta,estado);
            if (resultado.IsFailed)
                return NotFound(resultado.Errors[0].Message);

            return Ok(resultado.Value);
        }

        /// <summary>
        /// Obtiene una orden específica por su identificador.
        /// </summary>
        /// <param name="id">Identificador único de la orden.</param>
        /// <returns>La orden correspondiente al ID, si existe.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _repositorioOrdenes.ConsultarPorId(id);
            if (resultado.IsFailed)
                return NotFound(resultado.Errors[0].Message);

            return Ok(resultado.Value);
        }

        /// <summary>
        /// Crea una nueva orden en el sistema.
        /// </summary>
        /// <param name="orden">Datos de la orden a crear.</param>
        /// <returns>Mensaje de éxito o error.</returns>
        [HttpPost]
        public async Task<IActionResult> CrearOrden([FromBody] Order orden)
        {
            var resultado = await _repositorioOrdenes.CrearOrder(orden);
            if (resultado.IsFailed)
                return BadRequest(resultado.Errors[0].Message);

            return Ok(resultado.Value);
        }

        /// <summary>
        /// Actualiza una orden existente.
        /// </summary>
        /// <param name="id">ID de la orden a actualizar.</param>
        /// <param name="orden">Datos actualizados de la orden.</param>
        /// <returns>Mensaje de éxito o error.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarOrden(int id, [FromBody] Order orden)
        {
            if (id != orden.Id)
                return BadRequest("El ID proporcionado no coincide con el de la orden.");

            var resultado = await _repositorioOrdenes.ActualizarOrder(orden);
            if (resultado.IsFailed)
                return BadRequest(resultado.Errors[0].Message);

            return Ok(resultado.Value);
        }

        /// <summary>
        /// Elimina una orden del sistema.
        /// </summary>
        /// <param name="id">ID de la orden a eliminar.</param>
        /// <returns>Mensaje de éxito o error.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarOrden(int id)
        {
            var resultado = await _repositorioOrdenes.EliminarOrder(id);
            if (resultado.IsFailed)
                return NotFound(resultado.Errors[0].Message);

            return Ok(resultado.Value);
        }
    }
}
