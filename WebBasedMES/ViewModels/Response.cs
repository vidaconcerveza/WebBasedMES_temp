using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebBasedMES.ViewModels
{

    public class Response<T> 
    {
       // public T mData;
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class NullOrEmpty
    {

    }

    
}
