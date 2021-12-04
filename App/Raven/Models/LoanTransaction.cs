using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;

namespace App.Models
{
    public class LoanTransaction : Transaction
    {
        public bool _Status { get; set; }
        public double _PayBack { get; set; }
    }
}
