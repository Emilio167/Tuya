using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades
{
    /// <summary>
    /// Representa un cliente dentro del sistema.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Identificador �nico del cliente.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Direcci�n de correo electr�nico del cliente.
        /// </summary>
        [Required(ErrorMessage = "El correo electr�nico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electr�nico no tiene un formato v�lido.")]
        [StringLength(150, ErrorMessage = "El correo electr�nico no puede superar los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro del cliente.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
