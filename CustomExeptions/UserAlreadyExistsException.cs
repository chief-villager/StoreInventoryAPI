using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeInventoryApi.CustomExeptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base("A user with the same email already exists.")
        {
        }

        public UserAlreadyExistsException(string message) : base(message)
        {
        }

        public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}