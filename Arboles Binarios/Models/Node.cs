namespace Estructuras_de_Datos_Arboles
{
    public class Node
    {
        public int Id;     
        public Node? Left;
        public Node? Right;

        public Node(int id)
        {
            Id    = id;
            Left  = null;
            Right = null;
        }
    }
}
