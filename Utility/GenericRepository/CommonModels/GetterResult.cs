
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericRepository.CommonModels
{
    public class GetterResult<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; } = String.Empty;
        public T Data { get; set; }
    }
    public class GetterResultPaginated<T> : GetterResult<T>
    {
        public PaginatedResponse Page {  get; set; }   
    }

}
