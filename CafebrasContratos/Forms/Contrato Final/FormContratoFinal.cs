using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public class FormContratoFinal : FormContrato
    {
        #region :: Propriedades

        private const string SRF = "FormContratoFinal.srf";

        #endregion


        #region :: Overrides

        public override string FormType { get { return "FormContratoFinal"; } }
        public override string MainDbDataSource { get { return new TabelaContratoFinal().NomeComArroba; } }
        public override string AnexoDbDataSource { get { return new TabelaAnexosDoContratoFinal().NomeComArroba; } }
        public override AbasContrato Abas { get { return new AbasContratoFinal(); } }

        public override Type FormAberturaPorPeneiraType { get { return typeof(FormContratoFinalAberturaPorPeneira); } }
        public override Type FormComissoesType { get { return typeof(FormContratoFinalComissoes); } }
        public override Type FormDetalheCertificadoType { get { return typeof(FormContratoFinalDetalheCertificado); } }

        public override string FormAberturaPorPeneiraSRF { get { return "FormContratoFinalAberturaPorPeneira.srf"; } }
        public override string FormComissoesSRF { get { return "FormContratoFinalComissoes.srf"; } }
        public override string FormDetalheCertificadoSRF { get { return "FormContratoFinalDetalheCertificado.srf"; } }

        public static string _fatherFormUID = "";

        public override bool UsuarioPermitido()
        {
            switch (Program._grupoAprovador)
            {
                case GrupoAprovador.Planejador:
                case GrupoAprovador.Gestor:
                    return true;
                default:
                    return false;
            }
        }

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            base._OnAdicionarNovo(form);

            form.Items.Item(_descricao.ItemUID).Click();
        }

        protected override void FormEmModoVisualizacao(SAPbouiCOM.Form form)
        {
            base.FormEmModoVisualizacao(form);

            form.Items.Item(_statusQualidade.ItemUID).Enabled = false;
        }

        public override void IniciarValoresAoAdicionarNovo(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _status.SetValorDBDatasource(dbdts, StatusContratoFinal.Esboço);
            _statusQualidade.SetValorDBDatasource(dbdts, StatusContratoFinalQualidade.PreAprovado);
        }

        public override string ProximaChavePrimaria(DBDataSource dbdts)
        {
            var numPreContrato = dbdts.GetValue(_numeroDoPreContrato.Datasource, 0);
            using (var recordset = new RecordSet())
            {
                var rs = recordset.DoQuery(
                    $@"SELECT 
                        CONVERT(NVARCHAR, {numPreContrato}) + '.' + CONVERT(NVARCHAR, COUNT(*) + 1) as codigo
                    FROM [{dbdts.TableName}] WHERE {_numeroDoPreContrato.Datasource} = {numPreContrato}");
                return rs.Fields.Item("codigo").Value;
            }
        }

        public override void RegrasDeNegocioAoSalvar(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            var numPreContrato = dbdts.GetValue(_numeroDoPreContrato.Datasource, 0);
            if (string.IsNullOrEmpty(numPreContrato))
            {
                throw new BusinessRuleException("Não foi possível identificar qual o Pré-Contrato referente a este Contrato Final.");
            }
        }

        public override void QuandoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            ToggleBotao(form, true);
        }

        public override void QuandoNaoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            ToggleBotao(form, false);
        }

        public override bool ContratoPodeSerAlterado(string status)
        {
            return status == StatusContratoFinal.Esboço
                || status == StatusContratoFinal.Renegociacao
                || status == StatusContratoFinal.Liberado
                || String.IsNullOrEmpty(status)
            ;
        }

        public override void ValidaAlteracaoDeStatus(GestaoStatusContrato gestaoStatus)
        {

        }

        #endregion


        #region :: Campos

        public ItemForm _numeroDoPreContrato = new ItemForm()
        {
            Datasource = "U_DocNumCC"
        };

        public override ItemForm _numeroDoContrato
        {
            get
            {
                return new ItemForm()
                {
                    ItemUID = "DocNumCF",
                    Datasource = "U_DocNumCF",
                };
            }
        }
        public ItemFormContrato _statusQualidade = new ItemFormContrato()
        {
            ItemUID = "StatusQua",
            Datasource = "U_StatusQua",
            gestaoCamposEmStatus = new GestaoCamposContrato()
            {
                QuandoEmEsboco = true,
                QuandoEmLiberado = true,
                QuandoEmRenegociacao = true,
                QuandoEmAutorizado = false,
                QuandoEmEncerrado = false,
                QuandoEmCancelado = false,
            }
        };

        #endregion


        #region :: Definição de Botões

        private ComboForm _botaoComboCopiar = new ComboForm()
        {
            ItemUID = "btnCopiar",
            ValoresPadrao = new Dictionary<string, string>() {
                {"1","Pedido de Compra"},
                {"2","Adiantamento para Fornecedor"},
                {"3","Recebimento de Mercadoria"},
                {"4","Devolução de Mercadoria"},
                {"5","Nota Fiscal de Entrada"},
                {"6","Devolução de Nota Fiscal de Entrada"},
            }
        };

        #endregion


        #region :: Eventos de Formulários

        public override void OnAfterFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            base.OnAfterFormDataAdd(ref BusinessObjectInfo, out BubbleEvent);

            var form = GetForm(BusinessObjectInfo.FormUID);
            var dbdts = GetDBDatasource(form, MainDbDataSource);
            AtualizarSaldoPreContrato(dbdts);
        }

        public override void OnAfterFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            base.OnAfterFormDataUpdate(ref BusinessObjectInfo, out BubbleEvent);

            var dbdts = GetDBDatasource(BusinessObjectInfo.FormUID, MainDbDataSource);
            AtualizarSaldoPreContrato(dbdts);
        }

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormVisible(FormUID, ref pVal, out BubbleEvent);

            var form = GetForm(FormUID);
            form.EnableMenu(((int)EventosInternos.AdicionarNovo).ToString(), false);

            var botaoComboCopiar = (ButtonCombo)form.Items.Item(_botaoComboCopiar.ItemUID).Specific;
            _botaoComboCopiar.Popular(botaoComboCopiar.ValidValues);

            if (form.Mode == BoFormMode.fm_ADD_MODE)
            {
                Dialogs.Success("Carregando informações do Contrato de Compra Geral... Aguarde...");

                var fatherForm = GetForm(_fatherFormUID);

                var dbdtsCF = GetDBDatasource(form, MainDbDataSource);
                var dbdtsPC = GetDBDatasource(_fatherFormUID, new TabelaPreContrato().NomeComArroba);

                var dbdtsCFCertificado = GetDBDatasource(form, new TabelaCertificadosDoContratoFinal().NomeComArroba);
                var dbdtsPCCertificado = GetDBDatasource(_fatherFormUID, new TabelaCertificadosDoPreContrato().NomeComArroba);

                var dbdtsCFResponsavel = GetDBDatasource(form, new TabelaResponsaveisDoContratoFinal().NomeComArroba);
                var dbdtsPCResponsavel = GetDBDatasource(_fatherFormUID, new TabelaResponsaveisDoPreContrato().NomeComArroba);

                var dbdtsCFCorretor = GetDBDatasource(form, new TabelaCorretoresDoContratoFinal().NomeComArroba);
                var dbdtsPCCorretor = GetDBDatasource(_fatherFormUID, new TabelaCorretoresDoPreContrato().NomeComArroba);

                try
                {
                    form.Freeze(true);

                    var labelsIn = string.Empty;
                    for (int i = 0; i < _peneiras.Count; i++)
                    {
                        labelsIn += ",'" + _peneiras[i].Datasource.Replace("P", "L") + "'";
                    }

                    CopyIfFieldsMatch(dbdtsPC, ref dbdtsCF, labelsIn);
                    CopyIfFieldsMatch(dbdtsPCCertificado, ref dbdtsCFCertificado);
                    CopyIfFieldsMatch(dbdtsPCResponsavel, ref dbdtsCFResponsavel);
                    CopyIfFieldsMatch(dbdtsPCCorretor, ref dbdtsCFCorretor);

                    var saldoSacas = Helpers.ToDouble(dbdtsPC.GetValue(_saldoDeSacas.Datasource, 0));
                    var saldoPeso = Helpers.ToDouble(dbdtsPC.GetValue(_saldoDePeso.Datasource, 0));

                    saldoSacas = saldoSacas < 0 ? 0 : saldoSacas;
                    saldoPeso = saldoPeso < 0 ? 0 : saldoPeso;

                    _quantidadeDeSacas.SetValorDBDatasource(dbdtsCF, saldoSacas);
                    _quantidadeDePeso.SetValorDBDatasource(dbdtsCF, saldoPeso);

                    CalcularTotais(form, dbdtsCF);

                    _OnAdicionarNovo(form);

                    PopularPessoasDeContato(form, dbdtsPC.GetValue(_codigoPN.Datasource, 0), dbdtsPC.GetValue(_pessoasDeContato.Datasource, 0));
                    HabilitarCamposDePeneira(form, dbdtsCF, dbdtsCF.GetValue(_codigoItem.Datasource, 0));
                }
                finally
                {
                    form.Freeze(false);
                }

                Dialogs.Success("Ok.");
            }
        }

        public override void OnAfterFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormClose(FormUID, ref pVal, out BubbleEvent);

            var formTypePreContrato = new FormPreContrato().FormType;
            for (int i = 0; i < Global.SBOApplication.Forms.Count; i++)
            {
                var currentForm = Global.SBOApplication.Forms.Item(i);
                if (currentForm.TypeEx == formTypePreContrato)
                {
                    new FormPreContrato().AtualizarMatriz(currentForm);
                }
            }
        }

        #endregion


        #region :: Eventos Internos


        public override void _OnPesquisar(SAPbouiCOM.Form form)
        {
            base._OnPesquisar(form);
            form.Items.Item(_statusQualidade.ItemUID).Enabled = true;
        }

        #endregion


        #region :: Regras de Negócio

        private void AtualizarSaldoPreContrato(DBDataSource dbdts)
        {
            var tabelaPreContrato = Global.Company.UserTables.Item(new TabelaPreContrato().NomeSemArroba);
            var numPreContrato = dbdts.GetValue(_numeroDoPreContrato.Datasource, 0).Trim();
            var numContratoFinal = dbdts.GetValue(_numeroDoContrato.Datasource, 0).Trim();
            var codePreContrato = FormPreContrato.GetCode(numPreContrato);
            if (tabelaPreContrato.GetByKey(codePreContrato))
            {
                using (var recordset = new RecordSet())
                {
                    var rs = recordset.DoQuery(
                        $@"SELECT 
	                        SUM(U_QtdSaca) as SaldoSaca, SUM(U_QtdPeso) as SaldoPeso
	                        FROM [@UPD_OCFC] 
	                        WHERE 1 = 1
		                        AND U_DocNumCC = {numPreContrato} AND U_DocNumCF <> {numContratoFinal}"
                    );

                    double sacasPreContrato = tabelaPreContrato.UserFields.Fields.Item(_quantidadeDeSacas.Datasource).Value;
                    double pesoPreContrato = tabelaPreContrato.UserFields.Fields.Item(_quantidadeDePeso.Datasource).Value;

                    double saldoSacas = rs.Fields.Item("SaldoSaca").Value;
                    double saldoPeso = rs.Fields.Item("SaldoPeso").Value;

                    string strSacasContratoFinal = dbdts.GetValue(_saldoDeSacas.Datasource, 0);
                    double sacasContratoFinal = Helpers.ToDouble(strSacasContratoFinal);

                    string strPesoContratoFinal = dbdts.GetValue(_saldoDePeso.Datasource, 0);
                    double pesoContratoFinal = Helpers.ToDouble(strPesoContratoFinal);

                    tabelaPreContrato.UserFields.Fields.Item(_saldoDeSacas.Datasource).Value = sacasPreContrato - saldoSacas - sacasContratoFinal;
                    tabelaPreContrato.UserFields.Fields.Item(_saldoDePeso.Datasource).Value = pesoPreContrato - saldoPeso - pesoContratoFinal;

                    tabelaPreContrato.Update();
                }
            }
        }

        private void ToggleBotao(SAPbouiCOM.Form form, bool habilitado)
        {
            form.Items.Item(_botaoComboCopiar.ItemUID).Enabled = habilitado;
        }

        #endregion


        #region :: Métodos Estáticos

        public static void AbrirNoRegistro(string codigo)
        {
            var findParams = new CriarFormFindParams() { chavePrimariaUID = "DocNumCF", chavePrimariaValor = codigo };
            var criarFormParams = new CriarFormParams() { Mode = BoFormMode.fm_FIND_MODE, FindParams = findParams };
            CriarForm(AppDomain.CurrentDomain.BaseDirectory + SRF, criarFormParams);
        }

        public static void AbrirCriandoNovoRegistro(string fatherFormUID)
        {
            var formParams = new CriarFormParams() { Mode = BoFormMode.fm_ADD_MODE };
            CriarFormFilho(AppDomain.CurrentDomain.BaseDirectory + SRF, fatherFormUID, new FormContratoFinal(), formParams);
        }

        #endregion


        public class AbasContratoFinal : AbasContrato
        {
            public TabForm Documentos = new TabForm()
            {
                ItemUID = "AbaCFinal",
                PaneLevel = 4
            };
        }
    }
}
