using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;
using FluentResults;

namespace Domain.Interfases
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Consulta todos los clientes registrados en el sistema, con opción de aplicar filtros por identificador, nombre o correo electrónico.
        /// </summary>
        /// <param name="id">Identificador único del cliente. Si se proporciona, se filtrarán los clientes por este ID.</param>
        /// <param name="name">Nombre o parte del nombre del cliente. Si se proporciona, se filtrarán los clientes cuyo nombre contenga este valor.</param>
        /// <param name="email">Correo electrónico o parte del correo del cliente. Si se proporciona, se filtrarán los clientes cuyo correo contenga este valor.</param>
        /// <returns>Una lista de clientes que cumplen con los criterios de búsqueda especificados.</returns>
        Task<Result<IEnumerable<Customer>>> ConsultarTodos(int? id, string? name, string? email);
        /// <summary>
        /// Consulta un cliente por su ID.
        /// </summary>
        /// <param name="id">Identificador único del cliente.</param>
        Task<Result<Customer>> ConsultarPorId(int id);
        /// <summary>
        /// Guarda un nuevo cliente en la base de datos.
        /// </summary>
        /// <param name="customer">Objeto cliente a guardar.</param>
        Task<Result<string>> GuardarCustomer(Customer customer);
        /// <summary>
        /// Actualiza un cliente existente en la base de datos.
        /// </summary>
        /// <param name="customer">Objeto cliente a actualizar.</param>
        Task<Result<string>> ActualizarCustomer(Customer customer);
        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        /// <param name="id">Identificador único del cliente.</param>
        Task<Result<string>> EliminarCustomerPorId(int id);
    }
}
