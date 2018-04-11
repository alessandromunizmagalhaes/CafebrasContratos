using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormComissoes : SAPHelper.Form
    {
        public override string FormType { get { return "FormComissoes"; } }
        private const string corretorDbDataSource = "@UPD_CCC2";
        private const string responsavelDbDataSource = "@UPD_CCC3";
        public static string _fatherFormUID = "";

        #region :: Campos de Corretores

        public MatrizCorretores _corretores = new MatrizCorretores()
        {
            ItemUID = "matrix1",
            Datasource = corretorDbDataSource
        };
        public ButtonForm _adicionarCorretores = new ButtonForm()
        {
            ItemUID = "btnAdd1"
        };
        public ButtonForm _removerCorretores = new ButtonForm()
        {
            ItemUID = "btnRmv1"
        };

        #endregion


        #region :: Campos de Responsáveis

        public MatrizResponsaveis _responsaveis = new MatrizResponsaveis()
        {
            ItemUID = "matrix2",
            Datasource = responsavelDbDataSource
        };
        public ButtonForm _adicionarResponsavel = new ButtonForm()
        {
            ItemUID = "btnAdd2"
        };
        public ButtonForm _removerResponsavel = new ButtonForm()
        {
            ItemUID = "btnRmv2"
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

                _corretores.CriarColunaSumAuto(form, _corretores._comissao.ItemUID);
                _responsaveis.CriarColunaSumAuto(form, _responsaveis._comissao.ItemUID);

                CarregarDadosMatriz(form, _fatherFormUID, _corretores.ItemUID, corretorDbDataSource);
                CarregarDadosMatriz(form, _fatherFormUID, _responsaveis.ItemUID, responsavelDbDataSource);

                var mtxCorretores = ((Matrix)form.Items.Item(_corretores.ItemUID).Specific);
                var mtxResponsaveis = ((Matrix)form.Items.Item(_responsaveis.ItemUID).Specific);

                ClicarParaCalcularOsTotalizadores(mtxCorretores, _corretores._comissao.ItemUID);
                ClicarParaCalcularOsTotalizadores(mtxResponsaveis, _responsaveis._comissao.ItemUID);

                _corretores._participante.Popular(mtxCorretores);
                _responsaveis._participante.Popular(mtxResponsaveis);
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

            if (pVal.ItemUID == _adicionarCorretores.ItemUID)
            {
                OnAdicionarCorretorClick(pVal);
            }
            else if (pVal.ItemUID == _removerCorretores.ItemUID)
            {
                OnRemoverCorretorClick(pVal);
            }
            else if (pVal.ItemUID == _adicionarResponsavel.ItemUID)
            {
                OnAdicionarResponsavelClick(pVal);
            }
            else if (pVal.ItemUID == _removerResponsavel.ItemUID)
            {
                OnRemoverResponsavelClick(pVal);
            }
            else if (pVal.ItemUID == "1")
            {
                CarregarDataSourceFormPai(FormUID, _fatherFormUID, _corretores.ItemUID, corretorDbDataSource);
                CarregarDataSourceFormPai(FormUID, _fatherFormUID, _responsaveis.ItemUID, responsavelDbDataSource);
            }
        }

        private void OnAdicionarCorretorClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, corretorDbDataSource);

            _corretores.AdicionarLinha(form, dbdts);
        }

        private void OnRemoverCorretorClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, corretorDbDataSource);

            _corretores.RemoverLinha(form, dbdts);
        }

        private void OnAdicionarResponsavelClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, responsavelDbDataSource);

            _responsaveis.AdicionarLinha(form, dbdts);
        }

        private void OnRemoverResponsavelClick(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, responsavelDbDataSource);

            _responsaveis.RemoverLinha(form, dbdts);
        }

        #endregion


        #region :: Regras de Negócio

        private void ClicarParaCalcularOsTotalizadores(Matrix mtx, string colunaUID)
        {
            for (int i = 1; i <= mtx.RowCount; i++)
            {
                mtx.Columns.Item(colunaUID).Cells.Item(i).Click();
            }
        }

        #endregion


        #region :: Configuração das Matrizes

        public class Matriz : MatrizChildForm
        {
            public virtual ComboForm _participante
            {
                get
                {
                    return new ComboForm()
                    {
                        ItemUID = "PartCode",
                        Datasource = "U_PercCom"
                    };
                }
            }
            public ItemForm _comissao = new ItemForm()
            {
                ItemUID = "PercCom",
                Datasource = "U_PercCom"
            };

            protected string GetSQL(string tipoParticipante)
            {
                return $"SELECT Code, Name FROM [@UPD_PART] WHERE Canceled = 'N' AND U_Tipo = '{tipoParticipante}' ORDER BY Name";
            }
        }

        public class MatrizCorretores : Matriz
        {
            public override ComboForm _participante
            {
                get
                {
                    var participante = base._participante;
                    participante.SQL = GetSQL("C");
                    return participante;
                }
            }
        }

        public class MatrizResponsaveis : Matriz
        {
            public override ComboForm _participante
            {
                get
                {
                    var participante = base._participante;
                    participante.SQL = GetSQL("R");
                    return participante;
                }
            }
        }

        #endregion
    }
}
