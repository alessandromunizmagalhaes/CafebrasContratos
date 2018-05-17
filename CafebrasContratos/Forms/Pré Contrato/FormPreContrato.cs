using System;

namespace CafebrasContratos
{
    public class FormPreContrato : FormContrato
    {
        #region :: Overrides

        public override string FormType { get { return "FormPreContrato"; } }
        public override string MainDbDataSource { get { return new TabelaPreContrato().NomeComArroba; } }
        public override Type FormAberturaPorPeneiraType { get { return typeof(FormPreContratoAberturaPorPeneira); } }
        public override Type FormComissoesType { get { return typeof(FormPreContratoComissoes); } }
        public override Type FormDetalheCertificadoType { get { return typeof(FormPreContratoDetalheCertificado); } }

        public override bool UsuarioPermitido()
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

        #endregion
    }
}
