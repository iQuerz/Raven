using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Types;

namespace App.Models
{
    internal class IncomeTransaction : Transaction
    {
        public IncomeTransactionType _incomeTransactionType { get; set; }
        public string _Description { get; set; }
    }
}
