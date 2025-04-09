using System;

namespace FantasyDB.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class EditableForChapterAttribute : Attribute
    {
        public bool IsEditable { get; }

        public EditableForChapterAttribute(bool isEditable = true)
        {
            IsEditable = isEditable;
        }
    }
}