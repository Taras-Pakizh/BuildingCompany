using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Practice.Models
{
    public static class ModelValidator
    {
        public static EmployeeOrder Validate(this EmployeeOrder order)
        {
            order.orderDescription = order.orderDescription.Trim();
            if (order.orderDescription.Length > 50)
                order.orderDescription = order.orderDescription.Substring(0, 49);
            
            return order;
        }
    }
}