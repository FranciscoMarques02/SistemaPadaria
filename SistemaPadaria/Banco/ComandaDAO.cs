using BibliotecaFMWorks;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPadaria.Banco
{
    public static class ComandaDAO
    {
        public static DataTable BuscarComanda(int id)
        {
            string comando = "SELECT * FROM view_fichas WHERE Ficha = @id";

            ConexaoBD conexaoBD = new ConexaoBD();
            MySqlConnection con = conexaoBD.ObterConexao();
            MySqlCommand cmd = new MySqlCommand(comando, con);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.Prepare();

            // Declarar tabela que irá receber o resultado:
            DataTable tabela = new DataTable();
            // Preencher a tabela com o resultado da consulta
            tabela.Load(cmd.ExecuteReader());

            conexaoBD.Desconectar(con);

            return tabela;
        }


        public static bool Lancar(Comanda comanda)
        {
            string comando = "INSERT INTO ordens_comandas (id_ficha, id_produto, quantidade, id_resp) " +
                "VALUES (@id_ficha, @id_produto, @quantidade, @id_resp)";

            ConexaoBD conexaoBD = new ConexaoBD();
            MySqlConnection con = conexaoBD.ObterConexao();
            MySqlCommand cmd = new MySqlCommand(comando, con);

            cmd.Parameters.AddWithValue("@id_ficha", comanda.IdFicha);
            cmd.Parameters.AddWithValue("@id_produto", comanda.IdProduto);
            cmd.Parameters.AddWithValue("@quantidade", comanda.Quantidade);
            cmd.Parameters.AddWithValue("@id_resp", comanda.IdResponsavel);

            cmd.Prepare();

            try
            {
                if (cmd.ExecuteNonQuery() == 0)
                {
                    conexaoBD.Desconectar(con);
                    return false;
                }
                else
                {
                    conexaoBD.Desconectar(con);
                    return true;
                }
            }
            catch
            {
                conexaoBD.Desconectar(con);
                return true;
            }
        }


    }
}
