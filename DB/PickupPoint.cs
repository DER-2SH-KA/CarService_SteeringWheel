//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarService_SteeringWheel.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class PickupPoint
    {
        public PickupPoint()
        {
            this.Order = new HashSet<Order>();
        }
    
        public long ID { get; set; }
        public string Address { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
