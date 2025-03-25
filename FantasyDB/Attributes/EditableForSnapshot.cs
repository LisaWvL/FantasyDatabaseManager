using System;

namespace FantasyDB.Attributes
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