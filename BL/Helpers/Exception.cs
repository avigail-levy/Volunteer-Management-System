namespace BO;

//[Serializable]
//public class DalDoesNotExistException : Exception
//{
//    public DalDoesNotExistException(string? message) : base(message) { }
//}

[Serializable]
public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message,Exception ex) : base(message, ex) { }
}

[Serializable]
public class BlCantDeleteException : Exception
{
    public BlCantDeleteException(string? message,Exception ex) : base(message,ex) { }
    public BlCantDeleteException(string? message) : base(message) { }

}

[Serializable]
public class BlInvalidValueException : Exception
{
    public BlInvalidValueException(string? message) : base(message) { }
    public BlInvalidValueException(string? message,Exception ex) : base(message,ex) { }

}

