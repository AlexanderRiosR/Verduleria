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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Verduleria
{
    public partial class frmlogin : Form
    {
        private string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=verduleria;Integrated Security=True";
        public frmlogin()
        {
            InitializeComponent();
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            // 1. Validamos que el ComboBox no esté vacío
            if (cbonivel.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un nivel",
                    "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validamos que el campo CI no esté vacío
            if (string.IsNullOrWhiteSpace(txtci.Text))
            {
                MessageBox.Show("Debe ingresar su Cédula de Identidad (CI)",
                    "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtci.Focus();
                return;
            }

            // 3. Validamos que la contraseña no esté vacía
            if (string.IsNullOrWhiteSpace(txtcontraseña.Text))
            {
                MessageBox.Show("Debe ingresar una contraseña",
                    "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtcontraseña.Focus();
                return;
            }

            // Capturamos los datos ingresados por el usuario
            string nivelSeleccionado = cbonivel.SelectedItem.ToString();
            string ciIngresada = txtci.Text;
            string contrasenaIngresada = txtcontraseña.Text;

            // CONSULTA SQL PURA
            // Filtramos por CI, Nivel, Activo y forzamos Case Sensitivity en el Password
            string query = @"SELECT Nombre FROM [User] 
                     WHERE CI = @ci 
                       AND Nivel = @nivel 
                       AND Password COLLATE Modern_Spanish_CS_AS = @pass 
                       AND Activo = 'S'";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    // Pasamos los parámetros de forma segura
                    comando.Parameters.AddWithValue("@ci", ciIngresada);
                    comando.Parameters.AddWithValue("@nivel", nivelSeleccionado);
                    comando.Parameters.AddWithValue("@pass", contrasenaIngresada);

                    try
                    {
                        conexion.Open();
                        SqlDataReader lector = comando.ExecuteReader();

                        if (lector.Read()) // SI ENCONTRÓ AL USUARIO ACTIVO CON DATOS EXACTOS
                        {
                            string nombreUsuario = lector["Nombre"].ToString();

                            MessageBox.Show($"¡Bienvenido/a al sistema {nombreUsuario}!",
                                "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Abrimos el siguiente formulario pasando el nivel
                            frmmenu form2 = new frmmenu(nivelSeleccionado);
                            form2.Show();
                            this.Hide();
                        }
                        else // SI NO COINCIDE LA CI, EL PASSWORD (MAYÚS/MINÚS), EL NIVEL O ESTÁ INACTIVO
                        {
                            MessageBox.Show("Datos incorrectos, el nivel no corresponde o el usuario no está autorizado.",
                                "ERROR DE ACCESO", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            txtcontraseña.Clear();
                            txtcontraseña.Focus();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al conectar con la base de datos: " + ex.Message,
                            "ERROR CRÍTICO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
