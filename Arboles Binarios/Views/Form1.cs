using System;
using System.Drawing;
using System.Windows.Forms;

namespace Estructuras_de_Datos_Arboles
{
    public partial class Form1 : Form, ITreeView
    {
        private readonly TreePresenter  _presenter;
        private readonly TreeRenderer   _renderer;
        private Label               lblId        = null!;
        private TextBox             txtId        = null!;
        private Button              btnAdd       = null!;
        private Button              btnSearch    = null!;
        private Button              btnRemove    = null!;
        private Button              btnClear     = null!;
        private Button              btnInOrder   = null!;
        private Button              btnPreOrder  = null!;
        private Button              btnPostOrder = null!;
        private Label               lblCaso      = null!;
        private Label               lblRecorrido = null!;
        private Label               lblInfo      = null!;
        private DoubleBufferedPanel panelLienzo  = null!;
        public Form1()
        {
            InitializeComponent();
            BuildUI();
            var model  = new BinarySearchTree();
            _renderer  = new TreeRenderer(this.Font);
            _presenter = new TreePresenter(this, model);
            SetStatus("Estado: Arbol vacio.");
            SetTraversal("Recorrido: Ninguno");
            SetInfo("Nodos: 0 | Altura: 0");
        }
        public string InputId => txtId.Text;
        public void SetStatus(string message)    => lblCaso.Text      = message;
        public void SetTraversal(string message) => lblRecorrido.Text = message;
        public void SetInfo(string message)      => lblInfo.Text      = message;
        public void ClearInput()
        {
            txtId.Clear();
            txtId.Focus();
        }
        public void RefreshCanvas() => panelLienzo.Invalidate();
        public event EventHandler AddRequested       = delegate { };
        public event EventHandler SearchRequested    = delegate { };
        public event EventHandler RemoveRequested    = delegate { };
        public event EventHandler ClearRequested     = delegate { };
        public event EventHandler InOrderRequested   = delegate { };
        public event EventHandler PreOrderRequested  = delegate { };
        public event EventHandler PostOrderRequested = delegate { };
        private void PanelLienzo_Paint(object? sender, PaintEventArgs e)
        {
            _renderer.Draw(
                g:             e.Graphics,
                root:          _presenter.Root,
                highlightedId: _presenter.HighlightedNodeId,
                canvasBounds:  panelLienzo.ClientRectangle);
        }
        private void BuildUI()
        {
            this.Text          = "Arbol Binario de Busqueda";
            this.Size          = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            lblId = new Label { Text = "Id:", Location = new Point(20, 20), AutoSize = true };
            txtId = new TextBox { Location = new Point(50, 17), Width = 70 };
            txtId.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddRequested(s, e);
                    e.SuppressKeyPress = true;
                }
            };
            btnAdd    = CreateButton("Agregar",  135, () => AddRequested(this,    EventArgs.Empty));
            btnSearch = CreateButton("Buscar",   220, () => SearchRequested(this, EventArgs.Empty));
            btnRemove = CreateButton("Eliminar", 305, () => RemoveRequested(this, EventArgs.Empty));
            btnClear  = CreateButton("Limpiar",  390, () => ClearRequested(this,  EventArgs.Empty));
            btnInOrder   = CreateButton("InOrden",   500, () => InOrderRequested(this,   EventArgs.Empty));
            btnPreOrder  = CreateButton("PreOrden",  585, () => PreOrderRequested(this,  EventArgs.Empty));
            btnPostOrder = CreateButton("PostOrden", 670, () => PostOrderRequested(this, EventArgs.Empty));
            lblCaso      = new Label { Text = string.Empty, Location = new Point(20,  55), AutoSize = true };
            lblRecorrido = new Label { Text = string.Empty, Location = new Point(400, 55), AutoSize = true };
            lblInfo      = new Label { Text = string.Empty, Location = new Point(800, 55), AutoSize = true };
            panelLienzo = new DoubleBufferedPanel
            {
                Location    = new Point(20, 85),
                Size        = new Size(945, 500),
                Anchor      = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor   = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelLienzo.Paint += PanelLienzo_Paint;
            this.Controls.AddRange(new Control[]
            {
                lblId, txtId,
                btnAdd, btnSearch, btnRemove, btnClear,
                btnInOrder, btnPreOrder, btnPostOrder,
                lblCaso, lblRecorrido, lblInfo,
                panelLienzo
            });
        }
        private static Button CreateButton(string text, int x, Action onClick)
        {
            var btn = new Button
            {
                Text                    = text,
                Location                = new Point(x, 15),
                Width                   = 75,
                Height                  = 27,
                UseVisualStyleBackColor = true
            };
            btn.Click += (s, e) => onClick();
            return btn;
        }
    }
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint            |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();
        }
    }
}
