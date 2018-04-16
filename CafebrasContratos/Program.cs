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
        public static string _grupoAprovador;

        [STAThread]
        static void Main()
        {
            ConectarComSAP();

            CriarEstruturaDeDados();

            CriarMenus();

            DeclararEventos();

            Dialogs.Success(".:: " + _addonName + " ::. Iniciado", BoMessageTime.bmt_Medium);

            SetGrupoAprovador();

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
                if (!ConfigXML.JaCriouEstrutura)
                {
                    _company.StartTransaction();

                    using (Database db = new Database())
                    {
                        //db.ExcluirTabela("UPD_OCCC");

                        var modalidade = new TabelaUDO(
                                    "UPD_OMOD"
                                    , "Cadastro de Modalidade"
                                    , BoUTBTableType.bott_MasterData
                                    , new List<Coluna>() { }
                                    , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                                );

                        var unidadeComercial = new TabelaUDO(
                                "UPD_OUCM"
                                , "Cadastro de Unidade Comercial"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );


                        var tipoOperacao = new TabelaUDO(
                                "UPD_OTOP"
                                , "Cadastro de Tipo de Operação"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

                        var metodoFinanceiro = new TabelaUDO(
                                "UPD_OMFN"
                                , "Cadastro de Método Financeiro"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

                        var safra = new TabelaUDO(
                                "UPD_OSAF"
                                , "Cadastro de Safra do Item"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

                        var certificado = new TabelaUDO(
                                "UPD_CRTC"
                                , "Cadastro do Certificado"
                                , BoUTBTableType.bott_MasterData
                                , new List<Coluna>() { }
                                , new UDOParams() { CanDelete = BoYesNoEnum.tNO }
                            );

                        var participante = new TabelaUDO(
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

                        var grupoDeCafe = new Tabela(
                                "UPD_OCTC"
                                , "Grupos de Café"
                                , BoUTBTableType.bott_NoObject
                                , new List<Coluna>() {
                            new ColunaVarchar("ItmsGrpCod","Código Grupo de Item", 30)
                                }
                            );

                        db.CriarTabela(modalidade);
                        db.CriarTabela(unidadeComercial);
                        db.CriarTabela(tipoOperacao);
                        db.CriarTabela(metodoFinanceiro);
                        db.CriarTabela(safra);
                        db.CriarTabela(certificado);
                        db.CriarTabela(participante);
                        db.CriarTabela(grupoDeCafe);

                        db.CriarTabela(
                            new TabelaUDO(
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
                                    new Tabela("UPD_CCC1", "Itens do Contrato", BoUTBTableType.bott_MasterDataLines, new List<Coluna>(){
                                        new ColunaVarchar("ItemCode","Código do Item",60),
                                        new ColunaVarchar("ItemName","Nome do Item",100),
                                        new ColunaPercent("PercItem","Percentual"),
                                        new ColunaQuantity("Difere","Diferencial"),
                                    }),
                                    new Tabela("UPD_CCC2", "Corretores do Contrato", BoUTBTableType.bott_MasterDataLines, new List<Coluna>(){
                                        new ColunaVarchar("PartCode","Código do Corretor",30),
                                        new ColunaPercent("PercCom","Percentual"),
                                        new ColunaAtivo()
                                    }),
                                    new Tabela("UPD_CCC3", "Responsáveis do Contrato", BoUTBTableType.bott_MasterDataLines, new List<Coluna>(){
                                        new ColunaVarchar("PartCode","Código do Responsável",30),
                                        new ColunaPercent("PercCom","Percentual"),
                                        new ColunaAtivo()
                                    }),
                                    new Tabela("UPD_CCC4", "Certificados do Contrato", BoUTBTableType.bott_MasterDataLines, new List<Coluna>(){
                                        new ColunaVarchar("Certif","Certificado",30)
                                    })
                                }
                            )
                        );

                        db.CriarCampo("OUSR", new ColunaVarchar("GrupoAprov", "Grupo Aprovador", 2, false, "V", new List<ValorValido>() {
                            new ValorValido(GrupoAprovador.Planejador, "Planejador"),
                            new ValorValido(GrupoAprovador.Executor, "Executor"),
                            new ValorValido(GrupoAprovador.Autorizador, "Autorizador"),
                            new ValorValido(GrupoAprovador.Gestor, "Gestor"),
                            new ValorValido(GrupoAprovador.Visualizador, "Visualizador")
                        }));
                    }

                    _company.EndTransaction(BoWfTransOpt.wf_Commit);
                }

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
                var formGrupoDeItens = new FormGrupoDeItens();
                var formCertificado = new FormCertificado();
                var formMetodoFinanceiro = new FormMetodoFinanceiro();
                var formModalidade = new FormModalidade();
                var formSafra = new FormSafra();
                var formTipoOperacao = new FormTipoOperacao();
                var formUnidadeComercial = new FormUnidadeComercial();
                var formParticipante = new FormParticipante();

                var formPreContrato = new FormPreContrato();
                var formAberturaPorPeneira = new FormAberturaPorPeneira();
                var formDetalheCertificado = new FormDetalheCertificado();
                var formComissoes = new FormComissoes();

                var formUsuarios = new FormUsuarios();

                FormEvents.DeclararEventos(eventFilters, new List<MapEventsToForms>() {
                    new MapEventsToForms(BoEventTypes.et_FORM_VISIBLE, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formAberturaPorPeneira,
                        formDetalheCertificado,
                        formComissoes,
                        formGrupoDeItens
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_LOAD, formUsuarios),
                    new MapEventsToForms(BoEventTypes.et_COMBO_SELECT, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_VALIDATE, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_CHOOSE_FROM_LIST, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formAberturaPorPeneira
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_ADD, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formCertificado,
                        formMetodoFinanceiro,
                        formModalidade,
                        formSafra,
                        formTipoOperacao,
                        formUnidadeComercial,
                        formParticipante
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_UPDATE, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formCertificado,
                        formMetodoFinanceiro,
                        formModalidade,
                        formSafra,
                        formTipoOperacao,
                        formUnidadeComercial,
                        formParticipante
                    }),
                    new MapEventsToForms(BoEventTypes.et_FORM_DATA_LOAD, formPreContrato),
                    new MapEventsToForms(BoEventTypes.et_FORM_CLOSE, new List<SAPHelper.Form>(){
                        formAberturaPorPeneira,
                        formDetalheCertificado,
                        formComissoes
                    }),
                    new MapEventsToForms(BoEventTypes.et_ITEM_PRESSED, new List<SAPHelper.Form>(){
                        formPreContrato,
                        formAberturaPorPeneira,
                        formDetalheCertificado,
                        formComissoes,
                        formGrupoDeItens
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

        #region :: Regras de Negócio

        public static void SetGrupoAprovador()
        {
            var rs = Helpers.DoQuery($"SELECT U_GrupoAprov FROM OUSR WHERE USER_CODE = '{Global.Company.UserName}'");
            _grupoAprovador = rs.Fields.Item("U_GrupoAprov").Value;
        }

        #endregion
    }
}
