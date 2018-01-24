using System;

namespace Lightrail.Model
{
    public class Error
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string MessageCode { get; set; }
    }
}