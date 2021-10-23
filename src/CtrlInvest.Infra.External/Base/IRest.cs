using System;
using System.Collections.Generic;
using System.Text;

namespace CtrlInvest.Infra.External.Base
{
    public interface IRest
    {
        void Compute(char @operator, int operand);
        void Undo(int levels);
        void Redo(int levels);

    }
}
