using System;
using System.Drawing;
using System.Windows.Forms;

namespace Estructuras_de_Datos_Arboles
{
    // =========================================================================
    // FORM1 - Vista Pasiva (Passive View) del patron MVP.
    //
    // Principios SOLID aplicados:
    //   SRP - Form1 UNICAMENTE:
    //           * Construye y dispone los controles de WinForms.
    //           * Implementa ITreeView: expone InputId, eventos y metodos de UI.
    //           * Delega el dibujo al TreeRenderer.
    //         NO valida entradas, NO contiene logica condicional de negocio,
    //         NO llama directamente a BinarySearchTree.
    //   DIP - Se comunica con el arbol SOLO a traves del TreePresenter.
    //
    // Responsabilidades de Form1:
    //   1. Instanciar modelo, renderer y presenter (punto de composicion).
    //   2. Implementar ITreeView: propiedades, eventos y metodos de UI.
    //   3. En PanelLienzo_Paint: delegar al TreeRenderer con los datos del Presenter.
    //   4. Reenviar las acciones del usuario (clics, Enter) como eventos de ITreeView.
    // =========================================================================
    public partial class Form1 : Form, ITreeView
    {
        // -- Colaboradores (instanciados en el constructor) ---------------------
        private readonly TreePresenter _presenter;
        private readonly TreeRenderer  _renderer;

        // -- Controles de la UI ------------------------------------------------
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

        // =====================================================================
        // CONSTRUCTOR - Punto de composicion (Composition Root)
        // =====================================================================

        public Form1()
        {
            InitializeComponent();
            BuildUI();

            // Composicion: el modelo y el renderer se crean aqui; el presenter
            // recibe "this" como ITreeView, cumpliendo DIP.
            var model = new BinarySearchTree();
            _renderer  = new TreeRenderer(this.Font);
            _presenter = new TreePresenter(this, model);

            // Estado inicial de la UI.
            SetStatus("Estado: Arbol vacio.");
            SetTraversal("Recorrido: Ninguno");
            SetInfo("Nodos: 0 | Altura: 0");
        }

        // =====================================================================
        // IMPLEMENTACION DE ITREEVIEW
        // =====================================================================

        // -- Propiedad de entrada ----------------------------------------------

        /// <summary>Devuelve el texto del campo Id sin parsear.</summary>
        public string InputId => txtId.Text;

        // -- Metodos de salida -------------------------------------------------

        /// <inheritdoc/>
        public void SetStatus(string message)    => lblCaso.Text      = message;

        /// <inheritdoc/>
        public void SetTraversal(string message) => lblRecorrido.Text = message;

        /// <inheritdoc/>
        public void SetInfo(string message)      => lblInfo.Text      = message;

        /// <inheritdoc/>
        public void ClearInput()
        {
            txtId.Clear();
            txtId.Focus();
        }

        /// <inheritdoc/>
        public void RefreshCanvas() => panelLienzo.Invalidate();

        // -- Eventos de entrada ------------------------------------------------

        /// <inheritdoc/>
        public event EventHandler AddRequested       = delegate { };

        /// <inheritdoc/>
        public event EventHandler SearchRequested    = delegate { };

        /// <inheritdoc/>
        public event EventHandler RemoveRequested    = delegate { };

        /// <inheritdoc/>
        public event EventHandler ClearRequested     = delegate { };

        /// <inheritdoc/>
        public event EventHandler InOrderRequested   = delegate { };

        /// <inheritdoc/>
        public event EventHandler PreOrderRequested  = delegate { };

        /// <inheritdoc/>
        public event EventHandler PostOrderRequested = delegate { };

        // =====================================================================
        // EVENTO PAINT - Delega todo el dibujo al TreeRenderer
        // =====================================================================

        /// <summary>
        /// La View pinta; el Presenter provee los datos; el Renderer hace el trabajo.
        /// Form1 no tiene ningun algoritmo de dibujo propio.
        /// </summary>
        private void PanelLienzo_Paint(object? sender, PaintEventArgs e)
        {
            _renderer.Draw(
                g:             e.Graphics,
                root:          _presenter.Root,
                highlightedId: _presenter.HighlightedNodeId,
                canvasBounds:  panelLienzo.ClientRectangle);
        }

        // =====================================================================
        // CONSTRUCCION DE CONTROLES (BuildUI)
        // La View solo crea controles y dispara eventos; NUNCA ejecuta logica.
        // =====================================================================

        private void BuildUI()
        {
            this.Text          = "Arbol Binario de Busqueda";
            this.Size          = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // -- Etiqueta e input de Id ----------------------------------------
            lblId = new Label { Text = "Id:", Location = new Point(20, 20), AutoSize = true };

            txtId = new TextBox { Location = new Point(50, 17), Width = 70 };

            // Enter en el textbox dispara el evento AddRequested.
            txtId.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddRequested(s, e);
                    e.SuppressKeyPress = true;
                }
            };

            // -- Botones de operacion ------------------------------------------
            btnAdd    = CreateButton("Agregar",  135, () => AddRequested(this,    EventArgs.Empty));
            btnSearch = CreateButton("Buscar",   220, () => SearchRequested(this, EventArgs.Empty));
            btnRemove = CreateButton("Eliminar", 305, () => RemoveRequested(this, EventArgs.Empty));
            btnClear  = CreateButton("Limpiar",  390, () => ClearRequested(this,  EventArgs.Empty));

            // -- Botones de recorrido ------------------------------------------
            btnInOrder   = CreateButton("InOrden",   500, () => InOrderRequested(this,   EventArgs.Empty));
            btnPreOrder  = CreateButton("PreOrden",  585, () => PreOrderRequested(this,  EventArgs.Empty));
            btnPostOrder = CreateButton("PostOrden", 670, () => PostOrderRequested(this, EventArgs.Empty));

            // -- Etiquetas de estado -------------------------------------------
            lblCaso      = new Label { Text = string.Empty, Location = new Point(20,  55), AutoSize = true };
            lblRecorrido = new Label { Text = string.Empty, Location = new Point(400, 55), AutoSize = true };
            lblInfo      = new Label { Text = string.Empty, Location = new Point(800, 55), AutoSize = true };

            // -- Panel de dibujo con doble buffer ------------------------------
            panelLienzo = new DoubleBufferedPanel
            {
                Location    = new Point(20, 85),
                Size        = new Size(945, 500),
                Anchor      = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor   = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelLienzo.Paint += PanelLienzo_Paint;

            // -- Agregar controles al formulario -------------------------------
            this.Controls.AddRange(new Control[]
            {
                lblId, txtId,
                btnAdd, btnSearch, btnRemove, btnClear,
                btnInOrder, btnPreOrder, btnPostOrder,
                lblCaso, lblRecorrido, lblInfo,
                panelLienzo
            });
        }

        /// <summary>Helper: crea un boton estandar con posicion y accion definidas.</summary>
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

    // =========================================================================
    // DOUBLEBUFFEREDPANEL - Panel con doble buffer para evitar parpadeo GDI+.
    // =========================================================================
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
