using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfCalendar
{
    abstract class Generator
    {
        public virtual void Generate()
        {
            Header();
            Body();
            Footer();
        }

        protected virtual void Header()
        {
        }

        protected virtual void Body()
        {
        }

        protected virtual void Footer()
        {
        }
    }
}
