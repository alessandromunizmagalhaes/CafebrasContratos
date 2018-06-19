using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public abstract class FormComissoes : SAPHelper.Form
    {
        public abstract override string FormType { get; }
        public abstract string corretorDbDataSource { get; }
        public abstract string responsavelDbDataSource { get; }

        public string MatrixCorretoresUID { get; private set; } = "matrix1";
        public string MatrixResponsaveisUID { get; private set; } = "matrix2";

        public static string _fatherFormUID = "";

        #region :: Campos de Corretores

        public abstract MatrizCorretores _corretores { get; }
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

        public abstract MatrizResponsaveis _responsaveis { get; }
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

                var statusContratoPai = formPai.GetStatus(_fatherFormUID);
                form.Items.Item("1").Enabled = UsuarioPermitido() && formPai.ContratoPodeSerAlterado(statusContratoPai);
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
                var mtxCorretor = ((Matrix)form.Items.Item(_corretores.ItemUID).Specific);
                var mtxResponsaveis = ((Matrix)form.Items.Item(_responsaveis.ItemUID).Specific);

                mtxCorretor.FlushToDataSource();
                mtxResponsaveis.FlushToDataSource();

                var dbdtsCorretor = GetDBDatasource(form, corretorDbDataSource);
                var dbdtsResponsavel = GetDBDatasource(form, responsavelDbDataSource);

                if (!CamposMatrizEstaoValidos(form, dbdtsCorretor, _corretores) || !CamposMatrizEstaoValidos(form, dbdtsResponsavel, _responsaveis))
                {
                    BubbleEvent = false;
                }
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

                var fatherForm = GetFormIfExists(_fatherFormUID);
                if (fatherForm != null)
                {
                    ChangeFormMode(fatherForm);
                }
            }
        }

        public override void OnAfterComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var form = GetForm(FormUID);
            var matrix = GetMatrix(form, pVal.ItemUID);
            matrix.FlushToDataSource();

            var dbdtsName = string.Empty;
            Matriz matriz = null;

            if (pVal.ItemUID == _corretores.ItemUID)
            {
                dbdtsName = corretorDbDataSource;
                matriz = _corretores;
            }
            else if (pVal.ItemUID == _responsaveis.ItemUID)
            {
                dbdtsName = responsavelDbDataSource;
                matriz = _responsaveis;
            }

            var dbdts = GetDBDatasource(form, dbdtsName);
            var participante = matriz._participante.GetValorDBDatasource<string>(dbdts, pVal.Row - 1);
            if (!String.IsNullOrEmpty(participante))
            {
                matriz._comissao.GetValorDBDatasource<int>(dbdts);
                var comissao = GetComissaoPadrao(participante);
                matriz._comissao.SetValorDBDatasource(dbdts, comissao);
                matrix.LoadFromDataSourceEx();
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

            _corretores.RemoverLinhaSelecionada(form, dbdts);
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

            _responsaveis.RemoverLinhaSelecionada(form, dbdts);
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

        private double GetComissaoPadrao(string codigoParticipante)
        {
            var rs = Helpers.DoQuery($"SELECT U_PercCom FROM [@UPD_PART] WHERE Code = '{codigoParticipante}'");
            return rs.Fields.Item("U_PercCom").Value;
        }

        #endregion


        #region :: Configuração das Matrizes

        public class Matriz : MatrizChildForm
        {
            public ComboFormObrigatorio _participante = new ComboFormObrigatorio()
            {
                ItemUID = "PartCode",
                Datasource = "U_PartCode"
            };

            public ItemFormObrigatorio _comissao = new ItemFormObrigatorio()
            {
                ItemUID = "PercCom",
                Datasource = "U_PercCom",
            };

            protected string GetSQL(string tipoParticipante)
            {
                return $"SELECT Code, Name FROM [@UPD_PART] WHERE Canceled = 'N' AND U_Tipo = '{tipoParticipante}' ORDER BY Name";
            }
        }

        public class MatrizCorretores : Matriz
        {
            public MatrizCorretores()
            {
                _participante.Mensagem = "O corretor é obrigatório";
                _participante.SQL = GetSQL("C");

                _comissao.Mensagem = "A comissão do corretor é obrigatória";
            }
        }

        public class MatrizResponsaveis : Matriz
        {
            public MatrizResponsaveis()
            {
                _participante.Mensagem = "O responsável é obrigatório";
                _participante.SQL = GetSQL("R");

                _comissao.Mensagem = "A comissão do responsável é obrigatória";
            }
        }

        #endregion


        #region :: Abstracts

        public abstract bool UsuarioPermitido();
        public abstract FormContrato formPai { get; }

        #endregion
    }
}
