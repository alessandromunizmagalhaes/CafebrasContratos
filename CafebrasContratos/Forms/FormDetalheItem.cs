using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public class FormDetalheItem : SAPHelper.Form
    {
        public override string FormType { get { return "FormDetalheItem"; } }
        private const string mainDbDataSource = "@UPD_CCC1";
        public static string _fatherFormUID = "";

        #region :: Campos

        public ItemForm _matriz = new ItemForm()
        {
            ItemUID = "mtxItem"
        };
        public ItemForm _botaoAdicionar = new ItemForm()
        {
            ItemUID = "btnAdd"
        };
        public ItemForm _botaoRemover = new ItemForm()
        {
            ItemUID = "btnRmv"
        };

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            CarregarDadosMatriz(FormUID, _fatherFormUID);
        }

        private void CarregarDadosMatriz(string FormUID, string fatherFormUID)
        {
            var localForm = GetForm(FormUID);
            var localDbdts = GetDBDatasource(localForm, mainDbDataSource);

            var fatherForm = GetForm(fatherFormUID);
            var fatherDbdts = GetDBDatasource(fatherForm, mainDbDataSource);

            Copy(fatherDbdts, ref localDbdts);

            var mtx = ((Matrix)localForm.Items.Item(_matriz.ItemUID).Specific);
            mtx.LoadFromDataSourceEx();
        }

        public override void OnAfterFormClose(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            var fatherForm = GetForm(_fatherFormUID);
            if (fatherForm.Mode == BoFormMode.fm_OK_MODE)
            {
                fatherForm.Mode = BoFormMode.fm_UPDATE_MODE;
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ItemUID == _botaoAdicionar.ItemUID)
            {
                OnBotaoAdicionar(pVal);
            }
            else if (pVal.ItemUID == "1")
            {
                CarregarDataSourceFormPai(FormUID, _fatherFormUID);
            }
        }

        private void CarregarDataSourceFormPai(string localFormUID, string fatherFormUID)
        {
            var fatherForm = GetForm(fatherFormUID);
            var fatherDbdts = GetDBDatasource(fatherForm, mainDbDataSource);

            var localForm = GetForm(localFormUID);
            ((Matrix)localForm.Items.Item(_matriz.ItemUID).Specific).FlushToDataSource();
            var localDbdts = GetDBDatasource(localForm, mainDbDataSource);

            Copy(localDbdts, ref fatherDbdts);
        }

        private void OnBotaoAdicionar(ItemEvent pVal)
        {
            var form = GetForm(pVal.FormUID);
            var dbdts = GetDBDatasource(form, mainDbDataSource);
            var mtx = ((Matrix)form.Items.Item(_matriz.ItemUID).Specific);

            dbdts.InsertRecord(dbdts.Size);
            mtx.LoadFromDataSourceEx();
        }

        #endregion


    }
}
