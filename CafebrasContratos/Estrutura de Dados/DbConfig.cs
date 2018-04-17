using SAPbobsCOM;
using SAPHelper;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public static class DbConfig
    {
        #region :: Tabelas

        public static Tabela modalidade = new TabelaUDO(
                                    "UPD_OMOD"
                                    , "Cadastro de Modalidade"
                                    , BoUTBTableType.bott_MasterData
                                    , new List<Coluna>() { }
                                    , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                                );

        public static Tabela unidadeComercial = new TabelaUDO(
                                "UPD_OUCM"
                                , "Cadastro de Unidade Comercial"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

        public static Tabela tipoOperacao = new TabelaUDO(
                                "UPD_OTOP"
                                , "Cadastro de Tipo de Operação"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

        public static Tabela metodoFinanceiro = new TabelaUDO(
                                "UPD_OMFN"
                                , "Cadastro de Método Financeiro"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

        public static Tabela safra = new TabelaUDO(
                                "UPD_OSAF"
                                , "Cadastro de Safra do Item"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

        public static Tabela certificado = new TabelaUDO(
                                "UPD_CRTC"
                                , "Cadastro do Certificado"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

        public static Tabela participante = new TabelaUDO(
                                "UPD_PART"
                                , "Cadastro de Participantes"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() {
                                    new ColunaVarchar("Tipo","Tipo",1,false,"C", new List<ValorValido>(){
                                        new ValorValido("C","Corretor"),
                                        new ValorValido("R","Responsável"),
                                    }),
                                }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO, EnableEnhancedForm = BoYesNoEnum.tNO }
                            );

        public static Tabela grupoDeCafe = new Tabela(
                                "UPD_OCTC"
                                , "Grupos de Café"
                                , BoUTBTableType.bott_NoObject
                                , new List<Coluna>() {
                                new ColunaVarchar("ItmsGrpCod","Código Grupo de Item", 30)
                                }
                            );

        #endregion


        #region :: Campos de Usuário padrao SAP

        public static Coluna grupoAprovador = new ColunaVarchar("GrupoAprov", "Grupo Aprovador", 2, false, "V", new List<ValorValido>() {
                            new ValorValido(GrupoAprovador.Planejador, "Planejador"),
                            new ValorValido(GrupoAprovador.Executor, "Executor"),
                            new ValorValido(GrupoAprovador.Autorizador, "Autorizador"),
                            new ValorValido(GrupoAprovador.Gestor, "Gestor"),
                            new ValorValido(GrupoAprovador.Visualizador, "Visualizador")
                        });

        #endregion

    }
}
