namespace CafebrasContratos
{
    public class FormPreContratoDetalheCertificado : FormDetalheCertificado
    {
        public override string FormType { get { return "FormDetalheCertificado"; } }
        public override string mainDbDataSource { get { return new TabelaCertificadosDoPreContrato().NomeComArroba; } }

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
            return new FormPreContrato().UsuarioPermitido();
        }
    }
}
