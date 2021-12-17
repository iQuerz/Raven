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

        public virtual string GetTransactionType()
        {
            return "";
        }
        
        public virtual int GetTransactionStatus()
        {
            return -1; 
        }
        
        public virtual double GetTransactionPB()
        {
            return -1;
        }

    }
}
