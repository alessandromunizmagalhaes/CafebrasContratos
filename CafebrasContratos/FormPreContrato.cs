using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class FormPreContrato : SAPHelper.Form
    {
        public override string FormType { get { return "UPD_OCCC"; } }
        private const string mainDbdatasource = "@UPD_OCCC";

        #region :: Campos

        public ItemForm NumeroDoContrato { get; set; } = new ItemForm() { ItemUID = "DocNumCC", Datasource = "U_DocNumCC" };
        public ItemForm Modalidade { get; set; } = new ItemForm() { ItemUID = "ModCtto", Datasource = "U_ModCtto" };
        public ItemForm UnidadeComercial { get; set; } = new ItemForm() { ItemUID = "UnidCom", Datasource = "U_UnidCom" };
        public ItemForm TipoDeOperacao { get; set; } = new ItemForm() { ItemUID = "TipoOper", Datasource = "U_TipoOper" };
        public ItemForm MetodoFinanceiro { get; set; } = new ItemForm() { ItemUID = "MtdFin", Datasource = "U_MtdFin" };


        #endregion

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(pVal);

            PopularComboBox(form, Modalidade.ItemUID, "SELECT Code, Name FROM [@UPD_OMOD]");
            PopularComboBox(form, UnidadeComercial.ItemUID, "SELECT Code, Name FROM [@UPD_OUCM]");
            PopularComboBox(form, TipoDeOperacao.ItemUID, "SELECT Code, Name FROM [@UPD_OTOP]");
            PopularComboBox(form, MetodoFinanceiro.ItemUID, "SELECT Code, Name FROM [@UPD_OMFN]");
        }
    }
}
