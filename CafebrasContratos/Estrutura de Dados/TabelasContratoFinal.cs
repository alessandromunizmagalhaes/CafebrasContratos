﻿using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    #region :: Tabelas Filhas


    public class TabelaItensDoContratoFinal : Tabela
    {
        public Coluna ItemCode { get { return new ColunaVarchar("ItemCode", "Código do Item", 60); } }
        public Coluna ItemName { get { return new ColunaVarchar("ItemName", "Nome do Item", 100); } }
        public Coluna Percentual { get { return new ColunaPercent("PercItem", "Percentual"); } }
        public Coluna Diferencial { get { return new ColunaQuantity("Difere", "Diferencial"); } }

        public TabelaItensDoContratoFinal() : base("UPD_CFC1", "Itens do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }

    public class TabelaCorretoresDoContratoFinal : Tabela
    {
        public Coluna CodigoParticipante { get { return new ColunaVarchar("PartCode", "Código do Corretor", 30); } }
        public Coluna ItemName { get { return new ColunaPercent("PercCom", "Percentual"); } }
        public Coluna Percentual { get { return new ColunaAtivo(); } }

        public TabelaCorretoresDoContratoFinal() : base("UPD_CFC2", "Corretores do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }

    public class TabelaResponsaveisDoContratoFinal : Tabela
    {
        public Coluna CodigoParticipante { get { return new ColunaVarchar("PartCode", "Código do Corretor", 30); } }
        public Coluna ItemName { get { return new ColunaPercent("PercCom", "Percentual"); } }
        public Coluna Percentual { get { return new ColunaAtivo(); } }

        public TabelaResponsaveisDoContratoFinal() : base("UPD_CFC3", "Responsáveis do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }

    public class TabelaCertificadosDoContratoFinal : Tabela
    {
        public Coluna MyProperty { get { return new ColunaVarchar("Certif", "Certificado", 30); } }

        public TabelaCertificadosDoContratoFinal() : base("UPD_CFC4", "Certificados do Contrato", BoUTBTableType.bott_MasterDataLines)
        {

        }
    }


    #endregion


    #region :: Contrato Final

    public class TabelaContratoFinal : TabelaUDO
    {
        #region :: Cabecalho

        public Coluna NumeroDoContrato { get { return new ColunaInt("DocNumCF", "Numero do Contrato"); } }
        public Coluna NumeroDoPreContrato { get { return new ColunaInt("DocNumCC", "Numero do PréContrato"); } }
        public Coluna StatusQualidade
        {
            get
            {
                return new ColunaVarchar("StatusQua", "Status Qualidade", 1, false, StatusContratoFinalQualidade.PreAprovado, new List<ValorValido>(){
                    new ValorValido(StatusContratoFinalQualidade.PreAprovado, "Pré Aprovado"),
                    new ValorValido(StatusContratoFinalQualidade.Aprovado, "Aprovado"),
                    new ValorValido(StatusContratoFinalQualidade.NaoAprovado, "Não Aprovado"),
                });
            }
        }
        public Coluna StatusContrato
        {
            get
            {
                return new ColunaVarchar("StatusCtr", "Status Contrato", 1, false, StatusContratoFinal.Aberto, new List<ValorValido>(){
                    new ValorValido(StatusContratoFinal.Aberto, "Aberto"),
                    new ValorValido(StatusContratoFinal.Renegociacao, "Renegociação"),
                    new ValorValido(StatusContratoFinal.Liberado, "Liberado"),
                    new ValorValido(StatusContratoFinal.Autorizado, "Autorizado"),
                    new ValorValido(StatusContratoFinal.Cancelado, "Cancelado"),
                    new ValorValido(StatusContratoFinal.Fechado, "Fechado"),
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
        public Coluna Transportadora { get { return new ColunaVarchar("Transp", "Cardcode Transportadora", 100); } }
        public Coluna ValorSeguro { get { return new ColunaPrice("VSeguro", "Valor Seguro"); } }
        public Coluna LocalRetirada { get { return new ColunaVarchar("LocalRet", "Local de Retirada", 100); } }
        public Coluna NomeEstrangeiro { get { return new ColunaVarchar("FrgnName", "Nome estrangeiro do PN", 100); } }

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



        #region :: Campos de Peneira

        public Coluna Peneira01 { get { return new ColunaInt("P01", "Peneira 01"); } }
        public Coluna Peneira02 { get { return new ColunaInt("P02", "Peneira 02"); } }
        public Coluna Peneira03 { get { return new ColunaInt("P03", "Peneira 03"); } }
        public Coluna Peneira04 { get { return new ColunaInt("P04", "Peneira 04"); } }
        public Coluna Peneira05 { get { return new ColunaInt("P05", "Peneira 05"); } }
        public Coluna Peneira06 { get { return new ColunaInt("P06", "Peneira 06"); } }
        public Coluna Peneira07 { get { return new ColunaInt("P07", "Peneira 07"); } }
        public Coluna Peneira08 { get { return new ColunaInt("P08", "Peneira 08"); } }
        public Coluna Peneira09 { get { return new ColunaInt("P09", "Peneira 09"); } }
        public Coluna Peneira10 { get { return new ColunaInt("P10", "Peneira 10"); } }
        public Coluna Peneira11 { get { return new ColunaInt("P11", "Peneira 11"); } }
        public Coluna Peneira12 { get { return new ColunaInt("P12", "Peneira 12"); } }
        public Coluna Peneira13 { get { return new ColunaInt("P13", "Peneira 13"); } }
        public Coluna Peneira14 { get { return new ColunaInt("P14", "Peneira 14"); } }
        public Coluna Peneira15 { get { return new ColunaInt("P15", "Peneira 15"); } }


        public Coluna Diferencial01 { get { return new ColunaQuantity("D01", "Diferencial 01"); } }
        public Coluna Diferencial02 { get { return new ColunaQuantity("D02", "Diferencial 02"); } }
        public Coluna Diferencial03 { get { return new ColunaQuantity("D03", "Diferencial 03"); } }
        public Coluna Diferencial04 { get { return new ColunaQuantity("D04", "Diferencial 04"); } }
        public Coluna Diferencial05 { get { return new ColunaQuantity("D05", "Diferencial 05"); } }
        public Coluna Diferencial06 { get { return new ColunaQuantity("D06", "Diferencial 06"); } }
        public Coluna Diferencial07 { get { return new ColunaQuantity("D07", "Diferencial 07"); } }
        public Coluna Diferencial08 { get { return new ColunaQuantity("D08", "Diferencial 08"); } }
        public Coluna Diferencial09 { get { return new ColunaQuantity("D09", "Diferencial 09"); } }
        public Coluna Diferencial10 { get { return new ColunaQuantity("D10", "Diferencial 10"); } }
        public Coluna Diferencial11 { get { return new ColunaQuantity("D11", "Diferencial 11"); } }
        public Coluna Diferencial12 { get { return new ColunaQuantity("D12", "Diferencial 12"); } }
        public Coluna Diferencial13 { get { return new ColunaQuantity("D13", "Diferencial 13"); } }
        public Coluna Diferencial14 { get { return new ColunaQuantity("D14", "Diferencial 14"); } }
        public Coluna Diferencial15 { get { return new ColunaQuantity("D15", "Diferencial 15"); } }

        #endregion


        public TabelaContratoFinal() : base(
            "UPD_OCFC"
            , "Contrato de Compra Final"
            , BoUTBTableType.bott_MasterData
            , new UDOParams() { CanDelete = BoYesNoEnum.tNO, CanCancel = BoYesNoEnum.tNO }
            , new List<Tabela>() {
                    new TabelaItensDoContratoFinal()
                    , new TabelaCorretoresDoContratoFinal()
                    , new TabelaResponsaveisDoContratoFinal()
                    , new TabelaCertificadosDoContratoFinal()
                }
            )
        {

        }
    }


    #endregion
}