﻿using controleDeBar.Dominio.ModuloProduto;
using ControleDeBarWebApp.Models;

namespace ControleDeBarWebApp.Extensions
{
    public static class ProdutoExtensions
    {
        public static Produto ParaEntidade(this FormularioProdutoViewModel formularioVM)
        {
            return new Produto(formularioVM.Nome, formularioVM.Valor);
        }

        public static DetalhesProdutoViewModel ParaDetalhesVM(this Produto produto)
        {
            return new DetalhesProdutoViewModel(produto.Id, produto.Nome, produto.Valor);
        }

    }
}
