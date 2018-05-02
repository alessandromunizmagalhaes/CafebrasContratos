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
        public static double _versaoAddon = 0.2;

        [STAThread]
        static void Main()
        {
            ConectarComSAP();

            CriarEstruturaDeDados();
            //commit sem bug
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
                _company.StartTransaction();

                using (Database db = new Database(_versaoAddon))
                {
                    var versoes = new List<Versionamento>() {
                        new Versao_Zero_Um(),
                        new Versao_Zero_Dois(),
                    };

                    Versoes.Aplicar(db, versoes);
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
                #region :: Forms Cadastro Básico

                var formGrupoDeItens = new FormGrupoDeItens();
                var formCertificado = new FormCertificado();
                var formMetodoFinanceiro = new FormMetodoFinanceiro();
                var formModalidade = new FormModalidade();
                var formSafra = new FormSafra();
                var formTipoOperacao = new FormTipoOperacao();
                var formUnidadeComercial = new FormUnidadeComercial();
                var formParticipante = new FormParticipante();

                var formsCadastroBasico = new List<SAPHelper.Form>() {
                     formGrupoDeItens,
                     formCertificado,
                     formMetodoFinanceiro,
                     formModalidade,
                     formSafra,
                     formTipoOperacao,
                     formUnidadeComercial,
                     formParticipante
                };

                #endregion


                #region :: Forms Pré Contrato

                var formPreContrato = new FormPreContrato();
                var formAberturaPorPeneira = new FormAberturaPorPeneira();
                var formDetalheCertificado = new FormDetalheCertificado();
                var formComissoes = new FormComissoes();

                var formsDetalhePreContrato = new List<SAPHelper.Form>() {
                     formAberturaPorPeneira,
                     formDetalheCertificado,
                     formComissoes
                };

                #endregion


                #region :: Form SAP

                var formUsuarios = new FormUsuarios();

                #endregion


                #region :: Grupos de Forms

                var formsVisible = new List<SAPHelper.Form>() { formPreContrato };
                formsVisible.AddRange(formsCadastroBasico);
                formsVisible.AddRange(formsDetalhePreContrato);

                #endregion


                FormEvents.DeclararEventos(eventFilters, new List<MapEventsToForms>() {
                    new MapEventsToForms(BoEventTypes.et_FORM_VISIBLE, formsVisible),
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

                var formsAdicionarNovo = new List<SAPHelper.Form>() { formPreContrato };
                formsAdicionarNovo.AddRange(formsCadastroBasico);

                FormEvents.DeclararEventosInternos(EventosInternos.AdicionarNovo, formsAdicionarNovo);
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
