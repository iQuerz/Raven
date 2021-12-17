using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    public  class EntertainmentTransaction : Transaction
    {
        public EntertainmentType _EntertainmentType { get; set; }

        public override string GetTransactionType()
        {
            return _EntertainmentType.ToString();
        }

    }
}
