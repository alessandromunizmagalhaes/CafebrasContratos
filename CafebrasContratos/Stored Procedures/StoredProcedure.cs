using SAPHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CafebrasContratos
{
    public abstract class StoredProcedure
    {
        protected abstract string TransactionName { get; set; }

        private string MSGInicio { get; set; } = "--S1407 Criado automaticamente pelo Addon de Contratos";
        private string MSGFim { get; set; } = "--E1407";
        private List<string> TextoNoFinal { get; set; } = new List<string>() {
            "select", "@error", "@error_message"
        };

        public void Atualizar(IStoredProcedureImplementation storedProcedure)
        {
            try
            {
                using (var rsCOM = new RecordSet())
                {
                    var alter = new StringBuilder();
                    var rs = rsCOM.DoQuery($"EXEC sp_helptext '{TransactionName}';");

                    if (rs.Fields.Count > 0)
                    {
                        string text = rs.Fields.Item("Text").Value;
                        text = text.Replace("CREATE", "ALTER");
                        alter.AppendLine(text);
                        rs.MoveNext();
                    }

                    bool estouEmUmaFuncaoJaCriada = false;
                    while (!rs.EoF)
                    {
                        string text = rs.Fields.Item("Text").Value;
                        if (!String.IsNullOrEmpty(text))
                        {
                            if (EstouNoTextoChaveParaInserirMinhaFuncao(text))
                            {
                                alter.AppendLine(MSGInicio);
                                alter.AppendLine(storedProcedure.Codigo());
                                alter.AppendLine(MSGFim);
                                alter.AppendLine();
                            }
                            else if (text == MSGInicio)
                            {
                                estouEmUmaFuncaoJaCriada = true;
                            }
                            else if (text == MSGFim)
                            {
                                estouEmUmaFuncaoJaCriada = false;
                                rs.MoveNext();
                                continue;
                            }
                        }

                        if (!estouEmUmaFuncaoJaCriada)
                        {
                            alter.AppendLine(text);
                        }

                        rs.MoveNext();
                    }

                    if (alter.Length > 0)
                    {
                        rs.DoQuery(alter.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Dialogs.Error("Erro interno. Erro ao atualizar transaction.\nErro: " + e.Message);
            }
        }

        private bool EstouNoTextoChaveParaInserirMinhaFuncao(string text)
        {
            text = text.TrimStart();
            if (text.StartsWith("--"))
            {
                return false;
            }

            var lastIndexOf = text.IndexOf(TextoNoFinal[0]);
            if (lastIndexOf > -1)
            {
                text = text.Remove(lastIndexOf, TextoNoFinal[0].Length);
                lastIndexOf = text.IndexOf(TextoNoFinal[1]);
                if (lastIndexOf > -1)
                {
                    text = text.Remove(lastIndexOf, TextoNoFinal[1].Length);
                    lastIndexOf = text.IndexOf(TextoNoFinal[2]);
                    if (lastIndexOf > -1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
