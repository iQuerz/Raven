using System;
using System.Collections.Generic;
using System.Text;
using App.Models.Base;
using App.Models.Types;

namespace App.Models
{
    public class BeautyAndFashionTransaction : Transaction 
    {
        public BeautyAndFashionType _BeautyAndFashionType { get; set; }

        public override string GetTransactionType()
        {
            return _BeautyAndFashionType.ToString();
        }
    }
}
