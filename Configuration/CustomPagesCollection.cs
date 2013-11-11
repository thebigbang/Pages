using System.Configuration;

namespace CustomPages.Configuration
{
    [ConfigurationCollection(typeof(CustomPagesElement))]
    public class CustomPagesCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new CustomPagesElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CustomPagesElement)element).Name;
        }
    }
}
