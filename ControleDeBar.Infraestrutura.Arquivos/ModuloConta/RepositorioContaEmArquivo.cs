﻿using controleDeBar.Dominio.ModuloConta;
using ControleDeBar.Infraestrura.Arquivos.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeBar.Infraestrutura.Arquivos.ModuloConta
{
    public class RepositorioContaEmArquivo : IRepositorioConta
    {
        private readonly ContextoDados contexto;
        protected readonly List<Conta> registros;

        public RepositorioContaEmArquivo(ContextoDados contexto)
        {
            this.contexto = contexto;
            registros = contexto.Contas;
        }

        public void CadastrarConta(Conta novaConta)
        {
            registros.Add(novaConta);

            contexto.Salvar();
        }

        public Conta SelecionarPorId(Guid idRegistro)
        {
            foreach (var item in registros)
            {
                if (item.Id == idRegistro)
                    return item;
            }

            return null;
        }


        public List<Conta> SelecionarContas()
        {
            return registros;
        }

        public List<Conta> SelecionarContasAbertas()
        {
            var contasAbertas = new List<Conta>();

            foreach (var item in registros)
            {
                if (item.EstaAberta == true)
                    contasAbertas.Add(item);
            }

            return contasAbertas;
        }

        public List<Conta> SelecionarContasFechadas()
        {
            var contasFechadas = new List<Conta>();

            foreach (var item in registros)
            {
                if (item.EstaAberta == false)
                    contasFechadas.Add(item);
            }

            return contasFechadas;
        }

        public List<Conta> SelecionarContasPorPeriodo(DateTime data)
        {
            var contasDoPeriodo = new List<Conta>();

            foreach (var item in registros)
            {
                if (item.Fechamento?.Date == data.Date)
                    contasDoPeriodo.Add(item);
            }

            return contasDoPeriodo;
        }

        public void AdicionarPedido(Pedido pedido)
        {
            contexto.Salvar();
        }

        public void RemoverPedido(Pedido pedidoRemovido)
        {
            contexto.Salvar();
        }

        public void Fechar()
        {
           contexto.Salvar();
        }

        public void Fechar(Conta conta)
        {
            contexto.Salvar();
        }
    }
}
