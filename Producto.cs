using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Verduleria.frmcliente;

namespace Verduleria
{
    public partial class Producto : Form
    {
        public class Productos
        {
            #region Propiedades
            private string _codigo;
            public string Codigo
            {
                get { return _codigo; }
                set { _codigo = value; }
            }

            private string _descripcion;
            public string Descripcion
            {
                get { return _descripcion; }
                set { _descripcion = value; }
            }

            private int _tipoId;
            public int TipoId
            {
                get { return _tipoId; }
                set { _tipoId = value; }
            }

            private int _stock;
            public int Stock
            {
                get { return _stock; }
                set { _stock = value; }
            }
            #endregion

            #region Constructor
            public Productos()
            {
                this.Codigo = string.Empty;
                this.Descripcion = string.Empty;
                this.TipoId = 0;
                this.Stock = 0;
            }
            #endregion

            #region SQL
            private readonly string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=verduleria;Integrated Security=True";
            public void Insert()
            {
                // Mapeado exacto a tu tabla: prod_descripcion, tipo_id, stock, Codigo
                string query = "INSERT INTO Producto (prod_descripcion, tipo_id, stock, Codigo) " +
                               "VALUES (@descripcion, @tipoId, @stock, @codigo)";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@descripcion", this.Descripcion);
                        mySqlCommand.Parameters.AddWithValue("@tipoId", this.TipoId);
                        mySqlCommand.Parameters.AddWithValue("@stock", this.Stock);
                        mySqlCommand.Parameters.AddWithValue("@codigo", this.Codigo);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }
            public void Update()
            {
                string query = "UPDATE Producto SET prod_descripcion = @descripcion, tipo_id = @tipoId, stock = @stock " +
                               "WHERE Codigo = @codigo";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@descripcion", this.Descripcion);
                        mySqlCommand.Parameters.AddWithValue("@tipoId", this.TipoId);
                        mySqlCommand.Parameters.AddWithValue("@stock", this.Stock);
                        mySqlCommand.Parameters.AddWithValue("@codigo", this.Codigo);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }
            public void Delete()
            {
                string query = "DELETE FROM Producto WHERE Codigo = @codigo";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@codigo", this.Codigo);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }

            public DataTable ObtenerTipos()
            {
                DataTable dt = new DataTable();
                string query = "SELECT tipo_id, tipo FROM Tipo_producto ORDER BY tipo ASC";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(mySqlCommand))
                        {
                            da.Fill(dt);
                        }
                    }
                }
                return dt;
            }

