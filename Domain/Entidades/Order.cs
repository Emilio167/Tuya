using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades
{
    /// <summary>
    /// Representa una orden realizada por un cliente.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Identificador único de la orden.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Identificador del cliente que realizó la orden.
        /// </summary>
        [Required(ErrorMessage = "El campo CustomerId es obligatorio.")]
        public int CustomerId { get; set; }

        /// <summary>
        /// Fecha y hora en que se creó la orden.
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha estimada de entrega.
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Estado actual de la orden (Pendiente, Enviado, Entregado, Cancelado, etc.).
        /// </summary>
        [Required(ErrorMessage = "El estado de la orden es obligatorio.")]
        [StringLength(50, ErrorMessage = "El estado no puede tener más de 50 caracteres.")]
        public string Status { get; set; } = "Pendiente";

        /// <summary>
        /// Total de la orden en moneda local.
        /// </summary>
        [Required(ErrorMessage = "El monto total es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que cero.")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Observaciones adicionales o notas del pedido.
        /// </summary>
        [StringLength(500, ErrorMessage = "Las notas no pueden tener más de 500 caracteres.")]
        public string? Notes { get; set; }

        /// <summary>
        /// Cliente asociado a la orden.
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// Lista de productos incluidos en la orden.
        /// </summary>
        [MinLength(1, ErrorMessage = "La orden debe contener al menos un ítem.")]
        public List<OrderItem> Items { get; set; } = new();
    }
}
