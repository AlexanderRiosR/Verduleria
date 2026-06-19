using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Verduleria
{
    public partial class frmmenu : Form
    {
        // Variable para guardar el rol en toda la clase
        private string rolUsuario;
        public frmmenu(string nivelAcceso)
        {
            InitializeComponent();
            this.rolUsuario = nivelAcceso;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Menu_KeyDown);

            if (this.rolUsuario == "Admin")
            {
                // El administrador tiene acceso total a todos los módulos
                btncliente.Enabled = true;
                btnproducto.Enabled = true;
                btnproveedor.Enabled = true;
            }
            else if (this.rolUsuario == "Cajero")
            {
                // Acceso Parcial 1: El cajero solo gestiona clientes
                btncliente.Enabled = true;

                // Bloqueamos productos y proveedores
                btnproducto.Enabled = false;
                btnproveedor.Enabled = false;

            }
            else if (this.rolUsuario == "Repositor")
            {
                // Acceso Parcial 2: El repositor solo gestiona productos y stock
                btnproducto.Enabled = true;
                btnproveedor.Enabled = true;
                // Bloqueamos clientes
                btncliente.Enabled = false;
                
            }
        }

        private void Menu_KeyDown(object sender, KeyEventArgs e)
        {
            // Requisito del TP: Utilizar tecla de función F1 para la ayuda
            if (e.KeyCode == Keys.F1)
            {
                // Requisito del TP: Utilizar tecla de función F1 para la ayuda
                if (e.KeyCode == Keys.F1)
                {
                    string mensajeAyuda = "SISTEMA DE GESTIÓN - MENÚ PRINCIPAL\n\n" +
                                          "¿Qué funciones cumple este módulo?\n" +
                                          "Este formulario funciona como el panel central del sistema, permitiendo el acceso a los diferentes módulos de control según el nivel de usuario.\n\n" +
                                          "MÓDULOS DE GESTIÓN DISPONIBLES:\n\n" +
                                          "1. CLIENTES:\n" +
                                          "   - Permite el registro, validación y consulta de datos de compradores.\n" +
                                          "   - Disponible para niveles: Admin y Cajero.\n\n" +
                                          "2. PRODUCTOS:\n" +
                                          "   - Permite administrar las existencias, categorías y stock en el inventario.\n" +
                                          "   - Disponible para niveles: Admin y Repositor.\n\n" +
                                          "3. PROVEEDORES:\n" +
                                          "   - Permite gestionar la información de contacto y origen de la mercadería.\n" +
                                          "   - Disponible para niveles: Exclusivo Admin.\n\n" +
                                          "INSTRUCCIONES DE NAVEGACIÓN:\n" +
                                          "- Haga clic sobre el botón correspondiente para abrir el módulo deseado.\n" +
                                          "- Nivel de acceso actual detectado: " + rolUsuario + ".\n\n" +
                                          "Observación pertinente: Los botones deshabilitados o bloqueados corresponden a módulos que se encuentran fuera de los permisos asignados a su nivel de usuario.";

                    MessageBox.Show(mensajeAyuda, "Ayuda del Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Handled = true;
                }
            }
        }

        private void btncliente_Click(object sender, EventArgs e)
        {
            frmcliente frm = new frmcliente(this.rolUsuario);
            frm.ShowDialog();
        }

        private void btnproducto_Click(object sender, EventArgs e)
        {
            Producto frm = new Producto(this.rolUsuario);
            frm.ShowDialog();
        }

        private void btnproveedor_Click(object sender, EventArgs e)
        {
            Proveedores frm = new Proveedores(this.rolUsuario);
            frm.ShowDialog();
        }

        private void frmmenu_Load(object sender, EventArgs e)
        {

        }
    }
}
