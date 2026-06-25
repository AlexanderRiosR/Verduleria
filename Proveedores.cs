using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Verduleria.frmcliente;

namespace Verduleria
{
    public partial class Proveedores : Form
    {
        public class Proveedor
        {

            #region Propiedades
            private string _ruc;
            public string RUC
            {
                get { return _ruc; }
                set { _ruc = value; }
            }

            private string _razonSocial;
            public string RazonSocial
            {
                get { return _razonSocial; }
                set { _razonSocial = value; }
            }

            private string _correo;
            public string Correo
            {
                get { return _correo; }
                set { _correo = value; }
            }

            private string _telefono;
            public string Telefono
            {
                get { return _telefono; }
                set { _telefono = value; }
            }

            private string _ciudad;
            public string Ciudad
            {
                get { return _ciudad; }
                set { _ciudad = value; }
            }
            private string _producto;
            public string Producto
            {
                get { return _producto; }
                set { _producto = value; }
            }
            #endregion

            #region Constructor
            public Proveedor()
            {
                this.RUC = string.Empty;
                this.RazonSocial = string.Empty;
                this.Correo = string.Empty;
                this.Telefono = string.Empty;
                this.Ciudad = string.Empty;
                this.Producto = string.Empty;
            }
            #endregion

            #region SQL
            // Optimización con bloques 'using' para prevenir bloqueos de conexión ===
            public void Insert()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
             //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                string query = "INSERT INTO Proveedor (RUC, razon_social, correo, telefono, ciudad, productos) " +
                               "VALUES (@RUC, @razonSocial, @correo, @telefono, @ciudad, @productos)";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@RUC", this.RUC);
                        mySqlCommand.Parameters.AddWithValue("@razonSocial", this.RazonSocial);
                        mySqlCommand.Parameters.AddWithValue("@correo", this.Correo);
                        mySqlCommand.Parameters.AddWithValue("@telefono", this.Telefono);
                        mySqlCommand.Parameters.AddWithValue("@ciudad", this.Ciudad);
                        mySqlCommand.Parameters.AddWithValue("@productos", this.Producto);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }

            public void Update()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                string query = "UPDATE Proveedor SET razon_social = @razonSocial, correo = @correo, " +
                               "telefono = @telefono, ciudad = @ciudad, productos = @productos WHERE RUC = @RUC";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@RUC", this.RUC);
                        mySqlCommand.Parameters.AddWithValue("@razonSocial", this.RazonSocial);
                        mySqlCommand.Parameters.AddWithValue("@correo", this.Correo);
                        mySqlCommand.Parameters.AddWithValue("@telefono", this.Telefono);
                        mySqlCommand.Parameters.AddWithValue("@ciudad", this.Ciudad);
                        mySqlCommand.Parameters.AddWithValue("@productos", this.Producto);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }

            // === AGREGADO: Método Delete que faltaba ===
            public void Delete()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                string query = "DELETE FROM Proveedor WHERE RUC = @RUC";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@RUC", this.RUC);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }

            public DataTable ObtenerProveedoresOrdenados()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                DataTable dt = new DataTable();
                string query = "SELECT prov_id, RUC, razon_social, correo, telefono, ciudad, productos FROM Proveedor ORDER BY prov_id ASC";

                try
                {
                    using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                        {
                            mySqlConnection.Open();
                            using (SqlDataAdapter da = new SqlDataAdapter(mySqlCommand))
                            {
                                da.Fill(dt);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los proveedores: " + ex.Message);
                }

                return dt;
            }
            #endregion
        }
        public Proveedores(string rolUsuario)
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(frmproveedor_KeyDown);
            this.txtruc.KeyPress += new KeyPressEventHandler(txtruc_KeyPress);

            this.txttelefono.KeyPress += new KeyPressEventHandler(txttelefono_KeyPress);

