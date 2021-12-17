using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    public class Bills : Transaction
    {
        public BillType _BillType { get; set; }

        public override string GetTransactionType()
        {
            return _BillType.ToString();
        }
    }
}
