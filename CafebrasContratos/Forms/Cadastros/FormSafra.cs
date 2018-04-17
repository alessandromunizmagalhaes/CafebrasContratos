using SAPHelper;

namespace CafebrasContratos
{
    public class FormSafra : FormCadastroBasico
    {
        public override string FormType { get { return "FormSafra"; } }
        public override string mainDbDataSource { get { return DbConfig.safra.NomeComArroba; } }
    }
}
