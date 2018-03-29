using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{

    static class Program
    {
        private static string _addonName = "Cafébras Contratos";
        public static SAPbouiCOM.Application _sBOApplication;
        public static SAPbobsCOM.Company _company;

        [STAThread]
        static void Main()
        {
            ConectarComSAP();

            CriarEstruturaDeDados();

            CriarMenus();

            DeclararEventos();

            Dialogs.Success(".:: " + _addonName + " ::. Iniciado", BoMessageTime.bmt_Medium);

            // deixa a aplicação ativa
            System.Windows.Forms.Application.Run();
        }

        private static void ConectarComSAP()
        {
            SAPConnection.applicationHandler += Global.RecebeSBOApplication;
            SAPConnection.applicationHandler += applicationParam => _sBOApplication = applicationParam;

            SAPConnection.companyHandler += Global.RecebeCompany;
            SAPConnection.companyHandler += companyParam => _company = companyParam;

            try
            {
                SAPConnection.Connect();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                System.Windows.Forms.Application.Exit();
            }
        }

        private static void CriarEstruturaDeDados()
        {
            Dialogs.Info(":: " + _addonName + " :: Criando tabelas e estruturas de dados ...");

            try
            {
                _company.StartTransaction();

                /*
                Database.ExcluirTabela("UPD_OCCC");

                Database.ExcluirTabela("UPD_OMOD");
                Database.ExcluirTabela("UPD_OUCM");
                Database.ExcluirTabela("UPD_OTOP");
                Database.ExcluirTabela("UPD_OMFN");
                */

                var Modalidade = new TabelaUDO(
                            "UPD_OMOD"
                            , "Modalidade do Contrato"
                            , SAPbobsCOM.BoUTBTableType.bott_MasterData
                            , new List<Coluna>() { }
                            , new UDOParams() { CanDelete = SAPbobsCOM.BoYesNoEnum.tNO }
                        );

                var UnidadeComercial = new TabelaUDO(
                        "UPD_OUCM"
                        , "Unidade Comercial do Contrato"
                        , SAPbobsCOM.BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = SAPbobsCOM.BoYesNoEnum.tNO }
                    );


                var TipoOperacao = new TabelaUDO(
                        "UPD_OTOP"
                        , "Tipo de Operação do Contrato"
                        , SAPbobsCOM.BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = SAPbobsCOM.BoYesNoEnum.tNO }
                    );

                var MetodoFinanceiro = new TabelaUDO(
                        "UPD_OMFN"
                        , "Método Financeiro do Contrato"
                        , SAPbobsCOM.BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = SAPbobsCOM.BoYesNoEnum.tNO }
                    );

                Database.CriarTabela(Modalidade);
                Database.CriarTabela(UnidadeComercial);
                Database.CriarTabela(TipoOperacao);
                Database.CriarTabela(MetodoFinanceiro);

                var valores_validos_status_contrato = new List<ValorValido>() { };
                foreach (var status in Contrato._status)
                {
                    valores_validos_status_contrato.Add(new ValorValido(status.Key, status.Value));
                }

                Database.CriarTabela(
                    new TabelaUDO(
                        "UPD_OCCC"
                        , "Contrato de Compra Geral"
                        , SAPbobsCOM.BoUTBTableType.bott_MasterData
                        , new List<Coluna>()
                        {
                            new ColunaInt("DocNumCC","Numero do Contrato",true),
                            new ColunaDate("DataIni","Data Inicial",true),
                            new ColunaDate("DataFim","Data Final",true),
                            new ColunaVarchar("StatusQua","Situação",1,true,"A", valores_validos_status_contrato),
                            new ColunaVarchar("Descricao","Descrição",254,true),
                            new ColunaVarchar("CardCode","Código do PN",15,true),
                            new ColunaVarchar("CardName","Descrição do PN",100,true),
                            new ColunaVarchar("CtName", "Contato do PN",50,true),
                            new ColunaVarchar("Tel1", "Telefone do Contato",15,true),
                            new ColunaVarchar("EMail", "E-mail do Contato",50,true),
                            new ColunaDate("DtPrEnt", "Previsão de Entrega",true),
                            new ColunaDate("DtPrPgt", "Previsão de Pagamento",true),
                            new ColunaVarchar("ModCtto", "Modalidade", 30, true),
                            new ColunaVarchar("UnidCom", "Unidade Comercial", 30),
                            new ColunaVarchar("TipoOper", "Tipo Operação", 30),
                            new ColunaVarchar("MtdFin", "Método Financeiro", 30),
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
                        }
                        , new UDOParams() { CanDelete = SAPbobsCOM.BoYesNoEnum.tNO, CanCancel = SAPbobsCOM.BoYesNoEnum.tNO }
                    )
                );

                _company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
            }
            catch (DatabaseException e)
            {
                Dialogs.PopupError(e.Message);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao criar estrutura de dados.\nErro: " + e.Message);
                _company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            }
        }

        private static void CriarMenus()
        {
            Dialogs.Info(":: " + _addonName + " :: Criando menus ...");

            try
            {
                RemoverMenu();

                Menu.CriarMenus(System.Windows.Forms.Application.StartupPath + @"/criar_menus.xml");
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao inserir menus.\nErro: " + e.Message);
            }
        }

        private static void RemoverMenu()
        {
            Menu.RemoverMenus(System.Windows.Forms.Application.StartupPath + @"/remover_menus.xml");
        }

        private static void DeclararEventos()
        {
            var eventFilters = new EventFilters();
            eventFilters.Add(BoEventTypes.et_MENU_CLICK);

            try
            {
                var formPreContrato = new FormPreContrato();

                FormEvents.DeclararEventos(eventFilters, new List<MapEventsToForms>() {
                    new MapEventsToForms(BoEventTypes.et_FORM_VISIBLE, formPreContrato)
                });

                FormEvents.DeclararEventosInternos(EventosInternos.AdicionarNovo, formPreContrato);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao declarar eventos de formulário.\nErro: " + e.Message);
            }

            try
            {
                Global.SBOApplication.SetFilter(eventFilters);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao setar eventos declarados da aplicação.\nErro: " + e.Message);
            }

            _sBOApplication.AppEvent += AppEvent;
            _sBOApplication.ItemEvent += FormEvents.ItemEvent;
            _sBOApplication.MenuEvent += Menu.MenuEvent;
        }

        #region :: Declaração Eventos

        private static void AppEvent(BoAppEventTypes EventType)
        {
            if (EventType == BoAppEventTypes.aet_ShutDown)
            {
                RemoverMenu();
            }
        }

        #endregion
    }
}
