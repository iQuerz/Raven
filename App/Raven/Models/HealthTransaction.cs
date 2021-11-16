using App.Models.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Models
{
    public class HealthTransaction : Transaction
    {
        public HealthTransactionType _HealthTransactionType { get; set; } //note the "using App.Models.Types" up top.
    }
}
