namespace Estructuras_de_Datos_Arboles
{
    public class Node
    {
        public int Id;
        public Node? Left;
        public Node? Right;

        /// <param name="id">Valor entero que identifica al nodo.</param>
        public Node(int id)
        {
            Id    = id;
            Left  = null;
            Right = null;
        }
    }
}
