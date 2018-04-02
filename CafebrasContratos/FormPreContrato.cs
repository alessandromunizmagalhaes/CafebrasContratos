using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormPreContrato : SAPHelper.Form
    {
        public override string FormType { get { return "FrmPreContrato"; } }
        private const string mainDbDataSource = "@UPD_OCCC";

        #region :: Campos

        public ItemForm NumeroDoContrato { get; set; } = new ItemForm() { ItemUID = "DocNumCC", Datasource = "U_DocNumCC" };
        public ItemForm DataInicio { get; set; } = new ItemForm() { ItemUID = "DataIni", Datasource = "U_DataIni" };
        public ItemForm DataFim { get; set; } = new ItemForm() { ItemUID = "DataFim", Datasource = "U_DataFim" };
        public ItemForm Status { get; set; } = new ItemForm() { ItemUID = "StatusQua", Datasource = "U_StatusQua" };
        public ItemForm Descricao { get; set; } = new ItemForm() { ItemUID = "Descricao", Datasource = "U_Descricao" };

        public ItemForm CodigoPN { get; set; } = new ItemForm() { ItemUID = "CardCode", Datasource = "U_CardCode" };
        public ItemForm NomePN { get; set; } = new ItemForm() { ItemUID = "CardName", Datasource = "U_CardName" };
        public ItemForm Telefone { get; set; } = new ItemForm() { ItemUID = "Tel1", Datasource = "U_Tel1" };
        public ItemForm Email { get; set; } = new ItemForm() { ItemUID = "EMail", Datasource = "U_EMail" };
        public ItemForm PessoasDeContato { get; set; } = new ItemForm() { ItemUID = "CtName", Datasource = "U_CtName" };

        public ItemForm Modalidade { get; set; } = new ItemForm() { ItemUID = "ModCtto", Datasource = "U_ModCtto" };
        public ItemForm UnidadeComercial { get; set; } = new ItemForm() { ItemUID = "UnidCom", Datasource = "U_UnidCom" };
        public ItemForm TipoDeOperacao { get; set; } = new ItemForm() { ItemUID = "TipoOper", Datasource = "U_TipoOper" };
        public ItemForm MetodoFinanceiro { get; set; } = new ItemForm() { ItemUID = "MtdFin", Datasource = "U_MtdFin" };


        #endregion


        #region :: Eventos

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(pVal);

            PopularComboBox(form, Modalidade.ItemUID, "SELECT Code, Name FROM [@UPD_OMOD]");
            PopularComboBox(form, UnidadeComercial.ItemUID, "SELECT Code, Name FROM [@UPD_OUCM]");
            PopularComboBox(form, TipoDeOperacao.ItemUID, "SELECT Code, Name FROM [@UPD_OTOP]");
            PopularComboBox(form, MetodoFinanceiro.ItemUID, "SELECT Code, Name FROM [@UPD_OMFN]");

            ConditionsParaFornecedores(form);
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var dataTable = chooseEvent.SelectedObjects;

            string cardcode = dataTable.GetValue("CardCode", 0);
            string cardname = dataTable.GetValue("CardName", 0);
            string pessoaDeContato = dataTable.GetValue("CntctPrsn", 0);

            var dbdts = GetDBDatasource(pVal, mainDbDataSource);

            dbdts.SetValue(CodigoPN.Datasource, 0, cardcode);
            dbdts.SetValue(NomePN.Datasource, 0, cardname);

            PopularPessoasDeContato(form, cardcode, pessoaDeContato);
        }

        public override void OnAfterComboSelect(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == PessoasDeContato.ItemUID)
            {
                var dbdts = GetDBDatasource(pVal, mainDbDataSource);
                string pessoaDeContato = dbdts.GetValue(PessoasDeContato.Datasource, 0);
                string cardcode = dbdts.GetValue(CodigoPN.Datasource, 0);
                AtualizarDadosPessoaDeContato(cardcode, pessoaDeContato, dbdts);
            }
        }

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            const int mesesPraFrente = 6;
            DateTime dataFinal = new DateTime(DateTime.Now.AddMonths(mesesPraFrente).Year, DateTime.Now.AddMonths(mesesPraFrente).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(mesesPraFrente).Year, DateTime.Now.AddMonths(mesesPraFrente).Month));

            dbdts.SetValue(DataInicio.Datasource, 0, Helpers.DateToString(DateTime.Now));
            dbdts.SetValue(DataFim.Datasource, 0, Helpers.DateToString(dataFinal));
            dbdts.SetValue(Status.Datasource, 0, "A");
            dbdts.SetValue(NumeroDoContrato.Datasource, 0, GetNextPrimaryKey(mainDbDataSource, NumeroDoContrato.Datasource));

            form.Items.Item(Descricao.ItemUID).Click();
        }

        #endregion

        #region :: Regras de negócio


        private void PopularPessoasDeContato(SAPbouiCOM.Form form, string cardcode, string pessoaDeContatoSelecionada)
        {
            ComboBox combo = form.Items.Item(PessoasDeContato.ItemUID).Specific;
            string sql =
                $@"SELECT 
	                    Name, Name
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}'";
            PopularComboBox(combo, sql);

            if (!String.IsNullOrEmpty(pessoaDeContatoSelecionada))
            {
                var dbdts = GetDBDatasource(form, mainDbDataSource);
                dbdts.SetValue(PessoasDeContato.Datasource, 0, pessoaDeContatoSelecionada);

                AtualizarDadosPessoaDeContato(cardcode, pessoaDeContatoSelecionada, dbdts);
            }
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

            dbdts.SetValue(Telefone.Datasource, 0, telefone);
            dbdts.SetValue(Email.Datasource, 0, email);
        }

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
    }
}
