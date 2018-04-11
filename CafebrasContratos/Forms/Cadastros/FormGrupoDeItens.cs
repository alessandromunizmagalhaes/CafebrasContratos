using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class FormGrupoDeItens : SAPHelper.Form
    {
        public override string FormType { get { return "FormGrupoDeItens"; } }
        public const string mainDbDataSource = "@UPD_OCTC";

        #region :: Campos

        public MatrizItens _matriz = new MatrizItens()
        {
            ItemUID = "matrix",
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

        #region :: Eventos de Itens

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _adicionar.ItemUID)
            {
                var form = GetForm(FormUID);
            }
        }

        #endregion


        #region :: Configuração da Matriz

        public class MatrizItens : MatrizForm
        {
            public ComboForm _certificado = new ComboForm()
            {
                ItemUID = "Certif",
                Datasource = "U_Certif",
                SQL = "SELECT Code, Name FROM [@UPD_CRTC] WHERE Canceled = 'N' ORDER BY Name"
            };
        }

        #endregion
    }
}
