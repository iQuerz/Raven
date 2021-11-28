using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    internal class Bills : Transaction
    {
        public BillType _BillType { get; set; }
    }
}
