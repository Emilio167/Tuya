using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;
using Domain.Interfases;
using FluentResults;
using Infrasturura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrasturura.Reporsitorios
{
    /// <summary>
    /// Implementación del repositorio para manejar operaciones CRUD sobre clientes.
    /// </summary>
    public class CustomerReporsitorio(AppDbContext appDbContext) : ICustomerRepository
    {
        private readonly AppDbContext _appDbContext= appDbContext;
       
        /// <summary>
        /// Consulta todos los clientes registrados en el sistema, con opción de aplicar filtros por identificador, nombre o correo electrónico.
        /// </summary>
        /// <param name="id">Identificador único del cliente. Si se proporciona, se filtrarán los clientes por este ID.</param>
        /// <param name="name">Nombre o parte del nombre del cliente. Si se proporciona, se filtrarán los clientes cuyo nombre contenga este valor.</param>
        /// <param name="email">Correo electrónico o parte del correo del cliente. Si se proporciona, se filtrarán los clientes cuyo correo contenga este valor.</param>
        /// <returns>Una lista de clientes que cumplen con los criterios de búsqueda especificados.</returns>
        public async Task<Result<IEnumerable<Customer>>> ConsultarTodos(int? id, string? name, string? email)
        {
            IQueryable<Customer> query = _appDbContext.Customers.AsQueryable();

            if (id.HasValue && id.Value > 0)
                query = query.Where(x => x.Id == id.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(x => x.Email.Contains(email));

            IEnumerable<Customer>customers = await query.ToListAsync();

            return Result.Ok(customers);
        }

        /// <summary>
        /// Consulta un cliente por su ID.
        /// </summary>
        /// <param name="id">Identificador único del cliente.</param>
        public async Task<Result<Customer>> ConsultarPorId(int id)
        {
            if (id <= 0)
                return Result.Fail("El ID debe ser mayor a cero.");

            var customer = await _appDbContext.Customers.FindAsync(id);

            if (customer == null)
                return Result.Fail($"No se encontró un cliente con ID {id}.");

            return Result.Ok(customer);
        }

        /// <summary>
        /// Guarda un nuevo cliente en la base de datos.
        /// </summary>
        /// <param name="customer">Objeto cliente a guardar.</param>
        public async Task<Result<string>> GuardarCustomer(Customer customer)
        {
            if (customer == null)
                return Result.Fail("El cliente no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(customer.Name)) 
                return Result.Fail("El nombre del cliente es obligatorio.");

            try
            {
                _appDbContext.Customers.Add(customer);
                await _appDbContext.SaveChangesAsync();
                return Result.Ok("Se ha guardado el customer con exito");
            }
            catch (DbUpdateException ex)
            {
                return Result.Fail($"Error al guardar el cliente: {ex.Message}");
            }
        }
        /// <summary>
        /// Actualiza un cliente existente en la base de datos.
        /// </summary>
        /// <param name="customer">Objeto cliente a actualizar.</param>
        public async Task<Result<string>> ActualizarCustomer(Customer customer)
        {
            if (customer == null)
                return Result.Fail("El cliente no puede ser nulo.");

            if (customer.Id <= 0)
                return Result.Fail("El ID del cliente debe ser válido.");

            if (string.IsNullOrWhiteSpace(customer.Name))
                return Result.Fail("El nombre del cliente es obligatorio.");

            Customer? clienteExistente = await _appDbContext.Customers.FindAsync(customer.Id);
            if (clienteExistente == null)
                return Result.Fail("No se encontró un cliente con el ID especificado.");

            try
            {
                _appDbContext.Entry(clienteExistente).CurrentValues.SetValues(customer);
                await _appDbContext.SaveChangesAsync();
                return Result.Ok("El cliente ha sido actualizado con éxito.");
            }
            catch (DbUpdateException ex)
            {
                return Result.Fail($"Error al actualizar el cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Consulta un cliente por su ID.
        /// </summary>
        /// <param name="id">Identificador único del cliente.</param>
        public async Task<Result<string>> EliminarCustomerPorId(int id)
        {
            if (id <= 0)
                return Result.Fail("El ID debe ser mayor a cero.");

            Customer? customer = await _appDbContext.Customers.FindAsync(id);

            if (customer == null)
                return Result.Fail($"No se encontró un cliente con ID {id}.");

            try
            {
                _appDbContext.Remove(customer);
                await _appDbContext.SaveChangesAsync();
                return Result.Ok("Se ha Elimnado el customer con exito");
            }
            catch (DbUpdateException ex)
            {
                return Result.Fail($"Error al guardar el cliente: {ex.Message}");
            }
        }
    }
}
