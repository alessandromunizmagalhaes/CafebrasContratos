﻿using SAPHelper;

namespace CafebrasContratos
{
    public class FormModalidade : FormCadastroBasico
    {
        public override string FormType { get { return "FormModalidade"; } }
        public override string mainDbDataSource { get { return "@UPD_OMOD"; } }
    }
}