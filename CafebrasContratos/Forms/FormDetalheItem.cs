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
            try
            {
                form.Freeze(true);
                _matriz.CriarColunaSumAuto(form, _matriz._percentual.ItemUID);
                _matriz.CriarColunaSumAuto(form, _matriz._diferencial.ItemUID);
                CarregarDadosMatriz(form, _fatherFormUID);

                var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

                for (int i = 1; i <= mtx.RowCount; i++)
                {
                    mtx.Columns.Item(_matriz._percentual.ItemUID).Cells.Item(i).Click();
                }
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

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == "1")
            {
                var form = GetForm(FormUID);
                var matriz = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

                if (!SomaDosPercentuaisEstaCorreta(matriz))
                {
                    BubbleEvent = false;
                }
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
                CarregarDataSourceFormPai(FormUID, _fatherFormUID);
            }
        }

        private bool SomaDosPercentuaisEstaCorreta(Matrix mtx)
        {
            var percentual = 0.0;
            try
            {
                percentual = SomaDosPercentuais(mtx);
            }
            catch (Exception e)
            {
                Dialogs.PopupError(e.Message);
                return false;
            }

            if (percentual == 100)
            {
                return true;
            }
            else
            {
                Dialogs.PopupError("A coluna percentual deve conter o total de 100%");
            }
            return false;
        }

        private double SomaDosPercentuais(Matrix mtx)
        {
            var soma_percentual = 0.0;
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                var percentual = Helpers.ToDouble(mtx.GetCellSpecific(_matriz._percentual.ItemUID, i).Value);
                if (percentual == 0)
                {
                    throw new ArgumentException("O valor percentual não pode ser 0");
                }

                soma_percentual += percentual;
            }

            return soma_percentual;
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataTable = chooseEvent.SelectedObjects;
            string itemcode = dataTable.GetValue("ItemCode", 0);
            string itemname = dataTable.GetValue("ItemName", 0);

            var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
            mtx.SetCellWithoutValidation(pVal.Row, _matriz._codigoItem.ItemUID, itemcode);
            mtx.SetCellWithoutValidation(pVal.Row, _matriz._nomeItem.ItemUID, itemname);

            ChangeFormMode(form);
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
            var fatherForm = GetFormIfExists(fatherFormUID);
            if (fatherForm != null)
            {
                var fatherDbdts = GetDBDatasource(fatherForm, mainDbDataSource);

                var localForm = GetForm(localFormUID);
                ((Matrix)localForm.Items.Item(_matriz.ItemUID).Specific).FlushToDataSource();
                var localDbdts = GetDBDatasource(localForm, mainDbDataSource);

                Copy(localDbdts, ref fatherDbdts);
            }
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
