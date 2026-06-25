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
using System.Text.RegularExpressions;

namespace Verduleria
{
    public partial class frmcliente : Form
    {
        public class Cliente
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

            private string _nombre;
            public string Nombre
            {
                get { return _nombre; }
                set { _nombre = value; }
            }

            private string _apellido;
            public string Apellido
            {
                get { return _apellido; }
                set { _apellido = value; }
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

            private string _direccion;
            public string Direccion
            {
                get { return _direccion; }
                set { _direccion = value; }
            }
            #endregion

            #region Constructor
            public Cliente()
            {
                this.RUC = string.Empty;
                this.RazonSocial = string.Empty;
                this.Nombre = string.Empty;
                this.Apellido = string.Empty;
                this.Correo = string.Empty;
                this.Telefono = string.Empty;
                this.Direccion = string.Empty;

            }
            #endregion

            #region SQL

            public void Insert()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                string query = "INSERT INTO Clientes (RUC, razon_social, nombre, apellido, correo, telefono, direccion) " +
                               "VALUES (@RUC, @razonSocial, @nombre, @apellido, @correo, @telefono, @direccion)";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@RUC", this.RUC);
                        mySqlCommand.Parameters.AddWithValue("@razonSocial", this.RazonSocial);
                        mySqlCommand.Parameters.AddWithValue("@nombre", this.Nombre);
                        mySqlCommand.Parameters.AddWithValue("@apellido", this.Apellido);
                        mySqlCommand.Parameters.AddWithValue("@correo", this.Correo);
                        mySqlCommand.Parameters.AddWithValue("@telefono", this.Telefono);
                        mySqlCommand.Parameters.AddWithValue("@direccion", this.Direccion);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }

            public DataTable ObtenerClientesOrdenados()
            {
                SqlConnection mySqlConnection = new SqlConnection();
                SqlCommand mySqlCommand = new SqlCommand();
                DataTable dt = new DataTable();

                mySqlConnection.ConnectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //mySqlConnection.ConnectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.CommandText = "SELECT client_id, RUC, razon_social, nombre, apellido, correo, telefono, direccion FROM Clientes ORDER BY client_id ASC";

                try
                {
                    mySqlConnection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(mySqlCommand);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los clientes: " + ex.Message);
                }
                finally
                {
                    if (mySqlConnection.State == ConnectionState.Open)
                    {
                        mySqlConnection.Close();
                    }
                }

                return dt;
            }
            #endregion
            public void Update()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                // Modificamos los datos buscando por el RUC (que es único y no cambia)
                string query = "UPDATE Clientes SET razon_social = @razonSocial, nombre = @nombre, apellido = @apellido, " +
                               "correo = @correo, telefono = @telefono, direccion = @direccion WHERE RUC = @RUC";

                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand mySqlCommand = new SqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.AddWithValue("@RUC", this.RUC);
                        mySqlCommand.Parameters.AddWithValue("@razonSocial", this.RazonSocial);
                        mySqlCommand.Parameters.AddWithValue("@nombre", this.Nombre);
                        mySqlCommand.Parameters.AddWithValue("@apellido", this.Apellido);
                        mySqlCommand.Parameters.AddWithValue("@correo", this.Correo);
                        mySqlCommand.Parameters.AddWithValue("@telefono", this.Telefono);
                        mySqlCommand.Parameters.AddWithValue("@direccion", this.Direccion);

                        mySqlConnection.Open();
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
            }

