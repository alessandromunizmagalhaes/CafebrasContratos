using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormPedidoCompra : SAPHelper.Form
    {
        public override string FormType { get { return ((int)FormTypes.PedidoDeCompra).ToString(); } }
        private const string mainDbDataSource = "OPOR";
        private const string menuUID = "2305";

        public SAPbouiCOM.Form Abrir()
        {
            return Global.SBOApplication.OpenForm(BoFormObjectEnum.fo_PurchaseOrder, "", "");
        }


        public override void OnBeforeFormLoad(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            using (var formCOM = new FormCOM(FormUID))
            {
                var form = formCOM.Form;

                var itemRefUID = "46";
                var itemRef = form.Items.Item(itemRefUID);

                var itemLabelRefUID = "86";
                var itemLabelRef = form.Items.Item(itemLabelRefUID);

                var editNumeroContratoFinal = form.Items.Add("U_DocNumCF", BoFormItemTypes.it_EDIT);

                editNumeroContratoFinal.Enabled = false;
                editNumeroContratoFinal.FromPane = 0;
                editNumeroContratoFinal.ToPane = 0;

                int comboTop = itemRef.Top + 23;
                editNumeroContratoFinal.Top = comboTop;
                editNumeroContratoFinal.Left = itemRef.Left;
                editNumeroContratoFinal.Width = itemRef.Width;
                editNumeroContratoFinal.DisplayDesc = true;

                ((EditText)editNumeroContratoFinal.Specific).DataBind.SetBound(true, mainDbDataSource, "U_DocNumCF");

                var labelGrupoAprovador = form.Items.Add("L_DocNumCF", BoFormItemTypes.it_STATIC);

                labelGrupoAprovador.FromPane = 0;
                labelGrupoAprovador.ToPane = 0;
                labelGrupoAprovador.Top = comboTop;
                labelGrupoAprovador.Left = itemLabelRef.Left;
                labelGrupoAprovador.Width = itemLabelRef.Width;
                labelGrupoAprovador.LinkTo = editNumeroContratoFinal.UniqueID;

                ((StaticText)labelGrupoAprovador.Specific).Caption = "Nº do Contrato";

                var linkedButton = form.Items.Add("B_DocNumCF", BoFormItemTypes.it_LINKED_BUTTON);

                linkedButton.Top = editNumeroContratoFinal.Top - 1;
                linkedButton.Left = editNumeroContratoFinal.Left - 19;
                linkedButton.LinkTo = editNumeroContratoFinal.UniqueID;

                ((LinkedButton)linkedButton.Specific).LinkedObject = BoLinkedObject.lf_BusinessPartner;
            }
        }

        public void PreencherPedido(SAPbouiCOM.Form form, PedidoCompraParams param)
        {
            try
            {
                form.Freeze(true);
                form.Mode = BoFormMode.fm_ADD_MODE;
                form.Items.Item("U_DocNumCF").Specific.Value = param.NumContratoFinal;
                form.Items.Item("4").Specific.Value = param.Fornecedor;
                form.Items.Item("U_DocNumCF").Enabled = false;

                // clicando na aba imposto
                form.Items.Item("2013").Click();
                form.Items.Item("2022").Specific.Value = param.Transportadora;

                form.Items.Item("112").Click();

                var matrix = GetMatrix(form, "38");
                //matrix.Columns.Item("1").Cells.Item(1).Specific.Value = param.Item;
                if (param.Quantidade > 0)
                {
                    matrix.Columns.Item("11").Cells.Item(1).Specific.Value = Helpers.ToString(param.Quantidade);
                }
                ((ComboBox)matrix.Columns.Item("2011").Cells.Item(1).Specific).Select(param.Utilizacao, BoSearchKey.psk_ByValue);
                matrix.Columns.Item("U_ATL_Tipo_embalagem").Cells.Item(1).Specific.Value = param.Embalagem;
                matrix.Columns.Item("24").Cells.Item(1).Specific.Value = param.Deposito;

                form.Items.Item("14").Click();
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao preencher dados do pedido de compra.\nErro: " + e.Message);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        public override void OnBeforeItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            base.OnAfterItemPressed(FormUID, ref pVal, out BubbleEvent);

            if (pVal.ItemUID == "B_DocNumCF")
            {
                using (var formCOM = new FormCOM(FormUID))
                {
                    var form = formCOM.Form;
                    using (var dbdtsCOM = new DBDatasourceCOM(form, mainDbDataSource))
                    {
                        BubbleEvent = false;
                        var dbdts = dbdtsCOM.Dbdts;
                        var codigo = dbdts.GetValue("U_DocNumCF", 0).Trim();
                        FormContratoFinal.AbrirNoRegistro(codigo);
                    }
                }
            }
        }


    }
}
