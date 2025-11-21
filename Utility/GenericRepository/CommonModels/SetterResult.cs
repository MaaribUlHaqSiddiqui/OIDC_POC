using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.CommonModels
{

    public class SetterResult
    {
        public bool Result { get; set; }
        public bool IsException { get; set; }
        public string Message { get; set; }

    }
    public class SetterWithDataResult : SetterResult
    {

        public Object Data { get; set; }


    }
}
