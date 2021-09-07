using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zhao.Backend.Extra
{
    public class AvantDateValidation : RangeAttribute
    {
        public AvantDateValidation()
            : base(typeof(DateTime),
                DateTime.Now.AddYears(-2).ToString(),
                DateTime.Now.ToString())
        {
            ErrorMessage = $"La date doit être entre {DateTime.Now.AddYears(-2).ToShortDateString()} et {DateTime.Now.ToShortDateString()} :(";
        }
    }
}
