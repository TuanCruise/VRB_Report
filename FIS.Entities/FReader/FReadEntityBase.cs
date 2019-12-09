using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIS.FRead
{
    public abstract class FReadEntityBase
    {
        private bool _IsChanged;
        public bool IsNew { get; protected set; }
        public bool IsChanged {
            get { return _IsChanged; }
            set
            {
                _IsChanged = value;
            }
        }

        public FReadEntityBase()
        {
            IsNew = true;
            IsChanged = false;
        }

        public virtual void Commit()
        {
            IsNew = false;
            IsChanged = false;
        }
    }
}
