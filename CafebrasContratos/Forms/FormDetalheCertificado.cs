using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormDetalheCertificado : SAPHelper.Form
    {
        public override string FormType { get { return "FormDetalheCertificado"; } }
        private const string mainDbDataSource = "@UPD_CCC4";
        public static string _fatherFormUID = "";

        #region :: Campos

        public MatrizItens _matriz = new MatrizItens()
        {
            ItemUID = "matrix",
            Datasource = mainDbDataSource
        };
        public ItemForm _botaoAdicionar = new ItemForm()
        {
            ItemUID = "btnAdd"
        };
        public ItemForm _botaoRemover = new ItemForm()
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

                var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                _matriz._certificado.PopularComboBox(mtx, _matriz._certificado.ItemUID);

                CarregarDadosMatriz(form, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);
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

        public override void OnAfterFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var fatherForm = GetFormIfExists(_fatherFormUID);
            if (fatherForm != null)
            {
                ChangeFormMode(fatherForm);
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _botaoAdicionar.ItemUID)
            {
                OnBotaoAdicionarClick(pVal);
            }
            else if (pVal.ItemUID == _botaoRemover.ItemUID)
            {
                OnBotaoRemoverClick(pVal);
            }
            else if (pVal.ItemUID == "1")
            {
                CarregarDataSourceFormPai(FormUID, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);
            }
        }

        private void OnBotaoAdicionarClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            AddLine(form, _matriz.ItemUID, mainDbDataSource);
        }

        private void OnBotaoRemoverClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            RemoveSelectedLine(form, _matriz.ItemUID, mainDbDataSource);
        }

        #endregion

        #region :: Configuração da Matriz

        public class MatrizItens : MatrizForm
        {
            public ComboForm _certificado = new ComboForm()
            {
                ItemUID = "Certif",
                Datasource = "U_Certif",
                SQL = "SELECT Code, Name FROM [@UPD_CRTC] WHERE Canceled = 'N' ORDER BY Name"
            };
        }

        #endregion
    }
}