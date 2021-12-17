using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    public class HealthTransaction : Transaction
    {
        public HealthTransactionType _HealthTransactionType { get; set; } //note the "using App.Models.Types" up top.

        public override string GetTransactionType()
        {
            return _HealthTransactionType.ToString();
        }
    }
}
