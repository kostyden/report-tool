namespace ReportTool
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SupportedFileExtensionAttribute : Attribute
    {
        public string Extension { get; }

        public SupportedFileExtensionAttribute(string extension)
        {
            Extension = extension;
        }
    }
}
