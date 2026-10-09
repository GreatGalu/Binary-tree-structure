namespace Estructuras_de_Datos_Arboles
{
    public enum OperationCase
    {
        AddedAsRoot,      
        AddedLeft,       
        AddedRight, 
        DuplicateKey, 
        SearchInEmpty,    
        Found, 
        NotFound, 
        RemoveNotFound,   
        RemovedLeaf,      
        RemovedOneChild,  
        RemovedTwoChildren, 
        Cleared           
    }
    public sealed class OperationResult
    {
        public bool          Success    { get; }
        public OperationCase Case       { get; }
        public string        Message    { get; }
        public int?          AffectedId { get; }
        public OperationResult(bool success, OperationCase operationCase, string message, int? affectedId = null)
        {
            Success    = success;
            Case       = operationCase;
            Message    = message;
            AffectedId = affectedId;
        }
        public static OperationResult Ok(OperationCase operationCase, string message, int? affectedId = null)
            => new OperationResult(true,  operationCase, message, affectedId);

        public static OperationResult Fail(OperationCase operationCase, string message, int? affectedId = null)
            => new OperationResult(false, operationCase, message, affectedId);
    }
}
