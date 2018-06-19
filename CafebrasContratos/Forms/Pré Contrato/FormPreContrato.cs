using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormPreContrato : FormContrato
    {
        #region :: Propriedades

        public MatrizDTContratosFinais _matrizContratosFinais = new MatrizDTContratosFinais()
        {
            ItemUID = "mtxFinais",
            Datasource = "ContratosFinais"
        };

        #endregion


        #region :: Overrides

        public override string FormType { get { return "FormPreContrato"; } }
        public override string MainDbDataSource { get { return new TabelaPreContrato().NomeComArroba; } }
        public override string anexoDbDataSource { get { return new TabelaAnexosDoPreContrato().NomeComArroba; } }

        public override Type FormAberturaPorPeneiraType { get { return typeof(FormPreContratoAberturaPorPeneira); } }
        public override Type FormComissoesType { get { return typeof(FormPreContratoComissoes); } }
        public override Type FormDetalheCertificadoType { get { return typeof(FormPreContratoDetalheCertificado); } }

        public override string FormAberturaPorPeneiraSRF { get { return "FormPreContratoAberturaPorPeneira.srf"; } }
        public override string FormComissoesSRF { get { return "FormPreContratoComissoes.srf"; } }
        public override string FormDetalheCertificadoSRF { get { return "FormPreContratoDetalheCertificado.srf"; } }

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

        public override void IniciarValoresAoAdicionarNovo(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _status.SetValorDBDatasource(dbdts, StatusPreContrato.Esboço);
        }

        public override string ProximaChavePrimaria(DBDataSource dbdts)
        {
            return GetNextPrimaryKey(MainDbDataSource, _numeroDoContrato.Datasource);
        }

        protected override void FormEmModoVisualizacao(SAPbouiCOM.Form form)
        {
            base.FormEmModoVisualizacao(form);
        }

        public override void RegrasDeNegocioAoSalvar(SAPbouiCOM.Form form, DBDataSource dbdts)
        {

        }

        public override void QuandoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            ToggleBotaoAdicionar(form, true);
        }

        public override void QuandoNaoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            ToggleBotaoAdicionar(form, false);
        }

        public override bool ContratoPodeSerAlterado(string status)
        {
            return status == StatusPreContrato.Esboço;
        }

        #endregion


        #region :: Campos

        public override ItemForm _numeroDoContrato
        {
            get
            {
                return new ItemForm()
                {
                    ItemUID = "DocNumCC",
                    Datasource = "U_DocNumCC",
                };
            }
        }

        #endregion


        #region :: Eventos de Formulário

        public override void OnAfterFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;


            base.OnAfterFormDataLoad(ref BusinessObjectInfo, out BubbleEvent);

            var form = GetForm(BusinessObjectInfo.FormUID);
            AtualizarMatriz(form);

            var dbdts = GetDBDatasource(form, MainDbDataSource);
        }

        #endregion


        #region :: Definição de Botões

        private ButtonForm _botaoAdicionar = new ButtonForm()
        {
            ItemUID = "btnAdd",
        };

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormVisible(FormUID, ref pVal, out BubbleEvent);

            if (BubbleEvent)
            {
                var mtx = GetMatrix(FormUID, _matrizContratosFinais.ItemUID);
                _matrizContratosFinais.Bind(mtx);
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ItemUID == _botaoAdicionar.ItemUID)
            {
                var form = GetForm(FormUID);
                if (form.Mode == BoFormMode.fm_OK_MODE)
                {
                    FormContratoFinal.AbrirCriandoNovoRegistro(FormUID);
                }
                else
                {
                    Dialogs.PopupError("Salve o Contrato antes de criar um novo Contrato Final.");
                }
            }
            else
            {
                base.OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);
            }
        }

        public override void OnAfterMatrixLinkPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterMatrixLinkPressed(FormUID, ref pVal, out BubbleEvent);

            if (pVal.ColUID == _matrizContratosFinais.CodigoContrato.ItemUID)
            {
                var mtx = GetMatrix(FormUID, _matrizContratosFinais.ItemUID);
                var codigo = mtx.Columns.Item(pVal.ColUID).Cells.Item(pVal.Row).Specific.Value;

                FormContratoFinal.AbrirNoRegistro(codigo);
            }
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            base._OnAdicionarNovo(form);

            form.Items.Item(_dataFim.ItemUID).Click();
        }

        #endregion


        #region :: Regras de Negócio

        private void ToggleBotaoAdicionar(SAPbouiCOM.Form form, bool habilitado)
        {
            form.Items.Item(_botaoAdicionar.ItemUID).Enabled = habilitado;
        }

        public void AtualizarMatriz(SAPbouiCOM.Form form)
        {
            var dbdts = GetDBDatasource(form, MainDbDataSource);
            var dt = GetDatatable(form, _matrizContratosFinais.Datasource);

            var docnumcc = dbdts.GetValue(_numeroDoContrato.Datasource, 0);

            try
            {
                form.Freeze(true);
                dt.ExecuteQuery(
                    $@"SELECT 
                        U_DocNumCF,U_StatusCtr, U_CardCode, U_CardName, U_Descricao 
                    FROM [@UPD_OCFC] 
                    WHERE U_DocNumCC = {docnumcc} 
                    ORDER BY U_DocNumCF"
                    );

                var mtx = GetMatrix(form, _matrizContratosFinais.ItemUID);
                mtx.LoadFromDataSource();

                mtx.AutoResizeColumns();
            }
            finally
            {
                form.Freeze(false);
            }
        }

        public static string GetCode(string numPreContrato)
        {
            var rs = Helpers.DoQuery($@"SELECT Code FROM [@UPD_OCCC] WHERE U_DocNumCC = {numPreContrato}");
            return rs.Fields.Item("Code").Value;
        }

        #endregion


        public class MatrizDTContratosFinais : MatrizDatatable
        {
            public ItemForm CodigoContrato = new ItemForm()
            {
                ItemUID = "contrato",
                Datasource = "U_DocNumCF"
            };
            public ItemForm CodigoPN = new ItemForm()
            {
                ItemUID = "cardcode",
                Datasource = "U_CardCode"
            };
            public ItemForm NomePN = new ItemForm()
            {
                ItemUID = "cardname",
                Datasource = "U_CardName"
            };
            public ItemForm Descricao = new ItemForm()
            {
                ItemUID = "descricao",
                Datasource = "U_Descricao"
            };
        }
    }
}
