using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService
{
    internal class Service
    {
        private string _orderId;
        private double _weight;
        private string _district;
        private DateTime _deliveryTime;

        public string OrderId { get { return _orderId; } }
        public double Weight { get { return _weight; } }
        public string Disctrict { get { return _district; } }
        public DateTime DeliveryTime { get { return _deliveryTime; } }


        public Service (string orderId, double weight, string district, DateTime deliveryTime)
        {
            _orderId = orderId;
            _weight = weight;
            _district = district;
            _deliveryTime = deliveryTime;
        }

        public override string ToString()
        {
            return $"Номер заказа - {_orderId}, Вес - {_weight}, Район - {_district}, Время заказа - {_deliveryTime}";
        }
    }
}
