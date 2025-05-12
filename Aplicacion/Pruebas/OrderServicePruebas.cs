using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aplicacion.Servicios;
using Domain.Entidades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace Aplicacion.Pruebas
{
    public class OrderServicePruebas
    {
        [Fact]
        public void CreateOrder_DeberiaRetornarOrdenConClienteYFecha()
        {
            var service = new OrderService();
            var customer = new Customer { Id = 1, Name = "Juan", Email = "juan@test.com" };

            var order = service.CreateOrder(customer);

            Assert.Equals(customer.Id, order.CustomerId);
            Assert.IsTrue(order.CreatedAt <= DateTime.UtcNow);
        }
    }
}

