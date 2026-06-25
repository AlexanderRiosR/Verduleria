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
        private string connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=verduleria; Integrated Security=True; TrustServerCertificate = True";
        //  private string connectionString = @"Data Source=localhost\SQL2022; Initial Catalog=verduleria; User ID = sa; Password = lab02; TrustServerCertificate = True";
        public frmlogin()
        {
            InitializeComponent();
            this.AcceptButton = btnlogin; // Permite que al presionar Enter se ejecute el botón de login
        }
        

        private void btnlogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnlogin_Click(sender, e);
            }
        }
        private void btnlogin_Click(object sender, EventArgs e)
        {
            // 1. Validamos que el campo CI no esté vacío
            if (string.IsNullOrWhiteSpace(txtci.Text))
            {
                MessageBox.Show("Debe ingresar su Cédula de Identidad (CI)",
                    "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtci.Focus();
                return;
            }

            // 2. Validamos que la contraseña no esté vacía
            if (string.IsNullOrWhiteSpace(txtcontraseña.Text))
            {
                MessageBox.Show("Debe ingresar una contraseña",
                    "ATENCION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtcontraseña.Focus();
                return;
            }

            // Capturamos los datos ingresados por el usuario
            string ciIngresada = txtci.Text;
            string contrasenaIngresada = txtcontraseña.Text;

            // CONSULTA SQL MODIFICADA: Ahora también seleccionamos la columna 'Nivel'
            // Quitamos el filtro 'AND Nivel = @nivel' porque ya no lo conocemos de antemano
            string query = @"SELECT Nombre, Nivel FROM [User] 
                             WHERE CI = @ci 
                               AND Password COLLATE Modern_Spanish_CS_AS = @pass 
                               AND Activo = 'S'";

            using (SqlConnection conexion = new SqlConnection(connectionString))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    // Pasamos únicamente los parámetros que el usuario ingresó
                    comando.Parameters.AddWithValue("@ci", ciIngresada);
                    comando.Parameters.AddWithValue("@pass", contrasenaIngresada);

                    try
                    {
                        conexion.Open();
                        SqlDataReader lector = comando.ExecuteReader();

                        if (lector.Read()) // SI ENCONTRÓ AL USUARIO (CI y Contraseña correctas y Activo)
                        {
                            // Obtenemos los datos directamente desde las columnas de la fila encontrada
                            string nombreUsuario = lector["Nombre"].ToString();
                            string nivelDetectado = lector["Nivel"].ToString();

                            MessageBox.Show($"¡Bienvenido/a al sistema {nombreUsuario}!",
                                "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Ocultamos el login
                            this.Hide();

                            // Le pasamos 'nivelDetectado' (que vino de la BD) al constructor del menú
                            using (frmmenu form2 = new frmmenu(nivelDetectado))
                            {
                                DialogResult resultado = form2.ShowDialog();

                                if (resultado == DialogResult.OK)
                                {
                                    // Si cierra sesión de forma controlada, vuelve a mostrar el login limpio
                                    this.Show();

                                    // REINICIO DE CAMPOS
                                    txtci.Clear();
                                    txtcontraseña.Clear();
                                    txtci.Focus();
                                }
                                else
                                {
                                    // Si cierran con la "X", cerramos la app por completo
                                    Application.Exit();
                                }
                            }
                        }
                        else
                        {
                            // Al no especificar qué falló, mantienes el sistema más seguro
                            MessageBox.Show("Cédula de identidad o contraseña incorrectas, o usuario no autorizado.",
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
