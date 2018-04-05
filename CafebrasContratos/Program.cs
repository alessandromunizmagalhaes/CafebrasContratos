using SAPbobsCOM;
using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{

    static class Program
    {
        private static string _addonName = "Cafébras Contratos";
        public static Application _sBOApplication;
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
            Dialogs.Info(":: " + _addonName + " :: Criando tabelas e estruturas de dados ...", BoMessageTime.bmt_Long);

            try
            {
                _company.StartTransaction();


                //Database.ExcluirTabela("UPD_OCCC");

                var Modalidade = new TabelaUDO(
                            "UPD_OMOD"
                            , "Modalidade do Contrato"
                            , BoUTBTableType.bott_MasterData
                            , new List<Coluna>() { }
                            , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                        );

                var UnidadeComercial = new TabelaUDO(
                        "UPD_OUCM"
                        , "Unidade Comercial do Contrato"
                        , BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                    );


                var TipoOperacao = new TabelaUDO(
                        "UPD_OTOP"
                        , "Tipo de Operação do Contrato"
                        , BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                    );

                var MetodoFinanceiro = new TabelaUDO(
                        "UPD_OMFN"
                        , "Método Financeiro do Contrato"
                        , BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                    );

                var Safra = new TabelaUDO(
                        "UPD_OSAF"
                        , "Safra do Item"
                        , BoUTBTableType.bott_MasterData
                        , new List<Coluna>() { }
                        , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                    );

                Database.CriarTabela(Modalidade);
                Database.CriarTabela(UnidadeComercial);
                Database.CriarTabela(TipoOperacao);
                Database.CriarTabela(MetodoFinanceiro);
                Database.CriarTabela(Safra);

                var valores_validos_status_contrato = new List<ValorValido>() { };
                foreach (var status in Contrato._status)
                {
                    valores_validos_status_contrato.Add(new ValorValido(status.Key, status.Value));
                }

                Database.CriarTabela(
                    new TabelaUDO(
                        "UPD_OCCC"
                        , "Contrato de Compra Geral"
                        , BoUTBTableType.bott_MasterData
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
                            new ColunaVarchar("UnidCom", "Unidade Comercial", 30, true),
                            new ColunaVarchar("TipoOper", "Tipo Operação", 30, true),
                            new ColunaVarchar("MtdFin", "Método Financeiro", 30, true),

                            new ColunaVarchar("ItemCode", "Código do Item", 60,true),
                            new ColunaVarchar("ItemName", "Nome do Item", 100,true),
                            new ColunaVarchar("WhsCode", "Depósito do Item", 8,true),
                            new ColunaVarchar("Safra", "Safra", 30, true),
                            new ColunaVarchar("Usage", "Utilização", 10, true),
                            new ColunaQuantity("Difere", "Diferencial do Item", true),
                            new ColunaVarchar("Packg", "Embalagem", 30, true),
                            new ColunaPrice("RateNY", "Câmbio moeda em NY", true),
                            new ColunaPrice("RateUSD", "Câmbio moeda Dolar USA", true),
                            new ColunaVarchar("Bebida", "Descrição Bebida", 20, true),

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
                            new Tabela("UPD_CCC1", "Detalhes do Item", BoUTBTableType.bott_MasterDataLines, new List<Coluna>(){
                                new ColunaVarchar("ItemCode","Código do Item",60),
                                new ColunaVarchar("ItemName","Nome do Item",100),
                                new ColunaPercent("PercItem","Percentual"),
                                new ColunaQuantity("Difere","Diferencial"),
                            })
                        }
                    )
                );

                _company.EndTransaction(BoWfTransOpt.wf_Commit);
            }
            catch (DatabaseException e)
            {
                Dialogs.PopupError(e.Message);
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao criar estrutura de dados.\nErro: " + e.Message);
                if (_company.InTransaction)
                {
                    _company.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
            }
        }

        private static void CriarMenus()
        {
            Dialogs.Info(":: " + _addonName + " :: Criando menus ...");

            try
            {
                RemoverMenu();

                Menu.CriarMenus(AppDomain.CurrentDomain.BaseDirectory + @"/criar_menus.xml");
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro ao inserir menus.\nErro: " + e.Message);
            }
        }

        private static void RemoverMenu()
        {
            Menu.RemoverMenus(AppDomain.CurrentDomain.BaseDirectory + @"/remover_menus.xml");
        }

        private static void DeclararEventos()
        {
            var eventFilters = new EventFilters();
            eventFilters.Add(BoEventTypes.et_MENU_CLICK);

            try
            {
                var formPreContrato = new FormPreContrato();
                var formDetalheItem = new FormDetalheItem();

                FormEvents.DeclararEventos(eventFilters, new List<MapEventsToForms>() {
                    new MapEventsToForms(BoEventTypes.et_FORM_VISIBLE, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formDetalheItem
                    }),
                    new MapEventsToForms(BoEventTypes.et_COMBO_SELECT, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_CHOOSE_FROM_LIST, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formDetalheItem
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_ADD, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_UPDATE, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_LOAD, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_FORM_CLOSE, formDetalheItem),
                    new MapEventsToForms(BoEventTypes.et_ITEM_PRESSED, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formDetalheItem
                    }),
                });

                FormEvents.DeclararEventosInternos(EventosInternos.AdicionarNovo, formPreContrato);
                FormEvents.DeclararEventosInternos(EventosInternos.Pesquisar, formPreContrato);
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
            _sBOApplication.FormDataEvent += FormEvents.FormDataEvent;
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
