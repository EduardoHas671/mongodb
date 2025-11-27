using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotasApp
{
    public partial class MainForm : Form
    {
        private readonly MongoDBServices _mongoDBServices;
        private List<Nota> _notas;
        private Nota _notaSeleccionada;

        // Controles principales
        private TableLayoutPanel mainLayout;
        private Panel editorPanel, listPanel, headerPanel;
        private FlowLayoutPanel notesFlowPanel;
        private TextBox txtTitulo, txtContenido, txtBuscar;
        private Button btnNueva, btnGuardar, btnEliminar, btnBuscar;
        private Label lblTituloApp, lblContador;

        // Nueva paleta de colores
        private readonly Color primaryColor = Color.FromArgb(46, 125, 50);  // Verde oscuro
        private readonly Color secondaryColor = Color.FromArgb(121, 85, 72); // Marrón
        private readonly Color accentColor = Color.FromArgb(69, 90, 100);   // Gris azulado
        private readonly Color backgroundColor = Color.FromArgb(250, 250, 250);
        private readonly Color panelColor = Color.White;
        private readonly Color textColor = Color.FromArgb(33, 33, 33);
        private readonly Color lightTextColor = Color.FromArgb(117, 117, 117);
        private readonly Color borderColor = Color.FromArgb(224, 224, 224);

        public MainForm()
        {
            InitializeComponent();
            _mongoDBServices = new MongoDBServices();
            _notas = new List<Nota>();
            _notaSeleccionada = null;
            InitializeUI();
            this.Load += async (s, e) => await CargarNotas();
            this.Resize += MainForm_Resize;
        }

        private void InitializeUI()
        {
            this.Text = "Notas";
            this.Size = new Size(1300, 800);
            this.MinimumSize = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 10);

            // Layout principal usando TableLayoutPanel para mejor control
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = backgroundColor,
                ColumnCount = 1,
                RowCount = 2,
                RowStyles = {
                    new RowStyle(SizeType.Absolute, 80f), // Header
                    new RowStyle(SizeType.Percent, 100f)  // Content
                },
                Padding = new Padding(20),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            this.Controls.Add(mainLayout);

            CreateHeader();
            CreateContentArea();
        }

        private void CreateHeader()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = panelColor,
                Margin = new Padding(0, 0, 0, 10)
            };
            mainLayout.Controls.Add(headerPanel, 0, 0);

            lblTituloApp = new Label
            {
                Text = "Gestor de Notas",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.MediumPurple,
                Size = new Size(250, 40),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };
            headerPanel.Controls.Add(lblTituloApp);

            lblContador = new Label
            {
                Text = "0 notas",
                Font = new Font("Segoe UI", 11),
                ForeColor = lightTextColor,
                Size = new Size(120, 30),
                Location = new Point(headerPanel.Width - 130, 25),
                TextAlign = ContentAlignment.MiddleRight
            };
            lblContador.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            headerPanel.Controls.Add(lblContador);
        }

        private void CreateContentArea()
        {
            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent
            };
            mainLayout.Controls.Add(contentPanel, 0, 1);

            // Usar TableLayoutPanel para dividir editor y lista
            var contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 40f), // Editor
                    new ColumnStyle(SizeType.Percent, 60f)  // Lista
                },
                Margin = new Padding(0)
            };
            contentPanel.Controls.Add(contentLayout);

            CreateEditorPanel(contentLayout);
            CreateListPanel(contentLayout);
        }

        private void CreateEditorPanel(TableLayoutPanel parent)
        {
            editorPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = panelColor,
                Margin = new Padding(0, 0, 10, 0),
                Padding = new Padding(25)
            };
            parent.Controls.Add(editorPanel, 0, 0);

            // Título del editor
            var editorTitle = new Label
            {
                Text = "Editor",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = textColor,
                Size = new Size(200, 40),
                Location = new Point(0, 0)
            };
            editorPanel.Controls.Add(editorTitle);

            int yPos = 50;

            // Campo título
            var lblTitulo = new Label
            {
                Text = "Titulo",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = textColor,
                Size = new Size(300, 25),
                Location = new Point(0, yPos)
            };
            editorPanel.Controls.Add(lblTitulo);

            yPos += 25;

            txtTitulo = new TextBox
            {
                PlaceholderText = "Escribe el titulo de la nota...",
                Size = new Size(300, 35),
                Location = new Point(0, yPos),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            editorPanel.Controls.Add(txtTitulo);

            yPos += 50;

            // Campo contenido
            var lblContenido = new Label
            {
                Text = "Contenido",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = textColor,
                Size = new Size(300, 25),
                Location = new Point(0, yPos)
            };
            editorPanel.Controls.Add(lblContenido);

            yPos += 25;

            txtContenido = new TextBox
            {
                PlaceholderText = "Aqui escribe...",
                Size = new Size(300, 150),
                Location = new Point(0, yPos),
                Multiline = true,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                ScrollBars = ScrollBars.Vertical
            };
            editorPanel.Controls.Add(txtContenido);

            yPos += 170;

            // Panel de botones en la parte inferior
            var buttonsPanel = new Panel
            {
                Size = new Size(300, 50),
                Location = new Point(0, yPos),
                BackColor = Color.Transparent
            };
            editorPanel.Controls.Add(buttonsPanel);

            btnNueva = CreateModernButton("Nueva", Color.FromArgb(189, 189, 189), textColor, new Point(0, 0));
            btnGuardar = CreateModernButton("Guardar", Color.DeepSkyBlue, Color.White, new Point(105, 0));
            btnEliminar = CreateModernButton("Eliminar", Color.DarkRed, Color.White, new Point(210, 0));

            buttonsPanel.Controls.AddRange(new Control[] { btnNueva, btnGuardar, btnEliminar });

            // Eventos
            btnNueva.Click += (s, e) => LimpiarEditor();
            btnGuardar.Click += btnGuardar_Click;
            btnEliminar.Click += btnEliminar_Click;
        }

        private void CreateListPanel(TableLayoutPanel parent)
        {
            listPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = backgroundColor,
                Padding = new Padding(10, 0, 0, 0)
            };
            parent.Controls.Add(listPanel, 1, 0);

            // Barra de búsqueda
            var searchPanel = new Panel
            {
                Size = new Size(listPanel.Width - 20, 50),
                Location = new Point(0, 0),
                BackColor = Color.Transparent
            };
            listPanel.Controls.Add(searchPanel);

            txtBuscar = new TextBox
            {
                PlaceholderText = "Buscar en notas...",
                Size = new Size(350, 35),
                Location = new Point(0, 5),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            searchPanel.Controls.Add(txtBuscar);

            btnBuscar = CreateModernButton("Buscar", accentColor, Color.White, new Point(360, 5));
            btnBuscar.Size = new Size(80, 35);
            btnBuscar.Click += (s, e) => BuscarNotas();
            searchPanel.Controls.Add(btnBuscar);

            // Panel de notas con scroll
            notesFlowPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Size = new Size(listPanel.Width - 20, listPanel.Height - 60),
                Location = new Point(0, 55),
                AutoSize = false,
                Margin = new Padding(0)
            };
            listPanel.Controls.Add(notesFlowPanel);
        }

        private Button CreateModernButton(string text, Color backColor, Color foreColor, Point location)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(95, 35),
                Location = location,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.FlatAppearance.BorderSize = 0;

            // Efecto hover
            button.MouseEnter += (s, e) => button.BackColor = ControlPaint.Light(backColor, 0.2f);
            button.MouseLeave += (s, e) => button.BackColor = backColor;

            return button;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Ajustar controles cuando cambia el tamaño
            if (listPanel != null && notesFlowPanel != null)
            {
                notesFlowPanel.Size = new Size(listPanel.Width - 20, listPanel.Height - 60);
                
                // Recalcular el layout de las notas
                if (_notas?.Count > 0)
                {
                    MostrarNotas(_notas);
                }
            }

            // Ajustar posición del contador
            if (lblContador != null && headerPanel != null)
            {
                lblContador.Location = new Point(headerPanel.Width - 130, 25);
            }
        }

        private async Task CargarNotas()
        {
            try
            {
                _notas = await _mongoDBServices.GetNotasAsync();
                MostrarNotas(_notas);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar notas: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarNotas(List<Nota> notas)
        {
            notesFlowPanel.SuspendLayout();
            notesFlowPanel.Controls.Clear();

            if (!notas.Any())
            {
                var emptyLabel = new Label
                {
                    Text = "No hay notas disponibles\nComienza creando una nueva nota",
                    Size = new Size(400, 80),
                    Font = new Font("Segoe UI", 12),
                    ForeColor = lightTextColor,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                notesFlowPanel.Controls.Add(emptyLabel);
                lblContador.Text = "0 notas";
                notesFlowPanel.ResumeLayout();
                return;
            }

            // Calcular ancho de tarjetas basado en el espacio disponible
            int cardWidth = CalculateCardWidth();

            foreach (var nota in notas.OrderByDescending(n => n.FechaCreacion))
            {
                var noteCard = CreateNoteCard(nota, cardWidth);
                notesFlowPanel.Controls.Add(noteCard);
            }

            lblContador.Text = $"{notas.Count} nota{(notas.Count != 1 ? "s" : "")}";
            notesFlowPanel.ResumeLayout();
        }

        private int CalculateCardWidth()
        {
            if (notesFlowPanel.Width <= 400)
                return notesFlowPanel.Width - 30;

            int availableWidth = notesFlowPanel.Width - 40;
            int minCardWidth = 280;
            int maxCardWidth = 320;
            
            // Calcular columnas basado en el ancho disponible
            int columns = Math.Max(1, availableWidth / minCardWidth);
            int cardWidth = availableWidth / columns;
            
            return Math.Min(Math.Max(cardWidth, minCardWidth), maxCardWidth);
        }

        private Panel CreateNoteCard(Nota nota, int width)
        {
            var card = new Panel
            {
                Size = new Size(width, 120),
                BackColor = panelColor,
                Margin = new Padding(8),
                Cursor = Cursors.Hand,
                Tag = nota,
                Padding = new Padding(12)
            };

            // Borde sutil
            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(borderColor, 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
                }
            };

            int contentWidth = width - 24; // Padding interno

            // Título
            var lblTitle = new Label
            {
                Text = TruncateText(nota.Titulo, 30),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = textColor,
                Size = new Size(contentWidth, 22),
                Location = new Point(0, 0)
            };
            card.Controls.Add(lblTitle);

            // Contenido
            var lblContent = new Label
            {
                Text = TruncateText(nota.Contenido, 70),
                Font = new Font("Segoe UI", 9),
                ForeColor = lightTextColor,
                Size = new Size(contentWidth, 45),
                Location = new Point(0, 22)
            };
            card.Controls.Add(lblContent);

            // Panel inferior para metadata
            var metaPanel = new Panel
            {
                Size = new Size(contentWidth, 25),
                Location = new Point(0, 72),
                BackColor = Color.Transparent
            };
            card.Controls.Add(metaPanel);

            // Fecha
            var lblDate = new Label
            {
                Text = nota.FechaCreacion.ToString("dd/MM/yyyy"),
                Font = new Font("Segoe UI", 8),
                ForeColor = lightTextColor,
                Size = new Size(100, 20),
                Location = new Point(0, 5),
                TextAlign = ContentAlignment.MiddleLeft
            };
            metaPanel.Controls.Add(lblDate);

            // Tags si existen
            if (nota.Tags != null && nota.Tags.Any())
            {
                var tagsText = string.Join(", ", nota.Tags.Take(2));
                var lblTags = new Label
                {
                    Text = TruncateText(tagsText, 20),
                    Font = new Font("Segoe UI", 8, FontStyle.Italic),
                    ForeColor = accentColor,
                    Size = new Size(contentWidth - 110, 20),
                    Location = new Point(105, 5),
                    TextAlign = ContentAlignment.MiddleRight
                };
                metaPanel.Controls.Add(lblTags);
            }

            // Eventos de clic
            card.Click += (s, e) => SeleccionarNota(nota);
            foreach (Control control in card.Controls)
            {
                control.Click += (s, e) => SeleccionarNota(nota);
                if (control is Panel panel)
                {
                    foreach (Control child in panel.Controls)
                    {
                        child.Click += (s, e) => SeleccionarNota(nota);
                    }
                }
            }

            return card;
        }

        private string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length > maxLength ? text.Substring(0, maxLength) + "..." : text;
        }

        private void SeleccionarNota(Nota nota)
        {
            _notaSeleccionada = nota;
            txtTitulo.Text = nota.Titulo;
            txtContenido.Text = nota.Contenido;

            // Resaltar nota seleccionada
            foreach (Control control in notesFlowPanel.Controls)
            {
                if (control is Panel card && card.Tag is Nota cardNote)
                {
                    card.BackColor = cardNote.Id == nota.Id ? 
                        Color.FromArgb(240, 248, 255) : panelColor;
                }
            }
        }

        private void LimpiarEditor()
        {
            _notaSeleccionada = null;
            txtTitulo.Clear();
            txtContenido.Clear();
            
            // Quitar resaltado de todas las notas
            foreach (Control control in notesFlowPanel.Controls)
            {
                if (control is Panel card)
                    card.BackColor = panelColor;
            }
            
            txtTitulo.Focus();
        }

        private void BuscarNotas()
        {
            var searchText = txtBuscar.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
            {
                MostrarNotas(_notas);
                return;
            }

            var filteredNotes = _notas.Where(n =>
                n.Titulo.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                n.Contenido.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                (n.Tags != null && n.Tags.Any(t => t.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
            ).ToList();

            MostrarNotas(filteredNotes);
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El titulo es obligatorio", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return;
            }

            try
            {
                var nota = new Nota
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Contenido = txtContenido.Text.Trim(),
                    Tags = new List<string>() // Simplificado
                };

                if (_notaSeleccionada == null)
                {
                    await _mongoDBServices.CreateNotaAsync(nota);
                    MessageBox.Show("Nota creada exitosamente", "Exito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    nota.Id = _notaSeleccionada.Id;
                    nota.FechaCreacion = _notaSeleccionada.FechaCreacion;
                    await _mongoDBServices.UpdateNotaAsync(_notaSeleccionada.Id, nota);
                    MessageBox.Show("Nota actualizada exitosamente", "Exito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarEditor();
                await CargarNotas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la nota: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_notaSeleccionada == null)
            {
                MessageBox.Show("Selecciona una nota para eliminar", "Advertencia", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"¿Estas seguro de eliminar la nota: \"{_notaSeleccionada.Titulo}\"?", 
                "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _mongoDBServices.DeleteNotaAsync(_notaSeleccionada.Id);
                    MessageBox.Show("Nota eliminada", "Exito", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarEditor();
                    await CargarNotas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar nota: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(1300, 800);
            this.Name = "MainForm";
            this.ResumeLayout(false);
        }
    }
}