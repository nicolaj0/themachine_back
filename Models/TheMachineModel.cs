using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
    public class TheMachineModel
    {
        [Required] [StringLength(100)] public string Name { get; set; }
        [Required] public string IpAddress { get; set; }
        public bool IsInUse { get; set; }
    }
}