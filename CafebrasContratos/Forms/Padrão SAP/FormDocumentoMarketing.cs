﻿using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public abstract class FormDocumentoMarketing : SAPHelper.Form
    {
        public abstract override string FormType { get; }
        public abstract string mainDbDataSource { get; }
        private const string menuUID = "2305";

        public FormDocumentoMarketing()
        {
            var camposSAP = new CamposTabelaSAP();
            _numeroContratoFinal.ItemUID = camposSAP.numeroContratoFilho.NomeComU_NaFrente;
            _numeroContratoFinal.Datasource = _numeroContratoFinal.ItemUID;
            _filhoDeContrato.Datasource = camposSAP.filhoDeContrato.NomeComU_NaFrente;
        }

        #region :: Campos

        public ItemForm _numeroContratoFinal = new ItemForm();
        public ItemForm _filhoDeContrato = new ItemForm()
        {
            ItemUID = "SOContract"
        };

        #endregion


        #region :: Eventos de Item

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

                var editNumeroContratoFinal = form.Items.Add(_numeroContratoFinal.ItemUID, BoFormItemTypes.it_EDIT);

                editNumeroContratoFinal.Enabled = false;
                editNumeroContratoFinal.FromPane = 0;
                editNumeroContratoFinal.ToPane = 0;

                editNumeroContratoFinal.SetAutoManagedAttribute(BoAutoManagedAttr.ama_Editable, (int)BoAutoFormMode.afm_All, BoModeVisualBehavior.mvb_False);

                int comboTop = itemRef.Top + 23;
                editNumeroContratoFinal.Top = comboTop;
                editNumeroContratoFinal.Left = itemRef.Left;
                editNumeroContratoFinal.Width = itemRef.Width;
                editNumeroContratoFinal.DisplayDesc = true;

                ((EditText)editNumeroContratoFinal.Specific).DataBind.SetBound(true, mainDbDataSource, _numeroContratoFinal.Datasource);

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

                var editVeioContrato = form.Items.Add(_filhoDeContrato.ItemUID, BoFormItemTypes.it_EDIT);

                editVeioContrato.Visible = false;
                editVeioContrato.FromPane = 0;
                editVeioContrato.ToPane = 0;

                editVeioContrato.Top = editNumeroContratoFinal.Top;
                editVeioContrato.Left = editNumeroContratoFinal.Left - 120;
                editVeioContrato.Width = 15;

                ((EditText)editVeioContrato.Specific).DataBind.SetBound(true, mainDbDataSource, _filhoDeContrato.Datasource);
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

        #endregion


        #region :: Regras de Negócio

        public void PreencherPedido(SAPbouiCOM.Form form, PedidoCompraParams param)
        {
            try
            {
                form.Freeze(true);
                form.Mode = BoFormMode.fm_ADD_MODE;
                form.Items.Item(_numeroContratoFinal.ItemUID).Specific.Value = param.NumContratoFinal;
                form.Items.Item(_filhoDeContrato.ItemUID).Specific.Value = "S";
                form.Items.Item("4").Specific.Value = param.Fornecedor;
                form.Items.Item(_numeroContratoFinal.ItemUID).Enabled = false;

                if (!String.IsNullOrEmpty(param.Filial))
                {
                    ((ComboBox)form.Items.Item("2001").Specific).Select(param.Filial, BoSearchKey.psk_ByValue);
                }

                // aba imposto
                form.Items.Item("2013").Click();

                form.Items.Item("2022").Specific.Value = param.Transportadora;

                // aba geral
                form.Items.Item("112").Click();

                var matrix = GetMatrix(form, "38");
                matrix.Columns.Item("1").Cells.Item(1).Specific.Value = param.Item;
                if (param.Quantidade > 0)
                {
                    matrix.Columns.Item("11").Cells.Item(1).Specific.Value = Helpers.ToString(param.Quantidade);
                }

                form.Freeze(true);
                form.Freeze(false);

                ((ComboBox)matrix.Columns.Item("2011").Cells.Item(1).Specific).Select(param.Utilizacao, BoSearchKey.psk_ByValue);
                matrix.Columns.Item("U_ATL_Tipo_embalagem").Cells.Item(1).Specific.Value = param.Embalagem;
                matrix.Columns.Item("24").Cells.Item(1).Specific.Value = param.Deposito;
                matrix.Columns.Item("14").Cells.Item(1).Specific.Value = Helpers.ToString(param.PrecoUnitario);

                matrix.Columns.Item("1").Cells.Item(1).Click();
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

        public SAPbouiCOM.Form Abrir(string codigo = "")
        {
            return Global.SBOApplication.OpenForm(BoFormObjectEnum.fo_PurchaseOrder, "DocEntry", codigo);
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnDuplicar(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroContratoFinal.ItemUID).Specific.Value = "";
        }

        #endregion
    }
}
