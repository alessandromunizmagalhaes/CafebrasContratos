using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormPreContrato : SAPHelper.Form
    {
        public override string FormType { get { return "FormPreContrato"; } }
        private const string mainDbDataSource = "@UPD_OCCC";
        private const string nomeFormDetalheDoItem = "FormDetalheItem.srf";

        #region :: Campos



        public ItemForm _numeroDoContrato = new ItemForm()
        {
            ItemUID = "DocNumCC",
            Datasource = "U_DocNumCC",
        };
        public ItemFormObrigatorio _dataInicio = new ItemFormObrigatorio()
        {
            ItemUID = "DataIni",
            Datasource = "U_DataIni",
            Mensagem = "A Data de Início é obrigatória."
        };
        public ItemFormObrigatorio _dataFim = new ItemFormObrigatorio()
        {
            ItemUID = "DataFim",
            Datasource = "U_DataFim",
            Mensagem = "A Data Final é obrigatória."
        };
        public ItemForm _status = new ItemForm()
        {
            ItemUID = "StatusQua",
            Datasource = "U_StatusQua"
        };
        public ItemFormObrigatorio _descricao = new ItemFormObrigatorio()
        {
            ItemUID = "Descricao",
            Datasource = "U_Descricao",
            Mensagem = "A Descrição Geral é obrigatória."
        };

        public ItemFormObrigatorio _codigoPN = new ItemFormObrigatorio()
        {
            ItemUID = "CardCode",
            Datasource = "U_CardCode",
            Mensagem = "O Parceiro de Negócios é obrigatório."
        };
        public ItemForm _nomePN = new ItemForm()
        {
            ItemUID = "CardName",
            Datasource = "U_CardName",
        };
        public ItemFormObrigatorio _telefone = new ItemFormObrigatorio()
        {
            ItemUID = "Tel1",
            Datasource = "U_Tel1",
            Mensagem = "O telefone do contato é obrigatório."
        };
        public ItemFormObrigatorio _email = new ItemFormObrigatorio()
        {
            ItemUID = "EMail",
            Datasource = "U_EMail",
            Mensagem = "O E-mail do contato é obrigatório."
        };
        public ComboFormObrigatorio _pessoasDeContato = new ComboFormObrigatorio()
        {
            ItemUID = "CtName",
            Datasource = "U_CtName",
            Mensagem = "A pessoa de contato é obrigatória."
        };

        public ComboFormObrigatorio _modalidade = new ComboFormObrigatorio()
        {
            ItemUID = "ModCtto",
            Datasource = "U_ModCtto",
            Mensagem = "A modalidade do contrato é obrigatória",
            SQL = "SELECT Code, Name FROM [@UPD_OMOD] ORDER BY Name"
        };
        public ComboFormObrigatorio _unidadeComercial = new ComboFormObrigatorio()
        {
            ItemUID = "UnidCom",
            Datasource = "U_UnidCom",
            Mensagem = "A unidade de comercial é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OUCM] ORDER BY Name"
        };
        public ComboFormObrigatorio _tipoDeOperacao = new ComboFormObrigatorio()
        {
            ItemUID = "TipoOper",
            Datasource = "U_TipoOper",
            Mensagem = "O tipo de operação é obrigatório.",
            SQL = "SELECT Code, Name FROM [@UPD_OTOP] ORDER BY Name"
        };
        public ComboFormObrigatorio _metodoFinanceiro = new ComboFormObrigatorio()
        {
            ItemUID = "MtdFin",
            Datasource = "U_MtdFin",
            Mensagem = "O método financeiro é obrigatório.",
            SQL = "SELECT Code, Name FROM [@UPD_OMFN] ORDER BY Name"
        };

        public ComboFormObrigatorio _utilizacao = new ComboFormObrigatorio()
        {
            ItemUID = "Usage",
            Datasource = "U_Usage",
            Mensagem = "A utilização é obrigatória.",
            SQL = "SELECT ID, Usage FROM OUSG ORDER BY Usage"
        };
        public ComboFormObrigatorio _embalagem = new ComboFormObrigatorio()
        {
            ItemUID = "Packg",
            Datasource = "U_Packg",
            Mensagem = "A embalagem é obrigatória.",
            SQL = "SELECT PkgCode, PkgType FROM OPKG ORDER BY PkgType",
        };
        public ComboFormObrigatorio _safra = new ComboFormObrigatorio()
        {
            ItemUID = "Safra",
            Datasource = "U_Safra",
            Mensagem = "A safra é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OSAF] ORDER BY Name"
        };
        public ItemFormObrigatorio _codigoItem = new ItemFormObrigatorio()
        {
            ItemUID = "ItemCode",
            Datasource = "U_ItemCode",
            Mensagem = "O Item é obrigatório."
        };
        public ItemForm _nomeItem = new ItemForm()
        {
            ItemUID = "ItemName",
            Datasource = "U_ItemName"
        };
        public ItemFormObrigatorio _deposito = new ItemFormObrigatorio()
        {
            ItemUID = "WhsCode",
            Datasource = "U_WhsCode",
            Mensagem = "O depósito é obrigatório."
        };
        public ItemForm _botaoDetalheDoItem = new ItemForm()
        {
            ItemUID = "btnItem",
        };

        #endregion


        #region :: Eventos de Formulário

        public override void OnBeforeFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(BusinessObjectInfo.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            string next_code = GetNextCode(mainDbDataSource);

            dbdts.SetValue("Code", 0, next_code);
            dbdts.SetValue("Name", 0, next_code);
            dbdts.SetValue(_numeroDoContrato.Datasource, 0, GetNextCode(mainDbDataSource));

            Dialogs.Info("Adicionando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);
            try
            {
                ValidarCamposObrigatorios(dbdts);
            }
            catch (FormValidationException e)
            {
                Dialogs.MessageBox(e.Message);
                form.Items.Item(e.Campo).Click();
                BubbleEvent = false;
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao tentar adicionar valores do formulário.\nErro: " + e.Message);
            }
        }

        public override void OnAfterFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var form = GetForm(BusinessObjectInfo.FormUID);

            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = false;
            form.Items.Item(_status.ItemUID).Enabled = true;

            var dbdts = GetDBDatasource(form, mainDbDataSource);

            string codigoPN = dbdts.GetValue(_codigoPN.Datasource, 0);
            string pessoasDeContato = dbdts.GetValue(_pessoasDeContato.Datasource, 0);
            PopularPessoasDeContato(form, codigoPN, pessoasDeContato);
        }

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(FormUID);

            _modalidade.PopularComboBox(form);
            _unidadeComercial.PopularComboBox(form);
            _tipoDeOperacao.PopularComboBox(form);
            _metodoFinanceiro.PopularComboBox(form);
            _utilizacao.PopularComboBox(form);
            _embalagem.PopularComboBox(form);
            _safra.PopularComboBox(form);

            // clicando para a primeira aba já vir selecionada
            form.Items.Item("AbaGeral").Click();

            ConditionsParaFornecedores(form);
        }

        public override void OnAfterComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _pessoasDeContato.ItemUID)
            {
                var dbdts = GetDBDatasource(FormUID, mainDbDataSource);
                string pessoaDeContato = dbdts.GetValue(_pessoasDeContato.Datasource, 0);
                string cardcode = dbdts.GetValue(_codigoPN.Datasource, 0);
                AtualizarDadosPessoaDeContato(cardcode, pessoaDeContato, dbdts);
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _botaoDetalheDoItem.ItemUID)
            {
                CriarFormFilho(AppDomain.CurrentDomain.BaseDirectory + "/" + nomeFormDetalheDoItem, FormUID, new FormDetalheItem());
            }
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!choose.IsSystem)
            {
                var dbdts = GetDBDatasource(pVal.FormUID, mainDbDataSource);
                var dataTable = chooseEvent.SelectedObjects;

                if (chooseEvent.ItemUID == _codigoPN.ItemUID)
                {
                    OnCardCodeChoose(form, dbdts, dataTable);
                }
                else if (chooseEvent.ItemUID == _codigoItem.ItemUID)
                {
                    OnItemCodeChoose(dbdts, dataTable);
                }
                else if (chooseEvent.ItemUID == _deposito.ItemUID)
                {
                    OnWhsCodeChoose(dbdts, dataTable);
                }
            }
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = false;
            form.Items.Item(_status.ItemUID).Enabled = false;

            var dbdts = GetDBDatasource(form, mainDbDataSource);

            SugerirDataDoContrato(dbdts);

            PopularPessoasDeContato(form, "", "");

            form.Items.Item(_descricao.ItemUID).Click();
        }

        public override void _OnPesquisar(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = true;
            form.Items.Item(_status.ItemUID).Enabled = true;
        }

        #endregion


        #region :: Choose From Lists

        private void OnItemCodeChoose(DBDataSource dbdts, DataTable dataTable)
        {
            string itemcode = dataTable.GetValue("ItemCode", 0);
            string itemname = dataTable.GetValue("ItemName", 0);

            dbdts.SetValue(_codigoItem.Datasource, 0, itemcode);
            dbdts.SetValue(_nomeItem.Datasource, 0, itemname);
        }

        private void OnWhsCodeChoose(DBDataSource dbdts, DataTable dataTable)
        {
            string whscode = dataTable.GetValue("WhsCode", 0);

            dbdts.SetValue(_deposito.Datasource, 0, whscode);
        }

        private void OnCardCodeChoose(SAPbouiCOM.Form form, DBDataSource dbdts, DataTable dataTable)
        {
            string cardcode = dataTable.GetValue("CardCode", 0);
            string cardname = dataTable.GetValue("CardName", 0);
            string pessoaDeContato = dataTable.GetValue("CntctPrsn", 0);

            dbdts.SetValue(_codigoPN.Datasource, 0, cardcode);
            dbdts.SetValue(_nomePN.Datasource, 0, cardname);

            PopularPessoasDeContato(form, cardcode, pessoaDeContato);
        }

        #endregion


        #region :: Conditions

        private static void ConditionsParaFornecedores(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item("PN");
            Conditions oConds = oCFL.GetConditions();

            Condition oCond = oConds.Add();

            oCond.Alias = "CardType";
            oCond.Operation = BoConditionOperation.co_EQUAL;
            oCond.CondVal = "S";

            oCFL.SetConditions(oConds);
        }

        #endregion


        #region :: Regras de negócio

        private void PopularPessoasDeContato(SAPbouiCOM.Form form, string cardcode, string pessoaDeContatoSelecionada)
        {
            _pessoasDeContato.SQL =
                    $@"SELECT 
	                    Name, Name
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}'";
            _pessoasDeContato.PopularComboBox(form);

            var dbdts = GetDBDatasource(form, mainDbDataSource);
            dbdts.SetValue(_pessoasDeContato.Datasource, 0, pessoaDeContatoSelecionada);
            AtualizarDadosPessoaDeContato(cardcode, pessoaDeContatoSelecionada, dbdts);
        }

        private void AtualizarDadosPessoaDeContato(string cardcode, string pessoaDeContato, DBDataSource dbdts)
        {
            var sql =
                $@"SELECT
                        Tel1
	                    , E_MailL
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}' AND Name = '{pessoaDeContato}'";
            var rs = Helpers.DoQuery(sql);

            var telefone = "";
            var email = "";

            if (rs.RecordCount > 0)
            {
                telefone = rs.Fields.Item("Tel1").Value;
                email = rs.Fields.Item("E_MailL").Value;
            }

            dbdts.SetValue(_telefone.Datasource, 0, telefone);
            dbdts.SetValue(_email.Datasource, 0, email);
        }

        private void SugerirDataDoContrato(DBDataSource dbdts)
        {
            const int mesesPraFrente = 6;
            DateTime dataFinal = new DateTime(DateTime.Now.AddMonths(mesesPraFrente).Year, DateTime.Now.AddMonths(mesesPraFrente).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(mesesPraFrente).Year, DateTime.Now.AddMonths(mesesPraFrente).Month));

            dbdts.SetValue(_dataInicio.Datasource, 0, Helpers.DateToString(DateTime.Now));
            dbdts.SetValue(_dataFim.Datasource, 0, Helpers.DateToString(dataFinal));
            dbdts.SetValue(_status.Datasource, 0, "A");
            dbdts.SetValue(_numeroDoContrato.Datasource, 0, GetNextPrimaryKey(mainDbDataSource, _numeroDoContrato.Datasource));
        }

        #endregion
    }
}
