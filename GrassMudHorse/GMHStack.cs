using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GrassMudHorse
{
    class GMHStack : ArrayList
    {
        int top;

        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }

        public GMHStack()
            : base()
        { top = 0; }

        public GMHStack(ICollection c)
            : base(c)
        { top = 0; }

        public GMHStack(int capacity)
            : base(capacity)
        { top = 0; }

        public void Push(object obj)
        {
            this.Add(obj);
            top++;
        }

        public object Pop()
        {
            if (top <= 0)
                throw new ArgumentOutOfRangeException("top", "堆栈为空");
            else
            {
                object obj = this[--top];
                this.RemoveAt(top);
                return obj;
            }
        }

        public object Peek()
        {
            return this[top - 1];
        }

    }
}