            public void Delete()
            {
                string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
                //   string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
                string query = "DELETE FROM Clientes WHERE RUC = @RUC";

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
        }
        public frmcliente(string rolUsuario)
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(frmcliente_KeyDown);
            this.txttelefono.KeyPress += new KeyPressEventHandler(txttelefono_KeyPress);
            this.txtruc.KeyPress += new KeyPressEventHandler(txtruc_KeyPress);


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
        private void frmcliente_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                string mensajeAyuda = "SISTEMA DE GESTIÓN - CONTROL DE CLIENTES\n\n" +
                              "¿Qué funciones cumple este módulo?\n" +
                              "Este formulario permite realizar la carga, consulta y administración del registro de compradores de la verdulería.\n\n" +
                              "RESTRICCIÓN DE ACCESO:\n" +
                              "Las funciones de modificación y eliminación de registros están habilitadas exclusivamente para el personal con nivel de Administrador. Los niveles autorizados para registrar nuevos clientes son Admin y Cajero.\n\n" +
                              "INSTRUCCIONES DE USO:\n\n" +
                              "1. REGISTRAR UN NUEVO CLIENTE:\n" +
                              "   - Ingrese los datos obligatorios: RUC o Identificación, Razón Social, Nombre, Apellido, Correo, Teléfono y Dirección.\n" +
                              "   - Presione el botón 'GUARDAR'. El sistema validará los campos e insertará el registro en la base de datos.\n\n" +
                              "2. MODIFICAR O ELIMINAR DATOS:\n" +
                              "   - Seleccione al cliente correspondiente en la tabla o lista en pantalla.\n" +
                              "   - Presione el botón 'EDITAR' o 'BORRAR' según la acción requerida (Permiso requerido: Admin).\n\n" +
                              "Observaciones pertinentes:\n" +
                              "- El campo 'Teléfono' únicamente admite caracteres numéricos.\n" +
                              "- El sistema valida que el 'Correo electrónico' posea un formato institucional o comercial válido.\n" +
                              "- No se permiten registros duplicados bajo el mismo número de RUC o Identificación.";

