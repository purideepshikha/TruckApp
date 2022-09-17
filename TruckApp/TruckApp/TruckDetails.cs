using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TruckApp
{
    public class TruckDetails
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MakerName { get; set; }
        public string TruckId { get; set; }
        public string PurchaseDate { get; set; }

        public string TruckImage { get; set; }
        public bool IsAvailable { get; set; }

        public string EditorDelete { get; set; }
    }
}
