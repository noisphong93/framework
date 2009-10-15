﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Utilities;
using System.Collections.ObjectModel;
using Signum.Utilities.DataStructures;
using Signum.Entities.Reflection; 

namespace Signum.Entities
{
    [Serializable]
    public abstract class Modifiable
    {
        [Ignore]
        protected bool modified = false;

        [HiddenProperty]
        public bool Modified
        {
            get { return modified || SelfModified; }
            set
            {
                if (value)
                    modified = true;
                else
                {
                    SelfModified = false;
                    modified = false;
                }
            }
        }

        [HiddenProperty]
        public abstract bool SelfModified { get; internal set; }

        public virtual string IntegrityCheck()
        {
            return null;
        }

        public string FullIntegrityCheck()
        {
            return GraphExplorer.Integrity(GraphExplorer.FromRoot(this));
        }

        public Dictionary<Modifiable, string> FullIntegrityCheckDictionary()
        {
            return GraphExplorer.IntegrityDictionary(GraphExplorer.FromRoot(this));
        }

        protected internal virtual void PreSaving(ref bool graphModified)
        {
        }

        protected internal virtual void PostRetrieving()
        {
        }
    }
}
