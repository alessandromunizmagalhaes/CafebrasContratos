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
        }

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            DBDataSource dbdts = GetDBDatasource(form, mainDbDataSource);

            const int mesesPraFrente = 6;
            DateTime dataFinal = new DateTime(DateTime.Now.AddMonths(mesesPraFrente).Year, DateTime.Now.AddMonths(mesesPraFrente).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(mesesPraFrente).Year, DateTime.Now.AddMonths(mesesPraFrente).Month));

            dbdts.SetValue(DataInicio.Datasource, 0, Helpers.DateToString(DateTime.Now));
            dbdts.SetValue(DataFim.Datasource, 0, Helpers.DateToString(dataFinal));
            dbdts.SetValue(Status.Datasource, 0, "A");
            dbdts.SetValue(NumeroDoContrato.Datasource, 0, GetNextPrimaryKey(mainDbDataSource, NumeroDoContrato.Datasource));
        }

        #endregion
    }
}
