using SAPHelper;

namespace CafebrasContratos
{
    public static class PreContrato
    {
        public static string SQLGrupoDeItensPermitidos = "SELECT DISTINCT U_ItmsGrpCod FROM [@UPD_OCTC]";

        public static bool UsuarioPermitido()
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

        public static bool ItemTipoBica(string itemcode)
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
    }
}
