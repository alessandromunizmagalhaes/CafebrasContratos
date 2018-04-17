using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public static class DbPreContrato
    {
        #region :: Tabelas Filhas

        public static Tabela itensDoContrato = new Tabela(
                                        "UPD_CCC1"
                                        , "Itens do Contrato"
                                        , BoUTBTableType.bott_MasterDataLines
                                        , new List<Coluna>(){
                                            new ColunaVarchar("ItemCode","Código do Item",60),
                                            new ColunaVarchar("ItemName","Nome do Item",100),
                                            new ColunaPercent("PercItem","Percentual"),
                                            new ColunaQuantity("Difere","Diferencial"),
                                    });


        public static Tabela corretoresDoContrato = new Tabela(
                                    "UPD_CCC2"
                                    , "Corretores do Contrato"
                                    , BoUTBTableType.bott_MasterDataLines
                                    , new List<Coluna>(){
                                        new ColunaVarchar("PartCode","Código do Corretor",30),
                                        new ColunaPercent("PercCom","Percentual"),
                                        new ColunaAtivo()
                                    });

        public static Tabela responsaveisDoContrato = new Tabela(
                                    "UPD_CCC3"
                                    , "Responsáveis do Contrato"
                                    , BoUTBTableType.bott_MasterDataLines
                                    , new List<Coluna>(){
                                        new ColunaVarchar("PartCode","Código do Responsável",30),
                                        new ColunaPercent("PercCom","Percentual"),
                                        new ColunaAtivo()
                                    });

        public static Tabela certificadosDoContrato = new Tabela(
                                    "UPD_CCC4"
                                    , "Certificados do Contrato"
                                    , BoUTBTableType.bott_MasterDataLines
                                    , new List<Coluna>(){
                                        new ColunaVarchar("Certif","Certificado",30)
                                    });

        #endregion


        #region :: Tabela 

        public static TabelaUDO preContrato = new TabelaUDO(
                                "UPD_OCCC"
                                , "Contrato de Compra Geral"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>()
                                {
                                    new ColunaInt("DocNumCC","Numero do Contrato"),
                                    new ColunaDate("DataIni","Data Inicial"),
                                    new ColunaDate("DataFim","Data Final"),
                                    new ColunaVarchar("StatusQua","Situação",1, false,"A", new List<ValorValido>(){
                                        new ValorValido(StatusContrato.Aberto, "Aberto"),
                                        new ValorValido(StatusContrato.Autorizado, "Autorizado"),
                                        new ValorValido(StatusContrato.Cancelado, "Cancelado"),
                                    }),
                                    new ColunaVarchar("Descricao","Descrição",254),

                                    new ColunaVarchar("CardCode","Código do PN",15),
                                    new ColunaVarchar("CardName","Descrição do PN",100),
                                    new ColunaVarchar("CtName", "Contato do PN",50),
                                    new ColunaVarchar("Tel1", "Telefone do Contato",15),
                                    new ColunaVarchar("EMail", "E-mail do Contato",50),
                                    new ColunaDate("DtPrEnt", "Previsão de Entrega"),
                                    new ColunaDate("DtPrPgt", "Previsão de Pagamento"),
                                    new ColunaVarchar("ModCtto", "Modalidade", 30),
                                    new ColunaVarchar("UnidCom", "Unidade Comercial", 30),
                                    new ColunaVarchar("TipoOper", "Tipo Operação", 30),
                                    new ColunaVarchar("MtdFin", "Método Financeiro", 30),

                                    new ColunaVarchar("ItemCode", "Código do Item", 60),
                                    new ColunaVarchar("ItemName", "Nome do Item", 100),
                                    new ColunaVarchar("WhsCode", "Depósito do Item", 8),
                                    new ColunaVarchar("Safra", "Safra", 30),
                                    new ColunaVarchar("Usage", "Utilização", 10),
                                    new ColunaQuantity("Difere", "Diferencial do Item"),
                                    new ColunaVarchar("Packg", "Embalagem", 30),
                                    new ColunaPrice("RateNY", "Câmbio moeda em NY"),
                                    new ColunaPrice("RateUSD", "Câmbio moeda Dolar USA"),
                                    new ColunaVarchar("Bebida", "Descrição Bebida", 20),

                                    new ColunaPrice("VFat", "Valor Faturado por saca"),
                                    new ColunaPrice("VICMS", "Valor ICMS por saca"),
                                    new ColunaPrice("VSenar", "Valor Senar por saca"),
                                    new ColunaPrice("VLivre", "Valor Livre por saca"),
                                    new ColunaPrice("VBruto", "Valor Bruto por saca"),
                                    new ColunaQuantity("QtdPeso", "Qtd de Peso"),
                                    new ColunaQuantity("QtdSaca", "Qtd de Sacas"),
                                    new ColunaPrice("TFat", "Total Faturado por saca"),
                                    new ColunaPrice("TICMS", "Total ICMS por saca"),
                                    new ColunaPrice("TSenar", "Total Senar por saca"),
                                    new ColunaPrice("TLivre", "Total Livre por saca"),
                                    new ColunaPrice("TBruto", " Total Bruto por saca"),
                                    new ColunaQuantity("SPesoRec", "Saldo de Peso recebido"),
                                    new ColunaQuantity("SPesoNCT", "Saldo de Peso sem contrato"),
                                    new ColunaPrice("SScRec", "Saldo de sacas recebido"),
                                    new ColunaPrice("SScNCT", "Saldo de sacas sem contrato"),
                                    new ColunaPrice("SFin", "Saldo financeiro"),
                                    new ColunaPrice("VlrFrete", "Valor do frete"),
                                    new ColunaText("ObsIni", "Observações Iniciais"),
                                    new ColunaText("ObsFim", "Observações Finais"),
                                }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO, CanCancel = BoYesNoEnum.tNO }
                                , new List<Tabela>() {
                                    itensDoContrato
                                    , corretoresDoContrato
                                    , responsaveisDoContrato
                                    , certificadosDoContrato
                                }
                            );

        #endregion

    }
}
