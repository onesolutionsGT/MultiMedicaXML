using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos
{
    public class Articulo
    {
        private string name;
        private string old_cod;
        private string new_cod;
        private string codebars;
        private string family;
        private string subfamily;

        public string Name { get => name; set => name = value; }
        public string Old_code { get => old_cod; set => old_cod = value; }
        public string New_code { get => new_cod; set => new_cod = value; }
        public string Codebars { get => codebars; set => codebars = value; }
        public string Family { get => family; set => family = value; }
        public string Subfamily { get => subfamily; set => subfamily = value; }

    }
}
