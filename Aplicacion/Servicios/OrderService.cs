using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entidades;

namespace Aplicacion.Servicios
{
    public class OrderService
    {
        public Order CreateOrder(Customer customer)
        {
            return new Order
            {
                Customer = customer,
                CustomerId = customer.Id,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void CancelOrder(Order order)
        {
            // lógica para cancelar una orden (marcar como cancelada si tuviera estado)
        }
    }
}
