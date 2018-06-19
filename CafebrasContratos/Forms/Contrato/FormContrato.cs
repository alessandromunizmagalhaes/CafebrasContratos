using SAPbouiCOM;
using SAPHelper;
using System;
using System.Collections.Generic;

namespace CafebrasContratos
{
    public abstract class FormContrato : SAPHelper.Form
    {
        #region :: Propriedades

        public abstract override string FormType { get; }
        public abstract string MainDbDataSource { get; }
        public abstract string anexoDbDataSource { get; }

        private const string abaGeralUID = "AbaGeral";
        private const string abaItemUID = "AbaItem";
        private const string abaContratoFinalUID = "AbaCFinal";
        private const string abaObsUID = "AbaObs";

        private const string choosePNUID = "PN";
        private const string chooseItemUID = "Item";
        private const string chooseDepositoUID = "Warehouse";

        public static string SQLGrupoDeItensPermitidos = "SELECT DISTINCT U_ItmsGrpCod FROM [@UPD_OCTC]";

        #endregion


        #region :: Campos


        public abstract ItemForm _numeroDoContrato { get; }

        public ItemForm _status = new ItemForm()
        {
            ItemUID = "StatusCtr",
            Datasource = "U_StatusCtr"
        };
        public ItemFormObrigatorio _descricao = new ItemFormObrigatorio()
        {
            ItemUID = "Descricao",
            Datasource = "U_Descricao",
            Mensagem = "A Descrição Geral é obrigatória."
        };
        public ItemFormObrigatorioAutorizavel _dataInicio = new ItemFormObrigatorioAutorizavel()
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

        public ItemFormObrigatorioAutorizavel _codigoPN = new ItemFormObrigatorioAutorizavel()
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
        public ItemForm _nomeEstrangeiro = new ItemForm()
        {
            ItemUID = "FrgnName",
            Datasource = "U_FrgnName",
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
        public ComboFormObrigatorioAutorizavel _pessoasDeContato = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "CtName",
            Datasource = "U_CtName",
            Mensagem = "A pessoa de contato é obrigatória.",
            AbaUID = abaGeralUID,
        };

        public ItemFormAutorizavel _previsaoEntrega = new ItemFormAutorizavel()
        {
            ItemUID = "DtPrEnt",
            Datasource = "U_DtPrEnt",
        };
        public ItemFormAutorizavel _previsaoPagamento = new ItemFormAutorizavel()
        {
            ItemUID = "DtPrPgt",
            Datasource = "U_DtPrPgt",
        };

