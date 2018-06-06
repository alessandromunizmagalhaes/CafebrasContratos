﻿namespace CafebrasContratos
{
    public class FormContratoFinalComissoes : FormComissoes
    {
        public override string FormType { get { return "FormComissoes"; } }
        public override string corretorDbDataSource { get { return new TabelaCorretoresDoContratoFinal().NomeComArroba; } }
        public override string responsavelDbDataSource { get { return new TabelaResponsaveisDoContratoFinal().NomeComArroba; } }

        public override MatrizCorretores _corretores
        {
            get
            {
                return new MatrizCorretores()
                {
                    ItemUID = MatrixCorretoresUID,
                    Datasource = corretorDbDataSource
                };
            }
        }

        public override MatrizResponsaveis _responsaveis
        {
            get
            {
                return new MatrizResponsaveis()
                {
                    ItemUID = MatrixResponsaveisUID,
                    Datasource = responsavelDbDataSource
                };
            }
        }

        public override bool UsuarioPermitido()
        {
            return new FormContratoFinal().UsuarioPermitido();
        }
    }
}