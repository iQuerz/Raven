using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    public class IncomeTransaction : Transaction
    {
        public IncomeTransactionType _incomeTransactionType { get; set; }
    }
}
