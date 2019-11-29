using System;

namespace L2C.Budget.BL.Utils
{
    public class NewUserException : Exception
    {
        public NewUserException(string mes) : base(mes) { }
    }
}
