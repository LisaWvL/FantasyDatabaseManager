using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyDB.Services
{

    [AttributeUsage(AttributeTargets.Property)]
    public class EditableForSnapshotAttribute : Attribute
    {
        public bool IsEditable { get; }

        public EditableForSnapshotAttribute(bool isEditable = true)
        {
            IsEditable = isEditable;
        }
    }
}