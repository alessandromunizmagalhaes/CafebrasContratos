using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormAberturaPorPeneira : SAPHelper.Form
    {
        public override string FormType { get { return "FormAberturaPorPeneira"; } }
        private const string mainDbDataSource = "@UPD_CCC1";
        public static string _fatherFormUID = "";

        #region :: Campos

        public Matriz _matriz = new Matriz()
        {
            ItemUID = "mtxItem",
            Datasource = mainDbDataSource
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

                _matriz.CriarColunaSumAuto(form, _matriz._percentual.ItemUID);
                _matriz.CriarColunaSumAuto(form, _matriz._diferencial.ItemUID);

                CarregarDadosMatriz(form, _fatherFormUID, _matriz.ItemUID, mainDbDataSource);

                var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                ClicarParaCalcularOsTotalizadores(mtx);
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
            }
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataTable = chooseEvent.SelectedObjects;
            var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

            var itemcode = dataTable.GetValue("ItemCode", 0);
            var itemname = dataTable.GetValue("ItemName", 0);
            if (ItemJaFoiUsado(itemcode, mtx))
            {
                Dialogs.PopupError($"O Item '{itemcode}' - '{itemname}' já foi usado.");
            }
            else
            {
                mtx.SetCellWithoutValidation(pVal.Row, _matriz._codigoItem.ItemUID, itemcode);
                mtx.SetCellWithoutValidation(pVal.Row, _matriz._nomeItem.ItemUID, itemname);

                ChangeFormMode(form);
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


        #region :: Regras de Negócio

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

        private void ClicarParaCalcularOsTotalizadores(Matrix mtx)
        {
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                mtx.Columns.Item(_matriz._percentual.ItemUID).Cells.Item(i).Click();
            }
        }

        private bool ItemJaFoiUsado(string itemcode, Matrix mtx)
        {
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                if (mtx.GetCellSpecific(_matriz._codigoItem.ItemUID, i).Value == itemcode)
                {
                    return true;
                }
            }
            return false;
        }


        #endregion


        #region :: Configuração da Matriz

        public class Matriz : MatrizChildForm
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

        #endregion
    }
}
