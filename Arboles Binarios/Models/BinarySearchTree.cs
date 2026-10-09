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


        public OperationResult Add(int id)
        {
            Node newNode = new Node(id);


            if (Head == null)
            {
                Head = newNode;
                return OperationResult.Ok(
                    OperationCase.AddedAsRoot,
                    $"[Caso 1: Arbol Vacio] Se asigno Head = newNode ({id})",
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
                            $"[Caso 2: Menor que {current.Id}] Se inserto {id} en current.Left",
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
                            $"[Caso 3: Mayor que {current.Id}] Se inserto {id} en current.Right",
                            affectedId: id);
                    }
                    current = current.Right;
                }
                else
                {
                    return OperationResult.Fail(
                        OperationCase.DuplicateKey,
                        $"[Caso 4: Clave Duplicada] El Id {id} ya existe en el arbol.",
                        affectedId: id);
                }
            }
        }


        public OperationResult Search(int id)
        {

            if (Head == null)
            {
                return OperationResult.Fail(
                    OperationCase.SearchInEmpty,
                    "[Caso 1: Busqueda] Arbol vacio (Head == null).",
                    affectedId: id);
            }

            Node? current = Head;

            while (current != null)
            {
                if (current.Id == id)
                {
                    return OperationResult.Ok(
                        OperationCase.Found,
                        $"[Caso 2: Busqueda] Encontrado! current.Id == {id}",
                        affectedId: id);
                }

                if (id < current.Id)
                    current = current.Left;
                else
                    current = current.Right;
            }

            // CASO 5: Se llego a null, no existe
            return OperationResult.Fail(
                OperationCase.NotFound,
                $"[Caso 5: Busqueda] El Id {id} no existe en el arbol.",
                affectedId: id);
        }


        public OperationResult Remove(int id)
        {

            OperationResult searchResult = Search(id);
            if (!searchResult.Success)
            {
                return OperationResult.Fail(
                    OperationCase.RemoveNotFound,
                    $"No se puede eliminar: El Id {id} no existe.",
                    affectedId: id);
            }


            string        caseMessage = string.Empty;
            OperationCase caseType    = OperationCase.RemovedLeaf;

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
                    caseMessage = $"[Caso 1: Eliminar Hoja] Se elimino el nodo hoja ({id})";
                    return null;
                }

                if (current.Left == null)
                {
                    caseType    = OperationCase.RemovedOneChild;
                    caseMessage = $"[Caso 2: Eliminar con 1 Hijo] El nodo ({id}) fue reemplazado por su hijo derecho ({current.Right!.Id})";
                    return current.Right;
                }
                if (current.Right == null)
                {
                    caseType    = OperationCase.RemovedOneChild;
                    caseMessage = $"[Caso 2: Eliminar con 1 Hijo] El nodo ({id}) fue reemplazado por su hijo izquierdo ({current.Left!.Id})";
                    return current.Left;
                }

                Node sucesor = current.Right!;
                while (sucesor.Left != null)
                    sucesor = sucesor.Left;

                string        mensajeCaso3 = $"[Caso 3: Eliminar con 2 Hijos] El nodo ({id}) fue reemplazado por su sucesor inorden ({sucesor.Id})";
                OperationCase tipoCaso3    = OperationCase.RemovedTwoChildren;

                current.Id    = sucesor.Id;
                current.Right = RemoveRecursive(current.Right, sucesor.Id, ref caseMessage, ref caseType);

                caseType    = tipoCaso3;
                caseMessage = mensajeCaso3;
            }

            return current;
        }
        // Vacia el arbol por completo
        public OperationResult Clear()
        {
            Head = null;
            return OperationResult.Ok(OperationCase.Cleared, "El arbol fue vaciado correctamente.");
        }
        public string GetInOrder()   => InOrderRecursive(Head).Trim();
        public string GetPreOrder()  => PreOrderRecursive(Head).Trim();
        public string GetPostOrder() => PostOrderRecursive(Head).Trim();

        // InOrden: Izquierda -> Raiz -> Derecha (produce valores ascendentes en un BST)
        private string InOrderRecursive(Node? current)
        {
            if (current == null) return string.Empty;
            return $"{InOrderRecursive(current.Left)}{current.Id} {InOrderRecursive(current.Right)}";
        }

        // PreOrden: Raiz -> Izquierda -> Derecha
        private string PreOrderRecursive(Node? current)
        {
            if (current == null) return string.Empty;
            return $"{current.Id} {PreOrderRecursive(current.Left)}{PreOrderRecursive(current.Right)}";
        }

        // PostOrden: Izquierda -> Derecha -> Raiz
        private string PostOrderRecursive(Node? current)
        {
            if (current == null) return string.Empty;
            return $"{PostOrderRecursive(current.Left)}{PostOrderRecursive(current.Right)}{current.Id} ";
        }

        // Cuenta el total de nodos del arbol
        public int Count() => CountRecursive(Head);

        private int CountRecursive(Node? current)
        {
            if (current == null) return 0;
            return 1 + CountRecursive(current.Left) + CountRecursive(current.Right);
        }

        // Devuelve la altura del arbol (numero de niveles)
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
