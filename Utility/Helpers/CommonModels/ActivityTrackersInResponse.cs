using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.CommonModels
{
    public class ActivityTrackersInResponse
    {


        
        public  string? CreatedBy { get; set; }


      
        public  string? UpdatedBy { get; set; }


        
        public  DateTime CreatedDate { get; set; }


       
        public DateTime UpdatedDate { get; set; }
       
        public  bool IsActive { get; set; }
       
        public  bool IsArchived { get; set; }
    }
}
