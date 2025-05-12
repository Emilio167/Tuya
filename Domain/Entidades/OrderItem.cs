using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades
{
    /// <summary>
    /// Representa un ítem o producto dentro de una orden.
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Identificador único del ítem.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de la orden a la que pertenece el ítem.
        /// </summary>
        [Required(ErrorMessage = "El campo OrderId es obligatorio.")]
        public int OrderId { get; set; }

        /// <summary>
        /// Nombre o descripción del producto.
        /// </summary>
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad solicitada del producto.
        /// </summary>
        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Quantity { get; set; }

        /// <summary>
        /// Precio unitario del producto.
        /// </summary>
        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Subtotal del ítem (cantidad * precio unitario).
        /// </summary>
        public decimal Subtotal => Quantity * UnitPrice;
    }
}
