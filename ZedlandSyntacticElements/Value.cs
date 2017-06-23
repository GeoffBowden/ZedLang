using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZedLangSyntacticElements
{
    public enum State { Known, Unknown }
    public class ExecutableValue
    {
        public ExecutableValue(bool? value)
        {
            this.value = value;
        }
        public ExecutableValue()
        {
            this.value = null;
        }
        public virtual State State { get { return value == null ? State.Unknown : State.Known; } }
        public bool? value;
        public virtual bool IsKnown { get { return State == State.Known; } }
        public virtual bool? _Suggest(bool? newValue)
        {
            if (newValue == null)
            {
                if (this.value == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (newValue == true)
            {
                if (this.value == false)
                {
                    return false;
                }
                else
                {
                    this.value = true;
                    return true;
                }
            }
            else
            {
                if (this.value == true) 
                {
                    return false;
                }
                else
                {
                    this.value = false;
                    return true;
                }
            }
        }
    }
}

