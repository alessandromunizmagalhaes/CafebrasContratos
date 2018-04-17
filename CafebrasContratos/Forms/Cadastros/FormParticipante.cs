using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class FormParticipante : SAPHelper.Form
    {
        public override string FormType { get { return "FormParticipante"; } }
        private string mainDbDataSource = DbConfig.participante.NomeComArroba;

        #region :: Campos

        public ItemFormObrigatorio _codigo = new ItemFormObrigatorio()
        {
            ItemUID = "Code",
            Datasource = "Code",
            Mensagem = "O código é obrigatório",
        };
        public ItemFormObrigatorio _tipo = new ItemFormObrigatorio()
        {
            ItemUID = "Tipo",
            Datasource = "U_Tipo",
            Mensagem = "O tipo é obrigatório",
        };
        public ItemFormObrigatorio _nome = new ItemFormObrigatorio()
        {
            ItemUID = "Name",
            Datasource = "Name",
            Mensagem = "O nome é obrigatório",
        };

        #endregion

        #region :: Eventos de Formulário

        public override void OnBeforeFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(BusinessObjectInfo.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            BubbleEvent = CamposFormEstaoPreenchidos(form, dbdts);
        }

        public override void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(BusinessObjectInfo.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            BubbleEvent = CamposFormEstaoPreenchidos(form, dbdts);
        }

        #endregion
    }
}
