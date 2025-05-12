using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades
{
    /// <summary>
    /// Representa un cliente dentro del sistema.
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Identificador único del cliente.
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
        /// Dirección de correo electrónico del cliente.
        /// </summary>
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro del cliente.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
