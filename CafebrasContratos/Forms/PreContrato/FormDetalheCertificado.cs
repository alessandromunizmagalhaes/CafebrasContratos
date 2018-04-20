using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormDetalheCertificado : SAPHelper.Form
    {
        public override string FormType { get { return "FormDetalheCertificado"; } }
        private string mainDbDataSource = DbPreContrato.certificadosDoContrato.NomeComArroba;
        public static string _fatherFormUID = "";

        #region :: Campos

        public Matriz _matriz = new Matriz()
        {
            ItemUID = "matrix",
            Datasource = DbPreContrato.certificadosDoContrato.NomeComArroba
        };
        public ButtonForm _adicionar = new ButtonForm()
        {
            ItemUID = "btnAdd"
        };
        public ButtonForm _remover = new ButtonForm()
        {
            ItemUID = "btnRmv"
        };

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(FormUID);
            try
            {
                form.Freeze(true);

                _matriz._certificado.Popular(form, _matriz.ItemUID);

                CarregarDadosMatriz(form, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);

                form.Items.Item("1").Enabled = PreContrato.UsuarioPermitido();
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao desenhar o form.\nErro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == "1")
            {
                var form = GetForm(FormUID);
                var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                mtx.FlushToDataSource();
                var dbdts = GetDBDatasource(form, mainDbDataSource);

                if (!CamposMatrizEstaoPreenchidos(form, dbdts, _matriz))
                {
                    BubbleEvent = false;
                }
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _adicionar.ItemUID)
            {
                OnBotaoAdicionarClick(pVal);
            }
            else if (pVal.ItemUID == _remover.ItemUID)
            {
                OnBotaoRemoverClick(pVal);
            }
            else if (pVal.ItemUID == "1")
            {
                CarregarDataSourceFormPai(FormUID, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);

                var fatherForm = GetFormIfExists(_fatherFormUID);
                if (fatherForm != null)
                {
                    ChangeFormMode(fatherForm);
                }
            }
        }

        private void OnBotaoAdicionarClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            _matriz.AdicionarLinha(form, dbdts);
        }

        private void OnBotaoRemoverClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            _matriz.RemoverLinha(form, dbdts);
        }

        #endregion


        #region :: Configuração da Matriz

        public class Matriz : MatrizChildForm
        {
            public ComboFormObrigatorio _certificado = new ComboFormObrigatorio()
            {
                ItemUID = "Certif",
                Datasource = "U_Certif",
                SQL = "SELECT Code, Name FROM [@UPD_CRTC] WHERE Canceled = 'N' ORDER BY Name",
                Mensagem = "O certificado é obrigatório"
            };
        }

        #endregion
    }
}