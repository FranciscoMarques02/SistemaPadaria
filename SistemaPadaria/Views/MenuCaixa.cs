using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaPadaria.Views
{
    public partial class MenuCaixa : Form
    {
        public MenuCaixa()
        {
            InitializeComponent();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            if(txbComanda.Text == "")
            {
                MessageBox.Show("Informe o número da comanda!", "Atenção!", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Buscar as informações:
                int nComanda = int.Parse(txbComanda.Text);
                var r = Banco.ComandaDAO.BuscarComanda(nComanda);
                
                if(r.Rows.Count == 0)
                {
                    MessageBox.Show("Não existem produtos nessa comanda!");                    
                }
                else
                {
                    dgvCaixa.DataSource = r;
                    var soma = r.Compute("Sum(Total_Item)", "True");
                    lblValorTotal.Text = "Total: R$" + soma.ToString();
                }
            }
        }
    }
}
