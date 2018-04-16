namespace CafebrasContratos
{
    public static class PreContrato
    {
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

    }
}
