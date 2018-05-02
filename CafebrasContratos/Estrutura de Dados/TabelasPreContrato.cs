﻿using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    #region :: Tabelas Filhas


    public class TabelaItensDoPreContrato : Tabela
    {
        public Coluna ItemCode { get { return new ColunaVarchar("ItemCode", "Código do Item", 60); } }
        public Coluna ItemName { get { return new ColunaVarchar("ItemName", "Nome do Item", 100); } }
        public Coluna Percentual { get { return new ColunaPercent("PercItem", "Percentual"); } }
        public Coluna Diferencial { get { return new ColunaQuantity("Difere", "Diferencial"); } }

        public TabelaItensDoPreContrato() : base("UPD_CCC1", "Itens do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }

    public class TabelaCorretoresDoPreContrato : Tabela
    {
        public Coluna CodigoParticipante { get { return new ColunaVarchar("PartCode", "Código do Corretor", 30); } }
        public Coluna ItemName { get { return new ColunaPercent("PercCom", "Percentual"); } }
        public Coluna Percentual { get { return new ColunaAtivo(); } }

        public TabelaCorretoresDoPreContrato() : base("UPD_CCC2", "Corretores do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }

    public class TabelaResponsaveisDoPreContrato : Tabela
    {
        public Coluna CodigoParticipante { get { return new ColunaVarchar("PartCode", "Código do Corretor", 30); } }
        public Coluna ItemName { get { return new ColunaPercent("PercCom", "Percentual"); } }
        public Coluna Percentual { get { return new ColunaAtivo(); } }

        public TabelaResponsaveisDoPreContrato() : base("UPD_CCC3", "Responsáveis do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }

    public class TabelaCertificadosDoPreContrato : Tabela
    {
        public Coluna MyProperty { get { return new ColunaVarchar("Certif", "Certificado", 30); } }

        public TabelaCertificadosDoPreContrato() : base("UPD_CCC4", "Certificados do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }


    #endregion


    #region :: Pre Contrato

    public class TabelaPreContrato : TabelaUDO
    {
        #region :: Cabecalho

        public Coluna NumeroDoContrato { get { return new ColunaInt("DocNumCC", "Numero do Contrato"); } }
        public Coluna DataInicial { get { return new ColunaDate("DataIni", "Data Inicial"); } }
        public Coluna DataFinal { get { return new ColunaDate("DataFim", "Data Final"); } }
        public Coluna Status
        {
            get
            {
                return new ColunaVarchar("StatusQua", "Situação", 1, false, "A", new List<ValorValido>(){
                    new ValorValido(StatusContrato.Aberto, "Aberto"),
                    new ValorValido(StatusContrato.Autorizado, "Autorizado"),
                    new ValorValido(StatusContrato.Cancelado, "Cancelado"),
                });
            }
        }
        public Coluna Descricao { get { return new ColunaVarchar("Descricao", "Descrição", 254); } }

        #endregion


        #region :: Aba Definições Gerais

        public Coluna CardCode { get { return new ColunaVarchar("CardCode", "Código do PN", 15); } }
        public Coluna CardName { get { return new ColunaVarchar("CardName", "Descrição do PN", 100); } }
        public Coluna PessoaDeContato { get { return new ColunaVarchar("CtName", "Contato do PN", 50); } }
        public Coluna Telefone { get { return new ColunaVarchar("Tel1", "Telefone do Contato", 15); } }
        public Coluna Email { get { return new ColunaVarchar("EMail", "E-mail do Contato", 50); } }
        public Coluna PrevisaoEntrega { get { return new ColunaDate("DtPrEnt", "Previsão de Entrega"); } }
        public Coluna PrevisaoPgto { get { return new ColunaDate("DtPrPgt", "Previsão de Pagamento"); } }
        public Coluna Modalidade { get { return new ColunaVarchar("ModCtto", "Modalidade", 30); } }
        public Coluna UnidadeComercial { get { return new ColunaVarchar("UnidCom", "Unidade Comercial", 30); } }
        public Coluna TipoOperacao { get { return new ColunaVarchar("TipoOper", "Tipo Operação", 30); } }
        public Coluna MetodoFinanceiro { get { return new ColunaVarchar("MtdFin", "Método Financeiro", 30); } }

        #endregion


        #region :: Aba de Itens

        public Coluna ItemCode { get { return new ColunaVarchar("ItemCode", "Código do Item", 60); } }
        public Coluna ItemName { get { return new ColunaVarchar("ItemName", "Nome do Item", 100); } }
        public Coluna Deposito { get { return new ColunaVarchar("WhsCode", "Depósito do Item", 8); } }
        public Coluna Safra { get { return new ColunaVarchar("Safra", "Safra", 30); } }
        public Coluna Utilizacao { get { return new ColunaVarchar("Usage", "Utilização", 10); } }
        public Coluna Diferencial { get { return new ColunaQuantity("Difere", "Diferencial do Item"); } }
        public Coluna Embalagem { get { return new ColunaVarchar("Packg", "Embalagem", 30); } }
        public Coluna CambioNY { get { return new ColunaPrice("RateNY", "Câmbio moeda em NY"); } }
        public Coluna CambioUSD { get { return new ColunaPrice("RateUSD", "Câmbio moeda Dolar USA"); } }
        public Coluna Bebida { get { return new ColunaVarchar("Bebida", "Descrição Bebida", 20); } }

        #endregion


        #region :: Valores

        public Coluna ValorFaturado { get { return new ColunaPrice("VFat", "Valor Faturado por saca"); } }
        public Coluna ValorICMS { get { return new ColunaPrice("VICMS", "Valor ICMS por saca"); } }
        public Coluna ValorSENAR { get { return new ColunaPrice("VSenar", "Valor Senar por saca"); } }
        public Coluna ValorLivre { get { return new ColunaPrice("VLivre", "Valor Livre por saca"); } }
        public Coluna ValorBruto { get { return new ColunaPrice("VBruto", "Valor Bruto por saca"); } }
        public Coluna QuantidadePeso { get { return new ColunaQuantity("QtdPeso", "Qtd de Peso"); } }
        public Coluna QuantidadeSaca { get { return new ColunaQuantity("QtdSaca", "Qtd de Sacas"); } }
        public Coluna TotalFaturado { get { return new ColunaPrice("TFat", "Total Faturado por saca"); } }
        public Coluna TotalICMS { get { return new ColunaPrice("TICMS", "Total ICMS por saca"); } }
        public Coluna TotalSENAR { get { return new ColunaPrice("TSenar", "Total Senar por saca"); } }
        public Coluna TotalLivre { get { return new ColunaPrice("TLivre", "Total Livre por saca"); } }
        public Coluna TotalBruto { get { return new ColunaPrice("TBruto", " Total Bruto por saca"); } }
        public Coluna SaldoPeso { get { return new ColunaQuantity("SPesoRec", "Saldo de Peso recebido"); } }
        public Coluna SaldoPesoSemContrato { get { return new ColunaQuantity("SPesoNCT", "Saldo de Peso sem contrato"); } }
        public Coluna SaldoSacasRecebido { get { return new ColunaPrice("SScRec", "Saldo de sacas recebido"); } }
        public Coluna SaldoSacasSemContrato { get { return new ColunaPrice("SScNCT", "Saldo de sacas sem contrato"); } }
        public Coluna SaldoFinanceiro { get { return new ColunaPrice("SFin", "Saldo financeiro"); } }
        public Coluna ValorFrete { get { return new ColunaPrice("VlrFrete", "Valor do frete"); } }
        public Coluna ObservacoesIniciais { get { return new ColunaText("ObsIni", "Observações Iniciais"); } }
        public Coluna ObservacoesFinais { get { return new ColunaText("ObsFim", "Observações Finais"); } }

        #endregion


        #region :: Versão 0.2

        public Coluna Transportadora { get { return new ColunaVarchar("Transp", "Cardcode Transportadora", 100); } }

        #endregion


        public TabelaPreContrato() : base(
            "UPD_OCCC"
            , "Contrato de Compra Geral"
            , BoUTBTableType.bott_MasterData
            , new UDOParams() { CanDelete = BoYesNoEnum.tNO, CanCancel = BoYesNoEnum.tNO }
            , new List<Tabela>() {
                    new TabelaItensDoPreContrato()
                    , new TabelaCorretoresDoPreContrato()
                    , new TabelaResponsaveisDoPreContrato()
                    , new TabelaCertificadosDoPreContrato()
                }
            )
        {

        }
    }


    #endregion
}