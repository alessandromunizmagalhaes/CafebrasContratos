using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormContratoFinal : FormContrato
    {
        #region :: Overrides

        public override string FormType { get { return "FormContratoFinal"; } }
        public override string MainDbDataSource { get { return new TabelaContratoFinal().NomeComArroba; } }
        public override Type FormAberturaPorPeneiraType { get { return typeof(FormContratoFinalAberturaPorPeneira); } }
        public override Type FormComissoesType { get { return typeof(FormContratoFinalComissoes); } }
        public override Type FormDetalheCertificadoType { get { return typeof(FormContratoFinalDetalheCertificado); } }

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

            form.Items.Item(_statuscontrato.ItemUID).Enabled = false;
        }

        public override void IniciarValoresAoAdicionarNovo(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _status.SetaValorDBDatasource(dbdts, StatusContratoFinalQualidade.PreAprovado);
            _statuscontrato.SetaValorDBDatasource(dbdts, StatusContratoFinal.Aberto);
        }

        public override string ProximaChavePrimaria()
        {
            return GetNextPrimaryKey(MainDbDataSource, _numeroDoContrato.Datasource);
        }

        public override void RegrasDeNegocioAoSalvar(SAPbouiCOM.Form form, DBDataSource dbdts)
        {

        }

        public override void QuandoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            //TODO: habilitar botões que dizem respeito a criação de pedido de venda... não pode adicionar pedido se o contrato não estiver adicionado
        }

        public override void QuandoNaoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form)
        {
            //TODO: desabilitar botões que dizem respeito a criação de pedido de venda... não pode adicionar pedido se o contrato não estiver adicionado
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
        public ItemForm _statuscontrato = new ItemForm()
        {
            ItemUID = "StatusCtr",
            Datasource = "U_StatusCtr"
        };

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterFormVisible(FormUID, ref pVal, out BubbleEvent);

            var form = GetForm(FormUID);
            form.EnableMenu(((int)EventosInternos.AdicionarNovo).ToString(), false);
            form.Mode = BoFormMode.fm_ADD_MODE;

            var dbdtsContratoFinal = GetDBDatasource(form, MainDbDataSource);

            var fatherForm = GetForm(_fatherFormUID);
            var dbdtsPreContrato = GetDBDatasource(_fatherFormUID, new TabelaPreContrato().NomeComArroba);

            try
            {
                form.Freeze(true);
                CopyIfFieldsMatch(dbdtsPreContrato, ref dbdtsContratoFinal);

                _OnAdicionarNovo(form);

                PopularPessoasDeContato(form, dbdtsPreContrato.GetValue(_codigoPN.Datasource, 0), dbdtsPreContrato.GetValue(_pessoasDeContato.Datasource, 0));
            }
            finally
            {
                form.Freeze(false);
            }
        }

        #endregion
    }
}
