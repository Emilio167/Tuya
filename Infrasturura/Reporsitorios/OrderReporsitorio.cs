using System;
using Domain.Entidades;
using Domain.Interfases;
using FluentResults;
using Infrasturura.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrasturura.Reporsitorios
{
    /// <summary>
    /// Implementación del repositorio para manejar operaciones CRUD sobre órdenes.
    /// </summary>
    public class OrderRepository(AppDbContext appDbContext) : IOrderRepository
    {
        private readonly AppDbContext appDbContext= appDbContext;

        /// <summary>
        /// Consulta todas las órdenes según los filtros proporcionados.
        /// </summary>
        /// <param name="customerId">ID del cliente (opcional).</param>
        /// <param name="fechaDesde">Fecha de inicio del rango (opcional).</param>
        /// <param name="fechaHasta">Fecha final del rango (opcional).</param>
        /// <param name="estado">Estado de la orden (opcional).</param>
        /// <returns>Un resultado con la lista de órdenes filtradas.</returns>
        public async Task<Result<IEnumerable<Order>>> ConsultarTodos(int? customerId = null,DateTime? fechaDesde = null, DateTime? fechaHasta = null, string? estado = null)
        {
            var query = appDbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .AsQueryable();

            if (customerId.HasValue)
                query = query.Where(o => o.CustomerId == customerId.Value);

            if (fechaDesde.HasValue)
                query = query.Where(o => o.DeliveryDate >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                query = query.Where(o => o.DeliveryDate <= fechaHasta.Value);

            if (!string.IsNullOrWhiteSpace(estado))
                query = query.Where(o => o.Status == estado);

            IEnumerable<Order> orders = await query.ToListAsync();
            return Result.Ok(orders);
        }
        /// <summary>
        /// Consulta una orden específica por su identificador único.
        /// </summary>
        /// <param name="id">ID de la orden a consultar.</param>
        /// <returns>Un resultado con la orden encontrada o un mensaje de error si no existe.</returns>
        public async Task<Result<Order>> ConsultarPorId(int id)
        {
            if (id <= 0)
                return Result.Fail("El ID debe ser mayor que cero.");

            Order? order = await appDbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order is null
                ? Result.Fail($"No se encontró una orden con ID {id}.")
                : Result.Ok(order);
        }

        /// <summary>
        /// Crea una nueva orden en la base de datos.
        /// </summary>
        /// <param name="order">Objeto que representa la orden a crear.</param>
        /// <returns>Un resultado con el mensaje de éxito o error.</returns>
        public async Task<Result<string>> CrearOrder(Order order)
        {
            if (order == null || order.Items.Count == 0)
                return Result.Fail("La orden debe contener al menos un ítem.");

            Customer? customer = appDbContext.Customers.Where(x =>x.Id.Equals(order.CustomerId)).FirstOrDefault();
            if (customer == null) return Result.Fail($"No se encontró un cliente con ID {order.CustomerId}.");

            try
            {
                appDbContext.Orders.Add(order);
                await appDbContext.SaveChangesAsync();
                return Result.Ok("Orden creada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return Result.Fail($"Error al crear la orden: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualiza los datos de una orden existente.
        /// </summary>
        /// <param name="order">Objeto  con los datos actualizados.</param>
        /// <returns>Un resultado con el mensaje de éxito o error.</returns>
        public async Task<Result<string>> ActualizarOrder(Order order)
        {
            if (order == null || order.Id <= 0)
                return Result.Fail("La orden es inválida.");

            Customer? customer = appDbContext.Customers.Where(x => x.Id.Equals(order.CustomerId)).FirstOrDefault();
            if (customer == null) return Result.Fail($"No se encontró un cliente con ID {order.CustomerId}.");

            try
            {
                appDbContext.Orders.Update(order);
                await appDbContext.SaveChangesAsync();
                return Result.Ok("Orden actualizada correctamente.");
            }
            catch (DbUpdateException ex)
            {
                return Result.Fail($"Error al actualizar la orden: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina una orden por su identificador único.
        /// </summary>
        /// <param name="id">ID de la orden a eliminar.</param>
        /// <returns>Un resultado con el mensaje de éxito o error.</returns>
        public async Task<Result<string>> EliminarOrder(int id)
        {
            Order? order = await appDbContext.Orders.FindAsync(id);

            if (order == null)
                return Result.Fail($"No se encontró la orden con ID {id}.");

            try
            {
                appDbContext.Orders.Remove(order);
                await appDbContext.SaveChangesAsync();
                return Result.Ok("Orden eliminada exitosamente.");
            }
            catch (DbUpdateException ex)
            {
                return Result.Fail($"Error al eliminar la orden: {ex.Message}");
            }
        }
    }
}