            public DataTable ObtenerProductosConTipo()
            {
                DataTable dt = new DataTable();
                // INNER JOIN para traer la palabra (fruta/verdura) en lugar del ID numérico
                string query = "SELECT P.Codigo, P.prod_descripcion, T.tipo, P.stock " +
                               "FROM Producto P " +
                               "INNER JOIN Tipo_producto T ON P.tipo_id = T.tipo_id " +
                               "ORDER BY P.prod_id ASC";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        try
                        {
                            mySqlConnection.Open();
                            SqlDataAdapter da = new SqlDataAdapter(mySqlCommand);
                            da.Fill(dt);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error al obtener los productos: " + ex.Message);
                        }
                        finally
                        {
                            if (mySqlConnection.State == ConnectionState.Open)
                            {
                                mySqlConnection.Close();
                            }
                        }
                    }
                }
                return dt;
            }
            #endregion

            
        }

        private string nivelUsuarioLogueado;
        public Producto(string rolUsuario)
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(frmproducto_KeyDown);
            this.txtstock.KeyPress += new KeyPressEventHandler(txtstock_KeyPress);

            this.nivelUsuarioLogueado = rolUsuario;
        }

        private void txtstock_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si la tecla presionada NO es un número y NO es la tecla de borrar (BackSpace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Cancela el evento para que la letra no se escriba en el TextBox
                e.Handled = true;
            }
        }

        private void frmproducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string mensajeAyuda = "SISTEMA DE GESTIÓN - CONTROL DE PRODUCTOS\n\n" +
                       "¿Qué funciones cumple este módulo?\n" +
                       "Este formulario permite administrar de forma completa el inventario de productos de la verdulería.\n\n" +
                       "RESTRICCIÓN DE ACCESO Nivel Admin:\n" +
                       "Las funciones de modificación y eliminación de registros están habilitadas exclusivamente para el personal con nivel de Administrador.\n\n" +
                       "INSTRUCCIONES DE USO:\n\n" +
                       "1. REGISTRAR UN NUEVO PRODUCTO:\n" +
                       "   - Complete los campos de texto: Código, Descripción y Stock inicial.\n" +
                       "   - Seleccione la categoría correspondiente en el menú desplegable 'Tipo'.\n" +
                       "   - Presione el botón 'GUARDAR' para ingresar el registro en el sistema.\n\n" +
                       "2. MODIFICAR DATOS (EDITAR):\n" +
                       "   - Seleccione el producto correspondiente en la tabla o lista en pantalla.\n" +
                       "   - Presione el botón 'EDITAR' para actualizar sus valores (Permiso requerido: Admin).\n\n" +
                       "3. ELIMINAR REGISTRO (BORRAR):\n" +
                       "   - Seleccione el producto que desea dar de baja.\n" +
                       "   - Presione el botón 'BORRAR' para quitarlo del inventario activo (Permiso requerido: Admin).\n\n" +
                       "Nota técnica: El campo 'Stock' únicamente admite caracteres numéricos.";

                MessageBox.Show(mensajeAyuda, "Ayuda del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Handled = true;
            }
        }

        private void CargarComboTipos()
        {
            try
            {
                Productos prodServicio = new Productos();
                DataTable dtTipos = prodServicio.ObtenerTipos();

                cbotipo.DataSource = dtTipos;
                cbotipo.ValueMember = "tipo_id";  // Lo que va silenciosamente a la Base de Datos
                cbotipo.DisplayMember = "tipo";   // Lo que el usuario lee en el combo (fruta, verdura...)
                cbotipo.SelectedIndex = -1;       // Inicia limpio sin selección
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los tipos de producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarLista()
        {
            try
            {
                Productos prodServicio = new Productos(); // 🌟 Corregido el nombre aquí
                DataTable dtProducto = prodServicio.ObtenerProductosConTipo();
                // 🌟 AGREGA ESTA LÍNEA AQUÍ: Vincula el DataTable directamente al DataGridView
                dgvproducto.DataSource = dtProducto;

                // Si ya no usas el ListBox (lstlista), puedes borrar o comentar todo el bloque foreach de abajo

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar lista", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgvproducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que el clic haya sido en una fila válida y no en los encabezados de columna
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvproducto.Rows[e.RowIndex];

                // Carga de datos de manera segura controlando los nulos
                txtcodigo.Text = fila.Cells["codigo"].Value?.ToString() ?? string.Empty;
                txtdescripcion.Text = fila.Cells["prod_descripcion"].Value?.ToString() ?? string.Empty;
                cbotipo.Text = fila.Cells["tipo"].Value?.ToString() ?? string.Empty;
                txtstock.Text = fila.Cells["stock"].Value?.ToString() ?? string.Empty;

                // Deshabilitamos el código para proteger la clave primaria al editar
                txtcodigo.Enabled = false;

                // VALIDACIÓN DE ROL Y ACTIVACIÓN DE BOTONES
                if (nivelUsuarioLogueado == "Admin")
                {
                    btneditar.Enabled = true;
                    btnborrar.Enabled = true;
                }
                else
                {
                    btneditar.Enabled = false;
                    btnborrar.Enabled = false;
                }
            }
        }
             

        private void Producto_Load(object sender, EventArgs e)
        {
            CargarComboTipos();
            MostrarLista();

            btnborrar.Enabled = false; 
            btneditar.Enabled = false;
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones de campos vacíos siguiendo tu formato (.Trim())
                if (string.IsNullOrWhiteSpace(txtcodigo.Text))
                {
                    MessageBox.Show("Por favor, ingrese el código del producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcodigo.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtdescripcion.Text))
                {
                    MessageBox.Show("Por favor, ingrese la descripción del producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtdescripcion.Focus();
                    return;
                }

                if (cbotipo.SelectedIndex == -1)
                {
                    MessageBox.Show("Por favor, seleccione un tipo de producto.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbotipo.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtstock.Text))
                {
                    MessageBox.Show("Por favor, ingrese el stock inicial.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtstock.Focus();
                    return;
                }

                // 2. Mapear datos al objeto
                Productos nuevoProducto = new Productos();
                nuevoProducto.Codigo = txtcodigo.Text.Trim();
                nuevoProducto.Descripcion = txtdescripcion.Text.Trim();
                nuevoProducto.TipoId = Convert.ToInt32(cbotipo.SelectedValue); // Captura el ID numérico por detrás
                nuevoProducto.Stock = Convert.ToInt32(txtstock.Text.Trim());

                nuevoProducto.Insert();

                MessageBox.Show("¡Producto registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Limpieza de campos
                txtcodigo.Clear();
                txtdescripcion.Clear();
                txtstock.Clear();
                cbotipo.SelectedIndex = -1;

                this.Refresh();
                MostrarLista();
            }
            catch (SqlException ex)
            {
                // Captura de códigos duplicados (Llave primaria o restricciones únicas en Código)
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("El código de producto ingresado ya se encuentra registrado.",
                                    "Registro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Error en la Base de Datos: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el producto:\n" + ex.Message, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtcodigo.Clear();
            txtdescripcion.Clear();
            txtstock.Clear();
            cbotipo.SelectedIndex = -1;

            // Volvemos a permitir escribir el código para nuevos registros
            txtcodigo.Enabled = true;

            btneditar.Enabled = false;
            btnborrar.Enabled = false;
        }
        private void btneditar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validaciones de campos vacíos para evitar excepciones SQL
                if (string.IsNullOrWhiteSpace(txtcodigo.Text))
                {
                    MessageBox.Show("Por favor, seleccione un producto de la lista para editar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtdescripcion.Text) || cbotipo.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtstock.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios para actualizar el producto.", "Campos Requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. CORREGIDO: Instanciar la clase Productos correcta del módulo
                Productos productoEditar = new Productos();
                productoEditar.Codigo = txtcodigo.Text.Trim();
                productoEditar.Descripcion = txtdescripcion.Text.Trim();
                productoEditar.TipoId = Convert.ToInt32(cbotipo.SelectedValue); // Mapea el valor numérico (tipo_id)
                productoEditar.Stock = Convert.ToInt32(txtstock.Text.Trim());   // Mapea el stock modificado

                // 3. Ejecutar la actualización en la tabla Producto
                productoEditar.Update();

                MessageBox.Show("¡Producto actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 4. Limpiar componentes y recargar la grilla
                LimpiarCampos();
                MostrarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void btnborrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtcodigo.Text))
                {
                    MessageBox.Show("Seleccione un producto de la tabla para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Pedir confirmación al usuario
                DialogResult respuesta = MessageBox.Show("¿Está seguro de que desea eliminar a este producto?",
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    Productos productoEliminar = new Productos();
                    productoEliminar.Codigo = txtcodigo.Text.Trim();

                    // Ejecutar el DELETE
                    productoEliminar.Delete();

                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpiar y refrescar
                    LimpiarCampos();
                    MostrarLista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnvolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
