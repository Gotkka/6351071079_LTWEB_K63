using System;
using System.Linq;

namespace ThucHanh_22112024.Models
{
    public class Cart
    {
        QLBanXeGanMayEntities qLBanXeGanMayEntities = new QLBanXeGanMayEntities();

        public int motorcycleId { get; set; }
        public int quantity { get; set; }
        public XEGANMAY Motorcycle { get; set; }

        // Adding extra properties for Motorcycle information
        public string motorcycleName { get; set; }
        public string motorcyclePicture { get; set; }
        public double price { get; set; }

        public double totalMoney
        {
            get { return Convert.ToDouble(Motorcycle.Giaban ?? 0) * quantity; }
        }

        public Cart(int id)
        {
            motorcycleId = id;
            Motorcycle = qLBanXeGanMayEntities.XEGANMAYs.Single(n => n.MaXe == motorcycleId);
            quantity = 1;

            // Initialize additional information about the motorcycle
            motorcycleName = Motorcycle.TenXe; // Assuming you have the `TenXe` property in your model
            motorcyclePicture = Motorcycle.Anhbia; // Assuming you have the `Anhbia` property in your model
            price = Convert.ToDouble(Motorcycle.Giaban ?? 0); // Store the price
        }
    }
}
