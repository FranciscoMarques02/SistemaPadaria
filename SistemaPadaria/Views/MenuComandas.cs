using BibliotecaFMWorks;
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
    public partial class MenuComandas : Form
    {
        Usuario usuario;
        public MenuComandas(Usuario usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
            AtualizarDgv();
        }

        public void AtualizarDgv()
        {
            dgvProdutos.DataSource = Banco.ProdutoDAO.ListarTudo();
        }

        public void Resetar()
        {
            //Limpar campos e resetar grbs:
            txbComanda.Clear();
            txbProdutoInfo.Clear();
            txbProdutoLanc.Clear();
            txbQuantidade.Clear();
            grbInformacoes.Enabled = true;
            grbLancamento.Enabled = false;
        }

        private void dgvProdutos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            

            // Obter o número da linha selecionada:
            int linhaSelecionada = dgvProdutos.CurrentCell.RowIndex;
            // Obter toda a linha selecionada:
            var dadosLinha = dgvProdutos.Rows[linhaSelecionada];
                     
            txbProdutoInfo.Text = dadosLinha.Cells[0].Value.ToString();
            txbProdutoLanc.Text = dadosLinha.Cells[1].Value.ToString();          
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            if(txbComanda.Text == "")
            {
                MessageBox.Show("Informe o número da comanda!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else if(txbProdutoInfo.Text == "")
            {
                MessageBox.Show("Informe o código do produto!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Ativar o GRB de baixo:
                grbLancamento.Enabled = true;
                //Desativar o GRB atual:
                grbInformacoes.Enabled = false;
            }
        }

        private void btnLancar_Click(object sender, EventArgs e)
        {
            if(txbQuantidade.Text == "")
            {
                MessageBox.Show("Informe a quantiadade de produtos!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(int.Parse(txbQuantidade.Text) < 1)
            {
                MessageBox.Show("Verifique a quantidade informada!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //Efetuar cadastro:
                Comanda comanda = new Comanda();
                comanda.Quantidade = int.Parse(txbQuantidade.Text);
                comanda.IdFicha = int.Parse(txbComanda.Text);
                comanda.IdProduto = int.Parse(txbProdutoInfo.Text);
                comanda.IdResponsavel = usuario.Id;
                //Confirmar lançamento:
                var r = MessageBox.Show("Você deseja lançar " + comanda.Quantidade +
                     " unidades de " + txbProdutoLanc.Text + " na comanda " + comanda.IdFicha + "?",
                     "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                
                if(r == DialogResult.Yes)
                {
                    if(Banco.ComandaDAO.Lancar(comanda))
                    {
                        MessageBox.Show("O produto foi lançado na comanda.");
                        Resetar();
                    }
                    else
                    {
                        MessageBox.Show("Houve um erro ao efetuar o lançamento!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Resetar();
                }
            }

        }
    }
}
