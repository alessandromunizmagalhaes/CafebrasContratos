using SAPbouiCOM;

namespace SAPHelper
{
    public abstract class FormCadastroBasico : Form
    {
        public abstract string mainDbDataSource { get; }

        #region :: Campos

        public ItemFormObrigatorio _codigo = new ItemFormObrigatorio()
        {
            ItemUID = "Code",
            Datasource = "Code",
            Mensagem = "O código é obrigatório",
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

            BubbleEvent = ValidarCamposObrigatorios(form, dbdts);
        }

        public override void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var form = GetForm(BusinessObjectInfo.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);

            BubbleEvent = ValidarCamposObrigatorios(form, dbdts);
        }

        #endregion
    }
}