                MessageBox.Show(mensajeAyuda, "Ayuda del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Handled = true;
            }
        }

        private void MostrarLista()
        {
            try
            {
                Cliente clienteServicio = new Cliente();
                DataTable dtClientes = clienteServicio.ObtenerClientesOrdenados();

                // Vincula el DataTable directamente al DataGridView
                dataGridView1.DataSource = dtClientes;

           

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar lista", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ValidarEmail(string email)
        {
            string expresion = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, expresion);
        }

        private string nivelUsuarioLogueado;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                // Carga de datos en TextBox...
                txtruc.Text = fila.Cells["RUC"].Value.ToString();
                txtrazon.Text = fila.Cells["razon_social"].Value.ToString();
                txtnombre.Text = fila.Cells["nombre"].Value.ToString();
                txtapellido.Text = fila.Cells["apellido"].Value.ToString();
                txtcorreo.Text = fila.Cells["correo"].Value.ToString();
                txttelefono.Text = fila.Cells["telefono"].Value.ToString();
                txtdireccion.Text = fila.Cells["direccion"].Value.ToString();

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
                    txtruc.Enabled = true;
                }
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            try
            {
                // =========================================================================
                // BLOQUE DE VALIDACIONES DE ENTRADA
                // =========================================================================

                // 1. Validar campos vacíos o con puros espacios (.Trim())
                // Validación de RUC
                if (string.IsNullOrWhiteSpace(txtruc.Text))
                {
                    MessageBox.Show("Por favor, ingrese el RUC o Identificación.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtruc.Focus();
                    return;
                }

                // Validación de Razón Social
                if (string.IsNullOrWhiteSpace(txtrazon.Text))
                {
                    MessageBox.Show("Por favor, ingrese la Razón Social.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtrazon.Focus();
                    return;
                }

                // Validación de Nombre
                if (string.IsNullOrWhiteSpace(txtnombre.Text))
                {
                    MessageBox.Show("Por favor, ingrese el nombre.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtnombre.Focus();
                    return;
                }

                // Validación de Apellido
                if (string.IsNullOrWhiteSpace(txtapellido.Text))
                {
                    MessageBox.Show("Por favor, ingrese el apellido.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtapellido.Focus();
                    return;
                }

                // Validación de Correo
                if (string.IsNullOrWhiteSpace(txtcorreo.Text))
                {
                    MessageBox.Show("Por favor, ingrese el correo electrónico.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcorreo.Focus();
                    return;
                }

                // Validación de Teléfono
                if (string.IsNullOrWhiteSpace(txttelefono.Text))
                {
                    MessageBox.Show("Por favor, ingrese el número de teléfono.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txttelefono.Focus();
                    return;
                }


                // Validación de Dirección
                if (string.IsNullOrWhiteSpace(txtdireccion.Text))
                {
                    MessageBox.Show("Por favor, ingrese la dirección.", "Campo Requerido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtdireccion.Focus();
                    return;
                }

                // 2. Validar formato básico de correo electrónico
                if (!ValidarEmail(txtcorreo.Text.Trim()))
                {
                    MessageBox.Show("El formato del correo electrónico ingresado no es válido (ejemplo: usuario@dominio.com).",
                                    "Correo Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtcorreo.Focus(); // Coloca el cursor en el campo del error
                    return;
                }

                // =========================================================================

                Cliente nuevocliente = new Cliente();

                // Usamos .Trim() para limpiar espacios accidentales que el usuario deje al inicio o al final
                nuevocliente.RUC = txtruc.Text.Trim();
                nuevocliente.RazonSocial = txtrazon.Text.Trim();
                nuevocliente.Nombre = txtnombre.Text.Trim();
                nuevocliente.Apellido = txtapellido.Text.Trim();
                nuevocliente.Correo = txtcorreo.Text.Trim();
                nuevocliente.Telefono = txttelefono.Text.Trim();
                nuevocliente.Direccion = txtdireccion.Text.Trim();

                nuevocliente.Insert();

                MessageBox.Show("¡Cliente registrado en la verdulería con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtruc.Clear();
                txtrazon.Clear();
                txtnombre.Clear();
                txtapellido.Clear();
                txtcorreo.Clear();
                txttelefono.Clear();
                txtdireccion.Clear();

                this.Refresh();
                MostrarLista();
            }
            catch (SqlException ex)
            {
                // Captura de errores específicos de SQL (como RUCs duplicados) ===
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    MessageBox.Show("El RUC o Identificación ingresada ya se encuentra registrada para otro cliente.",
                                    "Registro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Error en la Base de Datos: " + ex.Message, "Error SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar el cliente:\n" + ex.Message, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtruc.Clear();
            txtrazon.Clear();
            txtnombre.Clear();
            txtapellido.Clear();
            txtcorreo.Clear();
            txttelefono.Clear();
            txtdireccion.Clear();

            // ¡Súper importante! Volvemos a habilitar el campo RUC
            txtruc.Enabled = true;
            btneditar.Enabled = false;
            btnborrar.Enabled = false;
        }
        private void frmcliente_Load(object sender, EventArgs e)
        {
            MostrarLista();
            btnborrar.Enabled = false;
            btneditar.Enabled = false;
        }

        private void btneditar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. (Opcional) Puedes copiar aquí las mismas validaciones de campos vacíos que usas en Guardar

                // 2. Creamos el objeto con los datos de los TextBox
                Cliente clienteEditar = new Cliente();
                clienteEditar.RUC = txtruc.Text.Trim(); // El RUC viaja para el WHERE del UPDATE
                clienteEditar.RazonSocial = txtrazon.Text.Trim();
                clienteEditar.Nombre = txtnombre.Text.Trim();
                clienteEditar.Apellido = txtapellido.Text.Trim();
                clienteEditar.Correo = txtcorreo.Text.Trim();
                clienteEditar.Telefono = txttelefono.Text.Trim();
                clienteEditar.Direccion = txtdireccion.Text.Trim();

                // 3. Ejecutar la actualización en la BD
                clienteEditar.Update();

                MessageBox.Show("¡Cliente actualizado con éxito!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 4. Limpiar controles y REHABILITAR el txtruc para nuevos registros
                LimpiarCampos();
                MostrarLista(); // Refresca tu DataGridView
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
                    MessageBox.Show("Seleccione un cliente de la tabla para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Pedir confirmación al usuario
                DialogResult respuesta = MessageBox.Show("¿Está seguro de que desea eliminar a este cliente?",
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    Cliente clienteEliminar = new Cliente();
                    clienteEliminar.RUC = txtruc.Text.Trim();

                    // Ejecutar el DELETE
                    clienteEliminar.Delete();

                    MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