            this.nivelUsuarioLogueado = rolUsuario;
        }

        private void txtruc_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si la tecla presionada NO es un número y NO es la tecla de borrar (BackSpace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Cancela el evento para que la letra no se escriba en el TextBox
                e.Handled = true;
            }
        }

        private void txttelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si la tecla presionada NO es un número y NO es la tecla de borrar (BackSpace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Cancela el evento para que la letra no se escriba en el TextBox
                e.Handled = true;
            }
        }
        private void frmproveedor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string mensajeAyuda = "SISTEMA DE GESTIÓN - CONTROL DE PROVEEDORES\n\n" +
                                      "¿Qué funciones cumple este módulo?\n" +
                                      "Este formulario permite realizar la carga, consulta y administración del registro de proveedores que abastecen a la verdulería.\n\n" +
                                      "RESTRICCIÓN DE ACCESO:\n" +
                                      "Las funciones de modificación, inserción y eliminación de registros están habilitadas exclusivamente para el personal con nivel de Administrador. El personal con nivel Repositor cuenta únicamente con permisos de lectura de datos. Los cajeros tienen el acceso denegado a este módulo.\n\n" +
                                      "INSTRUCCIONES DE USO:\n\n" +
                                      "1. REGISTRAR UN NUEVO PROVEEDOR:\n" +
                                      "   - Ingrese los datos obligatorios: RUC, Razón Social, Correo, Teléfono, Ciudad y los Productos que provee.\n" +
                                      "   - Presione el botón 'GUARDAR'. El sistema validará los campos e insertará el registro en la base de datos (Permiso requerido: Admin).\n\n" +
                                      "2. MODIFICAR O ELIMINAR DATOS:\n" +
                                      "   - Seleccione al proveedor correspondiente en la tabla o lista en pantalla.\n" +
                                      "   - Presione el botón 'EDITAR' o 'BORRAR' según la acción requerida (Permiso requerido: Admin).\n\n" +
                                      "Observaciones pertinentes:\n" +
                                      "- El campo 'Teléfono' únicamente admite caracteres numéricos.\n" +
                                      "- Al seleccionar un proveedor de la lista, el campo 'RUC' se deshabilitará para preservar la integridad de la clave primaria.\n" +
                                      "- No se permiten registros duplicados bajo el mismo número de RUC.";

                MessageBox.Show(mensajeAyuda, "Ayuda del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Handled = true;
            }
        }

        private void MostrarLista()
        {
            try
            {
                Proveedor proveedorServicio = new Proveedor();
                DataTable dtProveedores = proveedorServicio.ObtenerProveedoresOrdenados();

                // Vincula el DataTable directamente al DataGridView
                dgvproveedor.DataSource = dtProveedores;

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar lista", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string nivelUsuarioLogueado;
        private void dgvproveedor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvproveedor.Rows[e.RowIndex];

                // Carga de datos en TextBox...
                txtruc.Text = fila.Cells["RUC"].Value.ToString();
                txtrazon.Text = fila.Cells["razon_social"].Value.ToString();
                txtcorreo.Text = fila.Cells["correo"].Value.ToString();
                txttelefono.Text = fila.Cells["telefono"].Value.ToString();
                txtciudad.Text = fila.Cells["ciudad"].Value.ToString();
                txtproducto.Text = fila.Cells["productos"].Value.ToString();

                txtruc.Enabled = false;

                // VALIDACIÓN DE ROL Y ACTIVACIÓN DE BOTONES
                if (nivelUsuarioLogueado == "Admin")
                {
                    btneditar.Enabled = true;
                    btnborrar.Enabled = true;
                }
                else
                {
                    // Si no es Admin, permanecen apagados y podrías lanzar un aviso discreto
                    btneditar.Enabled = false;
                    btnborrar.Enabled = false;
                }
            }
            
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            MostrarLista();
            btnborrar.Enabled = false;
            btneditar.Enabled = false;

            if (nivelUsuarioLogueado == "Repositor")
            {
                btnguardar.Enabled = false;
                txtruc.Enabled = false;
                txtrazon.Enabled = false;
                txtcorreo.Enabled = false;
                txttelefono.Enabled = false;
                txtciudad.Enabled = false;
                txtproducto.Enabled = false;
            }
            else if (nivelUsuarioLogueado == "Admin")
            {
                btnguardar.Enabled = true;
                txtruc.Enabled = true;
                txtrazon.Enabled = true;
                txtcorreo.Enabled = true;
                txttelefono.Enabled = true;
                txtciudad.Enabled = true;
                txtproducto.Enabled = true;
            }
        }
        
        private bool ValidarEmail(string email)
        {
            string expresion = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, expresion);
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtruc.Text))
                {
                    MessageBox.Show("Por favor, ingrese el RUC o Identificación.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtruc.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtrazon.Text))
                {
                    MessageBox.Show("Por favor, ingrese la Razón Social.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtrazon.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtcorreo.Text))
                {
                    MessageBox.Show("Por favor, ingrese el correo electrónico.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcorreo.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txttelefono.Text))
                {
                    MessageBox.Show("Por favor, ingrese el número de teléfono.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txttelefono.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtciudad.Text))
                {
                    MessageBox.Show("Por favor, ingrese la ciudad.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtciudad.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtproducto.Text))
                {
                    MessageBox.Show("Por favor, ingrese los productos que provee.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtproducto.Focus();
                    return;
                }

                if (!ValidarEmail(txtcorreo.Text.Trim()))
                {
                    MessageBox.Show("El formato del correo electrónico ingresado no es válido (ejemplo: usuario@dominio.com).",
                                    "Correo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcorreo.Focus();
                    return;
                }

                Proveedor nuevoProveedor = new Proveedor();
                nuevoProveedor.RUC = txtruc.Text.Trim();
                nuevoProveedor.RazonSocial = txtrazon.Text.Trim();
                nuevoProveedor.Correo = txtcorreo.Text.Trim();
                nuevoProveedor.Telefono = txttelefono.Text.Trim();
                nuevoProveedor.Ciudad = txtciudad.Text.Trim();
                nuevoProveedor.Producto = txtproducto.Text.Trim();

                nuevoProveedor.Insert();

                MessageBox.Show("¡Proveedor registrado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MostrarLista();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("El RUC ingresado ya se encuentra registrado para otro proveedor.",
                                    "Registro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Error en la Base de Datos: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el proveedor:\n" + ex.Message, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtruc.Text)) return;

                Proveedor proveedorEditar = new Proveedor();
                proveedorEditar.RUC = txtruc.Text.Trim();
                proveedorEditar.RazonSocial = txtrazon.Text.Trim();
                proveedorEditar.Correo = txtcorreo.Text.Trim();
                proveedorEditar.Telefono = txttelefono.Text.Trim();
                proveedorEditar.Ciudad = txtciudad.Text.Trim();
                proveedorEditar.Producto = txtproducto.Text.Trim();

                proveedorEditar.Update();

                MessageBox.Show("¡Proveedor actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                MostrarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnborrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtruc.Text))
                {
                    MessageBox.Show("Seleccione un proveedor de la tabla para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult respuesta = MessageBox.Show("¿Está seguro de que desea eliminar a este proveedor?",
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    Proveedor proveedorEliminar = new Proveedor();
                    proveedorEliminar.RUC = txtruc.Text.Trim();
                    proveedorEliminar.Delete();

                    MessageBox.Show("Proveedor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    MostrarLista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtruc.Clear();
            txtrazon.Clear();
            txtcorreo.Clear();
            txttelefono.Clear();
            txtciudad.Clear();
            txtproducto.Clear();

            if (nivelUsuarioLogueado == "Admin")
            {
                txtruc.Enabled = true;
            }
            btneditar.Enabled = false;
            btnborrar.Enabled = false;
        }

        private void btnvolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
