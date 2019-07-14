using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Models
{
    public class UserBevarageModel
    {
        [Required] public int BeverageType { get; set; }
        [Required] public bool UseOwnMug { get; set; }
        [Required] public int Sugar { get; set; }
    }
}