        public ComboFormObrigatorioAutorizavel _modalidade = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "ModCtto",
            Datasource = "U_ModCtto",
            Mensagem = "A modalidade do contrato é obrigatória",
            SQL = "SELECT Code, Name FROM [@UPD_OMOD] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ComboFormObrigatorioAutorizavel _unidadeComercial = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "UnidCom",
            Datasource = "U_UnidCom",
            Mensagem = "A unidade de comercial é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OUCM] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ComboFormObrigatorioAutorizavel _tipoDeOperacao = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "TipoOper",
            Datasource = "U_TipoOper",
            Mensagem = "O tipo de operação é obrigatório.",
            SQL = "SELECT Code, Name FROM [@UPD_OTOP] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaGeralUID,
        };
        public ComboFormAutorizavel _metodoFinanceiro = new ComboFormAutorizavel()
        {
            ItemUID = "MtdFin",
            Datasource = "U_MtdFin",
            SQL = "SELECT Code, Name FROM [@UPD_OMFN] WHERE Canceled = 'N' ORDER BY Name",
        };
        public ItemFormObrigatorioAutorizavel _codigoItem = new ItemFormObrigatorioAutorizavel()
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
        public ItemFormObrigatorioAutorizavel _deposito = new ItemFormObrigatorioAutorizavel()
        {
            ItemUID = "WhsCode",
            Datasource = "U_WhsCode",
            Mensagem = "O depósito é obrigatório.",
            AbaUID = abaItemUID,
        };
        public ComboFormObrigatorioAutorizavel _utilizacao = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "Usage",
            Datasource = "U_Usage",
            Mensagem = "A utilização é obrigatória.",
            SQL = "SELECT ID, Usage FROM OUSG ORDER BY Usage",
            AbaUID = abaItemUID,
        };
        public ComboFormObrigatorioAutorizavel _safra = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "Safra",
            Datasource = "U_Safra",
            Mensagem = "A safra é obrigatória.",
            SQL = "SELECT Code, Name FROM [@UPD_OSAF] WHERE Canceled = 'N' ORDER BY Name",
            AbaUID = abaItemUID,
        };
        public ComboFormObrigatorioAutorizavel _embalagem = new ComboFormObrigatorioAutorizavel()
        {
            ItemUID = "Packg",
            Datasource = "U_Packg",
            Mensagem = "A embalagem é obrigatória.",
            SQL = "SELECT PkgCode, PkgType FROM OPKG ORDER BY PkgType",
            AbaUID = abaItemUID,
        };
        public ItemFormAutorizavel _bebida = new ItemFormAutorizavel()
        {
            ItemUID = "Bebida",
            Datasource = "U_Bebida",
        };
        public ItemFormAutorizavel _diferencial = new ItemFormAutorizavel()
        {
            ItemUID = "Difere",
            Datasource = "U_Difere",
        };
        public ItemFormAutorizavel _taxaNY = new ItemFormAutorizavel()
        {
            ItemUID = "RateNY",
            Datasource = "U_RateNY",
        };
        public ItemFormAutorizavel _taxaDollar = new ItemFormAutorizavel()
        {
            ItemUID = "RateUSD",
            Datasource = "U_RateUSD",
        };

        public ItemFormAutorizavel _quantidadeDePeso = new ItemFormAutorizavel()
        {
            ItemUID = "QtdPeso",
            Datasource = "U_QtdPeso"
        };
        public ItemForm _saldoDePeso = new ItemForm()
        {
            ItemUID = "SPesoRec",
            Datasource = "U_SPesoRec"
        };
        public ItemFormAutorizavel _quantidadeDeSacas = new ItemFormAutorizavel()
        {
            ItemUID = "QtdSaca",
            Datasource = "U_QtdSaca"
        };
        public ItemForm _saldoDeSacas = new ItemForm()
        {
            ItemUID = "SPesoNCT",
            Datasource = "U_SPesoNCT"
        };
        public ItemFormAutorizavel _valorLivre = new ItemFormAutorizavel()
        {
            ItemUID = "VLivre",
            Datasource = "U_VLivre"
        };
        public ItemForm _totalLivre = new ItemForm()
        {
            ItemUID = "TLivre",
            Datasource = "U_TLivre"
        };
        public ItemFormAutorizavel _valorICMS = new ItemFormAutorizavel()
        {
            ItemUID = "VICMS",
            Datasource = "U_VICMS"
        };
        public ItemForm _totalICMS = new ItemForm()
        {
            ItemUID = "TICMS",
            Datasource = "U_TICMS"
        };
        public ItemFormAutorizavel _valorSENAR = new ItemFormAutorizavel()
        {
            ItemUID = "VSenar",
            Datasource = "U_VSenar"
        };
        public ItemForm _totalSENAR = new ItemForm()
        {
            ItemUID = "TSenar",
            Datasource = "U_TSenar"
        };
        public ItemFormAutorizavel _valorFaturado = new ItemFormAutorizavel()
        {
            ItemUID = "VFat",
            Datasource = "U_VFat"
        };
        public ItemForm _totalFaturado = new ItemForm()
        {
            ItemUID = "TFat",
            Datasource = "U_TFat"
        };
        public ItemFormAutorizavel _valorBruto = new ItemFormAutorizavel()
        {
            ItemUID = "VBruto",
            Datasource = "U_VBruto"
        };
        public ItemForm _totalBruto = new ItemForm()
        {
            ItemUID = "TBruto",
            Datasource = "U_TBruto"
        };

        public ItemFormAutorizavel _valorFrete = new ItemFormAutorizavel()
        {
            ItemUID = "VlrFrete",
            Datasource = "U_VlrFrete"
        };
        public ItemFormAutorizavel _valorSeguro = new ItemFormAutorizavel()
        {
            ItemUID = "VSeguro",
            Datasource = "U_VSeguro"
        };
        public ItemFormAutorizavel _transportadora = new ItemFormAutorizavel()
        {
            ItemUID = "Transp",
            Datasource = "U_Transp"
        };
        public ItemFormAutorizavel _localRetirada = new ItemFormAutorizavel()
        {
            ItemUID = "LocalRet",
            Datasource = "U_LocalRet"
        };
        public ItemForm _saldoFinanceiro = new ItemForm()
        {
            ItemUID = "SFin",
            Datasource = "U_SFin"
        };

        #endregion


        #region :: Campos Peneira

        public readonly List<ItemFormAutorizavel> _peneiras = new List<ItemFormAutorizavel>() {
            new ItemFormAutorizavel(){ ItemUID = "P01", Datasource = "U_P01" },
            new ItemFormAutorizavel(){ ItemUID = "P02", Datasource = "U_P02" },
            new ItemFormAutorizavel(){ ItemUID = "P03", Datasource = "U_P03" },
            new ItemFormAutorizavel(){ ItemUID = "P04", Datasource = "U_P04" },
            new ItemFormAutorizavel(){ ItemUID = "P05", Datasource = "U_P05" },
            new ItemFormAutorizavel(){ ItemUID = "P06", Datasource = "U_P06" },
            new ItemFormAutorizavel(){ ItemUID = "P07", Datasource = "U_P07" },
            new ItemFormAutorizavel(){ ItemUID = "P08", Datasource = "U_P08" },
            new ItemFormAutorizavel(){ ItemUID = "P09", Datasource = "U_P09" },
            new ItemFormAutorizavel(){ ItemUID = "P10", Datasource = "U_P10" },
            new ItemFormAutorizavel(){ ItemUID = "P11", Datasource = "U_P11" },
            new ItemFormAutorizavel(){ ItemUID = "P12", Datasource = "U_P12" },
            new ItemFormAutorizavel(){ ItemUID = "P13", Datasource = "U_P13" },
            new ItemFormAutorizavel(){ ItemUID = "P14", Datasource = "U_P14" },
            new ItemFormAutorizavel(){ ItemUID = "P15", Datasource = "U_P15" },
        };

        public ItemForm _totalPeneira = new ItemForm() { ItemUID = "totalP", Datasource = "totalP" };
        public ItemForm _totalDiferencial = new ItemForm() { ItemUID = "totalD", Datasource = "totalD" };

        #endregion


        #region :: Matrizes

        public MatrizAnexos _matrixAnexos = new MatrizAnexos() { ItemUID = "mtxAnexos" };

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
            if (UsuarioPermitido())
            {
                var form = GetForm(BusinessObjectInfo.FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);

                string next_code = GetNextCode(MainDbDataSource);

                dbdts.SetValue("Code", 0, next_code);
                dbdts.SetValue("Name", 0, next_code);

                _numeroDoContrato.SetValorDBDatasource(dbdts, ProximaChavePrimaria(dbdts));
                SalvarLabelPeneiras(form, dbdts);

                Dialogs.Info("Adicionando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);

                BubbleEvent = FormEstaValido(form, dbdts);
            }
            else
            {
                BubbleEvent = false;
                Dialogs.PopupError("O usuário corrente não pode criar um novo contrato.");
            }
        }

        public override void OnBeforeFormDataUpdate(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            if (UsuarioPermitido())
            {
                var form = GetForm(BusinessObjectInfo.FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);

                Dialogs.Info("Atualizando pré contrato... Aguarde...", BoMessageTime.bmt_Medium);

                BubbleEvent = FormEstaValido(form, dbdts);
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
            form.Items.Item(_status.ItemUID).Enabled = UsuarioPermitido();

            var dbdts = GetDBDatasource(form, MainDbDataSource);

            string codigoPN = dbdts.GetValue(_codigoPN.Datasource, 0);
            string pessoasDeContato = dbdts.GetValue(_pessoasDeContato.Datasource, 0);
            PopularPessoasDeContato(form, codigoPN, pessoasDeContato);

            string itemcode = dbdts.GetValue(_codigoItem.Datasource, 0);
            HabilitarBotaoAberturaPorPeneira(form, itemcode);
            HabilitarCamposDePeneira(form, dbdts, itemcode);

            var status = dbdts.GetValue(_status.Datasource, 0);
            GerenciarCamposQuandoAutorizado(form, ContratoPodeSerAlterado(status));

            if (status == StatusPreContrato.Autorizado)
            {
                QuandoPuderAdicionarObjetoFilho(form);
            }
            else
            {
                QuandoNaoPuderAdicionarObjetoFilho(form);
            }
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
                BubbleEvent = false;
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

                    if (!UsuarioPermitido())
                    {
                        FormEmModoVisualizacao(form);
                    }

                    // clicando para a primeira aba já vir selecionada
                    form.Items.Item(abaGeralUID).Click();

                    ConditionsParaFornecedores(form);
                    ConditionsParaDeposito(form);
                    ConditionsParaItens(form);

                    GerirCamposPeneira(form);
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
                var dbdts = GetDBDatasource(FormUID, MainDbDataSource);
                string pessoaDeContato = dbdts.GetValue(_pessoasDeContato.Datasource, 0);
                string cardcode = dbdts.GetValue(_codigoPN.Datasource, 0);
                AtualizarDadosPessoaDeContato(cardcode, pessoaDeContato, dbdts);
            }
            else if (pVal.ItemUID == _status.ItemUID)
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);
                var status = _status.GetValorDBDatasource<string>(dbdts);

                GerenciarCamposQuandoAutorizado(form, status == StatusPreContrato.Esboço);
            }
        }

        public override void OnAfterItemPressed(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (pVal.ItemUID == _aberturaPorPeneira.ItemUID)
            {
                var formAberturaPorPeneira = Activator.CreateInstance(FormAberturaPorPeneiraType);
                CriarFormFilho(baseDirectory + FormAberturaPorPeneiraSRF, FormUID, (SAPHelper.Form)formAberturaPorPeneira);
            }
            else if (pVal.ItemUID == _certificado.ItemUID)
            {
                var formDetalheCertificado = Activator.CreateInstance(FormDetalheCertificadoType);
                CriarFormFilho(baseDirectory + FormDetalheCertificadoSRF, FormUID, (SAPHelper.Form)formDetalheCertificado);
            }
            else if (pVal.ItemUID == _comissoes.ItemUID)
            {
                var formComissoes = Activator.CreateInstance(FormComissoesType);
                CriarFormFilho(baseDirectory + FormComissoesSRF, FormUID, (SAPHelper.Form)formComissoes);
            }
            else if (pVal.ItemUID == _matrixAnexos._adicionar.ItemUID)
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, anexoDbDataSource);
                _matrixAnexos.AdicionarLinha(form, dbdts);
            }
            else if (pVal.ItemUID == _matrixAnexos._remover.ItemUID)
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, anexoDbDataSource);
                _matrixAnexos.RemoverLinhaSelecionada(form, dbdts);
            }
        }

        public override void OnAfterDoubleClick(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ItemUID == _matrixAnexos.ItemUID)
            {
                var form = GetForm(FormUID);
                var matrix = GetMatrix(form, _matrixAnexos.ItemUID);

                var data = matrix.GetCellSpecific(_matrixAnexos._data.ItemUID, pVal.Row).Value.ToString();
                if (String.IsNullOrEmpty(data))
                {
                    matrix.SetCellWithoutValidation(pVal.Row, _matrixAnexos._data.ItemUID, DateTime.Now.ToString("yyyyMMdd"));
                }
            }
        }

        public override void OnAfterValidate(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (EventoEmCampoDeValor(pVal.ItemUID))
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);
                CalcularTotais(form, dbdts);
            }
            else if (EventoEmCampoDePeneira(pVal.ItemUID))
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);
                AtualizarSomaDosPercentuaisDePeneira(form, dbdts);
            }
            else if (EventoEmCampoDeDiferencial(pVal.ItemUID))
            {
                var form = GetForm(FormUID);
                var dbdts = GetDBDatasource(form, MainDbDataSource);
                AtualizarSomaDosDiferenciais(form, dbdts);
            }
        }

        public override void OnBeforeChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            if (pVal.ItemUID == _codigoItem.ItemUID)
            {
                BubbleEvent = TemGrupoDeItemConfiguradoParaChoose();
            }
            else
            {
                BubbleEvent = true;
            }
        }

        public override void OnAfterChooseFromList(SAPbouiCOM.Form form, ChooseFromListEvent chooseEvent, ChooseFromList choose, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (!choose.IsSystem && form.Mode != BoFormMode.fm_FIND_MODE)
            {
                var dbdts = GetDBDatasource(pVal.FormUID, MainDbDataSource);
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

            var dbdts = GetDBDatasource(form, MainDbDataSource);

            IniciarValoresAoAdicionarNovo(form, dbdts);

            _numeroDoContrato.SetValorDBDatasource(dbdts, ProximaChavePrimaria(dbdts));
            _dataInicio.SetValorDBDatasource(dbdts, DateTime.Now);

            AtualizarSomaDosPercentuaisDePeneira(form, dbdts);
            AtualizarSomaDosDiferenciais(form, dbdts);

            PopularPessoasDeContato(form, "", "");

            GerenciarCamposQuandoAutorizado(form, true);
            QuandoNaoPuderAdicionarObjetoFilho(form);
        }

        public override void _OnPesquisar(SAPbouiCOM.Form form)
        {
            form.Items.Item(_numeroDoContrato.ItemUID).Enabled = true;
            form.Items.Item(_status.ItemUID).Enabled = true;

            form.Items.Item(_numeroDoContrato.ItemUID).Click();
        }

        #endregion


        #region :: Choose From Lists

        private void OnItemCodeChoose(SAPbouiCOM.Form form, DBDataSource dbdts, DataTable dataTable)
        {
            string itemcode = dataTable.GetValue("ItemCode", 0);
            string itemname = dataTable.GetValue("ItemName", 0);

            _codigoItem.SetValorDBDatasource(dbdts, itemcode);
            _nomeItem.SetValorDBDatasource(dbdts, itemname);

            HabilitarBotaoAberturaPorPeneira(form, itemcode);
            HabilitarCamposDePeneira(form, dbdts, itemcode);
        }

        private void OnWhsCodeChoose(DBDataSource dbdts, DataTable dataTable)
        {
            string whscode = dataTable.GetValue("WhsCode", 0);
            _deposito.SetValorDBDatasource(dbdts, whscode);
        }

        private void OnCardCodeChoose(SAPbouiCOM.Form form, DBDataSource dbdts, DataTable dataTable)
        {
            string cardcode = dataTable.GetValue("CardCode", 0);
            string cardname = dataTable.GetValue("CardName", 0);
            string pessoaDeContato = dataTable.GetValue("CntctPrsn", 0);
            string nomeEstrangeiro = dataTable.GetValue("CardFName", 0);

            _codigoPN.SetValorDBDatasource(dbdts, cardcode);
            _nomePN.SetValorDBDatasource(dbdts, cardname);
            _nomeEstrangeiro.SetValorDBDatasource(dbdts, nomeEstrangeiro);

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

            var rs = Helpers.DoQuery(SQLGrupoDeItensPermitidos);
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

        protected virtual void FormEmModoVisualizacao(SAPbouiCOM.Form form)
        {
            form.Items.Item(_descricao.ItemUID).Enabled = false;
            form.Items.Item(_codigoPN.ItemUID).Enabled = false;
            form.Items.Item(_dataInicio.ItemUID).Enabled = false;
            form.Items.Item(_dataFim.ItemUID).Enabled = false;
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
            form.Items.Item(_valorSeguro.ItemUID).Enabled = false;
            form.Items.Item(_transportadora.ItemUID).Enabled = false;
            form.Items.Item(_localRetirada.ItemUID).Enabled = false;

            form.Items.Item(_codigoItem.ItemUID).Enabled = false;
            form.Items.Item(_deposito.ItemUID).Enabled = false;
            form.Items.Item(_utilizacao.ItemUID).Enabled = false;
            form.Items.Item(_safra.ItemUID).Enabled = false;
            form.Items.Item(_embalagem.ItemUID).Enabled = false;
            form.Items.Item(_bebida.ItemUID).Enabled = false;
            form.Items.Item(_diferencial.ItemUID).Enabled = false;
            form.Items.Item(_taxaNY.ItemUID).Enabled = false;
            form.Items.Item(_taxaDollar.ItemUID).Enabled = false;

            foreach (var peneira in _peneiras)
            {
                form.Items.Item(peneira.ItemUID).Enabled = false;
                form.Items.Item(peneira.ItemUID.Replace("P", "D")).Enabled = false;
            }
        }

        protected void PopularPessoasDeContato(SAPbouiCOM.Form form, string cardcode, string pessoaDeContatoSelecionada)
        {
            _pessoasDeContato.SQL =
                    $@"SELECT 
	                    Name, ISNULL(FirstName,Name)
                    FROM OCPR 
                    WHERE CardCode = '{cardcode}'
                    ORDER BY ISNULL(FirstName,Name)";
            _pessoasDeContato.Popular(form);

            var dbdts = GetDBDatasource(form, MainDbDataSource);
            _pessoasDeContato.SetValorDBDatasource(dbdts, pessoaDeContatoSelecionada);
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

            _telefone.SetValorDBDatasource(dbdts, telefone);
            _email.SetValorDBDatasource(dbdts, email);
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

        private bool EventoEmCampoDePeneira(string itemUID)
        {
            return _peneiras.Find(p => p.ItemUID == itemUID) != null;
        }

        private bool EventoEmCampoDeDiferencial(string itemUID)
        {
            return _peneiras.Find(p => p.ItemUID.Replace("P", "D") == itemUID) != null;
        }

        protected void CalcularTotais(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            double qtdPeso = _quantidadeDePeso.GetValorDBDatasource<double>(dbdts);
            double qtdSacas = _quantidadeDeSacas.GetValorDBDatasource<double>(dbdts);
            double valorLivre = _valorLivre.GetValorDBDatasource<double>(dbdts);
            double valorICMS = _valorICMS.GetValorDBDatasource<double>(dbdts);
            double valorSENAR = _valorSENAR.GetValorDBDatasource<double>(dbdts);
            double valorFaturado = _valorFaturado.GetValorDBDatasource<double>(dbdts);
            double valorBruto = _valorBruto.GetValorDBDatasource<double>(dbdts);

            try
            {
                form.Freeze(true);
                var totalLivre = valorLivre * qtdSacas;
                _totalLivre.SetValorDBDatasource(dbdts, totalLivre);
                _totalICMS.SetValorDBDatasource(dbdts, valorICMS * qtdSacas);
                _totalSENAR.SetValorDBDatasource(dbdts, valorSENAR * qtdSacas);
                _totalFaturado.SetValorDBDatasource(dbdts, valorFaturado * qtdSacas);
                _totalBruto.SetValorDBDatasource(dbdts, valorBruto * qtdSacas);

                _saldoDePeso.SetValorDBDatasource(dbdts, qtdPeso);
                _saldoDeSacas.SetValorDBDatasource(dbdts, qtdSacas);
                _saldoFinanceiro.SetValorDBDatasource(dbdts, totalLivre);
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private void HabilitarBotaoAberturaPorPeneira(SAPbouiCOM.Form form, string itemcode)
        {
            /* -- código comentado porque agora a abertura de peneira são campos na tela.
            var botao_habilitado = false;
            var rs = Helpers.DoQuery($"SELECT U_UPD_TIPO_ITEM FROM OITM WHERE ItemCode = '{itemcode}'");
            if (rs.Fields.Item("U_UPD_TIPO_ITEM").Value == "B")
            {
                botao_habilitado = true;
            }
            form.Items.Item(_aberturaPorPeneira.ItemUID).Enabled = botao_habilitado;
            */
        }

        protected void HabilitarCamposDePeneira(SAPbouiCOM.Form form, DBDataSource dbdts, string itemcode)
        {
            var deve_habilitar = ItemTipoBica(itemcode);

            foreach (var peneira in _peneiras)
            {
                var diferencialItemUID = peneira.ItemUID.Replace("P", "D");
                form.Items.Item(peneira.ItemUID).Enabled = deve_habilitar;
                form.Items.Item(diferencialItemUID).Enabled = deve_habilitar;

                if (!deve_habilitar)
                {
                    peneira.SetValorDBDatasource(dbdts, 0);
                    new ItemForm() { ItemUID = diferencialItemUID, Datasource = "U_" + diferencialItemUID }.SetValorDBDatasource(dbdts, 0);
                }
            }
            AtualizarSomaDosPercentuaisDePeneira(form, dbdts);
            AtualizarSomaDosDiferenciais(form, dbdts);
        }

        private bool FormEstaValido(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            return CamposFormEstaoPreenchidos(form, dbdts) && RegrasDeNegocioEstaoValidas(form, dbdts);
        }

        private bool RegrasDeNegocioEstaoValidas(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            try
            {
                var dataInicial = _dataInicio.GetValorDBDatasource<DateTime>(dbdts);
                var dataFinal = _dataFim.GetValorDBDatasource<DateTime>(dbdts);

                if (dataFinal < dataInicial)
                {
                    throw new FormValidationException("Data final do contrato não pode ser menor do que a Data inicial.", _dataFim.ItemUID);
                }

                RegrasDeNegocioAoSalvar(form, dbdts);
                if (ItemTipoBica(dbdts.GetValue(_codigoItem.Datasource, 0)))
                {
                    ValidarSomaDosPercentuaisDePeneira(dbdts);
                }
            }
            catch (BusinessRuleException e)
            {
                Dialogs.MessageBox(e.Message);

                return false;
            }
            catch (FormValidationException e)
            {
                Dialogs.MessageBox(e.Message);
                if (!String.IsNullOrEmpty(e.AbaUID))
                {
                    form.Items.Item(e.AbaUID).Click();
                }
                form.Items.Item(e.Campo).Click();
                return false;
            }
            catch (Exception e)
            {
                Dialogs.PopupError("Erro interno. Erro ao realizar as validações de regras de negócio do formulário.\nErro: " + e.Message);
                return false;
            }

            return true;
        }

        private void ValidarSomaDosPercentuaisDePeneira(DBDataSource dbdts)
        {
            if (SomaDosPercentuaisDePeneira(dbdts) != 100.00)
            {
                throw new FormValidationException("A soma dos percentuais de peneira, devem ser 100%", "P01", abaItemUID);
            }
        }

        private int SomaDosPercentuaisDePeneira(DBDataSource dbdts)
        {
            var totalPeneiras = 0;
            foreach (var peneira in _peneiras)
            {
                string valor = dbdts.GetValue(peneira.Datasource, 0);
                if (!string.IsNullOrEmpty(valor))
                {
                    totalPeneiras += Helpers.ToInt(valor);
                }
            }

            return totalPeneiras;
        }

        private double SomaDosDiferenciais(DBDataSource dbdts)
        {
            var total = 0.0;
            foreach (var peneira in _peneiras)
            {
                var diferencialDataSource = peneira.Datasource.Replace("P", "D");
                total += Helpers.ToDouble(dbdts.GetValue(diferencialDataSource, 0));
            }

            return total;
        }

        private void AtualizarSomaDosPercentuaisDePeneira(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _totalPeneira.SetValorUserDataSource(form, SomaDosPercentuaisDePeneira(dbdts));
        }

        private void AtualizarSomaDosDiferenciais(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            _totalDiferencial.SetValorUserDataSource(form, SomaDosDiferenciais(dbdts));
        }

        private void PosicionarCamposDeTotaisDePeneiras(SAPbouiCOM.Form form)
        {
            var topFromLast = 21;
            var topLast = form.Items.Item(_peneiras[_peneiras.Count - 1].ItemUID).Top;

            for (int i = 0; i < _peneiras.Count; i++)
            {
                var peneira = _peneiras[i];
                if (!form.Items.Item(peneira.ItemUID).Visible)
                {
                    topLast = form.Items.Item(_peneiras[i - 1].ItemUID).Top;
                    break;
                }
            }

            var totalTop = topFromLast + topLast;

            form.Items.Item("lbl" + _totalPeneira.ItemUID).Top = totalTop;
            form.Items.Item(_totalPeneira.ItemUID).Top = totalTop;
            form.Items.Item(_totalDiferencial.ItemUID).Top = totalTop;
        }

        private void GerirCamposPeneira(SAPbouiCOM.Form form)
        {
            try
            {
                // tem que por esse try/finally porque precisa de dar um freeze
                // tem que dar o freeze porque eu só consigo fazer o item ficar invisivel
                // se a aba estiver visivel.
                // estou clicando na aba, deixando os itens invisiveis e voltando pra aba normal.
                form.Freeze(true);

                form.Items.Item(abaItemUID).Click();

                var rs = Helpers.DoQuery("SELECT U_Peneira, U_NomeP, U_Ativo FROM [@UPD_CONF_PENEIRA]");
                while (!rs.EoF)
                {
                    string peneiraUID = rs.Fields.Item("U_Peneira").Value;
                    string nomePeneira = rs.Fields.Item("U_NomeP").Value;
                    bool ativo = rs.Fields.Item("U_Ativo").Value == "Y";

                    var itemPeneira = _peneiras.Find(p => p.ItemUID == peneiraUID);
                    var labelPeneiraUID = "lbl" + itemPeneira.ItemUID;
                    var diferencialUID = itemPeneira.ItemUID.Replace("P", "D");
                    if (ativo)
                    {
                        ((StaticText)form.Items.Item(labelPeneiraUID).Specific).Caption = nomePeneira;
                    }
                    else
                    {
                        form.Items.Item(itemPeneira.ItemUID).Visible = false;
                        form.Items.Item(labelPeneiraUID).Visible = false;
                        form.Items.Item(diferencialUID).Visible = false;
                    }

                    rs.MoveNext();
                }

                // para posicionar o campo de total, tem que estar com a aba clicada, senão o visible sempre retorna false
                PosicionarCamposDeTotaisDePeneiras(form);

                form.Items.Item(abaGeralUID).Click();
            }
            finally
            {
                form.Freeze(false);
            }
        }

        private bool ItemTipoBica(string itemcode)
        {
            var rs = Helpers.DoQuery($"SELECT U_UPD_TIPO_ITEM FROM OITM WHERE ItemCode = '{itemcode}'");
            return rs.Fields.Item("U_UPD_TIPO_ITEM").Value == "B";
        }

        public static bool TemGrupoDeItemConfiguradoParaChoose()
        {
            var rs = Helpers.DoQuery(SQLGrupoDeItensPermitidos);
            if (rs.RecordCount == 0)
            {
                Dialogs.PopupInfo("Nenhum grupo de item foi configurado para filtrar esta apresentação de itens.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SalvarLabelPeneiras(SAPbouiCOM.Form form, DBDataSource dbdts)
        {
            for (int i = 0; i < _peneiras.Count; i++)
            {
                var lblUID = "lbl" + _peneiras[i].ItemUID;
                var label = ((StaticText)form.Items.Item(lblUID).Specific).Caption;

                var labelPadrao = _peneiras[i].ItemUID;
                if (label != labelPadrao)
                {
                    var lblDataSource = _peneiras[i].Datasource.Replace("P", "L");
                    dbdts.SetValue(lblDataSource, 0, label);
                }
            }
        }

        private void GerenciarCamposQuandoAutorizado(SAPbouiCOM.Form form, bool habilitar)
        {
            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(ItemFormAutorizavel)
                    || field.FieldType == typeof(ItemFormObrigatorioAutorizavel)
                    || field.FieldType == typeof(ComboFormAutorizavel)
                    || field.FieldType == typeof(ComboFormObrigatorioAutorizavel))
                {
                    var prop = (ItemForm)field.GetValue(this);
                    form.Items.Item(prop.ItemUID).Enabled = habilitar;
                }
                else if (field.FieldType == typeof(List<ItemFormAutorizavel>))
                {
                    var prop = (List<ItemFormAutorizavel>)field.GetValue(this);
                    foreach (var item in prop)
                    {
                        form.Items.Item(item.ItemUID).Enabled = habilitar;
                        form.Items.Item(item.ItemUID.Replace("P", "D")).Enabled = habilitar;
                    }
                }
            }
        }

        public string GetStatus(string formUID)
        {
            var dbdts = GetDBDatasource(formUID, MainDbDataSource);
            var status = _status.GetValorDBDatasource<string>(dbdts);

            return status;
        }

        #endregion


        #region :: Abstracts

        public abstract bool UsuarioPermitido();
        public abstract Type FormAberturaPorPeneiraType { get; }
        public abstract Type FormComissoesType { get; }
        public abstract Type FormDetalheCertificadoType { get; }
        public abstract string FormAberturaPorPeneiraSRF { get; }
        public abstract string FormComissoesSRF { get; }
        public abstract string FormDetalheCertificadoSRF { get; }
        public abstract void IniciarValoresAoAdicionarNovo(SAPbouiCOM.Form form, DBDataSource dbdts);
        public abstract string ProximaChavePrimaria(DBDataSource dbdts);
        public abstract void RegrasDeNegocioAoSalvar(SAPbouiCOM.Form form, DBDataSource dbdts);
        public abstract void QuandoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form);
        public abstract void QuandoNaoPuderAdicionarObjetoFilho(SAPbouiCOM.Form form);
        public abstract bool ContratoPodeSerAlterado(string status);

        #endregion


        public class ItemFormAutorizavel : ItemForm
        {
        }

        public class ItemFormObrigatorioAutorizavel : ItemFormObrigatorio
        {
        }

        public class ComboFormAutorizavel : ComboForm
        {
        }

        public class ComboFormObrigatorioAutorizavel : ComboFormObrigatorio
        {
        }

        public class MatrizAnexos : MatrizChildForm
        {
            public ItemForm _arquivoAnexado = new ItemForm()
            {
                ItemUID = "U_Path",
                Datasource = "U_Path"
            };
            public ItemForm _data = new ItemForm()
            {
                ItemUID = "U_Date",
                Datasource = "U_Date"
            };

            public ButtonForm _adicionar = new ButtonForm()
            {
                ItemUID = "btAddAnexo"
            };
            public ButtonForm _remover = new ButtonForm()
            {
                ItemUID = "btRmvAnexo"
            };
        }
    }
}
