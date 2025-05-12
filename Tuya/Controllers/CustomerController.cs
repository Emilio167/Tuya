using Domain.Entidades;
using Domain.Interfases;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controlador para gestionar operaciones relacionadas con clientes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController(ICustomerRepository customerRepository) : ControllerBase
{
    private readonly ICustomerRepository customerRepository= customerRepository;  

    /// <summary>
    /// Consulta todos los clientes según criterios opcionales.
    /// </summary>
    /// <param name="id">ID del cliente. Si se proporciona, se filtra por este valor.</param>
    /// <param name="name">Nombre del cliente o parte del mismo. Si se proporciona, se filtra por este valor.</param>
    /// <param name="email">Correo electrónico del cliente o parte del mismo. Si se proporciona, se filtra por este valor.</param>
    /// <returns>Una respuesta HTTP con la lista de clientes que cumplen con los filtros.</returns>
    [HttpGet]
    public async Task<IActionResult> ConsultarTodos(int? id, string? name, string? email)
    {
        var resultado = await customerRepository.ConsultarTodos(id, name, email);

        if (resultado.IsFailed)
            return StatusCode(500, resultado.Errors.First().Message);

        return Ok(resultado.Value);
    }

    /// <summary>
    /// Consulta un cliente específico por su ID.
    /// </summary>
    /// <param name="id">Identificador único del cliente.</param>
    /// <returns>Una respuesta HTTP con los datos del cliente si se encuentra; de lo contrario, NotFound.</returns>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ConsultarPorId(int id)
    {
        var resultado = await customerRepository.ConsultarPorId(id);

        if (resultado.IsFailed)
            return NotFound(resultado.Errors.First().Message);

        return Ok(resultado.Value);
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    /// <param name="customer">Objeto cliente a registrar.</param>
    /// <returns>Una respuesta HTTP con los datos del cliente creado y su ubicación.</returns>
    [HttpPost]
    public async Task<IActionResult> GuardarCustomer([FromBody] Customer customer)
    {
        var resultado = await customerRepository.GuardarCustomer(customer);

        if (resultado.IsFailed)
            return BadRequest(resultado.Errors.First().Message);

        return CreatedAtAction(nameof(ConsultarPorId), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Actualiza la información de un cliente existente.
    /// </summary>
    /// <param name="customer">Objeto cliente con los datos actualizados.</param>
    /// <returns>Una respuesta HTTP con los datos actualizados del cliente.</returns>
    [HttpPut]
    public async Task<IActionResult> ActualizarCustomer([FromBody] Customer customer)
    {
        var resultado = await customerRepository.ActualizarCustomer(customer);

        if (resultado.IsFailed)
            return BadRequest(resultado.Errors.First().Message);

        return CreatedAtAction(nameof(ConsultarPorId), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Elimina un cliente por su ID.
    /// </summary>
    /// <param name="id">Identificador único del cliente a eliminar.</param>
    /// <returns>Una respuesta HTTP indicando el resultado de la operación.</returns>
    [HttpDelete]
    public async Task<IActionResult> EliminarCustomerPorIdConsultarPorId(int id)
    {
        var resultado = await customerRepository.EliminarCustomerPorId(id);

        if (resultado.IsFailed)
            return BadRequest(resultado.Errors.First().Message);

        return Ok(resultado.Value);
    }
}
