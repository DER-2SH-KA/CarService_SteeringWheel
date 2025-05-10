using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService_SteeringWheel.DB
{
    public class SqlHelper
    {
        private static TradeEntities context = new TradeEntities();

        public static TradeEntities GetContext()
        {
            if ( context == null)
            {
                context = new TradeEntities();
            }

            return context;
        }
    }
}
