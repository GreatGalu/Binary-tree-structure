namespace Estructuras_de_Datos_Arboles
{
    public class TreePresenter
    {
        private readonly ITreeView        _view;
        private readonly BinarySearchTree _model;
        public int? HighlightedNodeId { get; private set; }
        public Node? Root => _model.Head;
        public TreePresenter(ITreeView view, BinarySearchTree model)
        {
            _view  = view;
            _model = model;
            _view.AddRequested       += HandleAdd;
            _view.SearchRequested    += HandleSearch;
            _view.RemoveRequested    += HandleRemove;
            _view.ClearRequested     += HandleClear;
            _view.InOrderRequested   += HandleInOrder;
            _view.PreOrderRequested  += HandlePreOrder;
            _view.PostOrderRequested += HandlePostOrder;
        }
        private void HandleAdd(object? sender, EventArgs e)
        {
            if (!TryParseId(out int id)) return;

            OperationResult result = _model.Add(id);

            HighlightedNodeId = result.Success ? id : (int?)null;

            _view.SetStatus($"Estado: {result.Message}");
            UpdateInfo();
            _view.ClearInput();
            _view.RefreshCanvas();
        }
        private void HandleSearch(object? sender, EventArgs e)
        {
            if (!TryParseId(out int id)) return;
            OperationResult result = _model.Search(id);
            HighlightedNodeId = result.Success ? id : (int?)null;
            _view.SetStatus($"Estado: {result.Message}");
            _view.RefreshCanvas();
        }
        private void HandleRemove(object? sender, EventArgs e)
        {
            if (!TryParseId(out int id)) return;
            OperationResult result = _model.Remove(id);
            HighlightedNodeId = null;
            _view.SetStatus($"Estado: {result.Message}");
            UpdateInfo();
            _view.ClearInput();
            _view.RefreshCanvas();
        }
        private void HandleClear(object? sender, EventArgs e)
        {
            _model.Clear();
            HighlightedNodeId = null;
            _view.SetStatus("Estado: Arbol vaciado.");
            _view.SetTraversal("Recorrido: Ninguno");
            UpdateInfo();
            _view.RefreshCanvas();
        }
        private void HandleInOrder(object? sender, EventArgs e)
            => _view.SetTraversal($"Recorrido InOrden: [ {_model.GetInOrder()} ]");
        private void HandlePreOrder(object? sender, EventArgs e)
            => _view.SetTraversal($"Recorrido PreOrden: [ {_model.GetPreOrder()} ]");
        private void HandlePostOrder(object? sender, EventArgs e)
            => _view.SetTraversal($"Recorrido PostOrden: [ {_model.GetPostOrder()} ]");
        private bool TryParseId(out int id)
        {
            if (int.TryParse(_view.InputId, out id))
                return true;

            _view.SetStatus("Ingresa un numero entero valido.");
            id = 0;
            return false;
        }
        private void UpdateInfo()
            => _view.SetInfo($"Nodos: {_model.Count()} | Altura: {_model.GetHeight()}");
    }
}
