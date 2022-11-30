using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaFMWorks;
using MySqlConnector;


namespace SistemaPadaria.Banco
{
    internal class ProdutoDAO
    {
        public static int Remover(int id)
        {
            string comando = "DELETE FROM produtos WHERE id = @id";

            ConexaoBD conexaoBD = new ConexaoBD();
            MySqlConnection con = conexaoBD.ObterConexao();
            MySqlCommand cmd = new MySqlCommand(comando, con);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            try
            {
                int linhasRemovidas = cmd.ExecuteNonQuery();
                conexaoBD.Desconectar(con);
                return linhasRemovidas;
            }
            catch
            {
                conexaoBD.Desconectar(con);
                // Em caso de erro, devolver -1:
                return -1;
            }
        }

        public static DataTable ListarTudo()
        {
            string comando = "SELECT id AS 'ID', nome AS 'Nome', preco AS 'Preço', " +
                "id_categoria AS 'IdCategoria', id_respcadastro AS 'IdRespCadastro' FROM produtos";
            
            ConexaoBD conexaoBD = new ConexaoBD();
            MySqlConnection con = conexaoBD.ObterConexao();
            MySqlCommand cmd = new MySqlCommand(comando, con);

            cmd.Prepare();

            // Declarar tabela que irá receber o resultado:
            DataTable tabela = new DataTable();
            // Preencher a tabela com o resultado da consulta
            tabela.Load(cmd.ExecuteReader());

            conexaoBD.Desconectar(con);

            return tabela;
        }

        public static bool Cadastrar(Produto p)
        {
            string comando = "INSERT INTO produtos (nome, preco, id_categoria, id_respcadastro) " +
                "VALUES (@nome, @preco, @id_categoria, @id_respcadastro)";

            ConexaoBD conexaoBD = new ConexaoBD();
            MySqlConnection con = conexaoBD.ObterConexao();
            MySqlCommand cmd = new MySqlCommand(comando, con);

            
            cmd.Parameters.AddWithValue("@nome", p.Nome);
            cmd.Parameters.AddWithValue("@preco", p.Preco);           
            cmd.Parameters.AddWithValue("@id_categoria", p.IdCategoria);
            cmd.Parameters.AddWithValue("@id_respcadastro", p.IdRespCadastro);

            cmd.Prepare();

            try
            {
                if (cmd.ExecuteNonQuery() != 0)
                {
                    conexaoBD.Desconectar(con);
                    return true;
                }
                else
                {
                    conexaoBD.Desconectar(con);
                    return false;
                }
            }
            catch
            {
                conexaoBD.Desconectar(con);
                return false;
            }

        }


        public static bool Editar(Produto p)
        {
            string comando = "UPDATE produtos SET nome = @nome, " +
                "preco = @preco, id_categoria = @id_categoria, " +
                "id_respcadastro = @id_respcadastro WHERE id = @id";

            ConexaoBD conexaoBD = new ConexaoBD();
            MySqlConnection con = conexaoBD.ObterConexao();
            MySqlCommand cmd = new MySqlCommand(comando, con);

            cmd.Parameters.AddWithValue("@id", p.Id);
            cmd.Parameters.AddWithValue("@nome", p.Nome);
            cmd.Parameters.AddWithValue("@preco", p.Preco);
            cmd.Parameters.AddWithValue("@id_categoria", p.IdCategoria);
            cmd.Parameters.AddWithValue("@id_respcadastro", p.IdRespCadastro);


            cmd.Prepare();

            try
            {
                if (cmd.ExecuteNonQuery() != 0)
                {
                    conexaoBD.Desconectar(con);
                    return true;
                }
                else
                {
                    conexaoBD.Desconectar(con);
                    return false;
                }
            }
            catch
            {
                conexaoBD.Desconectar(con);
                return false;
            }

        }
    }
}
