using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormDetalheItem : SAPHelper.Form
    {
        public override string FormType { get { return "FormDetalheItem"; } }
        private const string mainDbDataSource = "@UPD_CCC1";
        public static string _fatherFormUID = "";

        #region :: Campos

        public MatrizItens _matriz = new MatrizItens()
        {
            ItemUID = "mtxItem",
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
            _matriz.CriarColunaSumAuto(form, _matriz._percentual.ItemUID);
            CarregarDadosMatriz(form, _fatherFormUID);
        }

        public override void OnAfterFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var fatherForm = GetForm(_fatherFormUID);
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
                OnBotaoAdicionar(pVal);
            }
            else if (pVal.ItemUID == _botaoRemover.ItemUID)
            {
                OnBotaoRemover(pVal);
            }
            else if (pVal.ItemUID == "1")
            {
                CarregarDataSourceFormPai(FormUID, _fatherFormUID);
            }
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataTable = chooseEvent.SelectedObjects;
            string itemcode = dataTable.GetValue("ItemCode", 0);
            string itemname = dataTable.GetValue("ItemName", 0);

            var matrix = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
            matrix.SetCellWithoutValidation(pVal.Row, _matriz._codigoItem.ItemUID, itemcode);
            matrix.SetCellWithoutValidation(pVal.Row, _matriz._nomeItem.ItemUID, itemname);
        }

        private void OnBotaoAdicionar(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

            try
            {
                form.Freeze(true);
                mtx.AddRow();

                dynamic lineIDColumn = mtx.GetCellSpecific("LineId", mtx.VisualRowCount);
                if (lineIDColumn != null)
                {
                    lineIDColumn.Value = "";
                    mtx.ClearRowData(mtx.VisualRowCount);
                    mtx.SelectRow(mtx.VisualRowCount - 1, false, false);
                }
            }
            catch (Exception e)
            {
                Dialogs.Error("Erro ao tentar adicionar uma nova linha a matriz.\nErro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void OnBotaoRemover(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);
            var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

            int row = mtx.GetNextSelectedRow();
            if (row > 0)
            {
                mtx.DeleteRow(row);

                ChangeFormMode(form);
            }
            else
            {
                Dialogs.PopupError("Selecione uma linha.");
            }
        }

        #endregion

        #region :: Regras de Negócio

        private void CarregarDadosMatriz(SAPbouiCOM.Form localForm, string fatherFormUID)
        {
            var localDbdts = GetDBDatasource(localForm, mainDbDataSource);

            var fatherForm = GetForm(fatherFormUID);
            var fatherDbdts = GetDBDatasource(fatherForm, mainDbDataSource);

            Copy(fatherDbdts, ref localDbdts);

            var mtx = ((Matrix)localForm.Items.Item(_matriz.ItemUID).Specific);
            mtx.LoadFromDataSourceEx(true);
        }

        private void CarregarDataSourceFormPai(string localFormUID, string fatherFormUID)
        {
            var fatherForm = GetForm(fatherFormUID);
            var fatherDbdts = GetDBDatasource(fatherForm, mainDbDataSource);

            var localForm = GetForm(localFormUID);
            ((Matrix)localForm.Items.Item(_matriz.ItemUID).Specific).FlushToDataSource();
            var localDbdts = GetDBDatasource(localForm, mainDbDataSource);

            Copy(localDbdts, ref fatherDbdts);
        }

        #endregion

    }

    public class MatrizItens : MatrizForm
    {
        public ItemForm _codigoItem = new ItemForm()
        {
            ItemUID = "ItemCode",
            Datasource = "U_ItemCode"
        };
        public ItemForm _nomeItem = new ItemForm()
        {
            ItemUID = "ItemName",
            Datasource = "U_ItemName"
        };
        public ItemForm _percentual = new ItemForm()
        {
            ItemUID = "PercItem",
            Datasource = "U_PercItem"
        };
        public ItemForm _diferencial = new ItemForm()
        {
            ItemUID = "Difere",
            Datasource = "U_Difere"
        };
    }
}
