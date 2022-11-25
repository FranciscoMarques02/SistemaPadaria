using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BibliotecaFMWorks;

namespace SistemaPadaria.Views
{
    public partial class MenuProdutos : Form
    {
        //Global:
        Usuario usuario;
        int idSelecionado;

        public MenuProdutos(Usuario usuario)
        {
            InitializeComponent();

            // Salvar o resultado da listagem de categorias em um objeto:
            var r = Banco.CategoriaDAO.ListarTudo();
            ArrayList rows = new ArrayList();
            // Converter esse objeto para array:
            foreach (DataRow dataRow in r.Rows)
            {
                rows.Add(string.Join(" - ", dataRow.ItemArray.Select(item => item.ToString())));
            }
            // Atribuir os valores nos cmbs:
            cmbCategoriaCad.DataSource = rows;
            cmbCategoriaEdi.DataSource = rows.Clone();



            //Atribuir o local no global:
            this.usuario = usuario;
            //Mudar o label:
            lblInformacao.Text = "Você está logado como " + usuario.NomeCompleto;
            AtualizarDgv();
        }

        public void AtualizarDgv()
        {
            //Carregar os dados do banco no dgv:
            dgvProdutos.DataSource = Banco.ProdutoDAO.ListarTudo();
        }

        private int obterIDdaString(string texto)
        {
            // Ao chamar obterIDdaString("12 - Alimentação")            
            return int.Parse(texto.Split(' ')[0]);
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            var r = MessageBox.Show("Tem certeza que deseja remover o seguinte produto?\n"
                + lblRemover.Text, "Atenção!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                //Apagar:
                if (Banco.ProdutoDAO.Remover(this.idSelecionado) != -1)
                {
                    MessageBox.Show("Produto removido com sucesso!", "",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);

                    AtualizarDgv();
                    //Desativar os groupboxes:
                    grbEditar.Enabled = false;
                    grbRemover.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Erro ao remover o produto!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProdutos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ativar os groupboxes:            
            grbEditar.Enabled = true;
            grbRemover.Enabled = true;

            // Obter o número da linha selecionada:
            int linhaSelecionada = dgvProdutos.CurrentCell.RowIndex;
            // Obter toda a linha selecionada:
            var dadosLinha = dgvProdutos.Rows[linhaSelecionada];

            // Popular os campos do editar:
            txbNomeEdi.Text = dadosLinha.Cells[1].Value.ToString();
            txbPrecoEdi.Text = dadosLinha.Cells[2].Value.ToString();
            

            // Popular o remover:
            lblRemover.Text = dadosLinha.Cells[0].Value.ToString() + " - "
                 + dadosLinha.Cells[1].Value.ToString();

            // Salvar o id na variável global:
            this.idSelecionado = (int)dadosLinha.Cells[0].Value;
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            //Verificar se os campos estão vazios:
            if (txbNomeCad.Text.Length >= 2 && txbPrecoCad.Text.Length >= 1)
            {
                //Instanciar o produto:
                Produto produto = new Produto();
                try
                {
                    produto.Nome = txbNomeCad.Text;
                    produto.Preco = double.Parse(txbPrecoCad.Text.Replace('.', ','));
                    produto.IdCategoria = obterIDdaString(cmbCategoriaCad.Text);

                    produto.IdRespCadastro = usuario.Id;

                    //Enviar para o banco e verificar se deu certo:
                    if (Banco.ProdutoDAO.Cadastrar(produto))
                    {
                        MessageBox.Show("Produto cadastrado com sucesso!");
                        //Limpar os campos
                        txbNomeCad.Clear();
                        txbPrecoCad.Clear();
                        
                        //Atualizar o dgv:
                        AtualizarDgv();
                    }
                    else
                    {
                        MessageBox.Show("Verifique as informações digitadas!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }               
                catch
                {
                    MessageBox.Show("Erro ao cadastrar produto!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Verifique as informações digitadas!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            
            if (txbNomeEdi.Text.Length >= 2 && txbPrecoEdi.Text.Length >= 1)
            {
                Produto produto = new Produto();
                try
                {
                    produto.Id = idSelecionado;
                    produto.Nome = txbNomeEdi.Text;
                    produto.Preco = double.Parse(txbPrecoEdi.Text.Replace('.',','));
                    produto.IdCategoria = obterIDdaString(cmbCategoriaEdi.Text);

                    produto.IdRespCadastro = usuario.Id;


                    if (Banco.ProdutoDAO.Editar(produto))
                    {
                        MessageBox.Show("Produto editado com sucesso!");
                        //Limpar os campos
                        txbNomeEdi.Clear();
                        txbPrecoEdi.Clear();
                        
                        //Atualizar o dgv:
                        AtualizarDgv();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao editar produto!", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("Verifique as informações digitadas!", "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Verifique as informações digitadas!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
