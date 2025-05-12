using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;
using FluentResults;

namespace Domain.Interfases
{
    /// <summary>
    /// Define las operaciones de acceso a datos para las órdenes.
    /// </summary>
    public interface IOrderRepository
    {
        /// <summary>
        /// Consulta todas las órdenes según los filtros proporcionados.
        /// </summary>
        /// <param name="customerId">ID del cliente (opcional).</param>
        /// <param name="fechaDesde">Fecha de inicio del rango (opcional).</param>
        /// <param name="fechaHasta">Fecha final del rango (opcional).</param>
        /// <param name="estado">Estado de la orden (opcional).</param>
        /// <returns>Un resultado con la lista de órdenes filtradas.</returns>
        Task<Result<IEnumerable<Order>>> ConsultarTodos(int? customerId = null, DateTime? fechaDesde = null, DateTime? fechaHasta = null, string? estado = null);

        /// <summary>
        /// Consulta una orden específica por su identificador.
        /// </summary>
        /// <param name="id">ID de la orden a consultar.</param>
        /// <returns>La orden encontrada o un error si no existe.</returns>
        Task<Result<Order>> ConsultarPorId(int id);

        /// <summary>
        /// Registra una nueva orden en la base de datos.
        /// </summary>
        /// <param name="order">Orden a crear.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<Result<string>> CrearOrder(Order order);

        /// <summary>
        /// Actualiza una orden existente.
        /// </summary>
        /// <param name="order">Orden con los datos actualizados.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<Result<string>> ActualizarOrder(Order order);

        /// <summary>
        /// Elimina una orden por su identificador.
        /// </summary>
        /// <param name="id">ID de la orden a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        Task<Result<string>> EliminarOrder(int id);
    }
}