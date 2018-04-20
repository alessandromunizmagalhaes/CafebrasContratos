using SAPbouiCOM;
using SAPHelper;
using System;

namespace CafebrasContratos
{
    public class FormPreContrato : SAPHelper.Form
    {
        #region :: Propriedades

        public override string FormType { get { return "FormPreContrato"; } }
        private string mainDbDataSource = DbPreContrato.preContrato.NomeComArroba;

        private const string nomeFormAberturaPorPeneira = "FormAberturaPorPeneira.srf";
        private const string nomeFormDetalheCertificado = "FormDetalheCertificado.srf";
        private const string nomeFormComissoes = "FormComissoes.srf";

        private const string abaGeralUID = "AbaGeral";
        private const string abaItemUID = "AbaItem";
        private const string abaContratoFinalUID = "AbaCFinal";
        private const string abaObsUID = "AbaObs";

        private const string choosePNUID = "PN";
        private const string chooseItemUID = "Item";
        private const string chooseDepositoUID = "Warehouse";

        #endregion


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
            Mensagem = "O Parceiro de Negócios é obrigatório.",
            AbaUID = abaGeralUID,
        };
        public ItemForm _nomePN = new ItemForm()
        {
            ItemUID = "CardName",
            Datasource = "U_CardName",
        };
        public ItemForm _telefone = new ItemForm()
        {
            ItemUID = "Tel1",
            Datasource = "U_Tel1",
        };
        public ItemForm _email = new ItemForm()
        {
            ItemUID = "EMail",
            Datasource = "U_EMail",
        };
        public ComboFormObrigatorio _pessoasDeContato = new ComboFormObrigatorio()
        {
            ItemUID = "CtName",
            Datasource = "U_CtName",
            Mensagem = "A pessoa de contato é obrigatória.",
            AbaUID = abaGeralUID,
        };

        public ItemForm _previsaoEntrega = new ItemForm()
        {
            ItemUID = "DtPrEnt",
            Datasource = "U_DtPrEnt",
        };
        public ItemForm _previsaoPagamento = new ItemForm()
        {
            ItemUID = "DtPrPgt",
            Datasource = "U_DtPrPgt",
        };

