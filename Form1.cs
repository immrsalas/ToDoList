using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ToDoList
{
    // Complete Form1 implementation with programmatic UI (no Designer)
    public class Form1 : Form
    {
        // Business logic manager
        private GestorTareas gestor;

        // UI controls (declared as private fields as requested)
        private Label lblTitulo;
        private TextBox txtTitulo;
        private Label lblDescripcion;
        private TextBox txtDescripcion;

        private Button btnAgregar;
        private Button btnCompletar;
        private Button btnEliminar;
        private Button btnPendientes;
        private Button btnCompletadas;
        private Button btnTodas;

        private ListBox lstTareas;

        // Constructor
        public Form1()
        {
            InitializeComponent();
            gestor = new GestorTareas();
        }

        /// <summary>
        /// Initialize all controls by code. No Designer usage.
        /// </summary>
        private void InitializeComponent()
        {
            // Form properties
            this.ClientSize = new Size(800, 500);
            this.Text = "Gestor de Tareas";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Left column coordinates and spacing
            int marginLeft = 16;
            int currentY = 16;
            int controlWidth = 360;
            int labelHeight = 20;
            int spacingY = 8;

            // Title label
            lblTitulo = new Label
            {
                Text = "Título:",
                Location = new Point(marginLeft, currentY),
                Size = new Size(controlWidth, labelHeight)
            };
            this.Controls.Add(lblTitulo);

            // Title textbox
            currentY += labelHeight + 4;
            txtTitulo = new TextBox
            {
                Location = new Point(marginLeft, currentY),
                Size = new Size(controlWidth, 26)
            };
            this.Controls.Add(txtTitulo);

            // Description label
            currentY += txtTitulo.Height + spacingY;
            lblDescripcion = new Label
            {
                Text = "Descripción:",
                Location = new Point(marginLeft, currentY),
                Size = new Size(controlWidth, labelHeight)
            };
            this.Controls.Add(lblDescripcion);

            // Description textbox (multiline, height 60)
            currentY += labelHeight + 4;
            txtDescripcion = new TextBox
            {
                Location = new Point(marginLeft, currentY),
                Size = new Size(controlWidth, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtDescripcion);

            // Buttons: Agregar, Completar, Eliminar
            currentY += txtDescripcion.Height + 12;
            btnAgregar = new Button
            {
                Text = "Agregar",
                Location = new Point(marginLeft, currentY),
                Size = new Size(110, 32)
            };
            btnAgregar.Click += BtnAgregar_Click;
            this.Controls.Add(btnAgregar);

            btnCompletar = new Button
            {
                Text = "Completar",
                Location = new Point(marginLeft + 120, currentY),
                Size = new Size(110, 32)
            };
            btnCompletar.Click += BtnCompletar_Click;
            this.Controls.Add(btnCompletar);

            btnEliminar = new Button
            {
                Text = "Eliminar",
                Location = new Point(marginLeft + 240, currentY),
                Size = new Size(110, 32)
            };
            btnEliminar.Click += BtnEliminar_Click;
            this.Controls.Add(btnEliminar);

            // Filter buttons: Pendientes, Completadas, Todas (below)
            currentY += btnAgregar.Height + 12;
            btnPendientes = new Button
            {
                Text = "Pendientes",
                Location = new Point(marginLeft, currentY),
                Size = new Size(110, 32)
            };
            btnPendientes.Click += BtnPendientes_Click;
            this.Controls.Add(btnPendientes);

            btnCompletadas = new Button
            {
                Text = "Completadas",
                Location = new Point(marginLeft + 120, currentY),
                Size = new Size(110, 32)
            };
            btnCompletadas.Click += BtnCompletadas_Click;
            this.Controls.Add(btnCompletadas);

            btnTodas = new Button
            {
                Text = "Todas",
                Location = new Point(marginLeft + 240, currentY),
                Size = new Size(110, 32)
            };
            btnTodas.Click += BtnTodas_Click;
            this.Controls.Add(btnTodas);

            // ListBox on the right side (large)
            int listX = marginLeft + controlWidth + 20; // leave a gap between columns
            int listWidth = this.ClientSize.Width - listX - marginLeft;
            lstTareas = new ListBox
            {
                Location = new Point(listX, 16),
                Size = new Size(listWidth, this.ClientSize.Height - 32),
                HorizontalScrollbar = true
            };
            // ListBox will hold Tarea objects; ToString() is used for display.
            this.Controls.Add(lstTareas);
        }

        // Event handlers and behavior

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            try
            {
                string titulo = txtTitulo.Text.Trim();
                string descripcion = txtDescripcion.Text.Trim();

                var nueva = new Tarea(titulo, descripcion);
                gestor.AgregarTarea(nueva);

                UpdateLista(gestor.Tareas);

                // Clear inputs
                txtTitulo.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
                txtTitulo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al agregar tarea", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCompletar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (lstTareas.SelectedItem is Tarea seleccionado)
                {
                    gestor.MarcarCompleta(seleccionado);
                    UpdateLista(gestor.Tareas);
                }
                else
                {
                    MessageBox.Show(this, "Seleccione una tarea para marcar como completa.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al marcar completa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            try
            {
                if (lstTareas.SelectedItem is Tarea seleccionado)
                {
                    var confirm = MessageBox.Show(this, $"¿Eliminar la tarea \"{seleccionado.Titulo}\"?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        gestor.EliminarTarea(seleccionado);
                        UpdateLista(gestor.Tareas);
                    }
                }
                else
                {
                    MessageBox.Show(this, "Seleccione una tarea para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPendientes_Click(object? sender, EventArgs e)
        {
            try
            {
                UpdateLista(gestor.ObtenerPendientes());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al filtrar pendientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCompletadas_Click(object? sender, EventArgs e)
        {
            try
            {
                UpdateLista(gestor.ObtenerCompletadas());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al filtrar completadas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTodas_Click(object? sender, EventArgs e)
        {
            try
            {
                UpdateLista(gestor.Tareas);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al mostrar todas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Updates the ListBox with the provided tasks.
        /// </summary>
        /// <param name="tareas">Sequence of Tarea objects to display.</param>
        private void UpdateLista(IEnumerable<Tarea> tareas)
        {
            try
            {
                // Force materialization to avoid binding to a deferred enumerable
                var lista = tareas?.ToList() ?? new List<Tarea>();

                // Reset DataSource safely
                lstTareas.DataSource = null;
                lstTareas.DataSource = lista;

                // The Tarea.ToString() will be used for display. Clear selection.
                lstTareas.ClearSelected();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error al actualizar la lista", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
