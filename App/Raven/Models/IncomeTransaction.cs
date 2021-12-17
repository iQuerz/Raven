using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    public class IncomeTransaction : Transaction
    {
        public IncomeTransactionType _IncomeTransactionType { get; set; }

        public override string GetTransactionType()
        {
            return _IncomeTransactionType.ToString();
        }
    }
}
