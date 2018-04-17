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
            if (!ConfigXML.JaCriouEstrutura)
            {
                try
                {
                    _company.StartTransaction();

                    using (Database db = new Database())
                    {
                        //db.ExcluirTabela("UPD_OCCC");

                        db.CriarTabela(DbConfig.modalidade);
                        db.CriarTabela(DbConfig.unidadeComercial);
                        db.CriarTabela(DbConfig.tipoOperacao);
                        db.CriarTabela(DbConfig.metodoFinanceiro);
                        db.CriarTabela(DbConfig.safra);
                        db.CriarTabela(DbConfig.certificado);
                        db.CriarTabela(DbConfig.participante);
                        db.CriarTabela(DbConfig.grupoDeCafe);

                        db.CriarTabela(DbPreContrato.preContrato);

                        db.CriarCampo("OUSR", DbConfig.grupoAprovador);
                    }

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