        public ComboFormObrigatorio _modalidade = new ComboFormObrigatorio()
        {
            ItemUID = "ModCtto",
            Datasource = "U_ModCtto",
            Mensagem = "A modalidade do contrato é obrigatória",
            SQL = "SELECT Code, Name FROM [@UPD_OMOD] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ComboFormObrigatorio _unidadeComercial = new ComboFormObrigatorio()
        {
            ItemUID = "UnidCom",
            Datasource = "U_UnidCom",
            Mensagem = "A unidade de comercial é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OUCM] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ComboFormObrigatorio _tipoDeOperacao = new ComboFormObrigatorio()
        {
            ItemUID = "TipoOper",
            Datasource = "U_TipoOper",
            Mensagem = "O tipo de operação é obrigatório.",
            SQL = "SELECT Code, Name FROM [@UPD_OTOP] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ComboFormObrigatorio _metodoFinanceiro = new ComboFormObrigatorio()
        {
            ItemUID = "MtdFin",
            Datasource = "U_MtdFin",
            Mensagem = "O método financeiro é obrigatório.",
            SQL = "SELECT Code, Name FROM [@UPD_OMFN] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ItemFormObrigatorio _codigoItem = new ItemFormObrigatorio()
        {
            ItemUID = "ItemCode",
            Datasource = "U_ItemCode",
            Mensagem = "O Item é obrigatório.",
            AbaUID = abaItemUID,
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
            Mensagem = "O depósito é obrigatório.",
            AbaUID = abaItemUID,
        };
        public ComboFormObrigatorio _utilizacao = new ComboFormObrigatorio()
        {
            ItemUID = "Usage",
            Datasource = "U_Usage",
            Mensagem = "A utilização é obrigatória.",
            SQL = "SELECT ID, Usage FROM OUSG ORDER BY Usage",
            AbaUID = abaItemUID,
        };
        public ComboFormObrigatorio _safra = new ComboFormObrigatorio()
        {
            ItemUID = "Safra",
            Datasource = "U_Safra",
            Mensagem = "A safra é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OSAF] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaItemUID,
        };
        public ComboFormObrigatorio _embalagem = new ComboFormObrigatorio()
        {
            ItemUID = "Packg",
            Datasource = "U_Packg",
            Mensagem = "A embalagem é obrigatória.",
            SQL = "SELECT PkgCode, PkgType FROM OPKG ORDER BY PkgType",
            AbaUID = abaItemUID,
        };
        public ItemForm _bebida = new ItemForm()
        {
            ItemUID = "Bebida",
            Datasource = "U_Bebida",
        };
        public ItemForm _diferencial = new ItemForm()
        {
            ItemUID = "Difere",
            Datasource = "U_Difere",
        };
        public ItemForm _taxaNY = new ItemForm()
        {
            ItemUID = "RateNY",
            Datasource = "U_RateNY",
        };
        public ItemForm _taxaDollar = new ItemForm()
        {
            ItemUID = "RateUSD",
            Datasource = "U_RateUSD",
        };

        public ItemForm _quantidadeDePeso = new ItemForm()
        {
            ItemUID = "QtdPeso",
            Datasource = "U_QtdPeso"
        };
        public ItemForm _saldoDePeso = new ItemForm()
        {
            ItemUID = "SPesoRec",
            Datasource = "U_SPesoRec"
        };
        public ItemForm _quantidadeDeSacas = new ItemForm()
        {
            ItemUID = "QtdSaca",
            Datasource = "U_QtdSaca"
        };
        public ItemForm _saldoDeSacas = new ItemForm()
        {
            ItemUID = "SPesoNCT",
            Datasource = "U_SPesoNCT"
        };
        public ItemForm _valorLivre = new ItemForm()
        {
            ItemUID = "VLivre",
            Datasource = "U_VLivre"
        };
        public ItemForm _totalLivre = new ItemForm()
        {
            ItemUID = "TLivre",
            Datasource = "U_TLivre"
        };
        public ItemForm _valorICMS = new ItemForm()
        {
            ItemUID = "VICMS",
            Datasource = "U_VICMS"
        };
        public ItemForm _totalICMS = new ItemForm()
        {
            ItemUID = "TICMS",
            Datasource = "U_TICMS"
        };
        public ItemForm _valorSENAR = new ItemForm()
        {
            ItemUID = "VSenar",
            Datasource = "U_VSenar"
        };
        public ItemForm _totalSENAR = new ItemForm()
        {
            ItemUID = "TSenar",
            Datasource = "U_TSenar"
        };
        public ItemForm _valorFaturado = new ItemForm()
        {
            ItemUID = "VFat",
            Datasource = "U_VFat"
        };
        public ItemForm _totalFaturado = new ItemForm()
        {
            ItemUID = "TFat",
            Datasource = "U_TFat"
        };
        public ItemForm _valorBruto = new ItemForm()
        {
            ItemUID = "VBruto",
            Datasource = "U_VBruto"
        };
        public ItemForm _totalBruto = new ItemForm()
        {
            ItemUID = "TBruto",
            Datasource = "U_TBruto"
        };

        public ItemForm _valorFrete = new ItemForm()
        {
            ItemUID = "VlrFrete",
            Datasource = "U_VlrFrete"
        };
        public ItemForm _saldoFinanceiro = new ItemForm()
        {
            ItemUID = "SFin",
            Datasource = "U_SFin"
        };

        #endregion


        #region :: Botões

        public ButtonForm _aberturaPorPeneira = new ButtonForm()
        {
            ItemUID = "btnAbertur",
        };
        public ButtonForm _certificado = new ButtonForm()
        {
            ItemUID = "btnCertif",
        };
        public ButtonForm _comissoes = new ButtonForm()
        {
            ItemUID = "btnComiss",
        };

        #endregion


        #region :: Eventos de Formulário

        public override void OnBeforeFormDataAdd(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            if (PreContrato.UsuarioPermitido())
            {
                var form = GetForm(BusinessObjectInfo.FormUID);
                var dbdts = GetDBDatasource(form, mainDbDataSource);

                string next_code = GetNextCode(mainDbDataSource);

                dbdts.SetValue("Code", 0, next_code);
                dbdts.SetValue("Name", 0, next_code);
                dbdts.SetValue(_numeroDoContrato.Datasource, 0, GetNextCode(mainDbDataSource));

                Dialogs.Info("Adicionando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);

                BubbleEvent = CamposFormEstaoPreenchidos(form, dbdts);
            }
            else
            {
                BubbleEvent = false;
                Dialogs.PopupError("O usuário corrente não pode criar um novo contrato.");
            }
        }

        public override void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            if (PreContrato.UsuarioPermitido())
            {
                var form = GetForm(BusinessObjectInfo.FormUID);
                var dbdts = GetDBDatasource(form, mainDbDataSource);

                Dialogs.Info("Atualizando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);

                BubbleEvent = CamposFormEstaoPreenchidos(form, dbdts);
            }
            else
            {
                BubbleEvent = false;
                Dialogs.PopupError("O usuário corrente não pode atualizar os dados de um contrato.");
            }
        }

        public override void OnAfterFormDataLoad(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            var form = GetForm(BusinessObjectInfo.FormUID);

            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = false;
            form.Items.Item(_status.ItemUID).Enabled = PreContrato.UsuarioPermitido();

            var dbdts = GetDBDatasource(form, mainDbDataSource);

            string codigoPN = dbdts.GetValue(_codigoPN.Datasource, 0);
            string pessoasDeContato = dbdts.GetValue(_pessoasDeContato.Datasource, 0);
            PopularPessoasDeContato(form, codigoPN, pessoasDeContato);

            string itemcode = dbdts.GetValue(_codigoItem.Datasource, 0);
            HabilitarBotaoAberturaPorPeneira(form, itemcode);
        }

        #endregion


        #region :: Eventos de Item

        public override void OnAfterFormVisible(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (String.IsNullOrEmpty(Program._grupoAprovador))
            {
                Dialogs.PopupError("Nenhum Grupo Aprovador foi configurado para este usúario.\nNão será possível abrir a tela de contratos.");
                var form = GetForm(FormUID);
                form.Close();
            }
            else
            {
                var form = GetForm(FormUID);

                try
                {
                    form.Freeze(true);

                    _modalidade.Popular(form);
                    _unidadeComercial.Popular(form);
                    _tipoDeOperacao.Popular(form);
                    _metodoFinanceiro.Popular(form);
                    _utilizacao.Popular(form);
                    _embalagem.Popular(form);
                    _safra.Popular(form);

                    if (!PreContrato.UsuarioPermitido())
                    {
                        FormEmModoVisualizacao(form);
                    }

                    // clicando para a primeira aba já vir selecionada
                    form.Items.Item("AbaGeral").Click();

                    ConditionsParaFornecedores(form);
                    ConditionsParaDeposito(form);
                    ConditionsParaItens(form);
                }
                catch (Exception e)
                {
                    Dialogs.PopupError("Erro interno. Erro ao iniciar dados do formulário\nErro: " + e.Message);
                }
                finally
                {
                    form.Freeze(false);
                }
            }
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
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (pVal.ItemUID == _aberturaPorPeneira.ItemUID)
            {
                CriarFormFilho(baseDirectory + "/" + nomeFormAberturaPorPeneira, FormUID, new FormAberturaPorPeneira());
            }
            else if (pVal.ItemUID == _certificado.ItemUID)
            {
                CriarFormFilho(baseDirectory + "/" + nomeFormDetalheCertificado, FormUID, new FormDetalheCertificado());
            }
            else if (pVal.ItemUID == _comissoes.ItemUID)
            {
                CriarFormFilho(baseDirectory + "/" + nomeFormComissoes, FormUID, new FormComissoes());
            }
        }

        public override void OnAfterValidate(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;


            if (EventoEmCampoDeValor(pVal.ItemUID))
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, mainDbDataSource);
                CalcularTotais(form,dbdts);
            }
        }

        public override void OnBeforeChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            if (pVal.ItemUID == _codigoItem.ItemUID)
            {
                BubbleEvent = PreContrato.TemGrupoDeItemConfiguradoParaChoose();
            }
            else
            {
                BubbleEvent = true;
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
                    OnItemCodeChoose(form, dbdts, dataTable);
                }
                else if (chooseEvent.ItemUID == _deposito.ItemUID)
                {
                    OnWhsCodeChoose(dbdts, dataTable);
                }

                ChangeFormMode(form);
            }
        }

        #endregion


        #region :: Eventos Internos

        public override void _OnAdicionarNovo(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = false;
            form.Items.Item(_status.ItemUID).Enabled = false;

            var dbdts = GetDBDatasource(form, mainDbDataSource);

            dbdts.SetValue(_dataInicio.Datasource, 0, Helpers.ToString(DateTime.Now));
            dbdts.SetValue(_status.Datasource, 0, "O");
            dbdts.SetValue(_numeroDoContrato.Datasource, 0, GetNextPrimaryKey(mainDbDataSource, _numeroDoContrato.Datasource));

            PopularPessoasDeContato(form, "", "");

            form.Items.Item(_dataFim.ItemUID).Click();
        }

        public override void _OnPesquisar(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = true;
            form.Items.Item(_status.ItemUID).Enabled = true;
        }

        #endregion


        #region :: Choose From Lists

        private void OnItemCodeChoose(SAPbouiCOM.Form form, DBDataSource dbdts, DataTable dataTable)
        {
            string itemcode = dataTable.GetValue("ItemCode", 0);
            string itemname = dataTable.GetValue("ItemName", 0);

            dbdts.SetValue(_codigoItem.Datasource, 0, itemcode);
            dbdts.SetValue(_nomeItem.Datasource, 0, itemname);

            HabilitarBotaoAberturaPorPeneira(form, itemcode);
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

        private void ConditionsParaFornecedores(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item(choosePNUID);
            Conditions oConds = oCFL.GetConditions();

            Condition oCond = oConds.Add();

            oCond.Alias = "CardType";
            oCond.Operation = BoConditionOperation.co_EQUAL;
            oCond.CondVal = "S";

            oCFL.SetConditions(oConds);
        }

        private void ConditionsParaDeposito(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item(chooseDepositoUID);
            Conditions oConds = oCFL.GetConditions();

            Condition oCond = oConds.Add();

            oCond.Alias = "Inactive";
            oCond.Operation = BoConditionOperation.co_EQUAL;
            oCond.CondVal = "N";

            oCFL.SetConditions(oConds);
        }

        public void ConditionsParaItens(SAPbouiCOM.Form form)
        {
            ChooseFromList oCFL = form.ChooseFromLists.Item(chooseItemUID);
            Conditions oConds = oCFL.GetConditions();

            var rs = Helpers.DoQuery(PreContrato.SQLGrupoDeItensPermitidos);
            if (rs.RecordCount > 0)
            {
                int i = 0;
                while (!rs.EoF)
                {
                    i++;
                    string grupoDeItem = rs.Fields.Item("U_ItmsGrpCod").Value;

                    Condition oCond = oConds.Add();

                    if (i == 1)
                    {
                        oCond.BracketOpenNum = 1;
                    }

                    oCond.Alias = "ItmsGrpCod";
                    oCond.Operation = BoConditionOperation.co_EQUAL;
                    oCond.CondVal = grupoDeItem;

                    if (i == rs.RecordCount)
                    {
                        oCond.BracketCloseNum = 1;
                        oCond.Relationship = BoConditionRelationship.cr_AND;
                    }
                    else
                    {
                        oCond.Relationship = BoConditionRelationship.cr_OR;
                    }

                    rs.MoveNext();
                }

                // só trazer itens ativos
                Condition oCondAtivo = oConds.Add();
                oCondAtivo.BracketOpenNum = 2;
                oCondAtivo.Alias = "frozenFor";
                oCondAtivo.Operation = BoConditionOperation.co_EQUAL;
                oCondAtivo.CondVal = "N";
                oCondAtivo.BracketCloseNum = 2;

                oCFL.SetConditions(oConds);
            }
        }

        #endregion


        #region :: Regras de negócio

        private void FormEmModoVisualizacao(SAPbouiCOM.Form form)
        {
            form.Items.Item(_dataInicio.ItemUID).Enabled = false;
            form.Items.Item(_dataFim.ItemUID).Enabled = false;
            form.Items.Item(_descricao.ItemUID).Enabled = false;
            form.Items.Item(_codigoPN.ItemUID).Enabled = false;
            form.Items.Item(_pessoasDeContato.ItemUID).Enabled = false;
            form.Items.Item(_previsaoEntrega.ItemUID).Enabled = false;
            form.Items.Item(_previsaoPagamento.ItemUID).Enabled = false;
            form.Items.Item(_modalidade.ItemUID).Enabled = false;
            form.Items.Item(_tipoDeOperacao.ItemUID).Enabled = false;
            form.Items.Item(_unidadeComercial.ItemUID).Enabled = false;
            form.Items.Item(_metodoFinanceiro.ItemUID).Enabled = false;
            form.Items.Item(_quantidadeDePeso.ItemUID).Enabled = false;
            form.Items.Item(_quantidadeDeSacas.ItemUID).Enabled = false;
            form.Items.Item(_valorLivre.ItemUID).Enabled = false;
            form.Items.Item(_valorICMS.ItemUID).Enabled = false;
            form.Items.Item(_valorSENAR.ItemUID).Enabled = false;
            form.Items.Item(_valorFaturado.ItemUID).Enabled = false;
            form.Items.Item(_valorBruto.ItemUID).Enabled = false;
            form.Items.Item(_valorFrete.ItemUID).Enabled = false;

            form.Items.Item(_codigoItem.ItemUID).Enabled = false;
            form.Items.Item(_deposito.ItemUID).Enabled = false;
            form.Items.Item(_utilizacao.ItemUID).Enabled = false;
            form.Items.Item(_safra.ItemUID).Enabled = false;
            form.Items.Item(_embalagem.ItemUID).Enabled = false;
            form.Items.Item(_bebida.ItemUID).Enabled = false;
            form.Items.Item(_diferencial.ItemUID).Enabled = false;
            form.Items.Item(_taxaNY.ItemUID).Enabled = false;
            form.Items.Item(_taxaDollar.ItemUID).Enabled = false;
        }

        private void PopularPessoasDeContato(SAPbouiCOM.Form form, string cardcode, string pessoaDeContatoSelecionada)
        {
            _pessoasDeContato.SQL =
                    $@"SELECT 
	                    Name, Name
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}'";
            _pessoasDeContato.Popular(form);

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

        private bool EventoEmCampoDeValor(string itemUID)
        {
            return
                itemUID == _quantidadeDePeso.ItemUID
                || itemUID == _quantidadeDeSacas.ItemUID
                || itemUID == _valorLivre.ItemUID
                || itemUID == _valorICMS.ItemUID
                || itemUID == _valorSENAR.ItemUID
                || itemUID == _valorFaturado.ItemUID
                || itemUID == _valorBruto.ItemUID
            ;
        }

        private void CalcularTotais(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            double qtdPeso = Helpers.ToDouble(dbdts.GetValue(_quantidadeDePeso.Datasource, 0));
            double qtdSacas = Helpers.ToDouble(dbdts.GetValue(_quantidadeDeSacas.Datasource, 0));
            double valorLivre = Helpers.ToDouble(dbdts.GetValue(_valorLivre.Datasource, 0));
            double valorICMS = Helpers.ToDouble(dbdts.GetValue(_valorICMS.Datasource, 0));
            double valorSENAR = Helpers.ToDouble(dbdts.GetValue(_valorSENAR.Datasource, 0));
            double valorFaturado = Helpers.ToDouble(dbdts.GetValue(_valorFaturado.Datasource, 0));
            double valorBruto = Helpers.ToDouble(dbdts.GetValue(_valorBruto.Datasource, 0));
            try
            {
                form.Freeze(true);
                dbdts.SetValue(_totalLivre.Datasource, 0, Helpers.ToSAPString(valorLivre * qtdSacas));
                dbdts.SetValue(_totalICMS.Datasource, 0, Helpers.ToSAPString(valorICMS * qtdSacas));
                dbdts.SetValue(_totalSENAR.Datasource, 0, Helpers.ToSAPString(valorSENAR * qtdSacas));
                dbdts.SetValue(_totalFaturado.Datasource, 0, Helpers.ToSAPString(valorFaturado * qtdSacas));
                dbdts.SetValue(_totalBruto.Datasource, 0, Helpers.ToSAPString(valorBruto * qtdSacas));
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void HabilitarBotaoAberturaPorPeneira(SAPbouiCOM.Form form, string itemcode)
        {
            var botao_habilitado = false;
            var rs = Helpers.DoQuery($"SELECT U_UPD_TIPO_ITEM FROM OITM WHERE ItemCode = '{itemcode}'");
            if (rs.Fields.Item("U_UPD_TIPO_ITEM").Value == "B")
            {
                botao_habilitado = true;
            }
            form.Items.Item(_aberturaPorPeneira.ItemUID).Enabled = botao_habilitado;
        }

        #endregion
    }
}
