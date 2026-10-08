namespace Estructuras_de_Datos_Arboles
{
    public interface ITreeView
    {
        string InputId { get; }
        void SetStatus(string message);
        void SetTraversal(string message);
        void SetInfo(string message);
        void ClearInput();
        void RefreshCanvas();
        event EventHandler AddRequested;
        event EventHandler SearchRequested;
        event EventHandler RemoveRequested;
        event EventHandler ClearRequested;
        event EventHandler InOrderRequested;
        event EventHandler PreOrderRequested;
        event EventHandler PostOrderRequested;
    }
}
