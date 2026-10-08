using System;

namespace Estructuras_de_Datos_Arboles
{
    public class BinarySearchTree
    {
        public Node? Head { get; private set; }
        public BinarySearchTree()
        {
            Head = null;
        }
        /// <param name="id">Valor entero a insertar.</param>
        /// <see cref="OperationResult"/> con el caso ocurrido y los ids involucrados.
        public OperationResult Add(int id)
        {
            Node newNode = new Node(id);
            if (Head == null)
            {
                Head = newNode;
                return OperationResult.Ok(
                    OperationCase.AddedAsRoot,
                    $"[Caso 1: Árbol Vacío] Se asignó Head = newNode ({id})",
                    affectedId: id);
            }
            Node current = Head;

            while (true)
            {
                if (newNode.Id < current.Id)
                {
                    if (current.Left == null)
                    {
                        current.Left = newNode;
                        return OperationResult.Ok(
                            OperationCase.AddedLeft,
                            $"[Caso 2: Menor que {current.Id}] Se insertó {id} en current.Left",
                            affectedId: id);
                    }
                    current = current.Left;
                }
                else if (newNode.Id > current.Id)
                {
                    if (current.Right == null)
                    {
                        current.Right = newNode;
                        return OperationResult.Ok(
                            OperationCase.AddedRight,
                            $"[Caso 3: Mayor que {current.Id}] Se insertó {id} en current.Right",
                            affectedId: id);
                    }
                    current = current.Right;
                }
                else
                {
                    return OperationResult.Fail(
                        OperationCase.DuplicateKey,
                        $"[Caso 4: Clave Duplicada] El Id {id} ya existe en el árbol.",
                        affectedId: id);
                }
            }
        }
        /// Busca el nodo con el identificador <paramref name="id"/> en el árbol.
        /// <param name="id">Valor a buscar.</param>
        /// <see cref="OperationResult"/> indicando si fue encontrado y en qué caso.
        public OperationResult Search(int id)
        {
            if (Head == null)
            {
                return OperationResult.Fail(
                    OperationCase.SearchInEmpty,
                    "[Caso 1: Búsqueda] Árbol vacío (Head == null).",
                    affectedId: id);
            }

            Node? current = Head;

            while (current != null)
            {
                if (current.Id == id)
                {
                    return OperationResult.Ok(
                        OperationCase.Found,
                        $"[Caso 2: Búsqueda] ¡Encontrado! current.Id == {id}",
                        affectedId: id);
                }
                if (id < current.Id)
                    current = current.Left;
                else
                    current = current.Right;
            }
            return OperationResult.Fail(
                OperationCase.NotFound,
                $"[Caso 5: Búsqueda] El Id {id} no existe en el árbol.",
                affectedId: id);
        }
        /// Elimina el nodo con el identificador <paramref name="id"/> del árbol.
        /// <param name="id">Valor a eliminar.</param>
        /// <see cref="OperationResult"/> con el caso de eliminación ejecutado.
        public OperationResult Remove(int id)
        {
            OperationResult searchResult = Search(id);
            if (!searchResult.Success)
            {
                return OperationResult.Fail(
                    OperationCase.RemoveNotFound,
                    $"No se puede eliminar: El Id {id} no existe en el árbol.",
                    affectedId: id);
            }
            string caseMessage = string.Empty;
            OperationCase caseType = OperationCase.RemovedLeaf;

            Head = RemoveRecursive(Head, id, ref caseMessage, ref caseType);

            return OperationResult.Ok(caseType, caseMessage, affectedId: id);
        }
        private Node? RemoveRecursive(Node? current, int id, ref string caseMessage, ref OperationCase caseType)
        {
            if (current == null) return null;
            if (id < current.Id)
            {
                current.Left = RemoveRecursive(current.Left, id, ref caseMessage, ref caseType);
            }
            else if (id > current.Id)
            {
                current.Right = RemoveRecursive(current.Right, id, ref caseMessage, ref caseType);
            }
            else
            {
                if (current.Left == null && current.Right == null)
                {
                    caseType    = OperationCase.RemovedLeaf;
                    caseMessage = $"[Caso 1: Eliminar Hoja] Se eliminó el nodo hoja ({id})";
                    return null; 
                }
                if (current.Left == null)
                {
                    caseType    = OperationCase.RemovedOneChild;
                    caseMessage = $"[Caso 2: Eliminar 1 Hijo] El nodo ({id}) fue reemplazado por su hijo derecho ({current.Right!.Id})";
                    return current.Right;
                }
                if (current.Right == null)
                {
                    caseType    = OperationCase.RemovedOneChild;
                    caseMessage = $"[Caso 2: Eliminar 1 Hijo] El nodo ({id}) fue reemplazado por su hijo izquierdo ({current.Left!.Id})";
                    return current.Left;
                }
                Node sucesor = current.Right!;
                while (sucesor.Left != null)
                    sucesor = sucesor.Left;

                caseType    = OperationCase.RemovedTwoChildren;
                caseMessage = $"[Caso 3: Eliminar 2 Hijos] El nodo ({id}) fue reemplazado por su sucesor inorden ({sucesor.Id})";
                current.Id    = sucesor.Id;
                current.Right = RemoveRecursive(current.Right, sucesor.Id, ref caseMessage, ref caseType);
                caseType    = OperationCase.RemovedTwoChildren;
                caseMessage = $"[Caso 3: Eliminar 2 Hijos] El nodo ({id}) fue reemplazado por su sucesor inorden ({sucesor.Id})";
            }

            return current;
        }
        /// <returns><see cref="OperationResult"/> indicando que el árbol fue vaciado.</returns>
        public OperationResult Clear()
        {
            Head = null;
            return OperationResult.Ok(
                OperationCase.Cleared,
                "El árbol fue vaciado correctamente.");
        }
        public string GetInOrder()    => InOrderRecursive(Head).Trim();
        public string GetPreOrder()   => PreOrderRecursive(Head).Trim();
        public string GetPostOrder()  => PostOrderRecursive(Head).Trim();

        private string InOrderRecursive(Node? current)
        {
            if (current == null) return string.Empty;
            return $"{InOrderRecursive(current.Left)}{current.Id} {InOrderRecursive(current.Right)}";
        }

        private string PreOrderRecursive(Node? current)
        {
            if (current == null) return string.Empty;
            return $"{current.Id} {PreOrderRecursive(current.Left)}{PreOrderRecursive(current.Right)}";
        }

        private string PostOrderRecursive(Node? current)
        {
            if (current == null) return string.Empty;
            return $"{PostOrderRecursive(current.Left)}{PostOrderRecursive(current.Right)}{current.Id} ";
        }
        public int Count() => CountRecursive(Head);

        private int CountRecursive(Node? current)
        {
            if (current == null) return 0;
            return 1 + CountRecursive(current.Left) + CountRecursive(current.Right);
        }
        public int GetHeight() => HeightRecursive(Head);

        private int HeightRecursive(Node? current)
        {
            if (current == null) return 0;
            int hLeft  = HeightRecursive(current.Left);
            int hRight = HeightRecursive(current.Right);
            return Math.Max(hLeft, hRight) + 1;
        }
    }
}
