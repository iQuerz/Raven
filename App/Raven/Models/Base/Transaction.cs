using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models.Base
{
    public class Transaction
    {
        public int _ID { get; set; }
        public double _Value { get; set; }
        public DateTime _Date { get; set; }
        public string _Description { get; set; }
    }
}
