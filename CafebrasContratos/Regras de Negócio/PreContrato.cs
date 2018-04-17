using SAPbouiCOM;
using SAPHelper;

namespace CafebrasContratos
{
    public static class PreContrato
    {
        private static string SQLGrupoDeItensPermitidos = "SELECT DISTINCT U_ItmsGrpCod FROM [@UPD_OCTC]";

        public static bool GrupoAprovadorPermitido()
        {
            switch (Program._grupoAprovador)
            {
                case GrupoAprovador.Planejador:
                case GrupoAprovador.Gestor:
                    return true;
                default:
                    return false;
            }
        }

        public static void ConditionsParaItens(SAPbouiCOM.Form form, string chooseItemUID)
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

                    oCond.Alias = "ItmsGrpCod";
                    oCond.Operation = BoConditionOperation.co_EQUAL;
                    oCond.CondVal = grupoDeItem;

                    // põe OR em todos, menos no último.
                    if (i < rs.RecordCount)
                    {
                        oCond.Relationship = BoConditionRelationship.cr_OR;
                    }

                    rs.MoveNext();
                }
                oCFL.SetConditions(oConds);
            }
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
    }
}
