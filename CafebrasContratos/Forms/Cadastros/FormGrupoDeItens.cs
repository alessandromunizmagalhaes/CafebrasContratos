using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormGrupoDeItens : SAPHelper.Form
    {
        public override string FormType { get { return "FormGrupoDeItens"; } }
        public string mainDbDataSource = new TabelaGrupoCafe().NomeComArroba;

        #region :: Campos

        public Matriz _matriz = new Matriz()
        {
            ItemUID = "matrix",
            Datasource = new TabelaGrupoCafe().NomeComArroba
        };
        public ButtonForm _adicionar = new ButtonForm()
        {
            ItemUID = "btnAdd"
        };
        public ButtonForm _remover = new ButtonForm()
        {
            ItemUID = "btnRmv"
        };
        public ButtonForm _salvar = new ButtonForm()
        {
            ItemUID = "btnSalvar"
        };

        #endregion


        #region :: Eventos de Itens

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(FormUID);
            var mtx = GetMatrix(form, _matriz.ItemUID);
            _matriz._grupoDeItem.Popular(mtx);

            var dbdts = GetDBDatasource(form, mainDbDataSource);
            dbdts.Clear();

            var rs = Helpers.DoQuery("SELECT U_ItmsGrpCod FROM [@UPD_OCTC] ORDER BY CONVERT(INT,Code)");
            if (rs.RecordCount > 0)
            {
                while (!rs.EoF)
                {
                    var grupoDeItem = rs.Fields.Item(_matriz._grupoDeItem.Datasource).Value;
                    dbdts.InsertRecord(dbdts.Size);
                    dbdts.SetValue(_matriz._grupoDeItem.Datasource, dbdts.Size - 1, grupoDeItem);
                    rs.MoveNext();
                }

                mtx.LoadFromDataSourceEx();
            }
            else
            {
                _matriz.AdicionarLinha(form);
            }
        }

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _salvar.ItemUID)
            {
                var form = GetForm(FormUID);
                var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);
                mtx.FlushToDataSource();
                var dbdts = GetDBDatasource(form, mainDbDataSource);

                if (!CamposMatrizEstaoValidos(form, dbdts, _matriz))
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
                OnAdicionarLinha(FormUID);
            }
            else if (pVal.ItemUID == _remover.ItemUID)
            {
                OnRemoverLinha(FormUID);
            }
            else if (pVal.ItemUID == _salvar.ItemUID)
            {
                OnSalvar(FormUID);
            }
        }

        private void OnSalvar(string formUID)
        {
            var form = GetForm(formUID);
            try
            {
                form.Freeze(true);
                Global.Company.StartTransaction();

                var dbdts = GetDBDatasource(form, mainDbDataSource);
                var rs = Helpers.DoQuery($"DELETE FROM [{dbdts.TableName}];");
                var userTable = Global.Company.UserTables.Item(dbdts.TableName.Remove(0, 1));

                var mtx = GetMatrix(form, _matriz.ItemUID);
                mtx.FlushToDataSource();
                var grupoDeItemDataSource = _matriz._grupoDeItem.Datasource;
                bool ok = true;
                for (int i = 0; i < dbdts.Size; i++)
                {
                    var grupoDeItem = dbdts.GetValue(grupoDeItemDataSource, i);
                    if (!string.IsNullOrEmpty(grupoDeItem))
                    {
                        var codigo = (i + 1).ToString();
                        userTable.Code = codigo;
                        userTable.Name = codigo;
                        userTable.UserFields.Fields.Item(grupoDeItemDataSource).Value = grupoDeItem;

                        if (userTable.Add() != 0)
                        {
                            ok = false;
                            break;
                        }
                    }
                }

                if (!ok)
                {
                    Dialogs.PopupError("Erro ao salvar dados.\nErro: " + Global.Company.GetLastErrorDescription());
                }
                else
                {
                    Global.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    Dialogs.PopupSuccess("Dados salvos com sucesso.");
                }
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao salvar dados.\nErro: " + e.Message);
                Global.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void OnRemoverLinha(string FormUID)
        {
            var form = GetForm(FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);
            _matriz.RemoverLinha(form);
        }

        private void OnAdicionarLinha(string FormUID)
        {
            var form = GetForm(FormUID);

            _matriz.AdicionarLinha(form);
        }

        #endregion


        #region :: Configuração da Matriz

        public class Matriz : MatrizMasterDataForm
        {
            public ComboFormObrigatorio _grupoDeItem = new ComboFormObrigatorio()
            {
                ItemUID = "ItmsGrpCod",
                Datasource = "U_ItmsGrpCod",
                SQL = "SELECT ItmsGrpCod, ItmsGrpNam FROM OITB ORDER BY ItmsGrpNam",
                Mensagem = "O Grupo de Item é obrigatório."
            };
        }

        #endregion
    }
}
