using System.Runtime.Serialization;

namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception ex) : base(message, ex) { }

}
//attribute with null value
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}



[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message, Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlCantDeleteException : Exception
{
    public BlCantDeleteException(string? message, Exception ex) : base(message, ex) { }
    public BlCantDeleteException(string? message) : base(message) { }

}
[Serializable]
public class BlCantUpdateEception : Exception
{
    public BlCantUpdateEception(string? message, Exception ex) : base(message, ex) { }
    public BlCantUpdateEception(string? message) : base(message) { }

}



[Serializable]
public class BlInvalidValueException : Exception
{
    public BlInvalidValueException(string? message) : base(message) { }
    public BlInvalidValueException(string? message, Exception ex) : base(message, ex) { }
}
[Serializable]
public class BlUnauthorizedException:Exception
{
    public BlUnauthorizedException(string? message):base(message) { }
}
//[Serializable]
//public class DalXMLFileLoadCreateException : Exception
//{
//    public DalXMLFileLoadCreateException(string? message) : base(message) { }
//}


