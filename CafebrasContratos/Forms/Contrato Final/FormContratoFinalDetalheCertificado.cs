namespace CafebrasContratos
{
    public class FormContratoFinalDetalheCertificado : FormDetalheCertificado
    {
        public override string FormType { get { return "FormDetalheCertificado"; } }
        public override string mainDbDataSource { get { return new TabelaCertificadosDoContratoFinal().NomeComArroba; } }

        public override Matriz _matriz
        {
            get
            {
                return new Matriz()
                {
                    ItemUID = MatrixUID,
                    Datasource = mainDbDataSource
                };
            }
        }

        public override bool UsuarioPermitido()
        {
            return new FormContratoFinal().UsuarioPermitido();
        }
    }
}
